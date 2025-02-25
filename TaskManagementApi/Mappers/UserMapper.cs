using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappers
{
    public static class UserMapper
    {
        public static UserDataDto MapToDataDto(this User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return new UserDataDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }

        public static IEnumerable<UserDataDto> MapToDataDto(this IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));
            return users.Select(user => user.MapToDataDto()).ToList();
        }
    }
}