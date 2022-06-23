using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Assignments;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class DeleteAssignmentByIdRequest : IRequest<ValidateableResponse<AssignmentGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteAssignmentByIdRequestHandler : IRequestHandler<DeleteAssignmentByIdRequest, ValidateableResponse<AssignmentGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteAssignmentByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<AssignmentGetDto>> Handle(DeleteAssignmentByIdRequest request, CancellationToken tkn)//TODO WILL HAVE TO REMOVE THE RANGE OF ASSIGNMENT TESTS AFTER? SOFT DELETE? PROB
        {
            var entity = _dataContext.Set<Assignment>().FirstOrDefault(x => x.Id == request.Id);

            var dto = _mapper.Map<AssignmentGetDto>(entity);

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync();

            return new ValidateableResponse<AssignmentGetDto>(dto);
        }
    }

    public class DeleteAssignmentByIdRequestValidation : AbstractValidator<DeleteAssignmentByIdRequest>
    {
        private readonly DataContext _dataContext;

        public DeleteAssignmentByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Id)
                .EntityMustExist<DeleteAssignmentByIdRequest, Assignment>(_dataContext);
        }
    }
}
