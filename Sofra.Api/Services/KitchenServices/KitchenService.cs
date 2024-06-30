using Azure.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Address;
using Sofra.Api.Contracts.Kitchen;
using Sofra.Api.Data;
using Sofra.Api.Helpers;
using Sofra.Api.Models;
using Sofra.Application.Helper;
using System.Security.Claims;
using Sofra.Api.Errors;

namespace Sofra.Api.Services.KitchenServices
{
    public class KitchenService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IKitchenService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private const double EarthRadiusKm = 6371.0;


        public async Task<IEnumerable<KitchenResponse>> GetAllAsync(AddressRequest request, CancellationToken cancellationToken = default)
        {

            var query = await _dbContext.Kitchens.Include(k => k.Address).Include(k => k.Reviews).AsNoTracking().ToListAsync();

            var distanceTasks = query.Select(async x => new
            {
                Kitchen = x,
                Distance =  DistanceCalculator.CalculateDistance(x.Address.Latitude, x.Address.Longitude, request.Latitude, request.Longitude)
            }).ToList();

            var results = await Task.WhenAll(distanceTasks);

            var filteredKitchens = results
                .Where(x => x.Kitchen.MaxDeliveryDistance >= x.Distance)
                .Select(x => x.Kitchen)
                .ToList();

            return filteredKitchens.Adapt<IEnumerable<KitchenResponse>>();
        }


        public async Task<IEnumerable<KitchenResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var kitchens = await _dbContext.Kitchens.Include(k => k.Address).Include(k => k.Reviews).AsNoTracking().ToListAsync(cancellationToken);

            return kitchens.Adapt<IEnumerable<KitchenResponse>>();
        }



        public async Task<Result<KitchenResponse>> GetAsync(int id, AddressRequest request, CancellationToken cancellationToken = default)
        {
            var kitchen = await _dbContext.Kitchens
                .Where(k => k.Id == id && k.Address != null)
                .Include(k => k.Address)
                .FirstOrDefaultAsync(cancellationToken);

            if (kitchen is null)
                return Result.Failure<KitchenResponse>(KitchenErrors.KitchenNotFound);

            double distance =  DistanceCalculator.CalculateDistance(
                kitchen.Address.Latitude,
                kitchen.Address.Longitude,
                request.Latitude,
                request.Longitude);

            if (kitchen.MaxDeliveryDistance < distance)
                return Result.Failure<KitchenResponse>(KitchenErrors.KitchenNotFound);

            return Result.Success(kitchen.Adapt<KitchenResponse>());
        }


        public async Task<Result<KitchenResponse>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            var kitchen = await _dbContext.Kitchens
                .Where(k => k.Id == Id)
                .Include(k => k.Address)
                .Include(k => k.Reviews)
                .FirstOrDefaultAsync(cancellationToken);

            if (kitchen is null)
                return Result.Failure<KitchenResponse>(KitchenErrors.KitchenNotFound);

            return Result.Success(kitchen.Adapt<KitchenResponse>());
        }


        public async Task<Result> UpdateAsync(KitchenUpdateRequest request, CancellationToken cancellationToken = default)
        {

            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchen = await _dbContext.Kitchens.Where(k => k.ApplicationUserId == CurrentUserId).FirstOrDefaultAsync();

            var IsExistingName = await _dbContext.Kitchens.AnyAsync(k => k.Name.ToLower().Equals(request.Name) && k.Id != CurrentKitchen!.Id);

            if (IsExistingName)
                return Result.Failure(KitchenErrors.DuplicatedKitchenTitle);


            CurrentKitchen!.Name = request.Name;
            CurrentKitchen.MaxDeliveryDistance = request.MaxDeliveryDistance;

            if (request.Avatar is not null)
            {
                var oldAvatarPath = CurrentKitchen.Avatar;

                CurrentKitchen.Avatar = ImageHelper.UploadImage(request.Avatar!);

                ImageHelper.DeleteImage(oldAvatarPath);
            }


            CurrentKitchen.KitchenCategories.Clear();


            bool IsExistingCategory;

            foreach (var Item in request.Categories)
            {
                IsExistingCategory = await _dbContext.Categories.AnyAsync(c => c.Id == Item && !c.IsDeleted);
                if (!IsExistingCategory)
                    return Result.Failure(CategoryErrors.CategoryNotFound);

                CurrentKitchen.KitchenCategories.Add(new KitchenCategory() { CategoryId = Item });
            }

            _dbContext.Update(CurrentKitchen);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> ToggleStatus(CancellationToken cancellationToken = default)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchen = await _dbContext.Kitchens.Where(k => k.ApplicationUserId == CurrentUserId).FirstOrDefaultAsync();

            CurrentKitchen!.Enabled = !CurrentKitchen.Enabled;

            _dbContext.Update(CurrentKitchen);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> SetAddress(AddressRequest request, CancellationToken cancellationToken = default)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchen = await _dbContext.Kitchens.Where(k => k.ApplicationUserId == CurrentUserId).FirstOrDefaultAsync();

            var Address = new Address()
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                KitchenId = CurrentKitchen!.Id
            };

            await _dbContext.AddAsync(Address, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }


    }
}
