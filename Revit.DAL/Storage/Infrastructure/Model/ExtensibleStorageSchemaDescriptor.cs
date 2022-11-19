using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Storage.Schemas;

namespace Revit.DAL.Storage.Infrastructure.Model
{
    public class ExtensibleStorageSchemaDescriptor
    {
        public ExtensibleStorageSchemaDescriptor(Guid guid, Type schemaType, Type targetType, string fieldName)
        {
            Guid = guid;
            SchemaType = schemaType;
            TargetType = targetType;
            FieldName = fieldName;
            Kind = ConvertKind(TargetType);

        }

        public ExtensibleStorageSchemaDescriptor(ExtensibleStorageSchemaDescriptor descriptor) 
            : this(descriptor.Guid, descriptor.SchemaType, descriptor.TargetType, descriptor.FieldName)
        {
        }

        private static TargetObjectKindEnum ConvertKind(Type targetType)
        {
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            
            var switcher = new Dictionary<Type, TargetObjectKindEnum>
            {
                {typeof(FamilyInstance), TargetObjectKindEnum.FamilyInstance},
                {typeof(ViewDrafting), TargetObjectKindEnum.RevitInternalObject},
                {typeof(ProjectInfo), TargetObjectKindEnum.ProjectInfo},
            };

            return switcher.TryGetValue(targetType, out var result) ? result : TargetObjectKindEnum.Another;
        }

        public TargetObjectKindEnum Kind { get; }

        public Guid Guid { get; }

        public Type SchemaType { get; }

        public Type TargetType { get; }

        public string FieldName { get; }

        public Func<object, string> Converter { get; set; }

        public Type FieldType
        {
            get
            {
                if (typeof(IDataSchema).IsAssignableFrom(SchemaType))
                {
                    return SchemaType.GetProperty(nameof(IDataSchema.Data))?.PropertyType ?? SchemaType;
                }

                return SchemaType;
            }
        }

        public string GetData(Entity entity)
        {
            
            var getMethod = typeof(Entity).GetMethod(nameof(Entity.Get), new[] { typeof(string) });
            var getGeneric = getMethod?.MakeGenericMethod(FieldType);
            var result = getGeneric?.Invoke(entity, new object[] { FieldName });
            
            if (FieldType == typeof(string))
            {
                return (string)result;
            }

            var ggg = Converter?.Invoke(result);
            return ggg;
        }
    }
}
