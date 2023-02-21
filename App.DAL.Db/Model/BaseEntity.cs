namespace App.DAL.Db.Model
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid Guid { get; set; }

        public string Description { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}