using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Core
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, BankTransactionContext context)
        {
            SeedRoles(roleManager);
            var user = new ApplicationUser
            {
                PhoneNumber = "0909898978",
                UserName = "Andry",
                NormalizedUserName = "ANDRY",
                Email = "yutyt@gmail.com",
                NormalizedEmail = "yutyt@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            var person = (context.Persons.Where(e => e.Id == 1)).FirstOrDefault();
            person.ApplicationUserFkId = user.Id;
            var user2 = new ApplicationUser
            {
                PhoneNumber = "0819888978",
                UserName = "Vasil",
                NormalizedUserName = "ANDRY",
                Email = "yutyt2@gmail.com",
                NormalizedEmail = "yutyt2@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var person2 = (context.Persons.Where(e => e.Id == 2)).FirstOrDefault();
            person2.ApplicationUserFkId = user2.Id;
            //var user3 = new ApplicationUser
            //{
            //    PhoneNumber = "0819877978",
            //    UserName = "Masha",
            //    NormalizedUserName = "ANDRY",
            //    Email = "yutyt1@gmail.com",
            //    NormalizedEmail = "yutyt1@GMAIL.COM",
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true,
            //    SecurityStamp = Guid.NewGuid().ToString("D")
            //};
            //var person3 = (context.Persons.Where(e => e.Id == 3)).FirstOrDefault();
            //person3.ApplicationUserFkId = user3.Id;

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                SeedUsers(userManager, user, "qWerty1_");
                SeedUsers(userManager, user2, "qWerty1_");
                //SeedUsers(userManager, user3, "qWerty1_");
            }
            context.Persons.UpdateRange(person, person2);//, person3
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {

            }
          
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            if (userManager.FindByEmailAsync(user.Email).Result == null)
            {
                IdentityResult result = userManager.CreateAsync(user, "qWerty1_").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }

        public async static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new string[] { "Admin", "User" };

            foreach (string role in roles)
            {


                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    {
                        IdentityRole roleApp = new IdentityRole();
                        roleApp.Name = role;
                        await roleManager.CreateAsync(roleApp);
                    }
                }
            }

        }
    }
}
        //public static class InitializeIdentityDb
        //{
        //    public async static void InitializeDbIdentity(BankTransactionContext context, UserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        //    {




        //        var user = new ApplicationUser
        //        {
        //            PhoneNumber = "0909898978",
        //            UserName = "Andry",
        //            NormalizedUserName = "ANDRY",
        //            Email = "yutyt@gmail.com",
        //            NormalizedEmail = "yutyt@GMAIL.COM",
        //            EmailConfirmed = true,
        //            PhoneNumberConfirmed = true,
        //            SecurityStamp = Guid.NewGuid().ToString("D"),
        //        };
        //        var person = (context.Persons.Where(e => e.Id == 1)).FirstOrDefault();
        //        person.ApplicationUserFkId = user.Id;
        //        var user2 = new ApplicationUser
        //        {
        //            PhoneNumber = "0819888978",
        //            UserName = "Vasil",
        //            NormalizedUserName = "ANDRY",
        //            Email = "yutyt2@gmail.com",
        //            NormalizedEmail = "yutyt2@GMAIL.COM",
        //            EmailConfirmed = true,
        //            PhoneNumberConfirmed = true,
        //            SecurityStamp = Guid.NewGuid().ToString("D")
        //        };
        //        var person2 = (context.Persons.Where(e => e.Id == 2)).FirstOrDefault();
        //        person2.ApplicationUserFkId = user2.Id;
        //        var user3 = new ApplicationUser
        //        {
        //            PhoneNumber = "0819877978",
        //            UserName = "Masha",
        //            NormalizedUserName = "ANDRY",
        //            Email = "yutyt1@gmail.com",
        //            NormalizedEmail = "yutyt1@GMAIL.COM",
        //            EmailConfirmed = true,
        //            PhoneNumberConfirmed = true,
        //            SecurityStamp = Guid.NewGuid().ToString("D")
        //        };
        //        var person3 = (context.Persons.Where(e => e.Id == 3)).FirstOrDefault();
        //        person3.ApplicationUserFkId = user3.Id;

        //        if (!context.Users.Any(u => u.UserName == user.UserName))
        //        {
        //            await RegisterUser(userStore, userManager, context, user, "qWerty1_");
        //            await RegisterUser(userStore, userManager, context, user2, "qWerty1_");
        //            await RegisterUser(userStore, userManager, context, user3, "qWerty1_");
        //        }

        //        await AssignUserRoles(userStore, user.Email, roles);
        //        context.Persons.UpdateRange(new List<Person> { person, person2, person3 });
        //        await context.SaveChangesAsync();

        //    }
        //    public static async Task RegisterUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string passsword)
        //    {
        //        await userManager.CreateAsync(user, passsword);
        //    }

        //    public static async Task<IdentityResult> AssignUserRoles(UserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager, string email, string[] roles)
        //    {
        //        ApplicationUser user = await userManager.FindByEmailAsync(email);
        //        var result = await userManager.AddToRolesAsync(user, roles);
        //        return result;
        //    }
        //}
