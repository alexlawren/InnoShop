using InnoShop.Application.Contracts.Infrastructure;
using System.Net.Http.Json; 
namespace InnoShop.Infrastructure.ApiClients;
using System.Net.Http;
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SetUserProductsVisibilityAsync(Guid userId, bool isVisible)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ProductApiClient");
                var command = new { UserId = userId, IsVisible = isVisible };

                var response = await client.PatchAsJsonAsync($"/api/internal/users/{userId}/products-visibility", command);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not update products visibility for user {userId}: {ex.Message}");
            }
        }
    }
