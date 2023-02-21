namespace App.DAL.Db.Model
{
    public class Bar : BaseEntity
    {
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
    }
}
