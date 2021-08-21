﻿using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Validators
{
    public class RegisterUserDtoValidation : AbstractValidator<RegisterUserDto>
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserDtoValidation(UserManager<User> userManager)
        {
            _userManager = userManager;
            RuleFor(x => x.Email)
                .NotEmpty().NotNull().EmailAddress()
                .MustAsync(UniqueAsync).WithMessage("User with this email already exists");

            RuleFor(x => x.Password)
                .NotEmpty().NotNull();
            
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().NotNull()
                .Matches(x=>x.Password);

        }

        private async Task<bool> UniqueAsync(string email, CancellationToken cancellationToken) =>
            await _userManager.Users.AnyAsync(x => x.Email != email, cancellationToken);
    }
}