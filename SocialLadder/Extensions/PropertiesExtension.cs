using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Extensions
{
    public static class PropertiesExtension
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyDelegate)
        {
            var expression = (MemberExpression)propertyDelegate.Body;
            return expression.Member.Name;
        }
    }
}
