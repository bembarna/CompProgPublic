using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.JDoodles
{
    public class JDoodleLanguageVersion
    {
        public string Language { get; set; }
        public List<string> Versions { get; set; } = new List<string>();
    }
}
