using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Users
{
    public class GetUserByIdRequest : IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserByIdRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(GetUserByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<User>()
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: tkn);

            var dto = _mapper.Map<UserGetDto>(entity);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class GetUserByIdRequestValidation : AbstractValidator<GetUserByIdRequest>
    {
        private readonly DataContext _dataContext;

        public GetUserByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetUserByIdRequest, User>(_dataContext);
        }
    }
}
