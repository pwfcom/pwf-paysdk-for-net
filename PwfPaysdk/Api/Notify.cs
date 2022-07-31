using System;
using Pwf.PaySDK.Base;
using Pwf.PaySDK.Util;
using Pwf.PaySDK.Api.Response;

namespace Pwf.PaySDK.Api
{
    public class Notify
    {
        private static Kernel _kernel;

        public Notify(Kernel context)
        {
            _kernel = context;
        }

        public NotifyPayResponse Pay(string jsonString)
        {
            ApiResponse apiResponse = _kernel.GetApiResponseFromJsonString(jsonString);
            if (apiResponse.IsSuccess())
            {
                if (apiResponse.Verify())
                {
                    return DictionaryUtil.ToObject<NotifyPayResponse>(apiResponse.GetDataMap());
                }

                throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");

            }

            throw new PwfError(apiResponse.GetRet() + ":" + apiResponse.GetMsg());
        }
	

	    public NotifyRechargeResponse Rechage(string jsonString)
        {
            ApiResponse apiResponse = _kernel.GetApiResponseFromJsonString(jsonString);
            if (apiResponse.IsSuccess())
            {
                if (apiResponse.Verify())
                {
                    return DictionaryUtil.ToObject<NotifyRechargeResponse>(apiResponse.GetDataMap());
                }

                throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");

            }

            throw new PwfError(apiResponse.GetRet() + ":" + apiResponse.GetMsg());
        }
    }
}

