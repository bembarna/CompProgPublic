using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class GetDesiredOutputByIdRequest : IRequest<ValidateableResponse<DesiredOutputGetDto>>, IValidateable
    {
        public GetDesiredOutputByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class GetDesiredOutputByIdRequestHandler : IRequestHandler<GetDesiredOutputByIdRequest, ValidateableResponse<DesiredOutputGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetDesiredOutputByIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<DesiredOutputGetDto>> Handle(GetDesiredOutputByIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = _dataContext.Set<DesiredOutput>().First(x => x.Id == request.Id);
            var dto = _mapper.Map<DesiredOutputGetDto>(desiredOutput);
            return new ValidateableResponse<DesiredOutputGetDto>(dto);
        }
    }

    public class GetDesiredOutputByIdRequestValidation : AbstractValidator<GetDesiredOutputByIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetDesiredOutputByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}