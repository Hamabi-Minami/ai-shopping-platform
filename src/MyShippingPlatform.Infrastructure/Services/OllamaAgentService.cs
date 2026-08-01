using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyShippingPlatform.Application.Common.Interfaces;
using MyShippingPlatform.Application.DTOs;

namespace MyShippingPlatform.Infrastructure.Services;

// Explicitly implement IAiAgentService interface
public class OllamaAgentService : IAiAgentService
{
    private readonly HttpClient _httpClient;

    public OllamaAgentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AiAuditResponse?> AuditShipmentAsync(
        string shipperName,
        string consigneeName,
        string originCountry,
        string destinationCountry,
        CancellationToken cancellationToken = default)
    {
        var prompt = $@"
            You are a strict automated customs compliance checker.
            Analyze the following shipment:
            - Sender: {shipperName} ({originCountry})
            - Recipient: {consigneeName} ({destinationCountry})

            CRITICAL RULES:
            Evaluate compliance.
            1. The 'result' field MUST strictly be either 'yes' or 'no'. NO OTHER VALUES ARE ALLOWED.
            2. Do NOT quote any laws, regulations, or legal clauses.

            Expected JSON schema:
            {{
              ""result"": ""yes"",
              ""reason"": ""One short sentence explaining why""
            }}";

        var requestBody = new
        {
            model = "llama3.2",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            format = "json",
            options = new
            {
                temperature = 0.0
            },
            stream = false
        };

        var httpResponse = await _httpClient.PostAsJsonAsync("http://localhost:11434/api/chat", requestBody, cancellationToken);
        httpResponse.EnsureSuccessStatusCode();

        var jsonResult = await httpResponse.Content.ReadFromJsonAsync<OllamaChatResponse>(cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(jsonResult?.Message?.Content))
            return null;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<AiAuditResponse>(jsonResult.Message.Content, options);
    }
}

public class OllamaChatResponse
{
    [JsonPropertyName("message")]
    public OllamaChatMessage? Message { get; set; }
}

public class OllamaChatMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}