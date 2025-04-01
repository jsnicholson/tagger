using RestSharp;

namespace WebApp.Services {
    public class DataService {
        private readonly IRestClient _client;

        public DataService() {
            _client = new RestClient();
        }

        public async Task<List<string>> GetFiles(int pageIndex, int pageSize) {
            string uri = $"https://localhost:7237/api/file/absolute?startIndex={pageIndex * pageSize}&count={pageSize}";
            var files = await _client.GetAsync<List<string>>(new RestRequest(uri));
            return files;
        }
    }
}