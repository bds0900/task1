using AutoMapper;
using DataSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Mapping
{
    public class TweetMapping: Profile
    {
        public TweetMapping()
        {
            CreateMap<Tweet, TweetDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(source => source.TweetId))
                .ReverseMap();
        }
    }
}
