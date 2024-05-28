using System.Text.Json;

namespace Kidzplayground.DAL
{
    public class DataManagerAPI
    {
        private static Uri BaseAddress = new Uri("https://localhost:44357/");

        public static async Task<List<Models.Category>> GetCategories()
        {
            List<Models.Category> categories = new List<Models.Category>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Category");
                if(response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Models.Category>>(responseString);
                }

            }

            return categories;
        }
    }
}
