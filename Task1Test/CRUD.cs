using DataSource;
using DataSource.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Task1;
using Task1.Models;
using Xunit;

namespace Task1Test
{
    public class CRUD: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;
        public CRUD(WebApplicationFactory<Startup> fac)
        {
            httpClient =  fac.WithWebHostBuilder(builder =>
            {
                // Microsoft.AspNetCore.TestHost;
                builder.ConfigureTestServices(services =>
                {
                    var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                    var section = config.GetSection("MongoDbSettings");
                    services.Configure<MongoDbSettings>(section);
                    services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                        serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
                    services.AddScoped<TweetContext>();
                    services.AddScoped<ITweetService, TweetService>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                });
            }).CreateClient();
        }


        [Theory]
        [InlineData("/api/auth/signin", "/api/auth/login", "/api/status/", "doosan", "conestoga", "my name is doosan")]
        [InlineData("/api/auth/signin", "/api/auth/login", "/api/status/", "speer", "hithere", "recent grad")]
        public async Task Create(string signinurl, string loginurl,string status, string username, string password, string text)
        {
            httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, signinurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new SignInModel { Username = username, Password = password, ConfirmPassword = password }), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(request);



            request = new HttpRequestMessage(HttpMethod.Post, loginurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new LoginModel { Username = username, Password = password }), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);
            var logInResult = await response.Content.ReadAsAsync<LoginResult>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(logInResult.Succeeded);



            request = new HttpRequestMessage(HttpMethod.Post, status)
            {
                Content = new StringContent(JsonSerializer.Serialize(new Tweet { Text=text }), Encoding.UTF8, "application/json")
            };
            response = await httpClient.SendAsync(request);
            var createResult = await response.Content.ReadAsAsync<TweetDTO>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(text,createResult.Text);
            Assert.False(String.IsNullOrEmpty(createResult.Id));
        }



        [Theory]
        [InlineData("/api/status/", "my name is doosan")]
        [InlineData("/api/status/", "recent grad")]
        public async Task CreateWithoutLogin(string status, string text)
        {
            httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            

            var request = new HttpRequestMessage(HttpMethod.Post, status)
            {
                Content = new StringContent(JsonSerializer.Serialize(new Tweet { Text = text }), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);
            var createResult = await response.Content.ReadAsAsync<ErrorResult>();
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Please login first", createResult.Message);
            
        }


        [Theory]
        [InlineData("/api/auth/signin", "/api/auth/login", "/api/status/", "doosan", "conestoga", "")]
        [InlineData("/api/auth/signin", "/api/auth/login", "/api/status/", "speer", "hithere", "")]
        public async Task CreateEmptyTweet(string signinurl, string loginurl, string status, string username, string password, string text)
        {
            httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, signinurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new SignInModel { Username = username, Password = password, ConfirmPassword = password }), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(request);



            request = new HttpRequestMessage(HttpMethod.Post, loginurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new LoginModel { Username = username, Password = password }), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);
            var logInResult = await response.Content.ReadAsAsync<LoginResult>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(logInResult.Succeeded);


            request = new HttpRequestMessage(HttpMethod.Post, status)
            {
                Content = new StringContent(JsonSerializer.Serialize(new Tweet { Text = text }), Encoding.UTF8, "application/json")
            };
            response = await httpClient.SendAsync(request);
            var createResult = await response.Content.ReadAsAsync<ErrorResult>();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal("You cannot tweet empty", createResult.Message);
                    }
    }
}
