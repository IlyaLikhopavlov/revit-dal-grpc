namespace App.Catalog.Db.Model
{
    public class BaseCatalogEntity
    {
        public int Id { get; set; }
        
        public long Version { get; set; }

        public Guid IdGuid { get; set; }

        public string PartNumber { get; set; }

        public string ModelNumber { get; set; }
    }
}
