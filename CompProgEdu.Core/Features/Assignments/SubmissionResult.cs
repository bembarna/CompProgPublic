using CompProgEdu.Core.Features.TestCases;
using System.Collections.Generic;

namespace CompProgEdu.Core.Features.Assignments
{
    public class SubmissionResult
    {
        public int TotalScore { get; set; }
        public List<bool> OutputChecks { get; set; } = new List<bool>();
        public List<MethodTestCheckDto> MethodTestChecks { get; set; } = new List<MethodTestCheckDto>();
    }
}
