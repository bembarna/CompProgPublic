using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompProgEdu.Core.Features.Extensions;

namespace CompProgEdu.Core.Features.Users
{
    public class DeleteUserByIdRequest : IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteUserByIdRequestHandler : IRequestHandler<DeleteUserByIdRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public DeleteUserByIdRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(DeleteUserByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<User>().FirstOrDefault(x => x.Id == request.Id);

            var dto = _mapper.Map<UserGetDto>(entity);

            await _userManager.DeleteAsync(entity);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class DeleteUserByIdRequestValidation : AbstractValidator<DeleteUserByIdRequest>
    {
        private readonly DataContext _dataContext;

        public DeleteUserByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<DeleteUserByIdRequest, User>(_dataContext);
        }
    }
}
