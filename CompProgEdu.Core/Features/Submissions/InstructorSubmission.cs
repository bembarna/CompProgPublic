using AutoMapper;
using CompProgEdu.Core.Features.Assignments;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Instructors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.Submissions
{
    public class InstructorSubmission : InstructorSubmissionGetDto, IAuditableEntity
    {
        public Assignment Assignment { get; set; }
        public CurlySet CurlySet { get; set; }
    }

    public class InstructorSubmissionGetDto : InstructorSubmissionDto
    {
        public int Id { get; set; }
    }

    public class InstructorSubmissionDto
    {
        public int CurlySetId { get; set; }
        public int AssignmentId { get; set; }
    }

    public class InstructorSubmissionDetailDto : InstructorSubmissionGetDto
    {
        public CurlySetDetailDto CurlySet { get; set; }
    }

    public class InstructorSubmissionMapping : Profile
    {
        public InstructorSubmissionMapping()
        {
            CreateMap<InstructorSubmission, InstructorSubmissionDto>();
            CreateMap<InstructorSubmission, InstructorSubmissionGetDto>();
            CreateMap<InstructorSubmission, InstructorSubmissionDetailDto>();
        }
    }

    public class InstructorSubmissionEntityConfiguration : IEntityTypeConfiguration<InstructorSubmission>
    {
        public void Configure(EntityTypeBuilder<InstructorSubmission> builder)
        {
            builder.ToTable("InstructorSubmissions", "domain");

            builder.HasOne(x => x.CurlySet).WithMany();
            builder.HasOne(x => x.Assignment).WithMany();
        }
    }
}
