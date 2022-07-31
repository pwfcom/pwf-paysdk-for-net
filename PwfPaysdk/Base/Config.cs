
using Pwf.PaySDK.Base.Attributes;

namespace Pwf.PaySDK.Base
{
    public class Config
    {
		[NameInMap("ApiUrl")]
		[Validation(Required = true)]
		public string ApiUrl { get; set; }

		[NameInMap("AppToken")]
		[Validation(Required = true)]
		public string AppToken { get; set; }

		[NameInMap("MerchantNo")]
		[Validation(Required = true)]
		public string MerchantNo { get; set; }

		[NameInMap("MerchantPrivateCertPath")]
		[Validation(Required = true)]
		public string MerchantPrivateCertPath { get; set; }

		[NameInMap("PwfPublicCertPath")]
		[Validation(Required = true)]
		public string PwfPublicCertPath { get; set; }

		[NameInMap("NotifyUrl")]
		[Validation(Required = true)]
		public string NotifyUrl { get; set; }

		[NameInMap("Lang")]
		[Validation(Required = true)]
		public string Lang { get; set; }
	}
}

