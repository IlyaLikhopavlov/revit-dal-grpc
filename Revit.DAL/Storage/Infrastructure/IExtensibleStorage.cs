using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.DAL.Storage.Infrastructure
{
    public interface IExtensibleStorage
    {
        Type Type { get; }
    }
}
