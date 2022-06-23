using AutoMapper;
using CompProgEdu.Features.TestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CompProgEdu.Core.Features.TestEntities
{
    public class TestAccount : TestAccountGetDto, IAuditableEntity
    {
    }

    public class TestAccountGetDto : TestAccountDto
    {
        public int Id { get; set; }
    }

    public class TestAccountDto
    {
        public string AccountNumber { get; set; }
       
        public string AccountName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime LastVisit { get; set; }

        public bool IsPremium { get; set; }

        public int NumberOfPeople { get; set; }
    }

    public class TestAccountMapping : Profile
    {
        public TestAccountMapping()
        {
            CreateMap<TestAccount, TestAccountGetDto>();

            CreateMap<TestAccount, TestAccountDto>();

            CreateMap<UpdateTestAccountByIdRequest, TestAccount>();

            CreateMap<CreateTestAccountRequest, TestAccount>();
        }
    }

    public class TestAccountEntityConfiguration : IEntityTypeConfiguration<TestAccount>
    {
        public void Configure(EntityTypeBuilder<TestAccount> builder)
        {
            builder.ToTable("TestAccounts", "domain");
        }
    }
}
