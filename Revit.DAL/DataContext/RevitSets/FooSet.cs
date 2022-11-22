using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DAL.DataContext.RevitItems;
using Revit.DML;

namespace Revit.DAL.DataContext.RevitSets
{
    public class FooSet : RevitSet<Foo, FamilyInstance>
    {
        private readonly RevitInstanceConverter<Foo, FamilyInstance> _converter;

        public FooSet(IFactory<Document, RevitInstanceConverter<Foo, FamilyInstance>> converter, Document document) : 
            base(document)
        {
            _converter = converter.New(document);
        }

        protected override void PullEntities()
        {
            SourcesDictionary = Document
                ?.GetFooInstances()
                .ToDictionary(x => x.Id.IntegerValue, x => x);

            EntityProxiesDictionary = SourcesDictionary
                ?.ToDictionary(
                    x => x.Key,
                    x => new EntityProxy<Foo>(_converter.PullFromRevit(x.Value), x.Value.Id.IntegerValue));
        }

        protected override FamilyInstance CreateRevitElement(Foo modelElement)
        {
            throw new NotImplementedException();
        }

        protected override void SetToRevit(FamilyInstance revitElement, Foo modelElement)
        {
            _converter?.PushToRevit(revitElement, modelElement);
        }
    }
}
