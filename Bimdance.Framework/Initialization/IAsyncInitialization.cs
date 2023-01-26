using System;
using System.Collections.Generic;
using System.Text;

namespace Bimdance.Framework.Initialization
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }
}
