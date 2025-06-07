using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using velora.services.HandlerResponses;
using velora.repository.Interfaces;
using velora.repository.Interfaces.IdentityInterfaces;
using velora.repository.Repositories;
using velora.repository.Repositories.IdentityRepos;
using velora.Repository.Repositories;
using velora.services.Helper;
using velora.services.Services.AdminService;
using velora.services.Services.AdminService.Dto;
using velora.services.Services.AuthService;
using velora.services.Services.ContactsService;
using velora.services.Services.ContactsService.Dto;
using velora.services.Services.ProductService;
using velora.services.Services.ProductService.Dto;
using velora.services.Services.TokenService;
using velora.services.Services.UserService;
using velora.services.Services.CartService.Dto;
using velora.services.Services.CartService;
using velora.repository.Cart.Interface;
using velora.repository.Cart;
using velora.services.CacheService;
using velora.services.Services.OrderService.Dto;
using velora.services.Services.OrderService;
using velora.services.Services.FeedbackService;
using velora.services.Services.UserService.Dto;
using velora.services.Services.PaymentService;

namespace velora.api.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitWork, UnitWork>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IContactsService, ContactsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddAutoMapper(typeof(ContactsProfile));
            services.AddAutoMapper(typeof(OrderProfile));
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddAutoMapper(typeof(AdminProfile).Assembly);
            services.AddAutoMapper(typeof(CartProfile).Assembly);
			services.AddAutoMapper(typeof(FeedbackProfile)); 

			services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService , FeedbackService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(model => model.Value?.Errors.Count > 0)
                        .SelectMany(model => model.Value.Errors)
                        .Select(error => error.ErrorMessage)
                        .ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            }
            );
            return services;

        }
    }
}
