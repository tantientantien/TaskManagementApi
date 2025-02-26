using Humanizer;
using System;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class UserMapper
    {
        public static UserDataDto ToDataDto(this User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return new UserDataDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            };
        }
    }
}