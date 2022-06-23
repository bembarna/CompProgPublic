using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.Users
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public User User { get; set; }
    }

    public class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserLogins)
                .HasForeignKey(x => x.UserId);
        }
    }
}
