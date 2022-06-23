using AutoMapper;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.DesiredOutputs;
using CompProgEdu.Core.Features.Submissions;
using CompProgEdu.Core.Features.TestCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompProgEdu.Core.Features.Assignments
{
    public class Assignment : AssignmentGetDto, IAuditableEntity
    {
        public Course Course { get; set; }
        public List<DesiredOutput> DesiredOutputs { get; set; }
        public List<MethodTestCase> MethodTestCases { get; set; }
    }
    public class AssignmentGetDto : AssignmentDto
    {
        public int Id { get; set; }
        //public int InstructorSubmissionId { get; set; }
    }
    public class AssignmentDto
    {
        [Required]
        public string AssignmentName { get; set; }

        [Required]
        public string AllowedLanguages { get; set; }

        [Required]
        public string AssignmentInstructions { get; set; }

        [Required] 
        public string ExampleInput { get; set; }

        [Required]
        public string ExampleOutput { get; set; }

        [Required]
        public int TotalPointValue { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? VisibilityDate { get; set; }

        public string AssignmentSolutionFileName { get; set; }

        public int CourseId { get; set; }
    }

    public class AssignmentDetailDto : AssignmentGetDto
    {
        public CourseGetDto CourseDto { get; set; }
        public List<DesiredOutput> DesiredOutputs { get; set; }
        public List<MethodTestCase> MethodTestCases { get; set; }
        public int TotalPointsAssigned { get; set; }
    }

    public class AssignmentSummaryDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string AssignmentName { get; set; }

        public string AssignmentInstructions { get; set; }

        public int TotalPointValue { get; set; }

        public string AssignmentSolution { get; set; }

        public string DueDate { get; set; }

        public string VisibilityDate { get; set; }
    }


    public class AssignmentMapping : Profile
    {
        public AssignmentMapping()
        {
            CreateMap<Assignment, AssignmentGetDto>();
            CreateMap<Assignment, AssignmentDto>();
            CreateMap<Assignment, AssignmentSummaryDto>();
            CreateMap<Assignment, AssignmentDetailDto>()
                .ForMember(x => x.CourseDto, opts => opts.MapFrom(y => y.Course))
                .ForMember(x => x.TotalPointsAssigned, opts => opts.MapFrom(x => x.DesiredOutputs.Sum(y => y.PointValue) + x.MethodTestCases.Sum(y => y.PointValue)));
            //TODO: Add test cases to this once the entity is created.

            CreateMap<UpdateAssignmentByIdRequest, Assignment>();
            CreateMap<CreateAssignmentRequest, Assignment>();
        }
    }

    public class AssignmentEntityConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments", "domain");
            builder.HasOne(x => x.Course).WithMany();
            builder.HasMany(x => x.DesiredOutputs).WithOne(y => y.Assignment);
            builder.HasMany(x => x.MethodTestCases).WithOne(y => y.Assignment);
        }
    }

}
