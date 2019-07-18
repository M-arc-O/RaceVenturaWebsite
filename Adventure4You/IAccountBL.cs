﻿using Adventure4You.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Adventure4You
{
    public interface IAccountBL
    {
        Task<IdentityResult> CreateUser(AppUser userIdentity, string password);
        Task<AppUser> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(object userToVerify, string password);
    }
}