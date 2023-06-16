using System;
using MyAPI.Models;
using MyAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MyAPI.GraphQL
{
    // GraphQL/Query.cs

    public class Query
    {
        private readonly IDbContextFactory<BookDbContext> _dbContextFactory;

        public Query(IDbContextFactory<BookDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IQueryable<Book> GetBooks()
        {
            var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Books;
        }
    }

}

