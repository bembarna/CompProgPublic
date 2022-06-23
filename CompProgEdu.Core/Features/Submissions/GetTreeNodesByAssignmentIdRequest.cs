using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Submissions
{
    public class GetTreeNodesByAssignmentIdRequest : IRequest<ValidateableResponse<List<TreeNodeDto>>>, IValidateable
    {
        public GetTreeNodesByAssignmentIdRequest(int assignmentId)
        {
            AssignmentId = assignmentId;
        }

        public int AssignmentId { get; set; }
    }

    public class GetTreeNodesByAssignmentIdRequestHandler : IRequestHandler<GetTreeNodesByAssignmentIdRequest, ValidateableResponse<List<TreeNodeDto>>>
    {
        private readonly DataContext _dataContext;

        public GetTreeNodesByAssignmentIdRequestHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ValidateableResponse<List<TreeNodeDto>>> Handle(GetTreeNodesByAssignmentIdRequest request, CancellationToken cancellationToken)
        {
            var curlySets = await _dataContext.Set<CurlySet>().Where(x => x.AssignmentId == request.AssignmentId)
                .Include(x => x.MethodSignature)
                .Include(x => x.ClassSignature)
                .Include(x => x.StatementSignature)
                .Include(x => x.PrimitiveVariables)
                .ToListAsync(cancellationToken);

            var treeNodes = (from curlySet in curlySets where curlySet.ParentId == null select BuildNode(curlySet)).ToList();

            return new ValidateableResponse<List<TreeNodeDto>>(treeNodes);
        }

        public TreeNodeDto BuildNode(CurlySet curlySet)
        {
            var treeNode = new TreeNodeDto();

            if (curlySet.IsClass)
            {
                treeNode.Key = $"class-{curlySet.Id}";
                treeNode.Label = curlySet.ClassSignature.FullClassSignature;
            }

            if (curlySet.IsMethod)
            {
                treeNode.Key = $"method-{curlySet.Id}";
                treeNode.Label = curlySet.MethodSignature.FullMethodSignature;
            }

            if (curlySet.IsPrimitiveStatement)
            {
                treeNode.Key = $"statement-{curlySet.Id}";
                treeNode.Label = curlySet.StatementSignature.StatementSignature;
            }

            var variables = _dataContext.Set<PrimitiveVariable>().Where(x => x.CurlySetId == curlySet.Id).ToList();
            var properties = _dataContext.Set<PropertySignature>().Where(x => x.CurlySetId == curlySet.Id).ToList();
            var childCurlySets = _dataContext.Set<CurlySet>().Where(x => x.ParentId == curlySet.Id).ToList();

            if (variables.Count != 0)
            {
                foreach (var variable in variables)
                {
                    treeNode.Nodes.Add(new TreeNodeDto
                    {
                        Key = $"variable-{variable.Id}",
                        Label = variable.VariableSignature
                    });
                }
            }


            if (properties.Count != 0)
            {
                foreach (var prop in properties)
                {
                    treeNode.Nodes.Add(new TreeNodeDto
                    {
                        Key = $"property-{prop.Id}",
                        Label = prop.PropertyHead + prop.PropertyFunction
                    });
                }
            }

            if (childCurlySets.Count != 0)
            {
                foreach (var child in childCurlySets)
                {
                    treeNode.Nodes.Add(BuildNode(child));
                }
            }

            return treeNode;
        }
    }

    public class GetTreeNodesByAssignmentIdRequestValidation : AbstractValidator<GetTreeNodesByAssignmentIdRequest>
    {
        public GetTreeNodesByAssignmentIdRequestValidation()
        {
        }
    }
}
