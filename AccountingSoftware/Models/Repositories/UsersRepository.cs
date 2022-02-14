using AccountingSoftware.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Repositories
{
    public class UsersRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> GetUserFullName(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user.FirstName + " " + user.LastName;
        }
    }
}
