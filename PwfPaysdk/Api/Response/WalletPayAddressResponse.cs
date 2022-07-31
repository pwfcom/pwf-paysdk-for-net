using System;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Api.Response
{
    public class WalletPayAddressResponse
    {
        [NameInMap("order_num")]
        public string order_num { get; set; }

        [NameInMap("out_trade_no")]
        public string out_trade_no { get; set; }

        [NameInMap("fiat_currency")]
        public string fiat_currency { get; set; }

        [NameInMap("fiat_amount")]
        public float fiat_amount { get; set; }

        [NameInMap("request_time")]
        public int request_time { get; set; }

        [NameInMap("expire_time")]
        public int expire_time { get; set; }
 
        [NameInMap("pay_url")]
        public string pay_url { get; set; }
    }
}

