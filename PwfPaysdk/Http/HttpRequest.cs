using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pwf.PaySDK.Http
{
    public class HttpRequest
    {

		private string _method;

		private Dictionary<string, string> _query;

		private Dictionary<string, string> _headers;

		public Stream Body;

		public string Host { get; set; }

		public int Port { get; set; }

		public string Method
		{
			get
			{
				if (_method == null)
				{
					return "GET";
				}
				return _method;
			}
			set
			{
				_method = value;
			}
		}

		public string Pathname { get; set; }

		public Dictionary<string, string> Query
		{
			get
			{
				if (_query == null)
				{
					return new Dictionary<string, string>();
				}
				return _query;
			}
			set
			{
				_query = value;
			}
		}

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

		public HttpRequest()
		{
			Query = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
		}

		public string GetApiUrl()
		{

			StringBuilder urlBuilder = new StringBuilder();
			urlBuilder.Append(Host);
			if (null != Pathname)
			{
				urlBuilder.Append(Pathname);
			}

			return urlBuilder.ToString();
		}
	}
}

