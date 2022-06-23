using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class GetCurlyChildByIdRequest : IRequest<ValidateableResponse<CurlySetDetailDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetCurlyChildByIdRequestHandler : IRequestHandler<GetCurlyChildByIdRequest, ValidateableResponse<CurlySetDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetCurlyChildByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<CurlySetDetailDto>> Handle(GetCurlyChildByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<CurlySet>()
                .Where(x => x.Id == request.Id)
                .Include(x => x.ClassSignature)
                .Include(x => x.MethodSignature)
                .Include(x => x.StatementSignature)
                .Include(x => x.PrimitiveVariables)
                .Include(x => x.CurlySets)
                .FirstOrDefaultAsync();

            var dto = _mapper.Map<CurlySetDetailDto>(entity);

            return new ValidateableResponse<CurlySetDetailDto>(dto);
        }
    }

    public class GetCurlyChildByIdRequestValidation : AbstractValidator<GetCurlyChildByIdRequest>
    {
        private readonly DataContext _dataContext;

        public GetCurlyChildByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetCurlyChildByIdRequest, CurlySet>(_dataContext);
        }
    }
}
