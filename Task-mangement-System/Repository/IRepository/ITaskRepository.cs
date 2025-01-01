using Task_mangement_System.Models;
using Task = Task_mangement_System.Models.Task;

namespace Task_mangement_System.Repository.IRepository
{
    public interface ITaskRepository:IRepository<Task>
    {
        Task<Task> UpdateAsync(Task task);
    }
}
