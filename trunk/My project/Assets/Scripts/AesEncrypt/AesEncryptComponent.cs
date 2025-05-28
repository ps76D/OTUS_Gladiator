using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace AesEncrypt
{
    public class AesEncryptComponent
    {
        public byte[] Encrypt(string str)
        {
            byte[] encryptedBytes;

            using Aes aes = Aes.Create();
            aes.GenerateKey();
                
            aes.GenerateIV();

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            using (MemoryStream ms = new())
            {
                using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new(cs))
                    {
                        sw.Write(str);
                    }
                }
                
                encryptedBytes = ms.ToArray();
            }

            using (SHA256 sha = SHA256.Create())
            { 
                byte[] bytes = sha.ComputeHash(encryptedBytes);
                
                string hash = BitConverter.ToString(bytes).Replace("-", "");
                
                AesEncryptParametersStruct c = new()
                {
                    _key = aes.Key,
                    _iv = aes.IV
                };
                
                SaveEncryptParameters(c, hash);
            }

            return encryptedBytes;
        }
        
        public string Decrypt(byte[] encryptedBytes)
        {
            AesEncryptParametersStruct c = LoadEncryptParameters(encryptedBytes);

            using Aes aes = Aes.Create();
            aes.Key = c._key;
                
            aes.IV = c._iv;
                
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new(encryptedBytes);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            string decryptedString = sr.ReadToEnd();

            return decryptedString;
        }
    
        private void SaveEncryptParameters(AesEncryptParametersStruct c, string key)
        {
            string json = JsonUtility.ToJson(c);
            
            PlayerPrefs.SetString(key, json);
        }

        private AesEncryptParametersStruct LoadEncryptParameters(byte[] encryptedBytes)
        {
            string key;
            
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(encryptedBytes);
                
                key = BitConverter.ToString(hash).Replace("-", "");
            }
            
            if (!PlayerPrefs.HasKey(key))
            {
                throw new PlayerPrefsException("Key Not Exist");
            }
            
            string json = PlayerPrefs.GetString(key);
            
            return JsonUtility.FromJson<AesEncryptParametersStruct>(json);
        }

    





















    }
}
