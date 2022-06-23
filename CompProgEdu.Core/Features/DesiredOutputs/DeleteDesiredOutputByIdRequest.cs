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
    public class DeleteDesiredOutputByIdRequest : IRequest<ValidateableResponse<DesiredOutputGetDto>>, IValidateable
    {
        public DeleteDesiredOutputByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class DeleteDesiredOutputByIdRequestHandler : IRequestHandler<DeleteDesiredOutputByIdRequest, ValidateableResponse<DesiredOutputGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteDesiredOutputByIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<DesiredOutputGetDto>> Handle(DeleteDesiredOutputByIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = await _dataContext.Set<DesiredOutput>().FirstAsync(x => x.Id == request.Id, tkn);

            _dataContext.Remove(desiredOutput);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<DesiredOutputGetDto>(desiredOutput);
            return new ValidateableResponse<DesiredOutputGetDto>(dto);
        }
    }

    public class DeleteDesiredOutputByIdRequestValidation : AbstractValidator<DeleteDesiredOutputByIdRequest>
    {
        private readonly DataContext _dataContext;
        public DeleteDesiredOutputByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}