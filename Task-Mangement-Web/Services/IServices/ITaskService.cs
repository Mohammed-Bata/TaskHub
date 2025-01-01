using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Models.Dto;

namespace Task_Mangement_Web.Services.IServices
{
    public interface ITaskService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(TaskCreateDto dto, string token);
        Task<T> UpdateAsync<T>(TaskUpdateDto dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
