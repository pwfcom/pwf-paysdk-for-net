using System;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Api.Response
{
    public class NotifyRechargeResponse
    {
        [NameInMap("order_num")]
        public string order_num { get; set; }

        [NameInMap("merchant_no")]
        public string merchant_no { get; set; }

        [NameInMap("user_id")]
        public int user_id { get; set; }

        [NameInMap("status")]
        public int status { get; set; }

        [NameInMap("pay_at")]
        public int pay_at { get; set; }

        [NameInMap("public_chain")]
        public string public_chain { get; set; }

        [NameInMap("digital_currency")]
        public string digital_currency { get; set; }

        [NameInMap("wallet_address")]
        public float wallet_address { get; set; }

        [NameInMap("currency_val")]
        public float currency_val { get; set; }

    }
}

