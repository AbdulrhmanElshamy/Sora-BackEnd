using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Category;
using Sofra.Api.Data;
using Sofra.Api.Models;
using UniLearn.API.Errors;

namespace Sofra.Api.Services.CategoryServices
{
    public class CategoryService(ApplicationDbContext dbContext) : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
                      await _dbContext.Categories
                      .AsNoTracking()
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .Include(c => c.DeletedBy)
                      .ProjectToType<CategoryResponse>()
                      .ToListAsync(cancellationToken);


        public async Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(id, cancellationToken);

            return category is not null
                ? Result.Success(category.Adapt<CategoryResponse>())
                : Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);
        }

        public async Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default)
        {
            var isExistingName = await _dbContext.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

            if (isExistingName)
                return Result.Failure<CategoryResponse>(CategoryErrors.DuplicatedCategoryTitle);

            var category = request.Adapt<Category>();

            await _dbContext.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Adapt<CategoryResponse>());
        }



        public async Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
        {
            var isExistingTitle = await _dbContext.Categories.AnyAsync(x => x.Name == request.Name && x.Id != id, cancellationToken: cancellationToken);

            if (isExistingTitle)
                return Result.Failure<CategoryRequest>(CategoryErrors.DuplicatedCategoryTitle);

            var currentCategory = await _dbContext.Categories.FindAsync(id, cancellationToken);

            if (currentCategory is null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            currentCategory.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories.FindAsync(id, cancellationToken);

            if (category is null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            category.IsDeleted =!category.IsDeleted;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
