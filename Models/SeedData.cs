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

                // Add users if none exist
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            Username = "demo",
                            Password = "demo123", // In production, use password hashing
                            Role = UserRole.Standard,
                            Timestamp = Array.Empty<byte>()
                        }
                    );
                    context.SaveChanges();
                }

                // Look for any movies
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
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "G",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "G",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M,
                        Rating = "NA",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Inception",
                        ReleaseDate = DateTime.Parse("2010-7-16"),
                        Genre = "Sci-Fi",
                        Price = 9.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Genre = "Sci-Fi",
                        Price = 10.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "The Dark Knight",
                        ReleaseDate = DateTime.Parse("2008-7-18"),
                        Genre = "Action",
                        Price = 9.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "The Matrix",
                        ReleaseDate = DateTime.Parse("1999-3-31"),
                        Genre = "Action",
                        Price = 8.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Pulp Fiction",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Genre = "Crime",
                        Price = 7.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        ReleaseDate = DateTime.Parse("1994-9-23"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Forrest Gump",
                        ReleaseDate = DateTime.Parse("1994-7-6"),
                        Genre = "Drama",
                        Price = 7.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "The Godfather",
                        ReleaseDate = DateTime.Parse("1972-3-24"),
                        Genre = "Crime",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "The Lord of the Rings: The Fellowship of the Ring",
                        ReleaseDate = DateTime.Parse("2001-12-19"),
                        Genre = "Fantasy",
                        Price = 10.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Star Wars: Episode IV - A New Hope",
                        ReleaseDate = DateTime.Parse("1977-5-25"),
                        Genre = "Sci-Fi",
                        Price = 8.99M,
                        Rating = "PG",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },

                    new Movie
                    {
                        Title = "Interstellar",
                        ReleaseDate = DateTime.Parse("2014-11-7"),
                        Genre = "Science Fiction",
                        Price = 12.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Gladiator",
                        ReleaseDate = DateTime.Parse("2000-5-5"),
                        Genre = "Action",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Prestige",
                        ReleaseDate = DateTime.Parse("2006-10-20"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Departed",
                        ReleaseDate = DateTime.Parse("2006-10-6"),
                        Genre = "Crime",
                        Price = 10.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Whiplash",
                        ReleaseDate = DateTime.Parse("2014-10-10"),
                        Genre = "Drama",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Mad Max: Fury Road",
                        ReleaseDate = DateTime.Parse("2015-5-15"),
                        Genre = "Action",
                        Price = 11.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Social Network",
                        ReleaseDate = DateTime.Parse("2010-10-1"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Rating = "PG-13",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Parasite",
                        ReleaseDate = DateTime.Parse("2019-5-30"),
                        Genre = "Thriller",
                        Price = 12.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Coco",
                        ReleaseDate = DateTime.Parse("2017-11-22"),
                        Genre = "Animation",
                        Price = 7.99M,
                        Rating = "PG",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Grand Budapest Hotel",
                        ReleaseDate = DateTime.Parse("2014-3-28"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Truman Show",
                        ReleaseDate = DateTime.Parse("1998-6-5"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Rating = "PG",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Green Mile",
                        ReleaseDate = DateTime.Parse("1999-12-10"),
                        Genre = "Drama",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Big Lebowski",
                        ReleaseDate = DateTime.Parse("1998-3-6"),
                        Genre = "Comedy",
                        Price = 7.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Braveheart",
                        ReleaseDate = DateTime.Parse("1995-5-24"),
                        Genre = "Drama",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Toy Story",
                        ReleaseDate = DateTime.Parse("1995-11-22"),
                        Genre = "Animation",
                        Price = 6.99M,
                        Rating = "G",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Schindler's List",
                        ReleaseDate = DateTime.Parse("1993-12-15"),
                        Genre = "Drama",
                        Price = 10.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Goodfellas",
                        ReleaseDate = DateTime.Parse("1990-9-19"),
                        Genre = "Crime",
                        Price = 9.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Wolf of Wall Street",
                        ReleaseDate = DateTime.Parse("2013-12-25"),
                        Genre = "Biography",
                        Price = 11.99M,
                        Rating = "R",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "The Incredibles",
                        ReleaseDate = DateTime.Parse("2004-11-5"),
                        Genre = "Animation",
                        Price = 8.99M,
                        Rating = "PG",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    },
                    
                    new Movie
                    {
                        Title = "Finding Nemo",
                        ReleaseDate = DateTime.Parse("2003-5-30"),
                        Genre = "Animation",
                        Price = 7.99M,
                        Rating = "G",
                        Timestamp = Array.Empty<byte>(),
                        UserId = context.Users.First().Id // Associate with demo user
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

