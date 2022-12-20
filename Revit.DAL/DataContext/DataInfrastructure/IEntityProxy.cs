using App.DAL.DataContext.DataInfrastructure.Enums;

namespace App.DAL.DataContext.DataInfrastructure
{
    public interface IEntityProxy
    {
        public int Id { get; }

        public EntityState EntityState { get; set; }
    }
}