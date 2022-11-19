using System.Linq.Expressions;
using System.Reflection;

namespace Bimdance.Framework.Expressions
{
    public static class ExpressionUtils
    {
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> selector)
        {
            var memberExpression = ExtractMemberExpression(GetLambdaBody(selector));

            if (memberExpression == null)
            {
                throw new ArgumentException("Selector must be a access expression", nameof(selector));
            }

            if (memberExpression.Member.DeclaringType == null)
            {
                throw new InvalidOperationException("Property does not have declaring type");
            }

            return memberExpression.Member.DeclaringType.GetProperty(memberExpression.Member.Name);
        }

        public static void AssertIsLambda(Expression expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Selector must be a lambda expression", nameof(expression));
            }
        }

        public static Expression GetLambdaBody(Expression lambdaExpression)
        {
            if (lambdaExpression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Selector must be a lambda expression", nameof(lambdaExpression));
            }

            var lambda = (LambdaExpression)lambdaExpression;

            return lambda.Body;
        }

        public static MemberExpression ExtractMemberExpression(Expression expression)
        {
            while (true)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        return (MemberExpression)expression;

                    case ExpressionType.Convert:
                        {
                            var operand = ((UnaryExpression)expression).Operand;
                            expression = operand;
                            continue;
                        }

                    default:
                        return null;
                }
            }
        }

        public static object GetValueOfExpression(object target, Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return target;

                case ExpressionType.MemberAccess:
                    {
                        var memberExpression = (MemberExpression)expression;
                        var parentValue = GetValueOfExpression(target, memberExpression.Expression);

                        if (parentValue == null)
                        {
                            return null;
                        }

                        return memberExpression.Member is PropertyInfo info ? info.GetValue(parentValue, null) : ((FieldInfo)memberExpression.Member).GetValue(parentValue);
                    }

                default:
                    throw new ArgumentException("The expression must contain only member access calls", nameof(expression));
            }
        }
    }
}
