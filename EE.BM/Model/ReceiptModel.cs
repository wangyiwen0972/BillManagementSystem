using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using EE.BM.DAL;

namespace EE.BM.Model
{
    public class ReceiptModel:DataModel,IModel
    {
        /// <summary>
        /// 唯一序号
        /// </summary>
        [Column,PrimaryKey,Identity]
        public int ID { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        [Column,Nullable]
        public string Client { get; set; }
        /// <summary>
        /// 经营单位
        /// </summary>
        [Column, Nullable]
        public string Company { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        [Column,Nullable]
        public string Production { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        [Column, Nullable]
        public string BLNO { get; set; }
        /// <summary>
        /// 箱量
        /// </summary>
        [Column, Nullable]
        public string Container { get; set; }
        /// <summary>
        /// 动检号
        /// </summary>
        [Column, Nullable]
        public string AnimalNo { get; set; }
        /// <summary>
        /// 场地
        /// </summary>
        [Column, Nullable]
        public string Place { get; set; }
        /// <summary>
        /// 是否商检
        /// </summary>
        [Column, Nullable]
        public bool IsCommercial { get; set; }
        /// <summary>
        /// 是否动检
        /// </summary>
        [Column, Nullable]
        public bool IsAnimal { get; set; }
        /// <summary>
        /// 是否卫检
        /// </summary>
        [Column, Nullable]
        public bool IsHealth { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column, Nullable]
        public string Remark { get; set; }
        /// <summary>
        /// 检疫费
        /// </summary>
        [Column,Nullable]
        public decimal DiseaseFee { get; set; }
        /// <summary>
        /// 消毒费
        /// </summary>
        [Column, Nullable]
        public decimal DisinfectFee { get; set; }
        /// <summary>
        /// 检疫支票号码
        /// </summary>
        [Column, Nullable]
        public string DiseaseChequeNo { get; set; }
        /// <summary>
        /// 消毒支票号码
        /// </summary>
        [Column, Nullable]
        public string DisinfectChequeNo { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Column, Nullable]
        public string Mobile { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Column, Nullable]
        public DateTime Date { get; set; }
        /// <summary>
        /// 港口
        /// </summary>
        [Column, Nullable]
        public string Port { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Column, Nullable]
        public string Contacter { get; set; }



        public bool IsVaild()
        {
            return true;
        }
    }
}
