namespace Bimdance.Framework.Initialization
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }
}
