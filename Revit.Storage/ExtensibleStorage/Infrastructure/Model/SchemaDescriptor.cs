using Autodesk.Revit.DB;
using Revit.Storage.ExtensibleStorage.Infrastructure.Model.Enums;
using Revit.Storage.ExtensibleStorage.Schemas;

namespace Revit.Storage.ExtensibleStorage.Infrastructure.Model
{
    public class SchemaDescriptor
    {
        private readonly SchemaInfo _schemaInfo;

        public SchemaDescriptor(SchemaInfo schemaInfo)
        {
            _schemaInfo = schemaInfo;

            if (typeof(SchemaInfo)
                .GetProperties()
                .Where(x =>
                    x.Name != nameof(SchemaInfo.TargetElement) &&
                    x.Name != nameof(SchemaInfo.DomainModelType))
                .Select(p =>
                    p.GetValue(schemaInfo))
                .Any(v => v is null))
            {
                throw new ArgumentException($"One or several property values of {nameof(schemaInfo)} are null");
            }

            AssignConverter();
        }

        public SchemaDescriptor(SchemaDescriptor descriptor)
            : this(
                new SchemaInfo
                {
                    Guid = descriptor.SchemaInfo.Guid,
                    EntityName = descriptor.SchemaInfo.EntityName,
                    SchemaType = descriptor.SchemaInfo.SchemaType,
                    TargetElement = descriptor.SchemaInfo.TargetElement,
                    FieldName = descriptor.SchemaInfo.FieldName,
                    TargetType = descriptor.SchemaInfo.TargetType,
                    DomainModelType = descriptor.SchemaInfo.DomainModelType
                })
        {
        }

        private Func<object, string> _converter;

        private void AssignConverter()
        {
            var switcher = new Dictionary<Type, Func<object, string>>
            {
                {
                    typeof(IDictionary<string, string>),
                        o =>
                        {
                            var dictionary = (IDictionary<string, string>)o;
                            return "[" + string.Join(",",
                                dictionary.Select(kv => "{\"" + kv.Key + "\"" + ": " + kv.Value + "}").ToArray()) + "]";
                        }
                },
                {
                    typeof(IList<string>),
                        o =>
                        {
                            var list = (IList<int>)o;
                            return "[" + string.Join(",", list) + "]";
                        }
                }
            };

            _converter = switcher.TryGetValue(_schemaInfo.SchemaType, out var converter) ? converter : o => o.ToString();
        }

        public string FormatData(object data) => _converter?.Invoke(data);

        private static TargetObjectKindEnum ConvertKind(Type targetType)
        {
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            var switcher = new Dictionary<Type, TargetObjectKindEnum>
            {
                { typeof(FamilyInstance), TargetObjectKindEnum.FamilyInstance },
                { typeof(ViewDrafting), TargetObjectKindEnum.RevitInternalObject },
                { typeof(ProjectInfo), TargetObjectKindEnum.ProjectInfo },
            };

            return switcher.TryGetValue(targetType, out var result) ? result : TargetObjectKindEnum.Another;
        }

        public TargetObjectKindEnum Kind => ConvertKind(_schemaInfo.TargetType);

        public SchemaInfo SchemaInfo => new(_schemaInfo);

        //public Guid Guid => _schemaInfo.Guid;

        //public string EntityName => _schemaInfo.EntityName;

        //public Type SchemaType => _schemaInfo.SchemaType;

        //public Type TargetType => _schemaInfo.TargetType;

        //public Element TargetElement => _schemaInfo.TargetElement;

        //public string FieldName => _schemaInfo.FieldName;

        //public DomainModelTypesEnum? DomainModelType => _schemaInfo.DomainModelType;

        public Type FieldType
        {
            get
            {
                if (typeof(DataSchema).IsAssignableFrom(_schemaInfo.SchemaType))
                {
                    return _schemaInfo.SchemaType.GetProperty(nameof(DataSchema.Data))?.PropertyType ?? _schemaInfo.SchemaType;
                }

                return _schemaInfo.SchemaType;
            }
        }
    }
}
