
using Pwf.PaySDK.Api;

namespace Pwf.PaySDK.Base
{
    public class ApiClient
    {
        private static Kernel context;

        public static void SetOptions(Config options)
        {
            context = new Kernel(options);
        }


        public static Wallet Wallet()
        {
            return new Wallet(context);
        }

    }
}

