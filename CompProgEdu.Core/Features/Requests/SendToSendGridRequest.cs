using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace CompProgEdu.Core.Features.Requests
{
    public class SendToSendGridRequest : IRequest<ValidateableResponse<SendGrid.Response>>, IValidateable
    {
        //public string EmailFrom { get; set; }
        //public string PersonFrom { get; set; }
        public string EmailTo { get; set; } //add RuleFor if we're going to keep this as an editable field
        public string PersonTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class SendToSendGridRequestHandler : IRequestHandler<SendToSendGridRequest, ValidateableResponse<SendGrid.Response>>
    //public class SendToSendGridRequestHandler

    {

        private string Client;

        public IConfiguration _configuration { get; set; }

        public SendToSendGridRequestHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeIdAndSecretForRequestBody();

        }

        public async Task<ValidateableResponse<SendGrid.Response>> Handle(SendToSendGridRequest request, CancellationToken tkn)
        {

            var client = new SendGridClient(Client);

            var from = new EmailAddress("compprogedudevs@gmail.com", "Testing"); //not sure if this needs to be an editable field or if it will just be based on instructor
            var subject = request.Subject;
            var to = new EmailAddress(request.EmailTo, request.PersonTo);
            var plainTextContent = request.Message;
            var htmlContent = "<strong>" + request.Message + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return new ValidateableResponse<SendGrid.Response>(response);
        }
        private void InitializeIdAndSecretForRequestBody()
        { 
            Client = _configuration.GetValue<string>("SendGridInfo:Client");
        }
    }
    public class SendtoSendGridRequestValidation : FluentValidation.AbstractValidator<SendToSendGridRequest>
    {
        public SendtoSendGridRequestValidation()
        {

        }
    }
}

