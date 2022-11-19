using System.Linq.Expressions;
using Autodesk.Revit.DB;
using Bimdance.Framework.Expressions;

namespace Revit.DAL.DataContext.DataInfrastructure.Configurations
{
    public class PropertyRelation<TSource, TTarget> : IPropertyRelation
        where TSource : Element
        where TTarget : DML.Element
    {
        public Expression<Func<TSource, object>> SourceExpression { get; set; }

        public Expression<Func<TTarget, object>> TargetExpression { get; set; }

        public Func<object, object> Resolver { get; set; }

        public Type SourceType => typeof(TSource);

        public Type TargetType => typeof(TTarget);

        public void Resolve(object source, object? target)
        {
            if (SourceExpression == null)
            {
                throw new ArgumentNullException(
                    nameof(SourceExpression), $@"Source expression of relation isn't specified.");
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            ExpressionUtils.AssertIsLambda(SourceExpression);
            ExpressionUtils.AssertIsLambda(TargetExpression);

            if (Resolver == null)
            {
                throw new ArgumentNullException(nameof(Resolver), $@"Resolver delegate can't be null.");
            }

            var sourceToken = SourceExpression.Compile().Invoke((TSource) source);

            var targetValue = Resolver.Invoke(sourceToken);

            var targetPropertyInfo = ExpressionUtils.GetPropertyInfo(TargetExpression);
            targetPropertyInfo.SetValue(target, targetValue);
        }
    }
}
