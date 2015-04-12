using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.Model;
using EE.BM.Utility;
using LinqToDB;

namespace EE.BM.DAL
{
    public static partial class DBExtension
    {
        #region User Extension
        public static UserModel Find(this ITable<UserModel> User, int ID)
        {
            return User.FirstOrDefault<UserModel>(u => u.ID == ID);
        }

        public static UserModel Find(this ITable<UserModel> User, string userName, string password)
        {
            return User.FirstOrDefault<UserModel>(u => u.LoginName.ToLower() == userName.ToLower() && u.Password == password);
        }

        public static bool New(this ITable<UserModel> User, string userName,string password,string display, int rightID)
        {
            bool result = false;
            try
            {
                if (User.Insert(() => new UserModel()
                {
                    LoginName = userName,
                    Password = Helper.Encrypt(password),
                    LastLoginDate = DateTime.Now,
                    Right_ID = rightID
                }) > 0)
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Logger.CreateLogger().WriteError(string.Format("Create user failed. Exception: {0}", ex.Message));
            }
            return result;
        }

        #endregion

        #region Right Extension
        public static RightModel Find(this ITable<RightModel> Right, int ID)
        {
            return Right.FirstOrDefault<RightModel>(u => u.ID == ID);
        }

        public static bool New(this ITable<RightModel> Right, string cname, string ename, string comment)
        {
            bool result = false;
            try
            {
                if (Right.Insert(() => new RightModel()
                {
                    C_Name = cname,
                    E_Name = ename,
                    Comments = comment
                }) > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLogger().WriteError(string.Format("Create right failed. Exception: {0}", ex.Message));
            }
            return result;
        }

        #endregion

        #region Receipt Extension
        public static ReceiptModel Find(this ITable<ReceiptModel> Receipt, int ID)
        {
            return Receipt.FirstOrDefault(r=>r.ID == ID);
        }

        public static bool NewReceipt(this ITable<ReceiptModel> Receipt, string Client, string Company, string Production,
            string BLNO, string Container, string AnimalNo, string Place, bool IsCommercial, bool IsAnimal, bool IsHealth,
            string Remark, decimal DiseaseFee, decimal DisinfectFee, string DiseaseChequeNo, string DisinfectChequeNo,
            string Mobile, DateTime Date)
        {
            bool result = false;
            try
            {
                if (Receipt.Insert(() => new ReceiptModel()
                {
                }) > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

                Logger.CreateLogger().WriteError(string.Format("Create receipt failed. Exception: {0}", ex.Message));
            }
            return result;
        }

        #endregion
    }
}
