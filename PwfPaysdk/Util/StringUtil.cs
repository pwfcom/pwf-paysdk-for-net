using System;
namespace Pwf.PaySDK.Util
{
    public class StringUtil
    {
        public static string StrToLower(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return str.ToLower();
        }
    }
}

