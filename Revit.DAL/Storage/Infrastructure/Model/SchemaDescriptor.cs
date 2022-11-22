using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Storage.Infrastructure.Model.Enums;
using Revit.DAL.Storage.Schemas;

namespace Revit.DAL.Storage.Infrastructure.Model
{
    public class SchemaDescriptor
    {
        private readonly SchemaInfo _schemaInfo;
        
        public SchemaDescriptor(SchemaInfo schemaInfo)
        {

            _schemaInfo = schemaInfo;
            Kind = ConvertKind(TargetType);
        }

        public SchemaDescriptor(SchemaDescriptor descriptor) 
            : this(
                new SchemaInfo
                {
                    Guid = descriptor.Guid,
                    Name = descriptor.Name,
                    SchemaType = descriptor.SchemaType, 
                    TargetType = descriptor.TargetType,
                    TargetElement = descriptor.TargetElement,
                    FieldName = descriptor.FieldName
                })
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

        public Guid Guid => _schemaInfo.Guid;

        public string Name => _schemaInfo.Name;

        public Type SchemaType => _schemaInfo.SchemaType;

        public Type TargetType => _schemaInfo.TargetType;

        public Element TargetElement => _schemaInfo.TargetElement;

        public string FieldName => _schemaInfo.FieldName;

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

            return Converter?.Invoke(result);
        }
    }
}
