namespace GitHubApp
{
    public class snippets
    {

        public async Task<T> SendRequest<T>(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");       // TODO: use a suitably typed exception
            }

            // We used upper case names in the MovieInfo class so we need to turn off case-sensitivity when deserializing
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(responseBody, options);
        }
    }
}
