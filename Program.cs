using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Models;
using MyAPI.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPooledDbContextFactory<BookDbContext>(options =>
    options.UseInMemoryDatabase("Books")); // Use InMemory database for simplicity

builder.Services
    .AddGraphQLServer()
    .AddQueryType<MyAPI.GraphQL.Query>()
    .AddMutationType<MyAPI.GraphQL.Mutation>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000") // 更换为你的前端应用地址
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});



var app = builder.Build();

app.UseCors("AllowSpecificOrigin"); // 在 UseRouting 和 UseEndpoints 之间添加

// Initialize database
InitializeDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UsePlayground();
}


app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();


void InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BookDbContext>>().CreateDbContext();

    if (!db.Books.Any())
    {
        db.Books.Add(new Book
        {
            Id = 1,
            Title = "The Great Gatsby"
        });
        db.Books.Add(new Book
        {
            Id = 2,
            Title = "1984"
        });

        db.SaveChanges();
    }
}



