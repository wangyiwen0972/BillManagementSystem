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
        /// <summary>
        /// search by ID
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static ReceiptModel Receipt_Find(this ITable<ReceiptModel> Receipt, int ID)
        {
            return Receipt.FirstOrDefault(r=>r.ID == ID);
        }
        /// <summary>
        /// Search by single number
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="SingleNo"></param>
        /// <returns></returns>
        public static ReceiptModel Receipt_Find(this ITable<ReceiptModel> Receipt, string SingleNo)
        {
            return Receipt.FirstOrDefault(r => r.SingleNo == SingleNo);
        }
        /// <summary>
        /// Insert a new record
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="Client"></param>
        /// <param name="Company"></param>
        /// <param name="Production"></param>
        /// <param name="BLNO"></param>
        /// <param name="Container"></param>
        /// <param name="AnimalNo"></param>
        /// <param name="Place"></param>
        /// <param name="IsCommercial"></param>
        /// <param name="IsAnimal"></param>
        /// <param name="IsHealth"></param>
        /// <param name="Remark"></param>
        /// <param name="DiseaseFee"></param>
        /// <param name="DisinfectFee"></param>
        /// <param name="DiseaseChequeNo"></param>
        /// <param name="DisinfectChequeNo"></param>
        /// <param name="Mobile"></param>
        /// <param name="Date"></param>
        /// <param name="Port"></param>
        /// <param name="Contacter"></param>
        /// <param name="HealthNo"></param>
        /// <param name="CommercialNo"></param>
        /// <param name="InputTime"></param>
        /// <param name="InputerID"></param>
        /// <returns></returns>
        private static ReceiptModel Receipt_Insert(this ITable<ReceiptModel> Receipt,string SingleNo, string Client, string Company, string Production,
            string BLNO, string Container, string AnimalNo, string Place, bool IsCommercial, bool IsAnimal, bool IsHealth,
            string Remark, decimal DiseaseFee, decimal DisinfectFee, string DiseaseChequeNo, string DisinfectChequeNo,
            string Mobile, string Date, string Port, string Contacter, string HealthNo, string CommercialNo,DateTime InputTime, int InputerID)
        {
            try
            {
                int iResult = Receipt.Insert(() => new ReceiptModel()
                {
                    SingleNo = SingleNo,
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
                    DisinfectChequeNo = DisinfectChequeNo,
                    HealthNo = HealthNo,
                    CommercialNo = CommercialNo,
                    InputTime = InputTime,
                    InputerID = InputerID
                });
                ReceiptModel current = Receipt.Single((e) => e.InputerID == InputerID && e.InputTime == InputTime);

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
            ReceiptModel newRecord = Receipt.Receipt_Insert(receiptModel.SingleNo, receiptModel.Client, receiptModel.Company, receiptModel.Production, receiptModel.BLNO, receiptModel.Container, receiptModel.AnimalNo
                , receiptModel.Place,receiptModel.IsCommercial,receiptModel.IsAnimal,receiptModel.IsHealth,receiptModel.Remark,receiptModel.DiseaseFee,
                receiptModel.DisinfectFee,receiptModel.DiseaseChequeNo,receiptModel.DisinfectChequeNo,receiptModel.Mobile,receiptModel.YearMonth,receiptModel.Port,receiptModel.Contacter,
                receiptModel.HealthNo,receiptModel.CommercialNo,receiptModel.InputTime,receiptModel.InputerID);

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
        /// <summary>
        /// update a exist record
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="receiptModel"></param>
        /// <returns></returns>
        public static bool Receipt_Update(this ITable<ReceiptModel> Receipt, ReceiptModel receiptModel)
        {
            //return Receipt.Where(u => u.ID == receiptModel.ID).Update((u) => receiptModel) > 0 ? true : false;
            return Receipt.Update(r => r.ID == receiptModel.ID, r => receiptModel) > 0 ? true : false;
            
        }
        /// <summary>
        /// update a exist record
        /// </summary>
        public static int Receipt_Update(this DataConnection dataConnection, int? @id, string @Client, string @Company, string @Production,
            string @BLNO, string @Container, string @AnimalNo, string @Place, bool @IsCommercial, bool @IsAnimal, bool @IsHealth,
            string @Remark, decimal @DiseaseFee, decimal @DisinfectFee, string @DiseaseChequeNo, string @DisinfectChequeNo,
            string @Mobile, string @YearMonth, string @Contacter, string @Port, string @CommercialNo, string @HealthNo)
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
                new DataParameter("@Contacter",@Contacter), new DataParameter("@Port",@Port),
                new DataParameter("@CommercialNo",@CommercialNo), new DataParameter("@HealthNo",@HealthNo));
        }
        /// <summary>
        /// update a exist record
        /// </summary>
        public static bool Receipt_Update(this DataConnection dataConnection, ReceiptModel receiptModel)
        {
            return dataConnection.Receipt_Update(receiptModel.ID, receiptModel.Client, receiptModel.Company, receiptModel.Production, receiptModel.BLNO, receiptModel.Container, receiptModel.AnimalNo
                , receiptModel.Place, receiptModel.IsCommercial, receiptModel.IsAnimal, receiptModel.IsHealth, receiptModel.Remark, receiptModel.DiseaseFee,
                receiptModel.DisinfectFee, receiptModel.DiseaseChequeNo, receiptModel.DisinfectChequeNo, receiptModel.Mobile, receiptModel.YearMonth,
                receiptModel.Contacter,receiptModel.Port,receiptModel.CommercialNo,receiptModel.HealthNo) > 0 ? true : false;
        }

        /// <summary>
        /// delete a exist record
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Receipt_Delete(this ITable<ReceiptModel> Receipt, int ID)
        {
            return Receipt.Where(u => u.ID == ID).Delete() > 0 ? true : false;
        }
        
        /// <summary>
        /// get port list from database
        /// </summary>
        /// <param name="Receipt"></param>
        /// <returns></returns>
        public static ICollection<object> GetPortList(this ITable<ReceiptModel> Receipt)
        {
            List<object> portList = new List<object>();

            var query = Receipt.GroupBy(x => new { x.Port }).Select(g => g.Key);

            foreach (var q in query)
            {
                if (string.IsNullOrEmpty(q.Port)) continue;
                portList.Add(Helper.CreateKeyValueObject(q.Port,q.Port));
            }

            return portList;
        }

        public static ICollection<object> GetClientList(this ITable<ReceiptModel> Receipt)
        {
            List<object> portList = new List<object>();

            var query = Receipt.GroupBy(x => new { x.Client }).Select(g => g.Key);

            foreach (var q in query)
            {
                if (string.IsNullOrEmpty(q.Client)) continue;
                portList.Add(Helper.CreateKeyValueObject(q.Client, q.Client));
            }

            return portList;
        }

        public static ICollection<object> GetCompanyList(this ITable<ReceiptModel> Receipt)
        {
            List<object> portList = new List<object>();

            var query = Receipt.GroupBy(x => new { x.Company }).Select(g => g.Key);

            foreach (var q in query)
            {
                if (string.IsNullOrEmpty(q.Company)) continue;
                portList.Add(Helper.CreateKeyValueObject(q.Company, q.Company));
            }

            return portList;
        }

        public static ICollection<object> GetProductionList(this ITable<ReceiptModel> Receipt)
        {
            List<object> portList = new List<object>();

            var query = Receipt.GroupBy(x => new { x.Production }).Select(g => g.Key);

            foreach (var q in query)
            {
                if (string.IsNullOrEmpty(q.Production)) continue;
                portList.Add(Helper.CreateKeyValueObject(q.Production, q.Production));
            }

            return portList;
        }

        public static ICollection<object> GetYearList(this ITable<ReceiptModel> Receipt)
        {
            List<object> yearList = new List<object>();

            var query = Receipt.GroupBy(x => new { x.YearMonth }).Select(g => g.Key).OrderByDescending(g=>g.YearMonth);

            string pYear = string.Empty;

            foreach (var q in query)
            {
                if (string.IsNullOrEmpty(q.YearMonth)) continue;
                if (q.YearMonth.IndexOfAny(new char[] { '/', '-' }) > -1)
                {
                    string year = q.YearMonth.Split(new char[] { '/', '-' })[0];
                    if (!pYear.Equals(year))
                    {
                        yearList.Add(Helper.CreateKeyValueObject(year, year));
                        pYear = year;
                    }
                }
            }

            return yearList;
        }

        public static bool IsDuplicateDate(this ITable<ReceiptModel>Receipt, string date)
        {
            var query = Receipt.Where(x => x.YearMonth == date);

            return query.Any();
        }

        public static ICollection<ReceiptModel> Receipt_FindByYear(this ITable<ReceiptModel> Receipt, string Year)
        {
            List<ReceiptModel> receiptList = new List<ReceiptModel>();
            var query = Receipt.Where(r => r.YearMonth.Contains(Year)).OrderByDescending(x=>x.YearMonth);
            
            foreach (var q in query)
            {
                receiptList.Add(q);
            }

            return receiptList;
        }

        #endregion
    }
}
