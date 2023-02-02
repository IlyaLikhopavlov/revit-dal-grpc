﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DML;

namespace App.DAL.Common.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Project GetByUniqueId(string uniqueId);
    }
}
