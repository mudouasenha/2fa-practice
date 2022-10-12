﻿using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Users.Abstractions
{
    public interface IUsersService
    {
        Task<Result<User>> Register(UserRegisterInput input);

        Task<Result<User>> SignIn(UserSignInInput input);

        Task<Result<User>> SignOut(UserSignOutInput input);

        Task<User> VerifyUser(UserVerifyInput input);

        Task<Result<User>> DeleteUser(UserFilterDTO input);

        Task<Result<User>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword);
    }
}