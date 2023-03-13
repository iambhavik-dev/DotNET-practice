using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using rest_api.Models;
using rest_api.Repositories;
using rest_api.Repositories.Interfaces;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));

var mongoDbSettings = builder.Configuration
    .GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>();

// Add services to the container.

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoDbSettings.ConnectionString);
});
builder.Services.AddTransient<IUserRepository, UserRepository>();


builder.Services.AddControllers();
builder.Services
    .AddHealthChecks()
    .AddMongoDb(
        mongoDbSettings.ConnectionString,
        name: "mongodb",
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] { "ready" }
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthChecks/ready", new HealthCheckOptions
{
    Predicate = (checks) => checks.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    staus = entry.Value.ToString(),
                    exception = entry.Value.Exception == null ? "none" : entry.Value.Exception.Message,
                    duaration = entry.Value.Duration.ToString()
                })
            }
        );
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/healthChecks/live", new HealthCheckOptions
{
    Predicate = (_) => false
});

app.Run();
