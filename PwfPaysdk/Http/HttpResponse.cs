using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Pwf.PaySDK.Util;

namespace Pwf.PaySDK.Http
{
    public class HttpResponse
    {
		

		private HttpResponseMessage _responseAsync { get; set; }

		public int StatusCode { get; set; }

		public string StatusMessage { get; set; }

		public Dictionary<string, string> _headers;
		public Dictionary<string, string> Headers
		{
			get
			{
				if (_headers == null)
				{
					return new Dictionary<string, string>();
				}
				return _headers;
			}
			set
			{
				_headers = value;
			}
		}

		public Stream Body
		{
			get
			{
		 		if (_responseAsync != null)
				{
					return _responseAsync.Content.ReadAsStreamAsync().Result;
				}
				return null;
			}
		}

        public HttpResponse(HttpResponseMessage response)
		{
			if (response != null)
			{
				StatusCode = (int)response.StatusCode;
				StatusMessage = "";

				_responseAsync = response;

				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				Headers.GetEnumerator();
				foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
				{
					Headers.Add(StringUtil.StrToLower(header.Key), header.Value.First());
				}
			}
		}
	}


}

