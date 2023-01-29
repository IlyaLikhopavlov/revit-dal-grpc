namespace App.DML
{
    public abstract class BaseItem : Element
    {
        protected BaseItem(string name, Guid? guid = null, int id = -1) : base(guid, id, name)
        {
        }

        public string TypeName => GetType().FullName;
    }
}
