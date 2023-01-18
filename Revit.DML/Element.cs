namespace App.DML
{
    public abstract class Element : IElement
    {
        private int _id;

        public Guid Guid { get; set; }

        public string? Name { get; set; }

        public int Id
        {
            get => _id;
            set
            {
                if (value < -1)
                {
                    throw new ArgumentException("Incorrect ID provided");
                }

                _id = value;
            }
        }

        protected Element(Guid? guid = null, int id = -1, string? name = null)
        {
            Guid = guid ?? Guid.NewGuid();
            Id = id;
            Name = name;
        }

        protected bool Equals(Element other)
        {
            return _id == other._id && Guid.Equals(other.Guid) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Element)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _id;
                hashCode = (hashCode * 397) ^ Guid.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}