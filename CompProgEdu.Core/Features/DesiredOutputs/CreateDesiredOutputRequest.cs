using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class CreateDesiredOutputRequest : DesiredOutputDto, IRequest<ValidateableResponse<DesiredOutputGetDto>>, IValidateable
    {
    }

    public class CreateDesiredOutputRequestHandler : IRequestHandler<CreateDesiredOutputRequest, ValidateableResponse<DesiredOutputGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CreateDesiredOutputRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<DesiredOutputGetDto>> Handle(CreateDesiredOutputRequest request, CancellationToken tkn)
        {
            var desiredOutput = _mapper.Map<DesiredOutput>(request);

            await _dataContext.AddAsync(desiredOutput, tkn);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<DesiredOutputGetDto>(desiredOutput);
            return new ValidateableResponse<DesiredOutputGetDto>(dto);
        }
    }

    public class CreateDesiredOutputRequestValidation : AbstractValidator<CreateDesiredOutputRequest>
    {
        public CreateDesiredOutputRequestValidation(IValidator<DesiredOutputDto> mainDtoValidator)
        {
            Include(mainDtoValidator);
        }
    }
}
