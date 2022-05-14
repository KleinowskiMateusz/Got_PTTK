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

var postgresStr = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<TouristsBookContext>(options =>
    options.UseNpgsql(postgresStr));

builder.Services.AddScoped<IPublicTrailsRepository, TrasyPubliczneRepository>();
builder.Services.AddScoped<ITripRepository, WycieczkaRepository>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

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

app.Run();
