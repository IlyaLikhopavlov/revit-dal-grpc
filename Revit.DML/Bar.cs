namespace App.DML
{
    public class Bar : BaseItem
    {
        public Bar() : this(null, null)
        {
        }

        public Bar(string description, string name) : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        public Category Category { get; set; }
    }
}
