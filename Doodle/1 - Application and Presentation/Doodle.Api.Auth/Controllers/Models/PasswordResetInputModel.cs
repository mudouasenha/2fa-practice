﻿using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Models
{
    public class PasswordResetInputModel : PasswordResetInput
    {
        public static PasswordResetInput ToInput(PasswordResetInputModel inputModel) => new()
        {
            Email = inputModel.Email,
            Password = inputModel.Password,
            RePassword = inputModel.RePassword,
            Token = inputModel.Token
        };
    }
}