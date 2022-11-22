using Revit.DML;

namespace Revit.DAL.Converters.Common
{
    public abstract class RevitInstanceConverter<TModelElement, TRevitElement> 
        where TModelElement : Element
        where TRevitElement : Autodesk.Revit.DB.Element
    {
        public abstract void PushToRevit(TRevitElement revitElement, TModelElement modelElement);

        public abstract TModelElement PullFromRevit(TRevitElement revitElement);

        public string ModelElementName => typeof(TModelElement).Name;
    }
}
