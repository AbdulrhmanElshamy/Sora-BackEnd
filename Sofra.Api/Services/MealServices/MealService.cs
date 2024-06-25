using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Meal;
using Sofra.Api.Data;
using Sofra.Api.Models;
using Sofra.Application.Helper;
using System.Security.Claims;
using UniLearn.API.Errors;

namespace Sofra.Api.Services.MealServices
{
    public class MealService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IMealService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<IEnumerable<MealResponse>> GetAllAsync(int? KitchenId, string? SearchText, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Meals.Include(m => m.MealPhotos).AsQueryable().AsNoTracking();

            if (KitchenId is not null)
                query = query.Where(m => m.KitchenId == KitchenId);

            if (SearchText is not null)
                query = query.Where(m => m.Name.ToLower().Contains(SearchText.ToLower()));


            var meals = await query.ToListAsync();

            return meals.Adapt<IEnumerable<MealResponse>>();
        }

        public async Task<Result<MealResponse>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            var meal = await _dbContext
                        .Meals
                        .Where(m => m.Id == Id)
                        .AsNoTracking()
                        .Include(m => m.MealPhotos)
                        .FirstOrDefaultAsync();
            if (meal is null)
                return Result.Failure<MealResponse>(MealErrors.MealNotFound);

            return Result.Success(meal.Adapt<MealResponse>());
        }

        public async Task<Result<MealResponse>> AddAsync(MealRequest request, CancellationToken cancellationToken = default)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchenId = await _dbContext.Kitchens.Where(k => k.ApplicationUserId == CurrentUserId).Select(k => k.Id).FirstOrDefaultAsync();

            var IsExistingName = await _dbContext.Meals.AnyAsync(m => m.Name.ToLower().Equals(request.Name.ToLower()));
            if (IsExistingName)
                return Result.Failure<MealResponse>(MealErrors.DuplicatedMealTitle);

            var meal = new Meal()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                PreparationTimeInMinute = request.PreparationTimeInMinute,
                KitchenId = CurrentKitchenId
            };


            foreach (var item in request.MealPhotos)
            {
                meal.MealPhotos.Add(new MealPhoto { Image = ImageHelper.UploadImage(item) });
            }

            await _dbContext.AddAsync(meal, cancellationToken);
            await _dbContext.SaveChangesAsync();

            return Result.Success(meal.Adapt<MealResponse>());
        }



        public async Task<Result> UpdateAsync(int Id, MealRequest request, CancellationToken cancellationToken = default)
        {
            var meal = await _dbContext.Meals.Where(m => m.Id == Id).Include(m => m.MealPhotos).FirstOrDefaultAsync();

            if (meal is null)
                return Result.Failure(MealErrors.MealNotFound);

            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchenId = await _dbContext.Kitchens.Where(k => k.ApplicationUserId == CurrentUserId).Select(k => k.Id).FirstOrDefaultAsync();

            var IsExistingName = await _dbContext.Meals.AnyAsync(m => m.Name.ToLower().Equals(request.Name.ToLower()) && m.Id != Id);
            if (IsExistingName)
                return Result.Failure<MealResponse>(MealErrors.DuplicatedMealTitle);



            meal.Name = request.Name;
            meal.Description = request.Description;
            meal.Price = request.Price;
            meal.PreparationTimeInMinute = request.PreparationTimeInMinute;
            meal.KitchenId = CurrentKitchenId;

            foreach (var item in meal.MealPhotos)
            {
                ImageHelper.DeleteImage(item.Image);
            }

            meal.MealPhotos.Clear();

            foreach (var item in request.MealPhotos)
            {
                meal.MealPhotos.Add(new MealPhoto { Image = ImageHelper.UploadImage(item) });
            }


            _dbContext.Meals.Update(meal);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> ToggleStatus(int Id, CancellationToken cancellationToken = default)
        {
           var meal = await _dbContext.Meals.FindAsync(Id);
        
            if(meal is null)
                return Result.Failure(MealErrors.MealNotFound);

            meal.IsAvailable = !meal.IsAvailable;

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeletedAsync(int Id, CancellationToken cancellationToken = default)
        {
            var meal = await _dbContext.Meals.FindAsync(Id);

            if (meal is null)
                return Result.Failure(MealErrors.MealNotFound);

            meal.IsDeleted = true;

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
