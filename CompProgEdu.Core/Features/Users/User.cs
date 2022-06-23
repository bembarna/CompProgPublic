using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompProgEdu.Core.Features.Users
{
    public class User : IdentityUser<int>, IAuditableEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public List<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

        public List<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

        public List<UserToken> UserTokens { get; set; } = new List<UserToken>();

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public static IEnumerable<object> Firstordefault(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class UserGetDto : UserDto
    {
        public int Id { get; set; }
    }

    public class UserDto
    {
        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Role { get; set; }
    }

    public class UserGetMeDto : UserGetDto
    {
        public int? InstructorId { get; set; }

        public int? StudentId { get; set; }
    }

    public class UserBaseDtoValidator : AbstractValidator<UserDto>
    {
        public UserBaseDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }


    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserGetDto>()
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.Email))
                .ForMember(x => x.Role, opts => opts.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name ?? "ASd"));//TODO FIX THIS DUHHH

            CreateMap<User, UserDto>();
            CreateMap<User, UserGetMeDto>();
            CreateMap<CreateUserRequest, User>();
        }
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        }
    }
}
