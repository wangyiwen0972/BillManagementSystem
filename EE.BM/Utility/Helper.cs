using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using EE.BM.Model;
namespace EE.BM.Utility
{
    public static class Helper
    {
        private const string Key = "StephenI";
        public static string Encrypt(string pToEncrypt)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public static string Decrypt(string pToDecrypt)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public static object CreateKeyValueObject(string display, string value)
        {
            object rtnObject = new { DisplayMember = display, ValueMember = value };

            return rtnObject;
        }

        public static Action GetActionFromPermission(object command, UserModel user)
        {
            DelegateCommand delegateCommand = command as DelegateCommand;

            Type vmType = typeof(ReceiptViewModel);

            foreach (var property in vmType.GetProperties())
            {
                if (property.Name.Equals(delegateCommand.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    var permissionList = property.GetCustomAttributes<PermissionAttribute>();

                    foreach (PermissionAttribute permission in permissionList)
                    {
                        if (permission.RightID == user.Right_ID)
                        {
                            return permission.Action;
                        }
                    }
                }
            }
            
            return Action.Invisible;
        }

        public static string GetTimeTicks()
        {
            return DateTime.Now.ToString("yyMMddhhmmss");
        }
             

    }
}
