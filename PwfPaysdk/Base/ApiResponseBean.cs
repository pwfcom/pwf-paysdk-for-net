using System;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Base
{
    public class ApiResponseBean
    {
        [NameInMap("ret")]
        public int ret { get; set; }

        [NameInMap("msg")]
        public string msg { get; set; }

        [NameInMap("lang")]
        public string lang { get; set; }

        [NameInMap("data")]
        public string data { get; set; }
    }
}

