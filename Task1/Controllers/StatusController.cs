using AutoMapper;
using DataSource.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        private readonly IMapper _mapper;
        public StatusController(ITweetService tweetService, IMapper mapper)
        {
            _tweetService = tweetService ?? throw new ArgumentException(nameof(tweetService));
            _mapper = mapper;
        }

        /*[Authorize]
        [HttpGet]
        public async Task<IActionResult> Auth()
        {
            return Ok();
        }*/

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tweet tweet)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult {Succeeded=false,Message= "Please login first" });
            }
            if(String.IsNullOrEmpty(tweet.Text))
            {
                return StatusCode(StatusCodes.Status204NoContent, new ErrorResult { Succeeded = false, Message = "You cannot tweet empty" });
            }

            try
            {
                tweet.UserId = User.Identity.Name;
                var gennum= Task.Run(() => IdGeneraotr());
                tweet.TweetId = await gennum;
                await _tweetService.CreateAsync(tweet);
            }
            catch(MyException myex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult { Succeeded = false, Message = "Server Error" });
            }
    
            return Ok(_mapper.Map<Tweet,TweetDTO>(tweet));
        }

        [HttpGet("{id}",Name ="Read")]
        public async Task<IActionResult> Read(string id)
        {
            Tweet tweet = null;
            try
            {
                tweet=await _tweetService.GetTweetAsync(id);
                if(tweet==null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorResult { Succeeded = false, Message = "Id not found, please check id again" });
                }
            }
            catch (MyException myex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult { Succeeded = false, Message = "Server Error" });
            }

            return Ok(_mapper.Map<Tweet, TweetDTO>(tweet));
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Tweet tweet)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult { Succeeded = false, Message = "Please login first" });
            }

            if (String.IsNullOrEmpty(tweet.Text))
            {
                return StatusCode(StatusCodes.Status204NoContent, new ErrorResult { Succeeded = false, Message = "You cannot tweet empty" });
            }

            try
            {
                tweet = await _tweetService.GetTweetAsync(id);
                if (tweet == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorResult { Succeeded = false, Message = "Id not found, please check id again" });
                }
                if (tweet.UserId!= User.Identity.Name)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult { Succeeded = false, Message = "You are not authorized to update this tweet" });
                }

                tweet = await _tweetService.UpdateAsync(id,tweet);
            }
            catch (MyException myex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult { Succeeded = false, Message = "Server Error" });
            }

            return Ok(_mapper.Map<Tweet, TweetDTO>(tweet));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult { Succeeded = false, Message = "Please login first" });
            }

            Tweet tweet = null;
            try
            {
                tweet = await _tweetService.GetTweetAsync(id);
                if (tweet.UserId != User.Identity.Name)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult { Succeeded = false, Message = "You are not authorized to delete this tweet" } );
                }

                tweet = await _tweetService.DeleteAsync(id);
                if (tweet == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorResult { Succeeded = false, Message = "Id not found, please check id again" });
                }
            }
            catch (MyException myex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult { Succeeded = false, Message = "Server Error" });
            }
            return Ok(_mapper.Map<Tweet, TweetDTO>(tweet));
        }

        //generate id by using time and rand number
        private string IdGeneraotr()
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var rnd = new Random().Next(0,1000);
            return time.ToString() + rnd.ToString();
        }




    }
}
