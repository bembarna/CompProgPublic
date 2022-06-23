using System.Collections.Generic;
using System.Linq;

namespace CompProgEdu.Core
{
    public class StringEnum<T>
    {
        public static List<string> List =>
            typeof(T).GetFields().Select(x => x.GetRawConstantValue().ToString()).ToList();

    }
}
