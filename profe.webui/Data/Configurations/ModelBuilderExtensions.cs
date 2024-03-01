using System;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using profe.webui.Entities;

namespace profe.webui.Data.Configurations
{
	public static class ModelBuilderExtensions
	{
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product() { Id = 1, productName = "Urun 1",  Price = 2000, ImgUrl = "profe1.jpg", Description = "Profe Description ürün 1telefon", IsApproved = true },
                new Product() { Id = 2, productName = "Urun 2", Price = 3000, ImgUrl = "profe2.jpg", Description = "Profe Description ürün 2 telefon", IsApproved = true },
                new Product() { Id = 3, productName = "Urun 3", Price = 4000, ImgUrl = "profe1.jpg", Description = "Profe Description ürün 3telefon", IsApproved = false }
    
            );

            builder.Entity<Category>().HasData(
                new Category() { Id = 1, categoryName = "Telefon" },
                new Category() { Id = 2, categoryName = "Bilgisayar"},
                new Category() { Id = 3, categoryName = "Elektronik"}
            );

            builder.Entity<ProductCategory>().HasData(
                new ProductCategory() { ProductId = 1, CategoryId = 1 },
                new ProductCategory() { ProductId = 1, CategoryId = 2 },
                new ProductCategory() { ProductId = 1, CategoryId = 3 },
                new ProductCategory() { ProductId = 2, CategoryId = 1 },
                new ProductCategory() { ProductId = 2, CategoryId = 2 },
                new ProductCategory() { ProductId = 2, CategoryId = 3 },
                new ProductCategory() { ProductId = 3, CategoryId = 2 }

           );


            //SEED ROLES

            var adminRole = new IdentityRole()
            {
                //Id = Guid.NewGuid().ToString(),
                Name = "admin"
            };
            adminRole.NormalizedName = adminRole.Name.ToUpper();

          


            builder.Entity<IdentityRole>().HasData(adminRole);




            //SEED USERS

            User adminUser = new User()
            {
                //Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin",
                EmailConfirmed = true,
                
            };


            string password = "Admin123.";
            var passwordHasher = new PasswordHasher<User>();

            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, password);



            builder.Entity<User>().HasData(adminUser);


            IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>()
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            builder.Entity<IdentityUserRole<string>>().HasData(identityUserRole);


            Cart cart = new Cart()
            {
                Id = 1,
                UserId = adminUser.Id
            };

            builder.Entity<Cart>().HasData(cart);

            //var memberRole = new IdentityRole()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "member"
            //};


            //memberRole.NormalizedName = memberRole.Name.ToUpper();

            //List<IdentityRole> roles = new List<IdentityRole>()
            //{
            //    adminRole,memberRole
            //};

            //builder.Entity<IdentityRole>().HasData(roles);

            //User memberUser = new User()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "member",
            //    FirstName = "member",
            //    LastName = "admembermin",
            //    Email = "member@member",
            //    EmailConfirmed = true,
            //};

            //memberUser.NormalizedUserName = adminUser.UserName.ToUpper();
            //memberUser.NormalizedEmail = adminUser.Email.ToUpper();
            //memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, password);


            //List<User> users = new List<User>()
            //{
            //    adminUser,memberUser
            //};

            //builder.Entity<IdentityRole>().HasData(users);

            //List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            //userRoles.Add(new IdentityUserRole<string>
            //{

            //    UserId = users[0].Id,
            //    RoleId = roles.First(r => r.Name == "admin").Id
            //});

            //userRoles.Add(new IdentityUserRole<string>
            //{
            //    UserId = users[0].Id,
            //    RoleId = roles.First(r => r.Name == "member").Id
            //});

            //builder.Entity<IdentityUserRole<string>>().HasData(userRoles);






        }


    }
}

