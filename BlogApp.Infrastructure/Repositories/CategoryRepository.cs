using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
    {
       
    }
}
