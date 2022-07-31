using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pwf.PaySDK.Util;

namespace Pwf.PaySDK.Http
{
	public class HttpClientUtil
	{
		private static readonly int bufferLength = 1024;
		public static HttpClient httpClient;
		
		private static List<string> bodyMethod = new List<string> {
			"POST",
			"PUT",
			"PATCH"
		};


		public static HttpClient GetHttpClient(Dictionary<string, object> options)
		{
			if (httpClient == null)
			{
				string text = (options.Get("httpProxy") != null) ? options.Get("httpProxy").ToSafeString() : options.Get("httpsProxy").ToSafeString();
				bool num = options.Get("ignoreSSL").ToSafeBool();
				HttpClientHandler httpClientHandler = new HttpClientHandler();
				if (!string.IsNullOrWhiteSpace(text))
				{
					httpClientHandler.Proxy = new WebProxy(new Uri(text));
				}
				else
				{
					httpClientHandler.UseProxy = false;
				}
				if (num)
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (HttpRequestMessage message, X509Certificate2 cert, X509Chain chain, SslPolicyErrors error) => true;
				}

				httpClient = new HttpClient(httpClientHandler);
			}
			return httpClient;
		}

		public static HttpResponse Execute(HttpRequest request)
		{

			try
			{
				Dictionary<string, object> runtimeOptions = new Dictionary<string, object>
				{
					{"ignoreSSL", true},
					{"httpProxy", ""},
					{"connectTimeout", 15000},
					{"readTimeout", 15000}
				};


				int timeout;
				HttpRequestMessage requestMessage = GetRequestMessage(request, runtimeOptions, out timeout);
				HttpClient httpClient = GetHttpClient(runtimeOptions);


				HttpResponseMessage response = httpClient.SendAsync(requestMessage, new CancellationTokenSource(timeout).Token).Result;

				return new HttpResponse(response);
			}
			catch (TaskCanceledException)
			{
				throw new WebException("operation is timeout");
			}
		}

		public static HttpRequestMessage GetRequestMessage(HttpRequest request, Dictionary<string, object> runtimeOptions, out int timeout)
		{
			string uriString = request.GetApiUrl();
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
			httpRequestMessage.Method = GetHttpMethod(request.Method);
			httpRequestMessage.RequestUri = new Uri(uriString);


			timeout = 100000;
			int num = DictionaryUtil.GetDicValue(runtimeOptions, "readTimeout").ToSafeInt(0);
			int num2 = DictionaryUtil.GetDicValue(runtimeOptions, "connectTimeout").ToSafeInt(0);
			if (num != 0 || num2 != 0)
			{
				timeout = num + num2;
			}
			if (bodyMethod.Contains(request.Method) && request.Body != null)
			{
				Stream stream = new MemoryStream();
				request.Body.Position = 0L;
				byte[] array = new byte[4096];
				int count;
				while ((count = request.Body.Read(array, 0, array.Length)) != 0)
				{
					stream.Write(array, 0, count);
				}
				stream.Position = 0L;
				httpRequestMessage.Content = new StreamContent(stream);
			}
			foreach (KeyValuePair<string, string> header in request.Headers)
			{
				httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
				if (header.Key.ToLower().StartsWith("content-") && httpRequestMessage.Content != null)
				{
					httpRequestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
				}
			}
			return httpRequestMessage;
		}

		public static HttpMethod GetHttpMethod(string method)
		{
			switch (method.ToSafeString(string.Empty).ToLower())
			{
				case "get":
					return HttpMethod.Get;
				case "post":
					return HttpMethod.Post;
				case "delete":
					return HttpMethod.Delete;
				case "put":
					return HttpMethod.Put;
				case "head":
					return HttpMethod.Head;
				case "options":
					return HttpMethod.Options;
				case "trace":
					return HttpMethod.Trace;
				default:
					return HttpMethod.Get;
			}
		}

		public static string GetResponseBody(HttpResponse response)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] buffer = new byte[bufferLength];
				Stream body = response.Body;
				while (true)
				{
					int num = body.Read(buffer, 0, bufferLength);
					if (num == 0)
					{
						break;
					}
					memoryStream.Write(buffer, 0, num);
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, array.Length);
				body.Close();
				body.Dispose();
				return Encoding.UTF8.GetString(array);
			}
		}
	}
}