using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using Vives.Services.Model;

namespace PeopleManager.Ui.Mvc.ApiServices
{
    public class PersonApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PersonApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<PersonResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/people";
            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var people = await httpResponse.Content.ReadFromJsonAsync<IList<PersonResult>>();

            return people ?? new List<PersonResult>();
        }

        public async Task<PersonResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<PersonResult>();
        }

        public async Task<ServiceResult<PersonResult?>> Create(PersonRequest person)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/people";
            var httpResponse = await httpClient.PostAsJsonAsync(route, person);

            httpResponse.EnsureSuccessStatusCode();

            var serviceResult = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PersonResult?>>();
            if (serviceResult is null)
            {
                return new ServiceResult<PersonResult?>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "ApiError",
                            Title = "Api returned a null result",
                            Type = ServiceMessageType.Error
                        }
                    }
                };
            }
            return serviceResult;
        }

        public async Task<ServiceResult<PersonResult?>> Update(int id, PersonRequest person)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.PutAsJsonAsync(route, person);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PersonResult?>>();
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.DeleteAsync(route);

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
