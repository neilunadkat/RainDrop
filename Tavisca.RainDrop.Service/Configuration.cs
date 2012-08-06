using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Tavisca.RainDrop.Service
{
    public class Configuration
    {
        public static long ServerId = long.Parse(ConfigurationManager.AppSettings["ServerId"]);

        public static long DataCenterId = long.Parse(ConfigurationManager.AppSettings["DataCenterId"]);

    }
}