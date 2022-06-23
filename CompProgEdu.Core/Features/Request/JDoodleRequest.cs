using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.Request
{
    public class JDoodleRequest
    {
        public string output { get; set; }
        public string statusCode { get; set; }
        public string memory { get; set; }
        public string cpuTime { get; set; }
    }
}
