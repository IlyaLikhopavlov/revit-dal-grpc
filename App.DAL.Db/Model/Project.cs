namespace App.DAL.Db.Model
{
    public class Project
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Foo> Foos { get; set; } = new List<Foo>();

        public virtual ICollection<Bar> Bars { get; set; } = new List<Bar>();
    }
}
