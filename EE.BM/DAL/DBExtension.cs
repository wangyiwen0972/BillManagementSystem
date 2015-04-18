using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.Model;
using EE.BM.Utility;
using LinqToDB;
using LinqToDB.Data;

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
        public static ReceiptModel Receipt_Find(this ITable<ReceiptModel> Receipt, int ID)
        {
            return Receipt.FirstOrDefault(r=>r.ID == ID);
        }

        private static ReceiptModel Receipt_Insert(this ITable<ReceiptModel> Receipt, string Client, string Company, string Production,
            string BLNO, string Container, string AnimalNo, string Place, bool IsCommercial, bool IsAnimal, bool IsHealth,
            string Remark, decimal DiseaseFee, decimal DisinfectFee, string DiseaseChequeNo, string DisinfectChequeNo,
            string Mobile, string Date, string Port, string Contacter)
        {
            try
            {
                int iResult = Receipt.Insert(() => new ReceiptModel()
                {
                    Client = Client,
                    Company = Company,
                    Production = Production,
                    BLNO = BLNO,
                    Container = Container,
                    AnimalNo = AnimalNo,
                    Place = Place,
                    IsCommercial = IsCommercial,
                    IsAnimal = IsAnimal,
                    IsHealth = IsHealth,
                    Mobile = Mobile,
                    YearMonth = Date,
                    Remark = Remark,
                    Port = Port,
                    Contacter = Contacter,
                    DiseaseFee = DiseaseFee,
                    DiseaseChequeNo = DiseaseChequeNo,
                    DisinfectFee = DisinfectFee,
                    DisinfectChequeNo = DisinfectChequeNo
                });
                ReceiptModel current = Receipt.Single((e) => e.YearMonth == Date);

                return current;
            }
            catch (Exception ex)
            {

                Logger.CreateLogger().WriteError(string.Format("Create receipt failed. Exception: {0}", ex.Message));
            }
            return null;
        }

        public static bool Receipt_Insert(this ITable<ReceiptModel> Receipt, ref ReceiptModel receiptModel)
        {
            ReceiptModel newRecord = Receipt.Receipt_Insert(receiptModel.Client,receiptModel.Company,receiptModel.Production,receiptModel.BLNO,receiptModel.Container,receiptModel.AnimalNo
                , receiptModel.Place,receiptModel.IsCommercial,receiptModel.IsAnimal,receiptModel.IsHealth,receiptModel.Remark,receiptModel.DiseaseFee,
                receiptModel.DisinfectFee,receiptModel.DiseaseChequeNo,receiptModel.DisinfectChequeNo,receiptModel.Mobile,receiptModel.YearMonth,receiptModel.Port,receiptModel.Contacter);

            if (newRecord == null)
            {
                return false;
            }
            else
            {
                receiptModel = newRecord;
                return true;
            }
        }

        public static bool Receipt_Update(this ITable<ReceiptModel> Receipt, ReceiptModel receiptModel)
        {
            //return Receipt.Where(u => u.ID == receiptModel.ID).Update((u) => receiptModel) > 0 ? true : false;
            return Receipt.Update(r => r.ID == receiptModel.ID, r => receiptModel) > 0 ? true : false;
            
        }

        public static int Receipt_Update(this DataConnection dataConnection, int? @id, string @Client, string @Company, string @Production,
            string @BLNO, string @Container, string @AnimalNo, string @Place, bool @IsCommercial, bool @IsAnimal, bool @IsHealth,
            string @Remark, decimal @DiseaseFee, decimal @DisinfectFee, string @DiseaseChequeNo, string @DisinfectChequeNo,
            string @Mobile, string @YearMonth, string @Contacter)
        {
            return dataConnection.ExecuteProc("[Receipt_Update]",
                new DataParameter("@id", @id), new DataParameter("@Client", @Client),
                new DataParameter("@Company", @Company), new DataParameter("@Production", @Production),
                new DataParameter("@BLNO", @BLNO), new DataParameter("@Container", @Container),
                new DataParameter("@AnimalNo", @AnimalNo), new DataParameter("@Place", @Place),
                new DataParameter("@yearMonth", @YearMonth), new DataParameter("@IsCommercial", @IsCommercial),
                new DataParameter("@IsAnimal", @IsAnimal), new DataParameter("@IsHealth", @IsHealth),
                new DataParameter("@Remark", @Remark), new DataParameter("@DiseaseFee", @DiseaseFee),
                new DataParameter("@DisinfectFee", @DisinfectFee), new DataParameter("@DiseaseChequeNo", @DiseaseChequeNo),
                new DataParameter("@DisinfectChequeNo", @DisinfectChequeNo), new DataParameter("@Mobile", @Mobile),
                new DataParameter("@Contacter",@Contacter));
        }

        public static bool Receipt_Update(this DataConnection dataConnection, ReceiptModel receiptModel)
        {
            return dataConnection.Receipt_Update(receiptModel.ID, receiptModel.Client, receiptModel.Company, receiptModel.Production, receiptModel.BLNO, receiptModel.Container, receiptModel.AnimalNo
                , receiptModel.Place, receiptModel.IsCommercial, receiptModel.IsAnimal, receiptModel.IsHealth, receiptModel.Remark, receiptModel.DiseaseFee,
                receiptModel.DisinfectFee, receiptModel.DiseaseChequeNo, receiptModel.DisinfectChequeNo, receiptModel.Mobile, receiptModel.YearMonth,receiptModel.Contacter) > 0 ? true : false;
        }


        public static bool Receipt_Delete(this ITable<ReceiptModel> Receipt, int ID)
        {
            return Receipt.Where(u => u.ID == ID).Delete() > 0 ? true : false;
        }

        #endregion
    }
}
