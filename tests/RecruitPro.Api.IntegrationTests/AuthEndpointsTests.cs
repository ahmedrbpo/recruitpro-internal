using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace RecruitPro.Api.IntegrationTests;

public sealed class AuthEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Login_UnknownUser_Returns401WithEnvelope()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new { Email = "nobody@coventine.com", Password = "whatever123" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("success").GetBoolean().Should().BeFalse();
        body.GetProperty("error").GetProperty("code").GetString().Should().Be("Unauthorized");
    }

    [Fact]
    public async Task Login_MissingEmail_Returns400WithProblemDetails()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new { Email = "", Password = "whatever123" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
    }

    [Fact]
    public async Task Refresh_WithoutCookie_Returns401()
    {
        var response = await _client.PostAsync("/api/v1/auth/refresh", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
