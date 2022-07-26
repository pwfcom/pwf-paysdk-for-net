歡迎使用 Pwf PaySDK for .NET 。

Pwf PaySDK for .NET讓您不用複雜編程即可訪Pwf開放平台開放的各項常用能力，SDK可以自動幫您滿足能力調用過程中所需的證書校驗、加簽、驗簽、發送HTTP請求等非功能性要求。

## 環境要求
1. Pwf PaySDK for .NET基於`.Net Standard 2.0`開發，支持`.Net Framework 4.6.1`、.`Net Core 2.0`及其以上版本

2. 使用 Pwf PaySDK for .NET 之前 ，您需要先前往 https://pwf.com 申請開通賬號並完成開發者接入的一些準備工作，包括創建應用、為應用設置接口相關配置等。

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

namespace SDKDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Config config = GetConfig();
            PwfClient pwfClient = new PwfClient(config);

            //订单支付請求接口業務參數
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("trade_name", "trade_name");
                Params.Add("fiat_currency", "EUR");
                Params.Add("fiat_amount", 0.01);
                Params.Add("out_trade_no", "order-100");
                Params.Add("subject", "order-test");
                Params.Add("timestamp", 1657895835);
                Params.Add("return_url", "https://www.return.com/return");
                Params.Add("notify_url", "https://www.notify.com/notify"); //此處的異步回調通知地址需與商戶中心配置的異步通知地址一致
                Params.Add("collection_model", 1);
                Params.Add("merchant_no", "<-- 請填寫您的商戶號，例如：2022......81170 -->");
                

                ApiResponse resonpse = pwfClient.Execute("/api/v2/wallet/payAddress", Params);

                if (resonpse.IsSuccess())
                {
                    if (resonpse.Verify())
                    {
                        Dictionary<string, object> ss = resonpse.GetDataMap();
                        foreach (var dic in ss)
                        {
                            Console.WriteLine("Output Key : {0}, Value : {1} ", dic.Key, Convert.ToString(dic.Value));
                        }
                    }
                    else
                    {
                        throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");
                    }
                }
                else
                {
                    throw new PwfError(resonpse.GetRet() + ":" + resonpse.GetMsg());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("调用遭遇异常，原因：" + ex);
   
            }

            //處理異步回調通知例子
             string json_string = "{\"ret\":1000,\"lang\":\"CN\",\"msg\":\"\\u8bf7\\u6c42\\u6210\\u529f\",\"data\":\"........"}";

            ApiResponse resonpse = pwfClient.GetApiResponse(json_string);
            if (resonpse.IsSuccess())
            {
                if (resonpse.Verify())
                {
                    Dictionary<string, object> ss = resonpse.GetDataMap();
                    foreach (var dic in ss)
                    {
                        Console.WriteLine("Output Key : {0}, Value : {1} ", dic.Key, Convert.ToString(dic.Value));
                    }
                }else{
                    throw new PwfError("the signature check fails, please check whether the Pwf platform public key or merchant private key is configured correctly.");
                }
            }
        }


        static private Config GetConfig()
        {
            return new Config()
            {
                ApiUrl = "<-- 請填寫平台分配的接口域名，例如：https://xxx.pwf.com/ -->",
                AppToken = "<-- 請填寫您的appToken，例如：377b26eb8c25bd... -->",
 
                Lang = "TC",//語系(參考文檔中最下方語系表，如:TC)

                MerchantPrivateCertPath = "<-- 請填寫您的應用私鑰路徑，例如：/foo/MyPrivateKey.pem -->",
                PwfPublicCertPath = "<-- 請填寫PWF平台公鑰證書文件路徑，例如：/foo/PwfPublicKey.pem -->"
            };
        }

    }
}
```
