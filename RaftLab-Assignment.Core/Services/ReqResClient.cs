using RaftLab_Assignment.Interfaces;
using RaftLab_Assignment.Models;
using RaftLab_Assignment.Utils;
using System.Text.Json;

namespace RaftLab_Assignment.Services
{
    public class ReqResClient : IReqResInterface
    {
        private readonly HttpClient _httpClient;

        public ReqResClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("User ID must be greater than zero", nameof(userId));

                var response = await _httpClient.GetAsync($"/api/users/{userId}");

                response.EnsureSuccessStatusCode();

                return await response.ReadFromJsonDataFieldAsync<User>();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException($"User with ID {userId} not found.");

                throw new Exception("An error occurred while communicating with the user service.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error parsing user data from the service.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving user data.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int page = 1)
        {
            try
            {
                if (page <= 0)
                    throw new ArgumentException("Page number must be greater than zero", nameof(page));

                var response = await _httpClient.GetAsync($"/api/users?page={page}");

                response.EnsureSuccessStatusCode();

                return await response.ReadFromJsonDataFieldAsync<IEnumerable<User>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while communicating with the users service.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error parsing users data from the service.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving users data.", ex);
            }
        }
    }
}
