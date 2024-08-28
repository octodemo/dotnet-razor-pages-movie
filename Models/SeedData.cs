using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesMovie.Data;
using System;
using System.Linq;

namespace RazorPagesMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RazorPagesMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<RazorPagesMovieContext>>()))
            {
                if (context == null || context.Movie == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M,
                        Rating = "NA"
                    },

                    new Movie
                    {
                        Title = "Inception",
                        ReleaseDate = DateTime.Parse("2010-7-16"),
                        Genre = "Sci-Fi",
                        Price = 9.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "The Matrix",
                        ReleaseDate = DateTime.Parse("1999-3-31"),
                        Genre = "Action",
                        Price = 8.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Genre = "Sci-Fi",
                        Price = 10.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "The Dark Knight",
                        ReleaseDate = DateTime.Parse("2008-7-18"),
                        Genre = "Action",
                        Price = 9.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "Pulp Fiction",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Genre = "Crime",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Inception",
                        ReleaseDate = DateTime.Parse("2010-7-16"),
                        Genre = "Sci-Fi",
                        Price = 9.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "The Matrix",
                        ReleaseDate = DateTime.Parse("1999-3-31"),
                        Genre = "Action",
                        Price = 8.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Genre = "Sci-Fi",
                        Price = 10.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "The Dark Knight",
                        ReleaseDate = DateTime.Parse("2008-7-18"),
                        Genre = "Action",
                        Price = 9.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "Pulp Fiction",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Genre = "Crime",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        ReleaseDate = DateTime.Parse("1994-9-23"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Forrest Gump",
                        ReleaseDate = DateTime.Parse("1994-7-6"),
                        Genre = "Drama",
                        Price = 7.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "The Godfather",
                        ReleaseDate = DateTime.Parse("1972-3-24"),
                        Genre = "Crime",
                        Price = 9.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "The Lord of the Rings: The Fellowship of the Ring",
                        ReleaseDate = DateTime.Parse("2001-12-19"),
                        Genre = "Fantasy",
                        Price = 10.99M,
                        Rating = "PG-13"
                    },

                    new Movie
                    {
                        Title = "Star Wars: Episode IV - A New Hope",
                        ReleaseDate = DateTime.Parse("1977-5-25"),
                        Genre = "Sci-Fi",
                        Price = 8.99M,
                        Rating = "PG"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

