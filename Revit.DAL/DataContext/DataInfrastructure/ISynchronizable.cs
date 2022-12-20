namespace App.DAL.DataContext.DataInfrastructure
{
    public interface ISynchronizable
    {
        Task Sync();
    }
}
