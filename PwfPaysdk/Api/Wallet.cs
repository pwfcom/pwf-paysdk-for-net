using System;
using System.Collections.Generic;
using Pwf.PaySDK.Base;
using Pwf.PaySDK.Http;
using Pwf.PaySDK.Util;
using Pwf.PaySDK.Api.Response;

namespace Pwf.PaySDK.Api
{
    public class Wallet
    {

        private static Kernel _kernel;

        public Wallet(Kernel context)
        {
            _kernel = context;
        }

        public WalletPayAddressResponse PayAddress(Dictionary<string, object> Params)
        {
            HttpRequest request = new HttpRequest()
            {
                Host = _kernel.GetConfig("ApiUrl"),
                Method = "POST",
                Pathname = "/api/v2/wallet/payAddress",
                Body = _kernel.BuildPostRequestBody(Params),
                Headers = new Dictionary<string, string>{
                        {"content-type", "application/json;charset=utf-8"},
                }
            };

            HttpResponse response = _kernel.Execute(request);

            ApiResponse apiResponse = _kernel.GetApiResponse(response);
            if (apiResponse.IsSuccess())
            {
                if (apiResponse.Verify())
                {
                    return DictionaryUtil.ToObject<WalletPayAddressResponse>(apiResponse.GetDataMap());
                }

                throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");

            }

            throw new PwfError(apiResponse.GetRet()+":"+apiResponse.GetMsg());
        }

        public WalletOrderStatusResponse OrderStatus(Dictionary<string, object> Params)
        {
            HttpRequest request = new HttpRequest()
            {
                Host = _kernel.GetConfig("ApiUrl"),
                Method = "POST",
                Pathname = "/api/v2/wallet/orderStatus",
                Body = _kernel.BuildPostRequestBody(Params),
                Headers = new Dictionary<string, string>{
                        {"content-type", "application/json;charset=utf-8"},
                }
            };

            HttpResponse response = _kernel.Execute(request);

            ApiResponse apiResponse = _kernel.GetApiResponse(response);
            if (apiResponse.IsSuccess())
            {
                if (apiResponse.Verify())
                {
                    return DictionaryUtil.ToObject<WalletOrderStatusResponse>(apiResponse.GetDataMap());
                }

                throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");

            }

            throw new PwfError(apiResponse.GetRet() + ":" + apiResponse.GetMsg());
        }

        public WalletRechargeResponse Recharge(Dictionary<string, object> Params)
        {
            HttpRequest request = new HttpRequest()
            {
                Host = _kernel.GetConfig("ApiUrl"),
                Method = "POST",
                Pathname = "/api/v2/wallet/recharge",
                Body = _kernel.BuildPostRequestBody(Params),
                Headers = new Dictionary<string, string>{
                        {"content-type", "application/json;charset=utf-8"},
                }
            };

            HttpResponse response = _kernel.Execute(request);

            ApiResponse apiResponse = _kernel.GetApiResponse(response);
            if (apiResponse.IsSuccess())
            {
                if (apiResponse.Verify())
                {
                    return DictionaryUtil.ToObject<WalletRechargeResponse>(apiResponse.GetDataMap());
                }

                throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");

            }

            throw new PwfError(apiResponse.GetRet() + ":" + apiResponse.GetMsg());
        }
    }
}

