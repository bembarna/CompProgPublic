using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestCases
{
    public class DeleteMethodTestCaseByIdRequest : IRequest<ValidateableResponse<MethodTestCaseGetDto>>, IValidateable
    {
        public DeleteMethodTestCaseByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class DeleteMethodTestCaseByIdRequestHandler : IRequestHandler<DeleteMethodTestCaseByIdRequest, ValidateableResponse<MethodTestCaseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteMethodTestCaseByIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<MethodTestCaseGetDto>> Handle(DeleteMethodTestCaseByIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = await _dataContext.Set<MethodTestCase>().FirstAsync(x => x.Id == request.Id, tkn);

            _dataContext.Remove(desiredOutput);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<MethodTestCaseGetDto>(desiredOutput);
            return new ValidateableResponse<MethodTestCaseGetDto>(dto);
        }
    }

    public class DeleteMethodTestCaseByIdRequestValidation : AbstractValidator<DeleteMethodTestCaseByIdRequest>
    {
        private readonly DataContext _dataContext;
        public DeleteMethodTestCaseByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
   
