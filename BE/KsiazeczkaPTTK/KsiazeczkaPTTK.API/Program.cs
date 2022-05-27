using KsiazeczkaPttk.DAL;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.DAL.Repositories;
using KsiazeczkaPttk.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "KsiazeczkaPttk", Version = "v1" }));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(o => o.AddPolicy("DevelopmentCorsPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var mySqlConnectionStr = builder.Configuration.GetConnectionString("MySQLConnection");
Console.WriteLine($"Connection string: {mySqlConnectionStr}");
builder.Services.AddDbContext<TouristsBookContext>(options =>
                options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

builder.Services.AddScoped<IPublicTrailsRepository, PublicTrailsRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KsiazeczkaPttk v1"));
    app.UseCors("DevelopmentCorsPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await SeedDb(app.Services);

app.Run();

async Task SeedDb(IServiceProvider services)
{
    bool dbSeeded = false;
    while (!dbSeeded)
    {
        try
        {
            Console.WriteLine("Attempting to migrate database");
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TouristsBookContext>();
                bool dropDb = builder.Configuration.GetValue<bool>("DropDb");
                Console.WriteLine($"Drop bd: {dropDb}");
                if (dropDb)
                {
                    await context.Database.EnsureDeletedAsync();
                }
                await context.Migrate();
                Console.WriteLine("Migration: Done");
                await TouristsBookSeed.Seed(context);
                Console.WriteLine("Seed: Done");
                dbSeeded = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Db connection not established succesfully");
            Console.WriteLine(ex.Message);
            Thread.Sleep(10_000);
        }
    }
}