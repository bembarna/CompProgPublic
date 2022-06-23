using Antlr4.Runtime;
using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Assignments;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Submissions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CompProgEdu.Core.Features.TestCases;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class BuildCodeBlockRequest :  IRequest<ValidateableResponse<InstructorSubmissionDetailDto>>, IValidateable
    {
        public BuildCodeBlockRequest(int assignmentId, string code, string fileName)
        {
            AssignmentId = assignmentId;
            Code = code;
            FileName = fileName;
        }

        public int AssignmentId { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
    }

    public class BuildCodeBlockRequestHandler : IRequestHandler<BuildCodeBlockRequest, ValidateableResponse<InstructorSubmissionDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public BuildCodeBlockRequestHandler(
            DataContext dataContext,
            IMapper mapper, 
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<InstructorSubmissionDetailDto>> Handle(BuildCodeBlockRequest request, CancellationToken tkn)
        {
            await using var transaction = await _dataContext.Database.BeginTransactionAsync(tkn);
            try
            {
                var assignment = _dataContext.Set<Assignment>().First(x => x.Id == request.AssignmentId);
                if (assignment.AssignmentSolutionFileName != null)
                {
                    await _mediator.Send(new DeleteInstructorSubmissionByAssignmentIdRequest(request.AssignmentId), tkn);
                }
                assignment.AssignmentSolutionFileName = request.FileName;
                    var curlySet = CodeBuilder.BuildCode(request.Code, request.AssignmentId);
                _dataContext.Add(curlySet);
                _dataContext.SaveChanges();
                var entity = new InstructorSubmission
                    {
                        CurlySetId = curlySet.Id,
                        AssignmentId = request.AssignmentId
                    };

                var testCases = _dataContext.Set<MethodTestCase>().Where(x => x.AssignmentId == request.AssignmentId).ToList();
                if (testCases.Any())
                {
                    _dataContext.RemoveRange(testCases);
                }

                _dataContext.Add(entity);
                _dataContext.Update(assignment);
                await _dataContext.SaveChangesAsync(tkn);

                    var dto = _mapper.Map<InstructorSubmissionDetailDto>(entity);
                await transaction.CommitAsync(tkn);
                    return new ValidateableResponse<InstructorSubmissionDetailDto>(dto);
                }
                catch
                {
                    return null;
                }
            }
        
    }

        public class BuildCodeBlockRequestValidation : AbstractValidator<BuildCodeBlockRequest>
        {
            public BuildCodeBlockRequestValidation()
            {
            }
        }    
    
    public static class CodeBuilder
    {
        public static CurlySet BuildCode(string code, int assignmentId)
        {
            code = Regex.Replace(code, @"\t|\n|\r", "");
            CurlySet topCurly = new CurlySet();
            AntlrInputStream inputStream = new AntlrInputStream(code);
            speakLexer speakLexer = new speakLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            speakParser speakParser = new speakParser(commonTokenStream);

            speakParser.NamespaceContext parsedCode = speakParser.code_namespace().@namespace();
            speakParser.ClassContext parsedCode2 = speakParser.code_class().@class();
            if (parsedCode == null && parsedCode2 != null)
            {
                topCurly = BuildTopClass(parsedCode2, assignmentId);
            }
            else if (parsedCode != null)
            {
                topCurly = BuildTopNameSpace(parsedCode, assignmentId);
            }
            else
            {
                return null;
            }

            return topCurly;
        }

        public static CurlySet BuildTopNameSpace(speakParser.NamespaceContext code, int assignmentId)//new curly, exception, always parent
        {
            var topCurly = new CurlySet
            {
                //namespace desc TODO eventually
                OpenCurlyId = 0,
                OpenCurlyPositionInString = code.namespace_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = code.namespace_body().CLOSE_CURLY().Symbol.Column,
                AssignmentId = assignmentId
            };

            var namespaceStatements = code.namespace_body().namespace_statment_list_predicate().namespace_statment();

            if (namespaceStatements.Any())
            {
                foreach (var statement in namespaceStatements)
                {
                    BuildClass(statement.@class(), topCurly, assignmentId);
                }
            }

            return topCurly;
        }

        public static CurlySet BuildTopClass(speakParser.ClassContext @class, int assignmentId)//new curly, exception, always parent
        {
            //build class
            var topClassCurly = new CurlySet
            {
                OpenCurlyId = 0,
                OpenCurlyPositionInString = @class.class_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = @class.class_body().CLOSE_CURLY().Symbol.Column,
                IsClass = true,
                ClassSignature = new ClassSignature
                {
                    AccessModifier = @class.class_signature().class_head().class_head_list().access_modifier() != null ?
                    @class.class_signature().class_head().class_head_list().access_modifier().GetText() : null,
                    IsStatic = @class.class_signature().class_head().class_head_list().STATIC() != null ? true : false,
                    IsAbstract = @class.class_signature().class_head().class_head_list().ABSTRACT() != null ? true : false,
                    ClassName = @class.class_signature().class_head().complex_type().GetText(),
                    FullClassSignature = @class.class_signature().GetText(),
                    AssignmentId = assignmentId
                },
                AssignmentId = assignmentId
            };
            var classStatements = @class.class_body().class_statment_list_predicate().class_statment();

            if (classStatements.Any())
            {
                foreach (var statement in classStatements)
                {
                    if (statement.@class() != null)
                    {
                        BuildClass(statement.@class(), topClassCurly, assignmentId);
                    }
                    else if (statement.statement() != null)
                    {
                        BuildStatement(statement.statement(), topClassCurly, assignmentId);
                    }
                }
            }

            return topClassCurly;
        }

        public static void BuildClass(speakParser.ClassContext @class, CurlySet parentCurly, int assignmentId)//new curly
        {
            //build class
            var classCurly = new CurlySet
            {
                OpenCurlyId = 0,
                OpenCurlyPositionInString = @class.class_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = @class.class_body().CLOSE_CURLY().Symbol.Column,
                IsClass = true,
                ClassSignature = new ClassSignature
                {
                    AccessModifier = @class.class_signature().class_head().class_head_list().access_modifier() != null ?
                    @class.class_signature().class_head().class_head_list().access_modifier().GetText() : null,
                    IsStatic = @class.class_signature().class_head().class_head_list().STATIC() != null ? true : false,
                    IsAbstract = @class.class_signature().class_head().class_head_list().ABSTRACT() != null ? true : false,
                    ClassName = @class.class_signature().class_head().complex_type().GetText(),
                    FullClassSignature = @class.class_signature().GetText(),
                    AssignmentId = assignmentId
                },
                ParentCurlySet = parentCurly,
                AssignmentId = assignmentId
            };
            parentCurly.CurlySets.Add(classCurly);
            var classStatements = @class.class_body().class_statment_list_predicate().class_statment();

            if (classStatements.Any())
            {
                foreach (var statement in classStatements)
                {
                    if (statement.@class() != null)
                    {
                        BuildClass(statement.@class(), classCurly, assignmentId);
                    }
                    else if (statement.statement() != null)
                    {
                        BuildStatement(statement.statement(), classCurly, assignmentId);
                    }
                }
            }
        }

        public static void BuildMethod(speakParser.MethodContext method, MethodSignature methodSignature, CurlySet parentCurly, int assignmentId)//new curly
        {
            var isMain = methodSignature.MethodName.Trim().ToLower() == "main";
            var methodCurly = new CurlySet
            {
                IsMethod = true,
                IsMain = isMain,
                MethodSignature = methodSignature,
                OpenCurlyId = 0,
                OpenCurlyPositionInString = method.method_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = method.method_body().CLOSE_CURLY().Symbol.Column,
                ParentCurlySet = parentCurly,
                AssignmentId = assignmentId
            };
            parentCurly.CurlySets.Add(methodCurly);

            var methodStatements = method.method_body().method_statment_list_predicate().method_statment().statement_statement();
            BuildStatementsFromStatement(methodStatements, methodCurly, assignmentId);
        }


        public static List<MethodParameter> BuildMethodParams(speakParser.ParamContext[] methodParams)//not new, apart of method head/signature
        {
            List<MethodParameter> methodParameters = new List<MethodParameter>();
            if (methodParams.Any())
            {
                foreach (var param in methodParams)
                {
                    var paramComplexDesc = param.param_instance_predicate().param_instance().complex_descriptor();
                    methodParameters.Add(new MethodParameter
                    {
                        ParameterType = GetComplexDescirptorType(paramComplexDesc),
                        ParameterName = GetComplexDescirptorName(paramComplexDesc)
                    });
                }
            }
            return methodParameters;
        }

        public static string GetComplexDescirptorType(speakParser.Complex_descriptorContext complex_desc)
        {
            if (complex_desc.primitive_typed_descriptor() != null)
            {
                return complex_desc.primitive_typed_descriptor().primitive_type().GetText();
            }
            else if (complex_desc.custom_descriptor() != null)
            {
                return complex_desc.custom_descriptor().var_description()[0].GetText();
            }
            else if (complex_desc.generic_descriptor() != null)
            {
                return complex_desc.generic_descriptor().generic_type().GetText();
            }
            else
            {
                return complex_desc.void_descriptor().GetText();
            }
        }
        public static string GetComplexDescirptorName(speakParser.Complex_descriptorContext complex_desc)
        {
            if (complex_desc.primitive_typed_descriptor() != null)
            {
                return complex_desc.primitive_typed_descriptor().var_description().GetText();
            }
            else if (complex_desc.custom_descriptor() != null)
            {
                return complex_desc.custom_descriptor().var_description()[1].GetText();
            }
            else if (complex_desc.generic_descriptor() != null)
            {
                return complex_desc.generic_descriptor().var_description().GetText();
            }
            else
            {
                return complex_desc.void_descriptor().var_description().GetText();
            }
        }

        public static void BuildStatement(speakParser.StatementContext statement, CurlySet parentCurly, int assignmentId)//finder
        {
            if (statement.goto_method() != null)
            {
                var methodParams = statement.goto_method().method().method_param().method_param_body_predicate().param_body().param();
                BuildMethodParams(methodParams);
                var isVoid = statement.statement_head().complex_descriptor().void_descriptor() != null;
                var method = new MethodSignature
                {
                    FullMethodSignature = statement.statement_head().GetText() + statement.goto_method().method().method_param().GetText(),
                    AccessModifier = statement.statement_head().access_modifier() != null ? statement.statement_head().access_modifier().GetText() : null,
                    IsVoid = isVoid,
                    ReturnType = GetComplexDescirptorType(statement.statement_head().complex_descriptor()),
                    MethodName = GetComplexDescirptorName(statement.statement_head().complex_descriptor()),
                    IsReference = statement.statement_head().REFERANCE() != null,
                    IsReadOnly = statement.statement_head().READONLY() != null,
                    IsAsync = statement.statement_head().ASYNC() != null,
                    IsStatic = statement.statement_head().STATIC() != null,
                    MethodParameters = BuildMethodParams(statement.goto_method().method().method_param().method_param_body_predicate().param_body().param()),
                    AssignmentId = assignmentId
                };
                BuildMethod(statement.goto_method().method(), method, parentCurly, assignmentId);//current currly comes from the partial built curly from above
            }
            else if (statement.goto_property() != null)
            {
                var property = new PropertySignature
                {
                    PropertyHead = statement.statement_head().GetText(),
                    PropertyType = GetComplexDescirptorType(statement.statement_head().complex_descriptor()),
                    PropertyName = GetComplexDescirptorName(statement.statement_head().complex_descriptor()),
                    AccessModifier = statement.statement_head().access_modifier() != null ? statement.statement_head().access_modifier().GetText() : null,
                    IsStatic = statement.statement_head().STATIC() != null,
                    PropertyFunction = statement.goto_property().property().GetText(),
                    AssignmentId = assignmentId
                };
                parentCurly.PropertySignatures.Add(property);
            }
            else if (statement.goto_variable() != null)
            {
                var varComplexDesc = statement.statement_head().complex_descriptor();
                var variable = new PrimitiveVariable
                {
                    CurlySet = parentCurly,
                    VariableType = GetComplexDescirptorType(varComplexDesc),
                    VariableName = GetComplexDescirptorName(varComplexDesc),
                    VariableValue = statement.goto_variable().variable().variable_body().GetText(),
                    VariableSignature = statement.statement_head().GetText() + statement.goto_variable().variable().GetText(),
                    AssignmentId = assignmentId
                };

                parentCurly.PrimitiveVariables.Add(variable);
            }
            else if (statement.goto_undefined_variable() != null)//not new, current curly IS THE parent curly, added to list of vars
            {
                var varComplexDesc = statement.statement_head().complex_descriptor();
                var variable = new PrimitiveVariable
                {
                    CurlySet = parentCurly,
                    VariableType = GetComplexDescirptorType(varComplexDesc),
                    VariableName = GetComplexDescirptorName(varComplexDesc),
                    VariableValue = null,
                    VariableSignature = statement.statement_head().GetText(),
                    AssignmentId = assignmentId
                };

                parentCurly.PrimitiveVariables.Add(variable);
                //build statement head for undefined variable
                // add semicolon
                //undefined variable
            }
        }

        public static void BuildStatementsFromStatement(speakParser.Statement_statementContext[] statements, CurlySet parentCurly, int assignmentId)//finder
        {
            foreach (var statement in statements)
            {
                if (statement.statement() != null)
                {
                    BuildStatement(statement.statement(), parentCurly, assignmentId);
                }
                else if (statement.paren_statement() != null)
                {
                    BuildParenStatement(statement.paren_statement(), parentCurly, assignmentId);
                }
                else if (statement.non_parn_statement() != null)
                {
                    BuildNonParenStatement(statement.non_parn_statement(), parentCurly, assignmentId);
                }
                else if (statement.return_statement() != null)
                {
                    BuildReturnStatement(statement.return_statement(), parentCurly);
                }
            }
        }

        private static void BuildParenStatement(speakParser.Paren_statementContext paren_statement, CurlySet parentCurly, int assignmentId)//new curly
        {
            //build parenstatement head
            var curlyParenStatement = new CurlySet
            {
                IsPrimitiveStatement = true,
                OpenCurlyId = 0,
                OpenCurlyPositionInString = paren_statement.paren_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = paren_statement.paren_body().CLOSE_CURLY().Symbol.Column,
                StatementSignature = new PrimitiveStatement
                {
                    Statement = paren_statement.paren_keyword().GetText(),
                    StatementSignature = paren_statement.paren_keyword().GetText() + paren_statement.parn_param().GetText(),
                    AssignmentId = assignmentId
                },
                ParentCurlySet = parentCurly,
                AssignmentId = assignmentId
            };
            parentCurly.CurlySets.Add(curlyParenStatement);

            var parenStatements = paren_statement.paren_body().paren_list_predicate().paren_body_statment().statement_statement();
            BuildStatementsFromStatement(parenStatements, curlyParenStatement, assignmentId);
        }

        private static void BuildNonParenStatement(speakParser.Non_parn_statementContext non_parn_statement, CurlySet parentCurly, int assignmentId)//new curly
        {
            var curlyParenStatement = new CurlySet
            {
                IsPrimitiveStatement = true,
                OpenCurlyId = 0,
                OpenCurlyPositionInString = non_parn_statement.non_paren_body().OPEN_CURLY().Symbol.Column,
                ClosedCurlyId = 0,
                CloseCurlyPositionInString = non_parn_statement.non_paren_body().CLOSE_CURLY().Symbol.Column,
                StatementSignature = new PrimitiveStatement
                {
                    Statement = non_parn_statement.non_paren_keyword().GetText(),
                    StatementSignature = non_parn_statement.non_paren_keyword().GetText(),
                    AssignmentId = assignmentId
                },
                ParentCurlySet = parentCurly,
                AssignmentId = assignmentId
            };
            parentCurly.CurlySets.Add(curlyParenStatement);

            //build nonparenstatementHead
            var nonParenStatements = non_parn_statement.non_paren_body().non_paren_list_predicate().non_paren_body_statment().statement_statement();
            BuildStatementsFromStatement(nonParenStatements, curlyParenStatement, assignmentId);
        }

        public static void BuildReturnStatement(speakParser.Return_statementContext return_statement, CurlySet parentCurly)//not new
        {
            parentCurly.HasReturn = true;
            parentCurly.ReturnStatement = new ReturnStatement
            {
                ReturnSignature = return_statement.GetText(),
                ReturnStartIndex = return_statement.Start.StartIndex,
                ReturnEndIndex = return_statement.Stop.StartIndex
            };
        }
    }
}
