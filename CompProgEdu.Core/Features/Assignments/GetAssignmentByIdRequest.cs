using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class GetAssignmentByIdRequest : IRequest<ValidateableResponse<AssignmentDetailDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetAssignmentByIdRequestHandler : IRequestHandler<GetAssignmentByIdRequest, ValidateableResponse<AssignmentDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAssignmentByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<AssignmentDetailDto>> Handle(GetAssignmentByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<Assignment>()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Course)
                .Include(x => x.DesiredOutputs)
                .Include(x => x.MethodTestCases)
                .FirstOrDefaultAsync(tkn);

            var dto = _mapper.Map<AssignmentDetailDto>(entity);

            return new ValidateableResponse<AssignmentDetailDto>(dto);
        }
    }

    public class GetAssignmentByIdRequestValidation : AbstractValidator<GetAssignmentByIdRequest>
    {
        private readonly DataContext _dataContext;

        public GetAssignmentByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetAssignmentByIdRequest, Assignment>(_dataContext);
        }
    }
}
