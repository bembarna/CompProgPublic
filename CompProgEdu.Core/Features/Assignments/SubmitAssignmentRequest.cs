using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.DesiredOutputs;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Requests;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.TestCases;
using FluentValidation;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class SubmitAssignmentRequest : IRequest<ValidateableResponse<SubmissionResult>>, IValidateable
    {
        public SubmitAssignmentRequest(string submission, int assignmentId, string fileName, string? language)
        {
            Submission = submission;
            AssignmentId = assignmentId;
            FileName = fileName;
            Language = language;
        }
        public string Submission { get; set; }
        public int AssignmentId { get; set; }
        public string FileName { get; set; }
        public string? Language { get; set; }
    }

    public class SubmitAssignmentRequestHandler : IRequestHandler<SubmitAssignmentRequest, ValidateableResponse<SubmissionResult>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public string InjectTestLib = "public static class InputTester{public static bool TestVariableValue<T>(T variableToTest, T variable, System.Type conversionType)where T : class{var variableToTestConverted = System.Convert.ChangeType(variableToTest, conversionType);var variableConverted = System.Convert.ChangeType(variable, conversionType);return variableToTestConverted.Equals(variableConverted);}}";
        public CurlySet MainCurly;
        public SubmitAssignmentRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<SubmissionResult>> Handle(SubmitAssignmentRequest request, CancellationToken tkn)
        { 
            var desiredOutputs = await _dataContext.Set<DesiredOutput>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .OrderBy(x => x.Order)
                .ToListAsync(tkn);

            var methodTests = await _dataContext.Set<MethodTestCase>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);

            var allowedLanguages = _dataContext.Set<Assignment>()
                .Where(x => x.Id == request.AssignmentId)
                .Select(x => x.AllowedLanguages)
                .First()
                ?.Split(", ");

            if (allowedLanguages.IsNullOrEmpty())
            {
                return new ValidateableResponse<SubmissionResult>("Invalid language. Please check the allowed languages to see what can be used.", "");
            }

            var submissionResult = new SubmissionResult();

            var language = string.Empty;

            if (request.FileName.IsNullOrEmpty())
            {
                language = request.Language;
            }
            else
            {
                if (request.FileName.EndsWith(".cs") && allowedLanguages!.Contains("C#"))
                {
                    language = "csharp";
                }

                if (request.FileName.EndsWith(".java") && allowedLanguages!.Contains("Java"))
                {
                    language = "java";
                }
            }

            if (language == string.Empty)
            {
                return new ValidateableResponse<SubmissionResult>("Invalid language. Please check the allowed languages to see what can be used.", "");
            }

            foreach (var desiredOutput in desiredOutputs)
            {
                var inputs = desiredOutput.Input.Split(' ').ToList();
                var response = await _mediator.Send(new SendToJDoodleApiRequest
                {
                    Inputs = inputs,
                    language = language,
                    script = request.Submission,
                    versionIndex = "0"
                }, tkn);

                if (response.Result.output.Trim() == desiredOutput.Output)
                {
                    submissionResult.TotalScore += desiredOutput.PointValue;
                    submissionResult.OutputChecks.Add(true);
                }
                else
                {
                    submissionResult.OutputChecks.Add(false);
                }
            }

            if (methodTests.Any())
            {
                var curlySet = CodeBuilder.BuildCode(request.Submission, request.AssignmentId);
                var topCurlySetOpen = curlySet.OpenCurlyPositionInString;
                FindMainCurly(curlySet);
                foreach (var test in methodTests)
                {
                    var Trial1 = "Console.Write(InputTester.TestVariableValue({INPUT},(object){OUTPUT},typeof({TYPE})));return;";
                    Trial1 = Trial1.Replace("{INPUT}", test.MethodTestInjectable);
                    Trial1 = Trial1.Replace("{OUTPUT}", test.Output);
                    Trial1 = Trial1.Replace("{TYPE}", test.ReturnType);
                    var submissionToTest = Regex.Replace(request.Submission, @"\t|\n|\r", "");
                    submissionToTest = submissionToTest.Insert(MainCurly.OpenCurlyPositionInString + 1, Trial1);
                    submissionToTest = submissionToTest.Insert(submissionToTest.Length, InjectTestLib);
                    var response = await _mediator.Send(new SendToJDoodleApiRequest
                    {
                        language = language,
                        script = submissionToTest,
                        versionIndex = "0"
                    }, tkn);
                    if (response.Result.output.Contains("True"))
                    {
                        submissionResult.TotalScore += test.PointValue;
                        submissionResult.MethodTestChecks.Add(new MethodTestCheckDto
                        {
                            Passed = true,
                            Hint = test.Hint
                        });
                    }
                    else
                    {
                        submissionResult.MethodTestChecks.Add(new MethodTestCheckDto
                        {
                            Passed = false,
                            Hint = test.Hint
                        });
                    }
                    var check = submissionToTest;
                }
            }

            return new ValidateableResponse<SubmissionResult>(submissionResult);
        }

        public void FindMainCurly(CurlySet curlySet)
        {
            if (curlySet != null)
            {
                if (!curlySet.IsMain)
                {
                    var childCurly = curlySet.CurlySets.FirstOrDefault();
                    if (childCurly == null)
                    {
                        var parentCurly = curlySet.ParentCurlySet;
                        if (parentCurly == null)
                        {
                            MainCurly = null;
                        }
                        parentCurly.CurlySets.Remove(curlySet);
                        var test1 = parentCurly;
                        FindMainCurly(parentCurly);
                    }
                    FindMainCurly(childCurly);
                }
                else
                {
                    MainCurly = curlySet;
                }
            }
        }
    }

    public class SubmitAssignmentRequestValidation : AbstractValidator<SubmitAssignmentRequest>
    {
        public SubmitAssignmentRequestValidation()
        {
        }
    }
}

   

