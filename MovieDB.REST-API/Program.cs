using Database.Repository.Interfaces;
using Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MovieDB.REST_API.Services;
using Database;
using MovieDB.SharedModels;


public class Program
{
    public static void Main(string[] args)
    {
        const string corsPolicyName = "MyCorsPolicy";
        var webBuilder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        webBuilder.Services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicyName, policy =>
            {
                // policy.WithOrigins("http://localhost:2103", "http://localhost:2102", "http://localhost:2101", "http://localhost:8080")
                //     .AllowAnyHeader()
                //     .AllowAnyMethod()
                //     .AllowCredentials();
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        webBuilder.Services.AddControllers();
        webBuilder.Services.AddEndpointsApiExplorer();
        // AddSwaggerGenWithSecurity is configured in ServiceExtensions.cs
        webBuilder.Services.AddSwaggerGenWithSecurity(); 

        // Sets connection string.
        string connectionStr = webBuilder.Configuration.GetConnectionString("Development") ?? throw new InvalidOperationException("No connection string found.");
        webBuilder.Services.AddDbContext<YourMovieDBContext>(options => options.UseSqlServer(connectionStr));

        // Handling of JSON Web Token authentication.
        webBuilder.Services.AddSingleton<TokenProviderService>();
        webBuilder.Services.AddAuthorization();
        webBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webBuilder.Configuration["JWT:Key"]!)),
                    ValidIssuer = webBuilder.Configuration["JWT:Issuer"],
                    ValidAudience = webBuilder.Configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

        webBuilder.Services.AddScoped<IBaseRepository<Person>, PersonRepository>();
        webBuilder.Services.AddScoped<IBaseRepository<Movie>, MovieRepository>();
        webBuilder.Services.AddScoped<PersonRepository>();
        webBuilder.Services.AddScoped<MovieRepository>();
        webBuilder.Services.AddScoped<UserRepository>();

        var app = webBuilder.Build();

        // Execute EF Migrations
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<YourMovieDBContext>();
            context.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCors(corsPolicyName);
        app.Run();
    }
}
