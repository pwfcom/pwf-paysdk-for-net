using System.Text;

using Pwf.PaySDK.Util;
using Pwf.PaySDK.Http;
using System.Collections.Generic;
using System.IO;
using System;

namespace Pwf.PaySDK.Base
{

    public class PwfClient
    {

        private readonly static Encoding DEFAULT_CHARSET = Encoding.UTF8;
        private readonly static string SDK_VERSION = "V2.0";
        private static string CHARSET_UTF8 = "UTF-8";
        private readonly Dictionary<string, object> config;

        private readonly string merchantPrivateKey;
        private readonly string pwfPublicKey;

        public PwfClient(Config options)
        {
            config = DictionaryUtil.ToMap(options);

            merchantPrivateKey = RSAEncryptorUtil.GetPrivateKey(GetConfig("MerchantPrivateCertPath"));
            pwfPublicKey = RSAEncryptorUtil.GetPublicKey(GetConfig("PwfPublicCertPath"));
        }

        public string GetConfig(string key)
        {
            if (key != null && config.ContainsKey(key) == true)
            {
                return (string)config[key];
            }

            throw new PwfError("Configuration parameter ["+key+"] cannot be empty");
        }

        public Dictionary<string, object> BuildPostParams(Dictionary<string, object> Params)
        {

            IDictionary<string, object> sortedMap = DictionaryUtil.GetSortedMap (Params);
            string paramsJsonString = DictionaryUtil.ToJsonString(sortedMap);
            string encrypted = RSAEncryptorUtil.DoEncrypt(paramsJsonString, CHARSET_UTF8, pwfPublicKey);

            Dictionary<string, object> postBodyParams = new Dictionary<string, object>();
            postBodyParams.Add("data", encrypted);
            postBodyParams.Add("sign", Sign(encrypted, merchantPrivateKey));
            postBodyParams.Add("token", GetConfig("AppToken"));
            postBodyParams.Add("lang", GetConfig("Lang"));
            postBodyParams.Add("version", SDK_VERSION);

            return postBodyParams;
        }

        public Stream BuildPostRequestBody(Dictionary<string, object> Params)
        {
            Dictionary<string, object> posts = BuildPostParams(Params);
            byte[] bytes = DEFAULT_CHARSET.GetBytes(DictionaryUtil.ToJsonString(posts));

            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return memoryStream;
        }

        public string Sign(string encrypted, string privateKey)
        {
            return RSAEncryptorUtil.DoSign(encrypted, CHARSET_UTF8, privateKey);
        }


        public ApiResponse Execute(string pathname, Dictionary<string, object> parameters)
        {

            HttpRequest request = new HttpRequest()
            {
                Host = GetConfig("ApiUrl"),
                Method = "POST",
                Pathname = pathname,
                Body = BuildPostRequestBody(parameters),
                Headers = new Dictionary<string, string>{
                        {"content-type", "application/json;charset=utf-8"},
                }
            };

            HttpResponse response = HttpClientUtil.Execute(request);
            string responseBody = HttpClientUtil.GetResponseBody(response);
            return GetApiResponse(responseBody);
        }

        public ApiResponse GetApiResponse(string responseJson)
        {
            ApiResponse apiResponse = new ApiResponse(this);
            apiResponse.SetResponseBody(responseJson);
            return apiResponse;
        }

        public static string GetSignContent(IDictionary<string, object> parameters)
        {

            IDictionary<string, object> sortedParams = new SortedDictionary<string, object>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, object>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = Convert.ToString(dem.Current.Value);
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        public bool Verify(Dictionary<string, object> respMap)
        {
            string sign = (string)respMap["sign"];
            respMap.Remove("sign");
            string content = GetSignContent(respMap);
            return RSAEncryptorUtil.DoVerify(content, CHARSET_UTF8, pwfPublicKey, sign);
        }

        public string DecryptResponseData(string data)
        {
            return RSAEncryptorUtil.DoDecrypt(data, CHARSET_UTF8,merchantPrivateKey);
        }
    }
}
