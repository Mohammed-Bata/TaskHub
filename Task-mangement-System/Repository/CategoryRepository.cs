using Task_mangement_System.Data;
using Task_mangement_System.Models;
using Task_mangement_System.Repository.IRepository;

namespace Task_mangement_System.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _db.categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}
