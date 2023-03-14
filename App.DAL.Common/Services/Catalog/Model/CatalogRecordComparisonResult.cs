using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Common.Services.Catalog.Model.Enums;

namespace App.DAL.Common.Services.Catalog.Model
{
    public class CatalogRecordComparisonResult
    {
        public string PartNumber { get; set; }

        public string ModelNumber { get; set; }

        public Guid IdGuid { get; set; }

        public long DbVersion { get; set; }

        public long DocumentVersion { get; set; }

        public Type Type { get; set; }

        public ResolutionEnum Resolution
        {
            get
            {
                if (Type == null)
                {
                    return ResolutionEnum.UpdateInDb;
                }
                
                if (DbVersion < 1 || DocumentVersion < 1 || DbVersion == DocumentVersion)
                {
                    return ResolutionEnum.NothingToDo;
                }

                return
                    DbVersion > DocumentVersion
                        ? ResolutionEnum.UpdateInDocument
                        : ResolutionEnum.UpdateInDb;
            }
        }

        public bool IsIgnored { get; set; }
    }
}
