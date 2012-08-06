using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tavisca.RainDrop.Service
{
    public class RainDropService : IRainDrop
    {
        public NextId GetNextId()
        {
            RainDrop dr = new RainDrop();
            return new NextId() {Id = dr.GetNextId(Configuration.ServerId, Configuration.DataCenterId)};
        }
    }
}