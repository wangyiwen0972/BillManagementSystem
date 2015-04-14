using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.DAL;
using EE.BM.Model;
using EE.BM.Utility;

namespace EE.BM
{
    public class ReceiptViewModel : NotificationObject
    {
        #region private variables
        private ReceiptModel receipt = null;


        #endregion

        #region View Model Properties
        private int inputer;
        /// <summary>
        /// 录入员
        /// </summary>
        public int Inputer
        {
            get { return inputer; }
            set { base.SetProperty<int>(ref inputer, value, () => this.Inputer); }
        }

        private DateTime inputDate;
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime InputDate
        {
            get { return inputDate; }
            set { base.SetProperty<DateTime>(ref inputDate, value, () => this.InputDate); }
        }
        /// <summary>
        /// 单证状态
        /// </summary>
        private ReceiptStatus status;

        public ReceiptStatus Status
        {
            get { return status; }
            set { base.SetProperty<ReceiptStatus>(ref status, value, () => this.Status); }
        }
        private DateTime date;
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return receipt.Date; }
            set 
            {
                base.SetProperty<DateTime>(ref date, value, () => this.receipt.Date);
            }
        }
        private string port;
        /// <summary>
        /// 港口
        /// </summary>
        public string Port
        {
            get { return port; }
            set { base.SetProperty<string>(ref port, value, () => this.receipt.Port); }
        }

        private string company;
        /// <summary>
        /// 经营单位
        /// </summary>
        public string Company
        {
            get { return this.receipt.Company; }
            set { base.SetProperty<string>(ref company, value, () => this.receipt.Company); }
        }

        private string production;
        /// <summary>
        /// 产品
        /// </summary>
        public string Production
        {
            get { return this.receipt.Production; }
            set { base.SetProperty<string>(ref production, value, () => this.receipt.Production); }
        }

        private string container;
        /// <summary>
        /// 箱量
        /// </summary>
        public string Container
        {
            get { return this.receipt.Container; }
            set { base.SetProperty<string>(ref container, value, () => this.receipt.Container); }
        }

        private string blno;
        /// <summary>
        /// 提单号
        /// </summary>
        public string BLNO
        {
            get { return this.receipt.BLNO; }
            set { base.SetProperty<string>(ref blno, value, () => this.receipt.BLNO); }
        }

        private string contacter;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter
        {
            get { return this.receipt.Contacter; }
            set { base.SetProperty<string>(ref contacter, value, () => this.receipt.Contacter); }
        }

        private string mobile;

        public string Mobile
        {
            get { return mobile; }
            set { base.SetProperty<string>(ref mobile, value, () => this.receipt.Mobile); }
        }

        private string remark;

        public string Remark
        {
            get { return this.receipt.Remark; }
            set { base.SetProperty<string>(ref remark, value, () => this.receipt.Remark); }
        }


        #endregion

        #region View Model List Properties
        

        #endregion

        public enum ReceiptStatus
        {
            
        }
    }
}
