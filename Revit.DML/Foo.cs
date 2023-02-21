namespace App.DML
{

    public class Foo : BaseItem
    {
        public Foo() : this(null, null)
        {
        }
        
        public Foo(string description, string name) : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
