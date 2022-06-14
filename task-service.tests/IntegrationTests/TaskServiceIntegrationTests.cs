using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
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
        string url = "https://keycloak.marktempelman.duckdns.org/auth/realms/planning-app/protocol/openid-connect/token";
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
        using (var message = new HttpRequestMessage(HttpMethod.Get, "tasks/1"))
        {
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            HttpResponseMessage response = await _client.SendAsync(message);

            var rep = response.Content.ReadAsStringAsync().Result;
            List<TaskModel> myJObject = JsonConvert.DeserializeObject<List<TaskModel>>(rep);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, myJObject.Count);
        }
    }
}