using Revit.Storage.ExtensibleStorage.Infrastructure.Model;

namespace Revit.Storage.ExtensibleStorage.Infrastructure
{
    public class IntIdGenerator : ExtensibleStorageArray<int>, IIntIdGenerator
    {
        private HashSet<int> _ids;
        private readonly Random _random;

        private const int MinRangeBound = 10000;
        private const int MaxRangeBound = 10000000;

        public IntIdGenerator(SchemaDescriptor schemaDescriptor)
            : base(schemaDescriptor)
        {
            _random = new Random();
            Create();
        }

        public void Create()
        {
            Pull();
            _ids = new HashSet<int>(Storage);
        }

        public override void Save()
        {
            Storage = _ids.ToList();
            base.Save();
        }

        public int GetNewId()
        {
            if (_ids == null)
            {
                Create();
            }

            int result;

            do
            {
                result = _random.Next(MinRangeBound, MaxRangeBound);
            } while (!_ids.Add(result));

            return result;
        }

        public bool ReleaseId(int id)
        {
            if (_ids == null)
            {
                Create();
            }

            return _ids.Remove(id);
        }
    }
}
