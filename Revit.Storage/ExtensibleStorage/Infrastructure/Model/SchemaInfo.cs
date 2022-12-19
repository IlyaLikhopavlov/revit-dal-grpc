using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;

namespace Revit.Storage.ExtensibleStorage.Infrastructure.Model
{
    public class SchemaInfo
    {
        public SchemaInfo()
        {
        }

        public SchemaInfo(
            Guid guid,
            DomainModelTypesEnum? domainModelType,
            string entityName,
            Type schemaType,
            Type targetType,
            Element targetElement,
            string fieldName)
        {
            Guid = guid;
            DomainModelType = domainModelType;
            EntityName = entityName;
            SchemaType = schemaType;
            TargetType = targetType;
            TargetElement = targetElement;
            FieldName = fieldName;
        }

        public SchemaInfo(SchemaInfo schemaInfo) :
            this(
                schemaInfo.Guid,
                schemaInfo.DomainModelType,
                schemaInfo.EntityName,
                schemaInfo.SchemaType,
                schemaInfo.TargetType,
                schemaInfo.TargetElement,
                schemaInfo.FieldName)
        {
        }

        public Guid Guid { get; set; }

        public DomainModelTypesEnum? DomainModelType { get; set; }

        public string EntityName { get; set; }

        public Type SchemaType { get; set; }

        public Type TargetType { get; set; }

        public Element TargetElement { get; set; }

        public string FieldName { get; set; }
    }
}
