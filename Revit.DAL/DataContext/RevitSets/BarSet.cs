using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DML;
using Revit.DAL.DataContext.RevitItems;

namespace Revit.DAL.DataContext.RevitSets
{
    public class BarSet : RevitSet<Bar, FamilyInstance>
    {
        private readonly RevitInstanceConverter<Bar, FamilyInstance> _converter;

        public BarSet(IFactory<Document, RevitInstanceConverter<Bar, FamilyInstance>> converterFactory, Document document) :
            base(document)
        {
            _converter = converterFactory.New(document);
        }

        protected override void PullEntities()
        {
            SourcesDictionary = Document
                ?.GetBarInstances()
                .ToDictionary(x => x.Id.IntegerValue, x => x);

            EntityProxiesDictionary = SourcesDictionary
                ?.ToDictionary(
                    x => x.Key,
                    x => new EntityProxy<Bar>(_converter.PullFromRevit(x.Value), x.Value.Id.IntegerValue));
        }

        protected override FamilyInstance CreateRevitElement(Bar modelElement)
        {
            throw new NotImplementedException();
        }

        protected override void SetToRevit(FamilyInstance revitElement, Bar modelElement)
        {
            _converter?.PushToRevit(revitElement, modelElement);
        }
    }
}
