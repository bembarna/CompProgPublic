using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MediatR;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Interfaces;
using System.Threading;
using FluentValidation;
using Newtonsoft.Json.Linq;
using CompProgEdu.Core.Features.Request;
using System.Linq;
using System.Collections.Generic;
using CompProgEdu.Core.Features.JDoodles;

namespace CompProgEdu.Core.Features.Requests
{
    public class SendToJDoodleApiRequest : JDoodleResponse, IRequest<ValidateableResponse<JDoodleRequest>>, IValidateable
    {
        public List<string> Inputs { get; set; } = new List<string>();
    }
    public class SendToJDoodleApiRequestHandler : IRequestHandler<SendToJDoodleApiRequest, ValidateableResponse<JDoodleRequest>>
    {
        private readonly IConfiguration _config;

        private string ClientId;

        private string ClientSecret;

        private string UrlEndpoint;

        private readonly string NewLineKey = "-insertNewLine";//TODO change this to be something actually secret

        public SendToJDoodleApiRequestHandler(
            IConfiguration config)
        {
            
            _config = config;
            InitializeIdAndSecretForRequestBody();
        }

        public async Task<ValidateableResponse<JDoodleRequest>> Handle(SendToJDoodleApiRequest request, CancellationToken tkn)
        {
            var httpClient = new HttpClient();
            var json = GenerateJsonRequestBody(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(UrlEndpoint, httpContent);
            var httpResponseChildrenList = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result).Children().ToList();

            var errorList = new List<ErrorResponse>();

            if (httpResponseChildrenList.Count == 2)
            {
                errorList.Add(new ErrorResponse { FieldName = "Error", Error = (string)httpResponseChildrenList[0] });
                errorList.Add(new ErrorResponse { FieldName = "StatusCode", Error = (string)httpResponseChildrenList[1] });

                return new ValidateableResponse<JDoodleRequest>(errorList);
            }

            var jdoodleRequestResponse = new JDoodleRequest
            {
                output = (string)httpResponseChildrenList[0],
                statusCode = (string)httpResponseChildrenList[1],
                memory = (string)httpResponseChildrenList[2],
                cpuTime = (string)httpResponseChildrenList[3]
            };

            return new ValidateableResponse<JDoodleRequest>(jdoodleRequestResponse);
        }

        private string GenerateJsonRequestBody(SendToJDoodleApiRequest request)
        {
            string stdin = string.Empty;
            if (!request.Inputs.Any())
            {
                stdin = string.Empty;
            }

            foreach(var input in request.Inputs)
            {
                if(!string.IsNullOrEmpty(stdin))
                {
                    stdin = stdin + NewLineKey + input;
                }
                else
                {
                    stdin = input;
                }
            }

            stdin = stdin.Replace(NewLineKey, System.Environment.NewLine);

            var jDoodleRequestBody = new JDoodleRequestBody
            {
                clientId = ClientId,
                clientSecret = ClientSecret,
                stdin = stdin,
                script = request.script,
                language = request.language,
                versionIndex = request.versionIndex
            };
        
            return JsonConvert.SerializeObject(jDoodleRequestBody);
        }

        private void InitializeIdAndSecretForRequestBody()
        {
            ClientId = _config.GetValue<string>("JdoodleClientInformation:ClientId");
            ClientSecret = _config.GetValue<string>("JdoodleClientInformation:ClientSecret");
            UrlEndpoint = _config.GetValue<string>("JdoodleClientInformation:ExecuteEndpoint");
        }

        private class JDoodleRequestBody
        {
            public string clientId { get; set; }
            public string clientSecret { get; set; }
            public string script { get; set; }
            public string stdin { get; set; }
            public string language { get; set; }
            public string versionIndex { get; set; }
        }

        public class SendToJDoodleApiRequestValidation : AbstractValidator<SendToJDoodleApiRequest>
        {
            private readonly string NewLineKey = "-insertNewLine";

            public SendToJDoodleApiRequestValidation()
            {
                RuleFor(x => x.language)
                    .NotEmpty()
                    .Must(MustBeLanguageInList)
                    .WithMessage("Language used not supported!");

                RuleFor(x => x.script)
                    .NotEmpty();

                RuleFor(x => x.versionIndex)
                    .NotEmpty()
                    .Must(MustBeVersionInList)
                    .WithMessage("Version Index used not supported!");

                RuleFor(x => x.Inputs)
                    .Must(MustNotContainNewLineKey);
            }

            bool MustBeLanguageInList(string language)
            {
                return JDoodleLangList.JDoodleLanguageVersions.Any(x => x.Language == language);
            }

            bool MustBeVersionInList(SendToJDoodleApiRequest request, string version)
            {
                var languageList = JDoodleLangList.JDoodleLanguageVersions.FirstOrDefault(x => x.Language == request.language);
                if(languageList == null)
                {
                    return false;
                }

                return languageList.Versions.Any(x => x == version);
            }

            bool MustNotContainNewLineKey(SendToJDoodleApiRequest request, List<string> inputs)
            {
                return !inputs.Any(x => x == NewLineKey);
            }
        }
    }
}
