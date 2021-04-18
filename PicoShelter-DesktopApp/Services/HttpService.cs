using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Services
{
    public class HttpService
    {
        private HttpClient httpClient = new HttpClient();

        public string AccessToken
        {
            get => httpClient.DefaultRequestHeaders.Authorization.Parameter;
            set => httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", value);
        }

        public HttpService()
        {

        }

        public HttpService(string accessToken)
        {
            AccessToken = accessToken;
        }
        
        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var dto = new LoginRequestDto() { username = username, password = password };
            var sDto = JsonSerializer.Serialize(dto);

            var result = await httpClient.PostAsync(ServerRouting.LoginUrl, new StringContent(sDto));

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

            var result = await httpClient.PostAsync(ServerRouting.LoginByEmailUrl, new StringContent(sDto));

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
    }
}
