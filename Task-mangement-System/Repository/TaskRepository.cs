using Task_mangement_System.Data;
using Task_mangement_System.Models;
using Task_mangement_System.Repository.IRepository;
using Task = Task_mangement_System.Models.Task;

namespace Task_mangement_System.Repository
{
    public class TaskRepository:Repository<Task>,ITaskRepository
    {
        private readonly ApplicationDbContext _db;
        public TaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Task> UpdateAsync(Task task)
        {
            _db.tasks.Update(task);
            await _db.SaveChangesAsync();
            return task;
        }
    }
}
