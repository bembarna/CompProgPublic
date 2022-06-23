using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class UpdateDesiredOutputRequest : DesiredOutputGetDto, IRequest<ValidateableResponse<DesiredOutputGetDto>>, IValidateable
    {
    }

    public class UpdateDesiredOutputRequestHandler : IRequestHandler<UpdateDesiredOutputRequest, ValidateableResponse<DesiredOutputGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateDesiredOutputRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<DesiredOutputGetDto>> Handle(UpdateDesiredOutputRequest request, CancellationToken tkn)
        {
            var desiredOutput = await _dataContext.Set<DesiredOutput>().FirstAsync(x => x.Id == request.Id, tkn);

            desiredOutput.Input = request.Input;
            desiredOutput.Output = request.Output;
            desiredOutput.PointValue = request.PointValue;

            _dataContext.Update(desiredOutput);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<DesiredOutputGetDto>(desiredOutput);

            return new ValidateableResponse<DesiredOutputGetDto>(dto);
        }
    }

    public class UpdateDesiredOutputRequestValidation : AbstractValidator<UpdateDesiredOutputRequest>
    {
        private readonly DataContext _dataContext;

        public UpdateDesiredOutputRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(x => x.Input)
                .NotEmpty();

            RuleFor(x => x.Output)
                .NotEmpty();

            RuleFor(x => x.PointValue)
                .NotNull();
        }
    }
}
