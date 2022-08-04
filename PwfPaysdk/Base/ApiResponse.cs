using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pwf.PaySDK.Util;

namespace Pwf.PaySDK.Base
{
    public class ApiResponse
    {

		public const int SUCCESS_CODE = 1000;

		private PwfClient _kernel;

		public Dictionary<string, object> dataMap;

		private ApiResponseBean Bean { get; set; }

		public ApiResponse(PwfClient context)
		{
			_kernel = context;
			dataMap = new Dictionary<string, object>();
		}

		public void SetResponseBody(string responseBody)
		{
			//Console.WriteLine("responseBody : " + responseBody);
			if (!String.IsNullOrEmpty(responseBody ))
            {
				Dictionary<string, object> beanMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
				Bean = DictionaryUtil.ToObject<ApiResponseBean>(beanMap);
			}
            else
            {
				throw new PwfError("no response returned");
			}	
		}

		public bool IsSuccess()
		{
			if (( GetRet() ?? 0 )  == SUCCESS_CODE)
			{
				return true;
			}
			return false;
		}

		public bool Verify()
        {
			string decryptData = _kernel.DecryptResponseData(GetDataString());
			//Console.WriteLine("decryptData : " + decryptData);
			Dictionary<string, object> decryptDataMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptData);

			if (decryptDataMap != null && _kernel.Verify(decryptDataMap))
			{
				this.dataMap = decryptDataMap;
				return true;
			}

			return false;
		}

		public int? GetRet()
		{
            if (Bean != null)
            {
				return Bean.ret;
			}
			return null;
		}

		public string GetMsg()
		{
			if (Bean != null)
			{
				return Bean.msg;
			}
			return null;
		}

		public string GetLang()
		{
			if (Bean != null)
			{
				return Bean.lang;
			}
			return null;
		}

		public string GetDataString()
		{
			if (Bean != null)
			{
				return Bean.data;
			}
			return null;
		}

		public Dictionary<string, object> GetDataMap()
		{
			return dataMap;
		}
	}
}

