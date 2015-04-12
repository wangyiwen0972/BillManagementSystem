using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using LinqToDB.Data;
using EE.BM.DAL;
using System.Configuration;

namespace EE.BM.Model
{
    public class DataModel
    {
        protected static BMConnection dbConnection = null;
        public static BMConnection CreateDataConnection()
        {
            if (dbConnection == null)
            {
                dbConnection = new BMConnection(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationSettings.AppSettings["database"].ToString()));
            }
            
            return dbConnection;
        }
    }

    [Table("t_Right")]
    public class RightModel : DataModel
    {
        [Column,PrimaryKey, Identity]
        public int ID { get; set; }
        [Column,NotNull]
        public string C_Name { get; set; }
        [Column,NotNull]
        public string E_Name { get; set; }
        [Column,Nullable]
        public string Comments { get; set; }
    }
    

    [Flags]
    public enum Rights
    {
        Administrator = 4,
        Staff = 2,
        Guest = 1
    }
}
