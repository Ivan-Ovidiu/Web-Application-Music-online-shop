using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TargDeMuzica.Data;

namespace TargDeMuzica.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                // Verificam daca in baza de date exista cel putin rol
                // insemnand ca a fost rulat codul
                // De aceea facem return pentru a nu insera inca o data
                // Acesta metoda trebuie sa se execute o singura data
                 if (context.Roles.Any())
                 {
                     return; 
                 }

                 // CREAREA ROLURILOR IN BD
                 // daca nu contine roluri, acestea se vor crea
                 context.Roles.AddRange(
                     new IdentityRole
                     {
                         Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                         Name = "Administrator",
                         NormalizedName = "Administrator".ToUpper()
                     },

                     new IdentityRole
                     {
                         Id = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                         Name = "Colaborator",
                         NormalizedName = "Colaborator".ToUpper()
                     },

                     new IdentityRole
                     {
                         Id = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                         Name = "UserN",
                         NormalizedName = "UserN".ToUpper()
                     },

                     new IdentityRole
                     {
                         Id = "68de4b1f-fcdc-42cc-acbc-60f58da2d070",
                         Name = "UserI",
                         NormalizedName = "UserI".ToUpper()
                     }
                 );

                 // o noua instanta pe care o vom utiliza pentru  crearea parolelor utilizatorilor
                 // parolele sunt de tip hash
                 var hasher = new PasswordHasher<ApplicationUser>();

                 // CREAREA USERILOR IN BD
                 // Se creeaza cate un user pentru fiecare rol
                 context.Users.AddRange(
                     new ApplicationUser
                     {
                         Id = "8e445865-a24d-4543-a6c6-9443d048cdb0",
                         UserName = "admin@test.com",
                         EmailConfirmed = true,
                         NormalizedEmail = "ADMIN@TEST.COM",
                         Email = "admin@test.com",
                         NormalizedUserName = "ADMIN@TEST.COM",
                         PasswordHash = hasher.HashPassword(null, "Admin1!")
                     },

                     new ApplicationUser
                     {
                         Id = "8e445865-a24d-4543-a6c6-9443d048cdb1",
                         UserName = "editor@test.com",
                         EmailConfirmed = true,
                         NormalizedEmail = "EDITOR@TEST.COM",
                         Email = "editor@test.com",
                         NormalizedUserName = "EDITOR@TEST.COM",
                         PasswordHash = hasher.HashPassword(null, "Editor1!")
                     },

                     new ApplicationUser
                     {
                         Id = "8e445865-a24d-4543-a6c6-9443d048cdb2",
                         UserName = "user@test.com",
                         EmailConfirmed = true,
                         NormalizedEmail = "USER@TEST.COM",
                         Email = "user@test.com",
                         NormalizedUserName = "USER@TEST.COM",
                         PasswordHash = hasher.HashPassword(null, "User1!")
                     }
                 );

                 // ASOCIEREA USER-ROLE
                 context.UserRoles.AddRange(
                     new IdentityUserRole<string>
                     {
                         RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                         UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                     },

                     new IdentityUserRole<string>
                     {
                         RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                         UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                     },

                     new IdentityUserRole<string>
                     {
                         RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                         UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
                     }
                 );
                 context.SaveChanges();
                 
                 

                // Add Music Supports (Categories)
                context.MusicSuports.AddRange(
                    new MusicSuport { MusicSuportName = "Vinyl" },
                    new MusicSuport { MusicSuportName = "CD" },
                    new MusicSuport { MusicSuportName = "Digital Download" },
                    new MusicSuport { MusicSuportName = "Cassette" }
                );

                context.SaveChanges();

                // Add Artists
                context.Artists.AddRange(
                    new Artist { ArtistName = "Pink Floyd", ArtistAge = 0 },
                    new Artist { ArtistName = "Queen", ArtistAge = 0 },
                    new Artist { ArtistName = "David Bowie", ArtistAge = 0 },
                    new Artist { ArtistName = "The Beatles", ArtistAge = 0 },
                    new Artist { ArtistName = "Led Zeppelin", ArtistAge = 0 },
                    new Artist { ArtistName = "Metallica", ArtistAge = 0 },
                    new Artist { ArtistName = "Bob Dylan", ArtistAge = 0 }
                );

                context.SaveChanges();

                
                var user = context.Users.Find("8e445865-a24d-4543-a6c6-9443d048cdb2");

                var vinyl = context.MusicSuports.First(m => m.MusicSuportName == "Vinyl");
                var cd = context.MusicSuports.First(m => m.MusicSuportName == "CD");
                var pinkFloyd = context.Artists.First(a => a.ArtistName == "Pink Floyd");
                var queen = context.Artists.First(a => a.ArtistName == "Queen");
                var bowie = context.Artists.First(a => a.ArtistName == "David Bowie");
                var beatles = context.Artists.First(a => a.ArtistName == "The Beatles");
                var ledZeppelin = context.Artists.First(a => a.ArtistName == "Led Zeppelin");
                var metallica = context.Artists.First(a => a.ArtistName == "Metallica");
                var bobDylan = context.Artists.First(a => a.ArtistName == "Bob Dylan");

                // Add Products
                context.Products.AddRange(
                    new Product
                    {
                        ProductName = "The Dark Side of the Moon",
                        ProductDescription = "Iconic Pink Floyd album featuring 'Money' and 'Time'",
                        ProductPrice = 29.99f,
                        ProductStock = 50,
                        ProductImageLocation = "/images/dark-side-moon.jpg",
                        ProductScore = 4.9f,
                        ProductGenres = new List<string> { "Progressive Rock", "Psychedelic" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = pinkFloyd.ArtistID
                    },
                    new Product
                    {
                        ProductName = "A Night at the Opera",
                        ProductDescription = "Features the legendary 'Bohemian Rhapsody'",
                        ProductPrice = 24.99f,
                        ProductStock = 35,
                        ProductImageLocation = "/images/night-at-opera.jpg",
                        ProductScore = 4.8f,
                        ProductGenres = new List<string> { "Rock", "Classic Rock" },
                        MusicSuportID = cd.MusicSuportID,
                        ArtistID = queen.ArtistID
                    },
                    new Product
                    {
                        ProductName = "The Rise and Fall of Ziggy Stardust",
                        ProductDescription = "David Bowie's concept album about an alien rock star",
                        ProductPrice = 27.99f,
                        ProductStock = 25,
                        ProductImageLocation = "/images/ziggy-stardust.jpg",
                        ProductScore = 4.7f,
                        ProductGenres = new List<string> { "Glam Rock", "Art Rock" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = bowie.ArtistID
                    },
                    new Product
                    {
                        ProductName = "Abbey Road",
                        ProductDescription = "The Beatles' final recorded album",
                        ProductPrice = 34.99f,
                        ProductStock = 40,
                        ProductImageLocation = "/images/abbey-road.jpg",
                        ProductScore = 5.0f,
                        ProductGenres = new List<string> { "Rock", "Pop Rock" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = beatles.ArtistID
                    },
                    new Product
                    {
                        ProductName = "Led Zeppelin IV",
                        ProductDescription = "Features 'Stairway to Heaven'",
                        ProductPrice = 29.99f,
                        ProductStock = 30,
                        ProductImageLocation = "/images/led-zep-iv.jpg",
                        ProductScore = 4.9f,
                        ProductGenres = new List<string> { "Hard Rock", "Folk Rock" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = ledZeppelin.ArtistID
                    },
                    new Product
                    {
                        ProductName = "Master of Puppets",
                        ProductDescription = "Metallica's third studio album",
                        ProductPrice = 22.99f,
                        ProductStock = 45,
                        ProductImageLocation = "/images/master-puppets.jpg",
                        ProductScore = 4.8f,
                        ProductGenres = new List<string> { "Thrash Metal", "Heavy Metal" },
                        MusicSuportID = cd.MusicSuportID,
                        ArtistID = metallica.ArtistID
                    },
                    new Product
                    {
                        ProductName = "The Wall",
                        ProductDescription = "Pink Floyd's rock opera masterpiece",
                        ProductPrice = 39.99f,
                        ProductStock = 20,
                        ProductImageLocation = "/images/the-wall.jpg",
                        ProductScore = 4.9f,
                        ProductGenres = new List<string> { "Progressive Rock", "Rock Opera" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = pinkFloyd.ArtistID
                    },
                    new Product
                    {
                        ProductName = "Highway 61 Revisited",
                        ProductDescription = "Features 'Like a Rolling Stone'",
                        ProductPrice = 26.99f,
                        ProductStock = 25,
                        ProductImageLocation = "/images/highway61.jpg",
                        ProductScore = 4.7f,
                        ProductGenres = new List<string> { "Folk Rock", "Blues Rock" },
                        MusicSuportID = vinyl.MusicSuportID,
                        ArtistID = bobDylan.ArtistID
                    }
                );

                context.SaveChanges();

                var darkSide = context.Products.First(p => p.ProductName == "The Dark Side of the Moon");
                var wall = context.Products.First(p => p.ProductName == "The Wall");
                var abbeyRoad = context.Products.First(p => p.ProductName == "Abbey Road");

                if (user != null)
                {
                    // Add Reviews
                    context.Reviews.AddRange(
                        new Review
                        {
                            ReviewContent = "One of the greatest albums ever made!",
                            ReviewerName = "MusicFan123",
                            ProductId = darkSide.ProductID,
                            ReviewDate = DateTime.Now.AddDays(-30),
                            StarRating = 5,
                            User = user
                        },
                        new Review
                        {
                            ReviewContent = "Incredible sound quality on vinyl",
                            ReviewerName = "VinylCollector",
                            ProductId = darkSide.ProductID,
                            ReviewDate = DateTime.Now.AddDays(-15),
                            StarRating = 5,
                            User = user
                        },
                        new Review
                        {
                            ReviewContent = "A masterpiece of progressive rock",
                            ReviewerName = "RockExpert",
                            ProductId = wall.ProductID,
                            ReviewDate = DateTime.Now.AddDays(-10),
                            StarRating = 5,
                            User = user
                        },
                        new Review
                        {
                            ReviewContent = "Every track is perfect",
                            ReviewerName = "MusicLover",
                            ProductId = abbeyRoad.ProductID,
                            ReviewDate = DateTime.Now.AddDays(-5),
                            StarRating = 5,
                            User = user
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
