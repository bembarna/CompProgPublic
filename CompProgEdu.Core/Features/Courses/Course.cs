using AutoMapper;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Students;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompProgEdu.Core.Features.Courses
{
    public class Course : CourseGetDto, IAuditableEntity
    {
        public Instructor Instructor { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }
    }

    public class CourseGetDto : CourseDto
    {
        public int Id { get; set; }
    }

    public class CourseDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Section { get; set; }

        public int InstructorId { get; set; }
    }

    public class CourseSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Section { get; set; }

        public int StudentCount { get; set; }

        public string InstructorName { get; set; }
    }

    public class CourseDetailDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Section { get; set; }

        public string InstructorName { get; set; }
    }

    public class UserBaseDtoValidator : AbstractValidator<CourseDto>
    {
        public UserBaseDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(80)
                .NotEmpty();

            RuleFor(x => x.Section)
                .MaximumLength(10)
                .NotEmpty();
        }
    }

    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<Course, CourseGetDto>();
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CourseSummaryDto>()
                .ForMember(x => x.StudentCount, opts => opts.MapFrom(y => y.StudentCourses.Count))
                .ForMember(x => x.InstructorName, opts => opts.MapFrom(y => y.Instructor.Title + " " + y.Instructor.User.FirstName + " " + y.Instructor.User.LastName));
            CreateMap<Course, CourseDetailDto>()
                .ForMember(x => x.InstructorName, opts => opts.MapFrom(y => y.Instructor.Title + " " + y.Instructor.User.FirstName + " " + y.Instructor.User.LastName)); ;
        }
    }

    public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "domain");
            builder.HasOne(x => x.Instructor).WithMany();
            builder.HasMany(x => x.StudentCourses);
        }
    }
}