using AutoMapper;
using CompProgEdu.Core.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace CompProgEdu.Core.Features.Students
{
    public class Student : StudentGetDto, IAuditableEntity
    {
        public User User { get; set; }

        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }

    public class StudentGetDto : StudentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Grade { get; set; }
    }

    public class StudentDto
    {
        public string StudentSchoolNumber { get; set; }
        public bool ChangedPassword { get; set; }
    }


    public class StudentResponseDto : StudentDetailDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
    }

    public class StudentDetailDto : StudentDto
    {

        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class StudentUpdateDto : StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class StudentGetAllResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Grade { get; set; }
        public string StudentSchoolNumber { get; set; }
    }

    public class StudentMapping : Profile
    {
        public StudentMapping()
        {
            CreateMap<Student, StudentGetDto>();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<CreateStudentRequest, Student>();
            CreateMap<UpdateStudentRequest, Student>().ReverseMap();
            CreateMap<Student, StudentDetailDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email))
                .ReverseMap();

            CreateMap<Student, StudentResponseDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email));

            CreateMap<Student, StudentUpdateDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName));


            CreateMap<Student, StudentGetAllResponseDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email))
                .ForMember(x => x.Grade, opts => opts.MapFrom(src => src.Grade.ToString()));
        }
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(x => x.User).WithMany();

            builder.HasMany(x => x.StudentCourses);
        }
    }
}
