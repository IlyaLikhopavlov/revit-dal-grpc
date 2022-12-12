using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Main.UiFixtures
{
    public interface ICommand
    {
        Task Execute();
    }
}
