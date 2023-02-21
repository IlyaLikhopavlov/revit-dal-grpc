using App.DAL.Db.Mapping.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DML;
using AutoMapper;
using CategoryEntity = App.DAL.Db.Model.Category;

namespace App.DAL.Db.Mapping
{
    public class CategoryEntityConverter : EntityConverter<Category, CategoryEntity>
    {
        public CategoryEntityConverter(IMapper mapper) : base(mapper)
        {
        }
    }
}
