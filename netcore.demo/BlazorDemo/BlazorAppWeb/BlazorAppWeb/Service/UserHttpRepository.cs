using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAppWeb.Service
{
    public class UserHttpRepository : IUserHttpRepository
    {
        private readonly HttpClient _client;
        public UserHttpRepository(HttpClient httpClient)
        {
            _client = httpClient;
        }
        public async Task<UserInfo> AddUserinfo(UserInfo userinfo)
        {
            var userInfoJson = new StringContent(JsonSerializer.Serialize(userinfo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("user/AddUser", userInfoJson);
            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<UserInfo>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public Task<bool> DeleteUser(string userid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DeptInfo>> GetDeptInfos()
        {
            var response = await _client.GetAsync("dept/GetDeptInfos");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DeptInfo>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }

        public Task<UserInfo> GetUserInfoById(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserInfo>> GetUserInfos()
        {
            var response = await _client.GetAsync("user/GetAll");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserInfo>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public Task<PagingResponse<UserInfo>> GetUserInfos(UserParameters userParameters)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> UpdateUser(UserInfo userinfo)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFile(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }
    }
}
