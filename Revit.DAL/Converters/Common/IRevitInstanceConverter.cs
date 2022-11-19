namespace Revit.DAL.Converters.Common
{
    public interface IRevitInstanceConverter<TModelElement, in TRevitElement>
    {
        void PushToRevit(TRevitElement revitElement, TModelElement modelElement);

        TModelElement PullFromRevit(TRevitElement revitElement);
    }
}
