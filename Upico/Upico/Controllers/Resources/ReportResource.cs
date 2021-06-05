using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Controllers.Resources
{
    public class ReportResource
    {
        public string PostId { get; set; }
        public string ReporterUserName { get; set; }
        public string ReportContent { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
