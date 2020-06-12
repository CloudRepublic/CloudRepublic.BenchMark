using System;
using System.Collections.Generic;
using System.Text;

namespace CloudRepublic.BenchMark.API.Models
{
    public class BenchMarkOption
    {
        public string CloudProviderName { get; set; }
        public string HostEnvironmentName { get; set; }
        public string LanguageName { get; set; }
        public string AzureRuntimeVersionName { get; set; }

        public string Title { get { return CloudProviderName + " " + HostEnvironmentName + " " + LanguageName + " " + AzureRuntimeVersionName; } }

    }
}
