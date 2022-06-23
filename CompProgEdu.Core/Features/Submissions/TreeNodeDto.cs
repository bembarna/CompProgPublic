using System.Collections.Generic;

namespace CompProgEdu.Core.Features.Submissions
{
    public class TreeNodeDto
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public List<TreeNodeDto> Nodes { get; set; } = new List<TreeNodeDto>();
    }
}
