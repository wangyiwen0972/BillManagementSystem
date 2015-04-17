using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.DAL;
using EE.BM.Model;
using EE.BM.Utility;

namespace EE.BM
{
    public class ReceiptViewModel : NotificationObject,IViewModel
    {
        #region private variables
        //record model mainly
        private ReceiptModel receipt = null;
        //user model who login
        private UserModel loginUser = null;
        //basic connection of database
        private BMConnection connection = null;

        #endregion

        public ReceiptViewModel(UserModel user)
        {
            loginUser = user;
            receipt = new ReceiptModel();
            connection = DataModel.CreateDataConnection();

            SetCurrentStats(ViewModelStatus.NewRecord);
        }

        public ReceiptViewModel(UserModel user, ReceiptModel receipt):this(user)
        {
            SetCurrentStats(ViewModelStatus.NoChange);
        }

        #region View Model Properties
        /// <summary>
        /// 录入员
        /// </summary>
        public string Inputer
        {
            get { return loginUser.LoginName; }
        }

        private string inputDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        /// <summary>
        /// 录入时间
        /// </summary>
        public string InputDate
        {
            get { return inputDate; }
        }
        /// <summary>
        /// 单证状态
        /// </summary>
        private string status;

        public string Status
        {
            get 
            {
                return GetCurrentStatus().ToString();
            }
            set 
            { 
                base.SetProperty<string>(ref status, value.ToString(), () => this.Status);
            }
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
                if (!date.Equals(this.receipt.Date))
                {
                    this.receipt.Date = value;

                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }
        private string port;
        /// <summary>
        /// 港口
        /// </summary>
        public string Port
        {
            get { return port; }
            set 
            {
                base.SetProperty<string>(ref port, value, () => Port);
                if (!string.IsNullOrEmpty(port) && !port.Equals(this.receipt.Port))
                {
                    this.receipt.Port = value;

                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
                
            }
        }

        private string company;
        /// <summary>
        /// 经营单位
        /// </summary>
        public string Company
        {
            get { return company; }
            set 
            { 
                base.SetProperty<string>(ref company, value, () => this.Company);
                if (!string.IsNullOrEmpty(company) && !company.Equals(this.receipt.Company))
                {
                    this.receipt.Company = company;

                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string production;
        /// <summary>
        /// 产品
        /// </summary>
        public string Production
        {
            get { return this.receipt.Production; }
            set 
            {
                base.SetProperty<string>(ref production, value, () => this.receipt.Production);
                if (!string.IsNullOrEmpty(production) && !production.Equals(this.receipt.Production))
                {
                    this.receipt.Production = production;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string container;
        /// <summary>
        /// 箱量
        /// </summary>
        public string Container
        {
            get { return this.receipt.Container; }
            set 
            { 
                base.SetProperty<string>(ref container, value, () => this.receipt.Container);
                if (!string.IsNullOrEmpty(container) && !container.Equals(this.receipt.Container))
                {
                    this.receipt.Container = container;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string blno;
        /// <summary>
        /// 提单号
        /// </summary>
        public string BLNO
        {
            get { return this.receipt.BLNO; }
            set 
            { 
                base.SetProperty<string>(ref blno, value, () => this.receipt.BLNO);
                if (!string.IsNullOrEmpty(blno) && !blno.Equals(this.receipt.BLNO))
                {
                    this.receipt.BLNO = blno;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string contacter;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter
        {
            get { return this.receipt.Contacter; }
            set 
            { 
                base.SetProperty<string>(ref contacter, value, () => this.receipt.Contacter);
                if (!string.IsNullOrEmpty(contacter) && !contacter.Equals(this.receipt.Contacter))
                {
                    this.receipt.Contacter = contacter;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string mobile;

        public string Mobile
        {
            get { return mobile; }
            set 
            { 
                base.SetProperty<string>(ref mobile, value, () => this.receipt.Mobile);
                if (!string.IsNullOrEmpty(mobile) && !mobile.Equals(this.receipt.Mobile))
                {
                    this.receipt.Mobile = mobile;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string remark;

        public string Remark
        {
            get { return remark; }
            set 
            {
                base.SetProperty<string>(ref remark, value, () => this.Remark);
                if (!string.IsNullOrEmpty(this.receipt.Remark) && !remark.Equals(this.receipt.Remark))
                {
                    this.receipt.Remark = remark;
                    if(GetCurrentStatus()!= ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string message;

        public string OutputMessage
        {
            get
            {
                return message;
            }
            set
            {
                base.SetProperty<string>(ref message, value, () => this.OutputMessage);
            }
        }

        #endregion

        #region View Model List Properties
        private ObservableCollection<object> companyList = null;
        public ObservableCollection<object> CompanyList
        {
            get { return companyList; }
            set
            {
                base.SetProperty(ref companyList, value, () => CompanyList);
            }
        }

        private ObservableCollection<object> portList = null;
        public ObservableCollection<object> PortList
        {
            get { return portList; }
            set
            {
                base.SetProperty(ref portList, value, () => this.PortList);
            }
        }

        private ObservableCollection<object> productList = null;
        public ObservableCollection<object> ProductLsit
        {
            get { return productList; }
            set
            {
                base.SetProperty(ref productList, value, () => ProductLsit);
            }
        }

        #endregion

        #region View Model Commands
        /// <summary>
        /// 新建
        /// </summary>
        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand()
                {

                };
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand()
                {

                };
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand()
                {
                    ExecuteCommand = new Action<object>(save)
                };
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        public DelegateCommand ExportToExcelCommand
        {
            get
            {
                return new DelegateCommand()
                {

                };
            }
        }

        #endregion

        #region private methods
        private void save(object parameter)
        {
            ViewModelStatus status = this.GetCurrentStatus();
            //this is a new record, or a record changed
            if (status == ViewModelStatus.NewRecord || status == ViewModelStatus.Changed)
            {
                try
                {
                    //check there a neccessary field not set
                    if (this.receipt.IsVaild())
                    {
                        //connect to the db, and get an update
                        if (status == ViewModelStatus.NewRecord)
                        {
                            if (connection.GetTable<ReceiptModel>().NewReceipt(this.receipt))
                            {
                                this.SetCurrentStats(ViewModelStatus.Saved);
                            }
                        }
                        else
                        {
                            if (connection.GetTable<ReceiptModel>().UpdateReceipt(this.receipt))
                            {
                                this.SetCurrentStats(ViewModelStatus.Saved);
                            }
                        }
                    }
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        public void delete(object parameter)
        {
        }

        #endregion

        public ViewModelStatus GetCurrentStatus()
        {
            if (!string.IsNullOrEmpty(status))
            {
                ViewModelStatus vmStatus = ViewModelStatus.Invaild;
                Enum.TryParse<ViewModelStatus>(status, out vmStatus);

                return vmStatus;
            }
            return ViewModelStatus.Invaild;
        }



        public void SetCurrentStats(ViewModelStatus vmStatus)
        {
            this.Status = vmStatus.ToString();
        }


        public UserModel GetCurrentLoginUser()
        {
            return this.loginUser;
        }
    }
}
