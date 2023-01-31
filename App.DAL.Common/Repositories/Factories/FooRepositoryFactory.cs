using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.RevitRepositories;
using Microsoft.Extensions.Configuration;

namespace App.DAL.Common.Repositories.Factories
{
    public class FooRepositoryFactory : IFooRepositoryFactory
    {
        private readonly IEnumerable<IFooRepository> _fooRepositories;

        private readonly IConfiguration _configuration;

        public FooRepositoryFactory(
            IEnumerable<IFooRepository> fooRepositories,
            IConfiguration configuration)
        {
            _fooRepositories = fooRepositories;
            _configuration = configuration;
        }

        public IFooRepository Create()
        {
            return 
                _configuration["ApplicationMode"] switch
                {
                    "web" => _fooRepositories.First(x => x.GetType() == typeof(FooDbRepository)),
                    "desktop" => _fooRepositories.First(x => x.GetType() == typeof(FooRevitRepository)),
                    _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
                };
        }
    }
}
