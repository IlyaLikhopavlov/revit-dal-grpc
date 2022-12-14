﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace Revit.Services.Processing.EventArgs
{
    public class DocumentClosingEventArgs : System.EventArgs
    {
        public Document ClosingDocument { get; set; }
    }
}
