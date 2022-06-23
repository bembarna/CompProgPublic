using AutoMapper;
using CompProgEdu.Core.Features.Assignments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class DesiredOutput : DesiredOutputGetDto, IAuditableEntity
    {
        public Assignment Assignment { get; set; }
    }

    public class DesiredOutputGetDto : DesiredOutputDto
    {
        public int Id { get; set; }
    }

    public class DesiredOutputDto
    {
        public int AssignmentId { get; set; }

        [Required]
        public string Input { get; set; }

        [Required]
        public string Output { get; set; }

        public int PointValue { get; set; }

        public int Order { get; set; }
    }

    public class DesiredOutputBaseDtoValidator : AbstractValidator<DesiredOutputDto>
    {
        public DesiredOutputBaseDtoValidator()
        {
            RuleFor(x => x.Input)
                .NotEmpty();

            RuleFor(x => x.Output)
                .NotEmpty();

            RuleFor(x => x.PointValue)
                .NotNull();
        }
    }

    public class DesiredOutputMapping : Profile
    {
        public DesiredOutputMapping()
        {
            CreateMap<DesiredOutput, DesiredOutputDto>().ReverseMap();
            CreateMap<DesiredOutput, DesiredOutputGetDto>();
        }
    }

    public class DesiredOutputEntityConfiguration : IEntityTypeConfiguration<DesiredOutput>
    {
        public void Configure(EntityTypeBuilder<DesiredOutput> builder)
        {
            builder.ToTable("DesiredOutputs", "domain");
            builder.HasOne(x => x.Assignment).WithMany(y => y.DesiredOutputs);
        }
    }
}
