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

namespace CompProgEdu.Core.Features.Students
{
    public class GetStudentByIdRequest : StudentGetDto, IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
    }

    public class GetStudentRequestHandler : IRequestHandler<GetStudentByIdRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetStudentRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<StudentResponseDto>> Handle(GetStudentByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<Student>().Include(x => x.User).FirstOrDefaultAsync(x => x.Id == request.Id, tkn);

            var dto = _mapper.Map<StudentResponseDto>(entity);

            return new ValidateableResponse<StudentResponseDto>(dto);
        }
    }

    public class GetStudentByIdRequestValidation : AbstractValidator<GetStudentByIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetStudentByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;


            RuleFor(x => x.Id)
                .EntityMustExist<GetStudentByIdRequest, Student>(_dataContext);
        }
    }
}
