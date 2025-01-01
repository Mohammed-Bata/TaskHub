using Task_mangement_Web.Models;
using Task_Mangement_Web.Models;

namespace Task_Mangement_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest request);
    }
}
