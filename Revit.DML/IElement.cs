namespace Revit.DML
{
    public interface IElement
    {
        Guid Guid { get; set; }

        int Id { get; set; }

        string? Name { get; set; }
    }
}
