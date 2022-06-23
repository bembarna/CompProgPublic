using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CompProgEdu.Core.Features.Extensions
{
    public static class MethodExtensions
    {
        public static string GetPropertyNameAsString<T>(Expression<Func<T>> propExp)
        {
            return (propExp.Body as MemberExpression).Member.Name;
        }

        public static T CastObject<T>(object input)
        {
            return (T)input;
        }
    }
}
