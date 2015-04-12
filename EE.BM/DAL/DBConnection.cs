using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB;
using EE.BM.Model;
using System.Configuration;
using LinqToDB.DataProvider.Access;
using System.IO;
using EE.BM.Utility;
namespace EE.BM.DAL
{
    public partial class BMConnection : DataConnection
    {
        private static AccessDataProvider provider = new AccessDataProvider();

        private const string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User Id=admin;Password=";

        public ITable<UserModel> LoginUser { get { return this.GetTable<UserModel>(); } }

        public ITable<ReceiptModel> Receipts { get { return this.GetTable<ReceiptModel>(); } }

        public ITable<RightModel> Rights { get { return this.GetTable<RightModel>(); } }

        public BMConnection(string dbFullPath)
            : base(provider, string.Format(connectionString, dbFullPath))
        {
            InitDataContext();
            if (!File.Exists(dbFullPath))
            {
                provider.CreateDatabase(dbFullPath);
                ITable<UserModel> userTable = this.CreateTable<UserModel>();
                this.CreateTable<ReceiptModel>();
                ITable<RightModel> rightTable = this.CreateTable<RightModel>();
                rightTable.New("管理员","administrator","拥有最高权限");
                rightTable.New("员工","Staff","");
                rightTable.New("宾客","Guest","");
                userTable.New("admin", "admin", "管理员", 1);
            }
        }

        partial void InitDataContext();
        
    }
}
