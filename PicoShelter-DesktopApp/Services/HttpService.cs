using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Exceptions;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient = new HttpClient();

        public string AccessToken
        {
            get => httpClient.DefaultRequestHeaders.Authorization.Parameter;
            set => httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
        }

        public HttpService()
        {
            AppSettingsProvider.Provide().PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AppSettings.AppSettings.AccessToken))
                    AccessToken = (s as AppSettings.AppSettings)?.AccessToken;
            };
        }

        public HttpService(string accessToken) : this()
        {
            AccessToken = accessToken;
        }

        private static readonly HttpService instance = new HttpService();
        public static HttpService Current => instance;
        
        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var dto = new LoginRequestDto() { username = username, password = password };
            var sDto = JsonSerializer.Serialize(dto);

            var result = await httpClient.PostAsync(ServerRouting.LoginUrl, new StringContent(sDto, Encoding.UTF8, "application/json"));

            var resultStream = await result.Content.ReadAsStreamAsync();
            var response = await JsonSerializer.DeserializeAsync<HttpResponseDto<LoginResponseDto>>(resultStream);

            if (result.IsSuccessStatusCode && response.success)
            {
                return response.data;
            }

            throw new HttpResponseException(result.StatusCode, response.error, response.errors);
        }

        public async Task<LoginResponseDto> LoginByEmailAsync(string email, string password)
        {
            var dto = new LoginByEmailRequestDto() { email = email, password = password };
            var sDto = JsonSerializer.Serialize(dto);

            var result = await httpClient.PostAsync(ServerRouting.LoginByEmailUrl, new StringContent(sDto, Encoding.UTF8, "application/json"));

            var resultStream = await result.Content.ReadAsStreamAsync();
            var response = await JsonSerializer.DeserializeAsync<HttpResponseDto<LoginResponseDto>>(resultStream);

            if (result.IsSuccessStatusCode && response.success)
            {
                return response.data;
            }

            throw new HttpResponseException(result.StatusCode, response.error, response.errors);
        }

        public async Task<AccountInfoDto> GetCurrentUserAsync()
        {
            var result = await httpClient.GetAsync(ServerRouting.GetCurrentUser);
            if (result.IsSuccessStatusCode)
            {
                var resultStream = await result.Content.ReadAsStreamAsync();
                var dto = await JsonSerializer.DeserializeAsync<HttpResponseDto<AccountInfoDto>>(resultStream);

                if (dto.success)
                    return dto.data;
            }

            throw new HttpResponseException(result.StatusCode);
        }

        public async Task<ImageInfoDto> UploadImageAsync(string title, int deleteInHours, bool isPublic, int quality, Stream filestream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(string.IsNullOrWhiteSpace(title) ? "" : title), "title");
            
            if (deleteInHours <= 0)
                content.Add(new StringContent(""), "deleteInHours");
            else
                content.Add(new StringContent(deleteInHours.ToString()), "deleteInHours");

            content.Add(new StringContent(isPublic.ToString()), "isPublic");
            content.Add(new StringContent(quality.ToString()), "quality");
            content.Add(new StreamContent(filestream), "file", "image");

            var result = await httpClient.PostAsync(ServerRouting.UploadUrl, content);
            var resultStream = await result.Content.ReadAsStreamAsync();
            var response = await JsonSerializer.DeserializeAsync<HttpResponseDto<ImageInfoDto>>(resultStream);

            if (result.IsSuccessStatusCode && response.success)
            {
                return response.data;
            }

            throw new HttpResponseException(result.StatusCode, response.error, response.errors);
        }
    }
}
