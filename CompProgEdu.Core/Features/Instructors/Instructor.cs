using AutoMapper;
using CompProgEdu.Core.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompProgEdu.Core.Features.Instructors
{
    public class Instructor : InstructorGetDto, IAuditableEntity
    {
        public User User { get; set; }
    }

    public class InstructorGetDto : InstructorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class InstructorResponseDto : InstructorDetailDto
    {  
        public int Id { get; set; }

        public int UserId { get; set; }
    }

    public class InstructorDto
    {
        public string Title { get; set; }
        // public string Alias { get; set; } Alias? (Nickname Ex: GDoc)
        // Profile Picture
    }

    public class InstructorDetailDto : InstructorDto
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        //public List<CourseDto> { get; set; }
    }

    public class InstructorGetAllResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Title { get; set; }
    }

    public class InstructorMapping : Profile
    {
        public InstructorMapping()
        {
            CreateMap<Instructor, InstructorGetDto>();

            CreateMap<Instructor, InstructorDto>().ReverseMap();

            CreateMap<CreateInstructorRequest, Instructor>();
            CreateMap<UpdateInstructorRequest, Instructor>().ReverseMap();

            CreateMap<Instructor, InstructorDetailDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email));

            CreateMap<Instructor, InstructorResponseDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email));

            CreateMap<Instructor, InstructorGetAllResponseDto>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(x => x.EmailAddress, opts => opts.MapFrom(src => src.User.Email));
        }
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasOne(x => x.User).WithMany();
        }
    }
}
