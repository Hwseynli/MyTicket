﻿using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Domain.Entities.Categories;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Categories;
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}

