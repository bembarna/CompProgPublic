using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class GetAllStudentsRequest : IRequest<ValidateableResponse<List<StudentGetAllResponseDto>>>, IValidateable
    {
    }

    public class GetAllStudentsRequestHandler : IRequestHandler<GetAllStudentsRequest, ValidateableResponse<List<StudentGetAllResponseDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllStudentsRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<StudentGetAllResponseDto>>> Handle(GetAllStudentsRequest request, CancellationToken cancellationToken)
        {
            var students = await _dataContext.Set<Student>().Include(x => x.User).ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<StudentGetAllResponseDto>>(students);

            return new ValidateableResponse<List<StudentGetAllResponseDto>>(dtos);
        }
    }

    public class GetAllStudentsRequestValidation : AbstractValidator<GetAllStudentsRequest>
    {
        public GetAllStudentsRequestValidation()
        {

        }
    }
}
