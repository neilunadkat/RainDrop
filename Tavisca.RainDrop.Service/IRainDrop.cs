using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace Tavisca.RainDrop.Service
{
    [ServiceContract]
    public interface IRainDrop
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/next", ResponseFormat = WebMessageFormat.Json)]
        NextId GetNextId();
    }
    [DataContract]
    public class NextId
    {
        [DataMember] public long Id;
    }
}