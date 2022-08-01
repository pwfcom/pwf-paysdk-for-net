using System;
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Api.Response
{
    public class NotifyPayResponse
    {
        [NameInMap("order_num")]
        public string order_num { get; set; }

        [NameInMap("out_trade_no")]
        public string out_trade_no { get; set; }

        [NameInMap("status")]
        public int status { get; set; }

        [NameInMap("pay_at")]
        public string pay_at { get; set; }

        [NameInMap("fiat_currency")]
        public string fiat_currency { get; set; }

        [NameInMap("fiat_amount")]
        public float fiat_amount { get; set; }

        [NameInMap("currency_symbol")]
        public string currency_symbol { get; set; }

        [NameInMap("currency_val")]
        public float currency_val { get; set; }

        [NameInMap("wallet_address")]
        public string wallet_address { get; set; }

        [NameInMap("status_desc")]
        public string status_desc { get; set; }
    }
}

