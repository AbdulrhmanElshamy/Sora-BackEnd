using Sofra.Api.Authentication;
using System.Security.Cryptography;
using Sofra.Api.Contracts;
using Sofra.Api.Data;
using Microsoft.AspNetCore.Identity;
using Sofra.Api.Models;
using Sofra.Api.Contracts.Authentication;
using Sofra.Api.Abstractions;
using Sofra.Api.Errors;
using Sofra.Api.Contracts.Customer;
using Sofra.Api.Enums;
using Sofra.Api.Contracts.Kitchen;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sofra.Application.Helper;
using Microsoft.EntityFrameworkCore;
using UniLearn.API.Errors;
namespace Sofra.Api.Services;

public class AuthService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials); ;

        var (token, expiresIn) =  _jwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName,user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }


    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if(user is null )
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null) 
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) =  _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email,user.FirstName,user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


    public async Task<Result<AuthResponse>> CustomerRegisterAsync(CustomerRegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email.Split("@")[0],
            Email = request.Email,
            FirstName = request.FristName,
            LastName = request.LastName,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());

            var customer = new Customer()
            {
                ApplicationUserId = user.Id,
            };

            await _dbContext.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            return await GetTokenAsync(request.Email, request.Password, cancellationToken);

        }

        return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
    }

    public async Task<Result<AuthResponse>> KitchenRegisterAsync(KitchenRegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email.Split("@")[0],
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {

            await _userManager.AddToRoleAsync(user, Roles.Kitchen.ToString());
            var Kitchen = new Kitchen()
            {
                ApplicationUserId = user.Id,
                Name = request.Name,
                MaxDeliveryDistance = request.MaxDeliveryDistance,
                Avatar = ImageHelper.UploadImage(request.Avatar),
            };

            var IsExistingCategories = await Task.WhenAll(request.Categories.Select(async x =>
                await _dbContext.Categories.AnyAsync(c => c.Id == x && !c.IsDeleted && c.KitchenCategories.Any(x => x.KitchenId == c.Id) , cancellationToken)
            )).ContinueWith(t => t.Result.All(result => result));

            if (!IsExistingCategories)
                Result.Failure<AuthResponse>(CategoryErrors.CategoryNotFound);

            foreach (var item in request.Categories)
            {
                Kitchen.KitchenCategories.Add(new KitchenCategory() {CategoryId = item });
            }

            await _dbContext.AddAsync(Kitchen);
            await _dbContext.SaveChangesAsync();

            return await GetTokenAsync(request.Email, request.Password, cancellationToken);
        }

        return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
    }
}