using AutoMapper;
using CompProgEdu.Core.Features.Assignments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace CompProgEdu.Core.Features.TestCases
{
    public class MethodTestCase : MethodTestCaseGetDto, IAuditableEntity
    {
        public Assignment Assignment { get; set; }
        public string Input { get; internal set; }
    }
    public class MethodTestCaseGetDto : MethodTestCaseDto
    {
        public int Id { get; set; }
        public string ReturnType { get; set; }
        public string MethodTestInjectable { get; set; }
    }

    public class MethodTestCaseDto
    {
        public int AssignmentId { get; set; }

        [Required]
        public string ParamInputs { get; set; }

        [Required]
        public string Output { get; set; }

        public string Hint { get; set; }

        public int PointValue { get; set; }
    }

    public class MethodTestCaseDtoValidator : AbstractValidator<MethodTestCaseDto>
    {
        public MethodTestCaseDtoValidator()
        {
            RuleFor(x => x.ParamInputs)
                .NotEmpty();

            RuleFor(x => x.Output)
                .NotEmpty();

            RuleFor(x => x.PointValue)
                .NotNull();
        }
    }

    public class MethodTestCaseMapping : Profile
    {
        public MethodTestCaseMapping()
        {
            CreateMap<MethodTestCase, MethodTestCaseDto>().ReverseMap();
            CreateMap<MethodTestCase, MethodTestCaseGetDto>();
        }
    }

    public class MethodTestCaseEntityConfiguration : IEntityTypeConfiguration<MethodTestCase>
    {
        public void Configure(EntityTypeBuilder<MethodTestCase> builder)
        {
            builder.ToTable("MethodTestCases", "domain");
            builder.HasOne(x => x.Assignment).WithMany(y => y.MethodTestCases);
        }
    }
}
