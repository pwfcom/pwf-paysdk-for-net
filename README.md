歡迎使用 Pwf PaySDK for .NET 。

Pwf PaySDK for .NET讓您不用複雜編程即可訪Pwf開放平台開放的各項常用能力，SDK可以自動幫您滿足能力調用過程中所需的證書校驗、加簽、驗簽、發送HTTP請求等非功能性要求。

## 環境要求
1. Pwf PaySDK for .NET基於`.Net Standard 2.0`開發，支持`.Net Framework 4.6.1`、.`Net Core 2.0`及其以上版本

2. 使用 Pwf PaySDK for .NET 之前 ，您需要先前往[Pwf平台-開發者中心](https://pwf.com)完成開發者接入的一些準備工作，包括創建應用、為應用添加功能包、設置應用的接口加簽方式等。

3. 準備工作完成後，注意保存如下信息，後續將作為使用SDK的輸入。

* 加簽模式為公鑰模式時

`AppToken`、`應用的私鑰`、`Pwf公鑰`

## 安裝依賴
1. 使用`Visual Studio`打開本`README.md`所在文件夾下的`PwfPaySDK.sln`解決方案，在`生成`菜單欄下，執行`全部重新生成`。
2. 在`PwfPaySDK/bin/Debug`或`PwfPaySDK/bin/Release`目錄下，找到`PwfPaySDK.[version].nupkg`文件，該文件即為本SDK的NuGet離線包。
3. 在您的.NET應用項目工程中引入本SDK的NuGet離線包，即可完成SDK的依賴安裝。

## 快速開始
### 普通調用
以下這段代碼示例向您展示了使用Pwf PaySDK for .NET調用一個API的3個主要步驟：

1. 設置參數（全局只需設置一次）。
2. 發起API調用。
3. 處理響應或異常。

```charp
using Pwf.PaySDK.Base;

using Pwf.PaySDK.Api.Response;
using System.Collections.Generic;


namespace SDKDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Config config = GetConfig();
           
            ApiClient.SetOptions(config);
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("trade_name", "trade_name");
                Params.Add("pay_type", 1);
                Params.Add("fiat_currency", "EUR");
                Params.Add("fiat_amount", 0.01);
                Params.Add("out_trade_no", "order-100");
                Params.Add("subject", "order-test");
                Params.Add("timestamp", 1657895835);
                Params.Add("return_url", "https://www.return.com/return");
                Params.Add("collection_model", 1);
                Params.Add("merchant_no", config.MerchantNo);
                Params.Add("notify_url", config.NotifyUrl);

                WalletPayAddressResponse response = ApiClient.Wallet().PayAddress(Params);

                Console.WriteLine("返回：" + response.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("调用遭遇异常，原因：" + ex);
   
            }
        }


        static private Config GetConfig()
        {
            return new Config()
            {
                ApiUrl = "<-- 請填寫平台分配的接口域名，例如：https://xxx.pwf.com/ -->",
                AppToken = "<-- 請填寫您的appToken，例如：377b26eb8c25bd... -->",
                MerchantNo = "<-- 請填寫您的商戶號，例如：202207...964 -->",

                Lang = "TC",//語系(參考文檔中最下方語系表，如:TC)

                MerchantPrivateCertPath = "<-- 請填寫您的應用私鑰路徑，例如：/foo/MyPrivateKey.pem -->",
                PwfPublicCertPath = "<-- 請填寫PWF平台公鑰證書文件路徑，例如：/foo/PwfPublicKey.pem -->",
                NotifyUrl = "<-- 請填寫您的異步通知接收服務地址，例如：https://www.notify.com/notify -->"
            };
        }

    }
}
```
