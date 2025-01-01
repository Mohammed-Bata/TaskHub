using System_Utility;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Models;
using Task_Mangement_Web.Models.Dto;
using Task_Mangement_Web.Services.IServices;

namespace Task_Mangement_Web.Services
{
    public class TaskService:BaseService,ITaskService
    {
        private readonly IHttpClientFactory _httpClient;
        private string taskurl;
        public TaskService(IHttpClientFactory httpClient, IConfiguration configuration):base(httpClient)
        {
            _httpClient = httpClient;
            taskurl = configuration.GetValue<string>("ServiceUrls:TaskApi");
        }

        public Task<T> CreateAsync<T>(TaskCreateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.APIType.POST,
                Data = dto,
                Url = taskurl + "/api/Task",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.APIType.DELETE,
                Url = taskurl + "/api/Task/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.APIType.GET,
                Url = taskurl + "/api/Task",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.APIType.GET,
                Url = taskurl + "/api/Task/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(TaskUpdateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.APIType.PUT,
                Data = dto,
                Url = taskurl + "/api/Task/" + dto.Id,
                Token = token
            });
        }
    }
}
