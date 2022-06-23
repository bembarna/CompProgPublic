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

namespace CompProgEdu.Core.Features.Instructors
{
    public class GetAllInstructorsRequest : IRequest<ValidateableResponse<List<InstructorGetAllResponseDto>>>, IValidateable
    {
    }

    public class GetAllInstructorsRequestHandler : IRequestHandler<GetAllInstructorsRequest, ValidateableResponse<List<InstructorGetAllResponseDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllInstructorsRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<InstructorGetAllResponseDto>>> Handle(GetAllInstructorsRequest request, CancellationToken cancellationToken)
        {
            var instructors = await _dataContext.Set<Instructor>().Include(x => x.User).ToListAsync();

            var dtos = _mapper.Map<List<InstructorGetAllResponseDto>>(instructors);

            return new ValidateableResponse<List<InstructorGetAllResponseDto>>(dtos);
        }
    }

    public class GetAllInstructorsRequestValidation : AbstractValidator<GetAllInstructorsRequest>
    {
        public GetAllInstructorsRequestValidation()
        {

        }
    }
}
