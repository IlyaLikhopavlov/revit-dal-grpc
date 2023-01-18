namespace App.DML
{

    public class Foo : BaseEntity
    {
        public Foo() : this(null, null)
        {
        }
        
        public Foo(string? description, string? name) : base(name)
        {
            Description = description;
        }

        public string? Description { get; set; }
    }
}
