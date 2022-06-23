using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class UpdateDesiredOutputOrderRequest : List<DesiredOutputGetDto>, IRequest<ValidateableResponse<List<DesiredOutputGetDto>>>, IValidateable
    {
    }

    public class UpdateDesiredOutputOrderRequestHandler : IRequestHandler<UpdateDesiredOutputOrderRequest, ValidateableResponse<List<DesiredOutputGetDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateDesiredOutputOrderRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<DesiredOutputGetDto>>> Handle(UpdateDesiredOutputOrderRequest request, CancellationToken tkn)
        {
            var assignmentId = request.First().AssignmentId;
            var desiredOutputs = await _dataContext.Set<DesiredOutput>().Where(x => x.AssignmentId == assignmentId)
                .ToListAsync(tkn);

            foreach (var desiredOutput in desiredOutputs)
            {
                desiredOutput.Order = request.Where(x => x.Id == desiredOutput.Id).Select(x => x.Order).First();
            }

            _dataContext.UpdateRange(desiredOutputs);
            await _dataContext.SaveChangesAsync(tkn);

            return new ValidateableResponse<List<DesiredOutputGetDto>>(request);
        }
    }

    public class UpdateDesiredOutputOrderRequestValidation : AbstractValidator<UpdateDesiredOutputOrderRequest>
    {
        private readonly DataContext _dataContext;

        public UpdateDesiredOutputOrderRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
