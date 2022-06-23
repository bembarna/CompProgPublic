using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Instructors
{
    public class DeleteInstructorRequest : InstructorDetailDto, IRequest<ValidateableResponse<InstructorResponseDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteInstructorRequestHandler : IRequestHandler<DeleteInstructorRequest, ValidateableResponse<InstructorResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public DeleteInstructorRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ValidateableResponse<InstructorResponseDto>> Handle(DeleteInstructorRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Instructor>().Include(x => x.User).FirstOrDefault(x => x.Id == request.Id);

            var dto = _mapper.Map<InstructorResponseDto>(entity);

            _dataContext.Remove(entity);

            await _dataContext.SaveChangesAsync();

            await _userManager.DeleteAsync(entity.User);

            return new ValidateableResponse<InstructorResponseDto>(dto);
        }
    }

    public class DeleteInstructorRequestValidation : AbstractValidator<DeleteInstructorRequest>
    {
        private readonly DataContext _dataContext;

        public DeleteInstructorRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<DeleteInstructorRequest, Instructor>(_dataContext);
        }
    }
}
