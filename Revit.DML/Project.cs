namespace App.DML
{
    public class Project : BaseItem
    {
        public Project(string name, Guid? guid = null, int id = -1) : base(name, guid, id)
        {
        }

        public string UniqueId { get; set; }

        public string Title { get; set; }
    }
}
