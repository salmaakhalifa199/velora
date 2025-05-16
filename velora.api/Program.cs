using Microsoft.EntityFrameworkCore;
using velora.api.Extensions;
using velora.core.Data.Contexts;
using velora.api.Helper;
using System.Text.Json.Serialization;
using StackExchange.Redis;
using velora.api.MiddleWares;
using velora.services.Services.FeedbackService;

namespace velora.api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllers()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                  });

			

			builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<StoreIdentityDBContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddIdentityService(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();


            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configurations = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configurations);
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder.WithOrigins("http://localhost:5173") // your frontend URL
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

        

            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();   

            app.MapControllers();
            app.Run();
        }
    }
}
