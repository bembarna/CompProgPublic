using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.TestEntities;
using CompProgEdu.Features.TestEntities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompProgEdu.Features.Auth;
using CompProgEdu.Core.Security;

namespace CompProgEdu.Features.Controllers
{
    [RoleAuthorization(Roles.GlobalAdmin)]
    [Route("api/test-account")]
    public class TestAccountController : StandardApiController
    {
        public TestAccountController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<TestAccountGetDto>>> Create(
            [FromBody] CreateTestAccountRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<ValidateableResponse<List<TestAccountGetDto>>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllTestAccountsRequest());

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<TestAccountGetDto>>> GetById(
          int id)
        {
            var result = await Mediator.Send(new GetTestAccountByIdRequest { Id = id });

            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ValidateableResponse<TestAccountGetDto>>> Update(//Yeah this is messy, but it must be done this way until we find a better way, "Id" shouldnt be in FromBody, but it needs to be in the UpdateRequest
         [FromBody] TestAccountDto request, int id)
        {
            var result = await Mediator.Send(new UpdateTestAccountByIdRequest {
                Id = id,
                AccountNumber = request.AccountNumber,
                AccountName = request.AccountName,
                EmailAddress = request.EmailAddress,
                LastVisit = request.LastVisit,
                IsPremium = request.IsPremium,
                NumberOfPeople = request.NumberOfPeople
            });

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<TestAccountGetDto>>> Delete(
         int id)
        {
            var result = await Mediator.Send(new DeleteTestAccountByIdRequest { Id = id });

            return result;
        }
    }
}
