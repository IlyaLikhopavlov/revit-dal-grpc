namespace App.DAL.Common.Repositories.Factories.Base
{
    public interface IRepositoryFactory<out T> where T : class
    {
        T Create();
    }
}
