using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using task_service.DBContexts;
using task_service.Models;
using Xunit;

namespace task_service.tests.IntegrationTests;

public class TaskServiceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _applicationFactory;
    private readonly HttpClient _client;
    private readonly string _authToken;

    public TaskServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _applicationFactory = factory;
        _client = factory.CreateClient();
        _authToken = GetAuthToken();
    }

    private string GetAuthToken()
    {
        string url =
            "https://keycloak.marktempelman.duckdns.org/auth/realms/planning-app/protocol/openid-connect/token";
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Post);

        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", "password");
        request.AddParameter("username", "admin");
        request.AddParameter("password", "admin");
        request.AddParameter("client_id", "angular-web-client");
        request.AddParameter("client_secret", "");

        RestResponse response = client.Execute(request);
        var content = response.Content;
        return content.Split(',')[0].Split(':')[1].Trim('"');

    }

    [Fact]
    public async Task GetByListId_Returns_List_Of_Tasks()
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, "tasks/1");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        HttpResponseMessage response = await _client.SendAsync(message);

        var rep = response.Content.ReadAsStringAsync().Result;
        List<TaskModel> myJObject = JsonConvert.DeserializeObject<List<TaskModel>>(rep);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, myJObject.Count);
    }

    [Fact]
    public async Task GetByListId_Returns_404_When_List_Does_Not_Exist()
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, "tasks/3");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetByListId_Returns_401_When_Not_Authorized()
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, "tasks/1");
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_Returns_200()
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "tasks");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        message.Content = new StringContent(JsonConvert.SerializeObject(new TaskModel
        {
            ListId = 2,
            Name = "Test Task",
            Description = "Test Task Description",
        }));
        message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateTask_Returns_Bad_Request_When_Task_Invalid()
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "tasks");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        message.Content = new StringContent(JsonConvert.SerializeObject(new TaskModel
        {
            ListId = 1,
            Name = "Test Task"
        }));
        message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_Returns_200()
    {
        using var message = new HttpRequestMessage(HttpMethod.Put, "tasks");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        message.Content = new StringContent(JsonConvert.SerializeObject(new TaskModel
        {
            Id = 1,
            ListId = 1,
            Name = "Test Task",
            Description = "Test Task Description",
        }));
        message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTask_Returns_200()
    {
        using var message = new HttpRequestMessage(HttpMethod.Delete, "tasks/3");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        HttpResponseMessage response = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}