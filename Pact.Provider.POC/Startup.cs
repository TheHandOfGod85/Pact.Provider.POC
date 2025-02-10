using Provider.POC.Repositories;

namespace Provider.POC
{
    public class Startup
    {
        public static WebApplication WebApp(params string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapGet("/user", (int Id, IUserRepository repository) =>
            {
                var user = repository.Get(Id);
                return Results.Ok(user);
            })
            .Produces<object>(200)
            .WithOpenApi();

            return app;
        }
    }
}
