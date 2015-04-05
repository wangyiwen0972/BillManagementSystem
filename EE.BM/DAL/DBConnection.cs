using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB;
using EE.BM.Model;

namespace EE.BM.DAL
{
    public partial class BMConnection : DataConnection
    {
        public ITable<UserModel> LoginUser { get { return this.GetTable<UserModel>(); } }

        public ITable<ReceiptModel> Receipts { get { return this.GetTable<ReceiptModel>(); } }

        public ITable<RightModel> Rights { get { return this.GetTable<RightModel>(); } }

        public BMConnection()
            : base("name=database")
        {
            InitDataContext();
        }

        partial void InitDataContext();
    }
}
