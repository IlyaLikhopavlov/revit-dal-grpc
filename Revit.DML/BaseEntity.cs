namespace Revit.DML
{
    public abstract class BaseEntity : Element
    {
        protected BaseEntity(string? name, Guid? guid = null, int id = -1) : base(guid, id, name)
        {
        }
    }
}
