using Task_mangement_System.Models;

namespace Task_mangement_System.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<Category> UpdateAsync(Category category);
    }
}
