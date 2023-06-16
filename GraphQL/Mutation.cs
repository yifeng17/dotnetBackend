using System;
using MyAPI.Models;
using MyAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MyAPI.GraphQL
{

    public class Mutation
    {
        private readonly IDbContextFactory<BookDbContext> _dbContextFactory;

        public Mutation(IDbContextFactory<BookDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        public async Task<Book> AddBook(Book input)
        {
            var dbContext = _dbContextFactory.CreateDbContext();

            var book = new Book
            {
                Title = input.Title
            };

            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();

            return book;
        }
    }


}



