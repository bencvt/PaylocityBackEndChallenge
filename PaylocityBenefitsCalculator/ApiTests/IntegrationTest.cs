using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace ApiTests;

public class IntegrationTest : IDisposable
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _httpClient;

    protected WebApplicationFactory<Program> Factory
    {
        get
        {
            if (_factory == default)
            {
                _factory = new();
            }

            return _factory;
        }
    }

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = Factory.CreateClient();
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
        Factory?.Dispose();
    }
}

