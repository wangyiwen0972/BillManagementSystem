using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using EE.BM.DAL;
namespace EE.BM.Model
{
    [Table("t_User")]
    public class UserModel : DataModel
    {
        [Column, PrimaryKey, Identity]
        public int ID { get; set; }
        [Column, NotNull]
        public string LoginName { get; set; }
        [Column, NotNull]
        public string Password { get; set; }
        [Column, Nullable]
        public DateTime? LastLoginDate { get; set; }
        [Column, NotNull]
        public int Right_ID { get; set; }

        private RightModel right;
        [NotColumn]
        public RightModel Right
        {
            get 
            { 
                if(right == null) right = CreateDataConnection().GetTable<RightModel>().Find(this.Right_ID);
                return right;
            }
        }
    }
}
