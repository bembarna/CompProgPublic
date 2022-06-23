using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Instructors
{
    public class GetInstructorByIdRequest : InstructorGetDto, IRequest<ValidateableResponse<InstructorResponseDto>>, IValidateable
    {
    }

    public class GetInstructorRequestHandler : IRequestHandler<GetInstructorByIdRequest, ValidateableResponse<InstructorResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetInstructorRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<InstructorResponseDto>> Handle(GetInstructorByIdRequest request, CancellationToken tkn)
        {

            var entity = await _dataContext.Set<Instructor>().Include(x => x.User).FirstOrDefaultAsync(x => x.Id == request.Id, tkn);

            var dto = _mapper.Map<InstructorResponseDto>(entity);

            return new ValidateableResponse<InstructorResponseDto>(dto);
        }
    }

    public class GetInstructorByIdRequestValidation : AbstractValidator<GetInstructorByIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetInstructorByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetInstructorByIdRequest, Instructor>(_dataContext);
        }
    }
}
