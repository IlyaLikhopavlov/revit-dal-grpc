using App.DAL.Common.Repositories.DbRepositories.Generic;
using App.DAL.Db;
using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bar = App.DML.Bar;
using BarEntity = App.DAL.Db.Model.Bar;
using CategoryEntity = App.DAL.Db.Model.Category;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class BarDbRepository : GenericDbRepository<Bar, BarEntity>, IBarRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public BarDbRepository(
            IDbContextFactory<ProjectsDataContext> dbContextFactory, 
            IEntityConverter<Bar, BarEntity> entityConverter,
            IServiceProvider serviceProvider,
            DocumentDescriptor documentDescriptor) 
            : base(dbContextFactory, entityConverter, documentDescriptor)
        {
            _serviceProvider = serviceProvider;
        }

        public void Insert(Bar bar, Category obtainedCategory)
        {
            var categoryEntity = DbContext.Categories.FirstOrDefault(x => x.Name == obtainedCategory.Name);

            if (categoryEntity is not null)
            {
                var converter = _serviceProvider.GetRequiredService<IEntityConverter<Category, CategoryEntity>>();
                bar.Category = converter.ConvertToModel(categoryEntity);
            }
            else
            {
                bar.Category = obtainedCategory;
            }

            Insert(bar);
        }
    }
}
