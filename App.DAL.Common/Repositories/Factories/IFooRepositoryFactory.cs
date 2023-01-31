using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Common.Repositories.Factories
{
    public interface IFooRepositoryFactory
    {
        IFooRepository Create();
    }
}
