using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Domain.Entities.Categories;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Categories;
public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
{
    public SubCategoryRepository(AppDbContext context) : base(context)
    {
    }
}

