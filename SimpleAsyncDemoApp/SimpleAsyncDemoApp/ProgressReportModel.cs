using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAsyncDemoApp
{
    public class ProgressReportModel
    {
        public int PrecentageComplete { get; set; } = 0;
        public List<WebsiteDataModel> SitesDownloaded { get; set; } = new List<WebsiteDataModel>();
    }
}
