using System;
using System.Text;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace Pwf.PaySDK.Util
{
    public class RSAEncryptorUtil
    {

        public static string GetShaType()
        {
            return "SHA1WithRSA";
        }

        public static string GetAsymmetricType()
        {
            return "RSA";
        }

        public static string GetPrivateKey(string certPath)
        {
            using (FileStream fs = File.OpenRead(certPath))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    return GetPem("PRIVATE KEY", data);
                }
                throw new Exception("Invalid private key format");
            }
        }

        public static string GetPublicKey(string certPath)
        {

            using (FileStream fs = File.OpenRead(certPath))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    return GetPem("PUBLIC KEY", data);
                }
                throw new Exception("Invalid public key format");
            }
        }

        private static string GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header, StringComparison.Ordinal) + header.Length;
            int end = pem.IndexOf(footer, start, StringComparison.Ordinal);

            return pem.Substring(start, (end - start));
        }

        public static string DoDecrypt(string cipherTextBase64, string charset, string privateKey)
        {

            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            byte[] privateInfoByte = Convert.FromBase64String(privateKey);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);

            engine.Init(false, priKey);
            byte[] byteData = Convert.FromBase64String(cipherTextBase64);

            int inputLen = byteData.Length;
            MemoryStream ms = new MemoryStream();
            int offSet = 0;
            byte[] cache;
            int i = 0;

            while (inputLen - offSet > 0)
            {
                if (inputLen - offSet > 128)
                {
                    cache = engine.ProcessBlock(byteData, offSet, 128);
                }
                else
                {
                    cache = engine.ProcessBlock(byteData, offSet, inputLen - offSet);
                }
                ms.Write(cache, 0, cache.Length);
                i++;
                offSet = i * 128;
            }

            return Encoding.GetEncoding(charset).GetString(ms.ToArray());
        }


        public static string DoEncrypt(string plainText, string charset, string publicKey)
        {

            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            byte[] publicInfoByte = Convert.FromBase64String(publicKey);
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);

            engine.Init(true, pubKey);
            byte[] byteData = Encoding.GetEncoding(charset).GetBytes(plainText);

            int inputLen = byteData.Length;
            MemoryStream ms = new MemoryStream();
            int offSet = 0;
            byte[] cache;
            int i = 0;

            while (inputLen - offSet > 0)
            {
                if (inputLen - offSet > 117)
                {
                    cache = engine.ProcessBlock(byteData, offSet, 117);
                }
                else
                {
                    cache = engine.ProcessBlock(byteData, offSet, inputLen - offSet);
                }
                ms.Write(cache, 0, cache.Length);
                i++;
                offSet = i * 117;
            }

            return  Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
        }


        public static string DoSign(string content, string charset, string privateKey)
        {
            byte[] privateInfoByte = Convert.FromBase64String(privateKey);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);

            byte[] byteData = Encoding.UTF8.GetBytes(content);

            ISigner normalSig = SignerUtilities.GetSigner(GetShaType());
            normalSig.Init(true, priKey);
            normalSig.BlockUpdate(byteData, 0, content.Length);
            byte[] normalResult = normalSig.GenerateSignature();
            return Convert.ToBase64String(normalResult);
        }

        public static bool DoVerify(string content, string charset, string publicKey, string sign)
        {

            byte[] publicInfoByte = Convert.FromBase64String(publicKey);
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);

            byte[] signBytes = Convert.FromBase64String(sign);
            byte[] plainBytes = Encoding.GetEncoding(charset).GetBytes(content);


            ISigner verifier = SignerUtilities.GetSigner(GetShaType());
            verifier.Init(false, pubKey);
            verifier.BlockUpdate(plainBytes, 0, plainBytes.Length);

            return verifier.VerifySignature(signBytes);
        }

    }
}

