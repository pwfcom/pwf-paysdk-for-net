using System;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Api.Response
{
    public class WalletRechargeResponse
    {

        [NameInMap("user_id")]
        public int user_id { get; set; }

        [NameInMap("public_chain")]
        public string public_chain { get; set; }

        [NameInMap("digital_currency")]
        public string digital_currency { get; set; }

        [NameInMap("wallet_address")]
        public string wallet_address { get; set; }

        [NameInMap("timestamp")]
        public int timestamp { get; set; }
 
    }
}