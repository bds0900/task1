using System;
using Xunit;
using Task1;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Task1.Models;
using Microsoft.AspNetCore.Identity;

namespace Task1Test
{
    public class Auth: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;
        public Auth(WebApplicationFactory<Startup> fac)
        {
            httpClient = fac.CreateClient();
        }


        [Theory]
        [InlineData("/api/auth/signin", "hamburger", "conestoga")]
        [InlineData("/api/auth/signin", "mcdonald", "zxcasdqwe")]
        public async Task Signin(string url, string username, string password  )
        {
            httpClient.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                              //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
                                                                                              //httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    new SignInModel
                    {
                        Username = username,
                        Password = password,
                        ConfirmPassword = password
                    }),
                    Encoding.UTF8, "application/json"
                    )
            };

            var response = await httpClient.SendAsync(request);
            var signInResult = await response.Content.ReadAsAsync<SigninResult>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(signInResult.Succeeded);
        }




        [Theory]
        [InlineData("/api/auth/signin", "John", "conestoga")]
        [InlineData("/api/auth/signin", "Peter", "zxcasdqwe")]
        public async Task SigninDuplicate(string url, string username, string password)
        {
            httpClient.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                              //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
                                                                                              //httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    new SignInModel
                    {
                        Username = username,
                        Password = password,
                        ConfirmPassword = password
                    }),
                    Encoding.UTF8, "application/json"
                    )
            };

            var response1 = await httpClient.SendAsync(request1);
            var signInResult1 = await response1.Content.ReadAsAsync<SigninResult>();
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.True(signInResult1.Succeeded);



            HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    new SignInModel
                    {
                        Username = username,
                        Password = password,
                        ConfirmPassword = password
                    }),
                    Encoding.UTF8, "application/json"
                    )
            };
            var response2 = await httpClient.SendAsync(request2);
            var signInResult2 = await response2.Content.ReadAsAsync<SigninResult>();
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            Assert.False(signInResult2.Succeeded);

        }




        [Theory]
        [InlineData("/api/auth/signin","/api/auth/login", "doosan","conestoga")]
        [InlineData("/api/auth/signin","/api/auth/login", "speer","hithere")]
        public async Task Login(string signinurl,string loginurl,string username,string password)
        {
            httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, signinurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new SignInModel{ Username=username,Password=password, ConfirmPassword=password}), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(request);



            request = new HttpRequestMessage(HttpMethod.Post, loginurl)
            {
                Content = new StringContent(JsonSerializer.Serialize(new LoginModel { Username = username, Password = password }), Encoding.UTF8, "application/json")
            };


            var response = await httpClient.SendAsync(request);
            var logInResult= await response.Content.ReadAsAsync<LoginResult>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(logInResult.Succeeded);
            
        }




        [Theory]
        [InlineData("/api/auth/signin", "/api/auth/login", "Adam", "conestoga")]
        [InlineData("/api/auth/signin", "/api/auth/login", "Smith", "hithere")]
        public async Task LoginWrongPassword(string signinurl, string loginurl, string username, string password)
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
                Content = new StringContent(JsonSerializer.Serialize(new LoginModel { Username = username, Password = password+"wrong" }), Encoding.UTF8, "application/json")
            };


            var response = await httpClient.SendAsync(request);
            var logInResult = await response.Content.ReadAsAsync<LoginResult>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(logInResult.Succeeded);

        }


        
    }
}
