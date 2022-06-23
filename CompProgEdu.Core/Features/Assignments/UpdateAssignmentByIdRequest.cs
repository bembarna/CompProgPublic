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
    public class UpdateAssignmentByIdRequest : AssignmentGetDto, IRequest<ValidateableResponse<AssignmentDetailDto>>, IValidateable
    {
    }

    public class UpdateAssignmentByIdRequestHandler : IRequestHandler<UpdateAssignmentByIdRequest, ValidateableResponse<AssignmentDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateAssignmentByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<AssignmentDetailDto>> Handle(UpdateAssignmentByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Assignment>().Include(x => x.Course).First(x => x.Id == request.Id);

            _mapper.Map(request, entity);
            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<AssignmentDetailDto>(entity);

            return new ValidateableResponse<AssignmentDetailDto>(dto);
        }

    }

    public class UpdateAssignmentByIdRequestValidation : AbstractValidator<UpdateAssignmentByIdRequest>
    {
        private readonly DataContext _dataContext;
        public UpdateAssignmentByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<UpdateAssignmentByIdRequest, Assignment>(_dataContext);

            RuleFor(x => x.AssignmentName)
                .NotEmpty();

            RuleFor(x => x.TotalPointValue)
                .NotEmpty();

            RuleFor(x => x.AllowedLanguages)
                .NotEmpty();

            RuleFor(x => x.AssignmentInstructions)
                .NotEmpty();

            RuleFor(x => x.ExampleInput)
                .NotEmpty();

            RuleFor(x => x.ExampleOutput)
                .NotEmpty();
        }
    }
}
