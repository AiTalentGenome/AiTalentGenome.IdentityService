// Infrastructure/ExternalServices/HeadHunter/HeadHunterProvider.cs

using System.Net.Http.Headers;
using System.Net.Http.Json;
using AiTalentGenome.IdentityService.Application.DTOs;
using AiTalentGenome.IdentityService.Application.Interfaces;
using AiTalentGenome.IdentityService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiTalentGenome.IdentityService.Infrastructure.Services;

public class HeadHunterProvider : IHeadHunterProvider
{
    private readonly HttpClient _httpClient;
    private readonly HhOptions _options;
    private readonly ILogger<HeadHunterProvider> _logger;

    public HeadHunterProvider(HttpClient httpClient, IOptions<HhOptions> options, ILogger<HeadHunterProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        // HH требует User-Agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _options.AppName);
    }

    public async Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["code"] = code,
            ["redirect_uri"] = _options.RedirectUri
        });

        var response = await _httpClient.PostAsync("https://hh.ru/oauth/token", content, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            // Логируем сырую ошибку для отладки
            _logger.LogWarning("HH API вернул ошибку: {Error}", errorContent);
        
            // Выбрасываем исключение, которое поймает интерцептор
            throw new HttpRequestException($"Ошибка обмена кода HH: {errorContent}");
        }

        var data = await response.Content.ReadFromJsonAsync<HhTokenResponse>(cancellationToken: ct);
        return data?.AccessToken ?? throw new Exception("Не удалось получить Access Token от HH");
    }

    public async Task<HhProfileDto> GetUserProfileAsync(string accessToken, CancellationToken ct = default)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var profile = await _httpClient.GetFromJsonAsync<HhProfileDto>("https://api.hh.ru/me", ct);

        return profile ?? throw new Exception("Не удалось загрузить профиль пользователя HH");
    }

    // Вспомогательный класс только для этого метода
    private record HhTokenResponse(
        [property: System.Text.Json.Serialization.JsonPropertyName("access_token")] string AccessToken
    );
}