using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using rest_api.Models;
using rest_api.Repositories;
using rest_api.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
// Add services to the container.

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = builder
        .Configuration
        .GetSection(nameof(MongoDbSettings))
        .Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);

});
builder.Services.AddTransient<IUserRepository, UserRepository>();


builder.Services.AddControllers();

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

app.Run();
