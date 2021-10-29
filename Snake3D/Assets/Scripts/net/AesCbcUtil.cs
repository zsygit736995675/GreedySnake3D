using System;
using System. Security. Cryptography;
using System. Text;

/**
 * 加密解密
 */

public class AesCbcUtil
{
    private static string IV = "5481649756531687";
    private static string KEY = "4I0wrCt6pTJXjdLm";


    public static string Decipher (string content)
    {
        byte [] keyBytes = UTF8Encoding. UTF8. GetBytes(KEY);
        byte [] ivArray = UTF8Encoding. UTF8. GetBytes(IV);
        RijndaelManaged rm = new RijndaelManaged();
        rm. Key = keyBytes;
        rm. IV = ivArray;
        rm. Mode = CipherMode. CBC;
        rm. Padding = PaddingMode. PKCS7;
        ICryptoTransform ict = rm. CreateDecryptor();
        byte [] contentBytes = Convert. FromBase64String(content);
        byte [] resultBytes = ict. TransformFinalBlock(contentBytes, 0, contentBytes. Length);
        return UTF8Encoding. UTF8. GetString(resultBytes);
    }

    public static string Encrypt (string content)
    {
        byte [] keyArray = UTF8Encoding. UTF8. GetBytes(KEY);
        byte [] ivArray = UTF8Encoding. UTF8. GetBytes(IV);
        byte [] toEncryptArray = UTF8Encoding. UTF8. GetBytes(content);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel. Key = keyArray;
        rDel. IV = ivArray;
        rDel. Mode = CipherMode. CBC;
        rDel. Padding = PaddingMode. PKCS7;
        ICryptoTransform cTransform = rDel. CreateEncryptor();
        byte [] resultArray = cTransform. TransformFinalBlock(toEncryptArray, 0, toEncryptArray. Length);
        return Convert. ToBase64String(resultArray, 0, resultArray. Length);
    }
}