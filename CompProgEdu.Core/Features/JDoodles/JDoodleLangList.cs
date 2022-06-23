using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompProgEdu.Core.Features.JDoodles
{
    public static class JDoodleLangList
    {
        public static List<JDoodleLanguageVersion> JDoodleLanguageVersions { get; set; }
            = new List<JDoodleLanguageVersion>()
            {
            new JDoodleLanguageVersion
            {
                Language = "java",
                Versions = new List<string>() {"0", "1", "2", "3" }

            },
            new JDoodleLanguageVersion
            {
                Language = "csharp",
                Versions = new List<string>() { "0", "1", "2", "3" }
            },
            new JDoodleLanguageVersion
            {
                Language = "cpp",
                Versions = new List<string>() {"0", "1", "2", "3", "4" }

            },
            new JDoodleLanguageVersion
            {
                Language = "c",
                Versions = new List<string>() { "0", "1", "2", "3", "4" }
            },
        };
    }
}
