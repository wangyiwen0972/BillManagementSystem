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

            //get vm lists from database
            InitializePropertyList();
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
        private string yearMonth;
        /// <summary>
        /// 日期
        /// </summary>
        public string YearMonth
        {
            get { return receipt.YearMonth; }
            set 
            {
                if (string.IsNullOrEmpty(value)) return;

                string date = DateTime.Parse(value).ToString("yyyy-MM");

                if (GetCurrentStatus() == ViewModelStatus.NewRecord && !connection.GetTable<ReceiptModel>().IsDuplicateDate(date))
                {
                    base.SetProperty<string>(ref yearMonth, date, () => this.receipt.YearMonth);

                    if (!yearMonth.Equals(this.receipt.YearMonth))
                    {
                        this.receipt.YearMonth = date;

                        if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    }
                }
                else
                {
                    OutputMessage += "该日期记录已经存在，请重新选择日期！" + Environment.NewLine;
                    //this.SetCurrentStats(ViewModelStatus.Invaild);
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
        /// <summary>
        /// 联系电话
        /// </summary>
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
        //备注
        public string Remark
        {
            get { return remark; }
            set 
            {
                base.SetProperty<string>(ref remark, value, () => this.Remark);
                if (!string.IsNullOrEmpty(remark) && !remark.Equals(this.receipt.Remark))
                {
                    this.receipt.Remark = remark;
                    if(GetCurrentStatus()!= ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string message;
        /// <summary>
        /// 输出信息
        /// </summary>
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

        private string diseaseFee;
        /// <summary>
        /// 检疫费
        /// </summary>
        public string DiseaseFee
        {
            get { return diseaseFee; }
            set
            {
                base.SetProperty<string>(ref diseaseFee, value, () => this.DiseaseFee);
                if (!string.IsNullOrEmpty(diseaseFee) && !diseaseFee.Equals(this.receipt.DiseaseFee))
                {
                    decimal fee = 0;
                    if (decimal.TryParse(value, out fee))
                    {
                        this.receipt.DiseaseFee = fee;
                        if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    }
                    else
                    {
                        this.SetCurrentStats(ViewModelStatus.Invaild);
                    }
                }
            }
        }

        private string disinfectFee;
        /// <summary>
        /// 消毒费
        /// </summary>
        public string DisinfectFee
        {
            get { return disinfectFee; }
            set 
            { 
                base.SetProperty<string>(ref disinfectFee, value, () => this.DisinfectFee);
                if (!string.IsNullOrEmpty(disinfectFee) && !disinfectFee.Equals(this.receipt.DisinfectFee))
                {
                    decimal fee = 0;
                    if (decimal.TryParse(value, out fee))
                    {
                        this.receipt.DisinfectFee = fee;
                        if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    }
                    else
                    {
                        this.SetCurrentStats(ViewModelStatus.Invaild);
                    }
                }
            }
        }

        private string disinfectChequeNo;
        /// <summary>
        /// 消毒支票号码
        /// </summary>
        public string DisinfectChequeNo
        {
            get { return disinfectChequeNo; }
            set 
            { 
                base.SetProperty<string>(ref disinfectChequeNo, value, () => this.DisinfectChequeNo);
                if (!string.IsNullOrEmpty(disinfectChequeNo) && !disinfectChequeNo.Equals(this.receipt.DisinfectChequeNo))
                {
                    this.receipt.DisinfectChequeNo = value;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private string diseaseChequeNo;
        /// <summary>
        /// 检疫支票号码
        /// </summary>
        public string DiseaseChequeNo
        {
            get { return diseaseChequeNo; }
            set 
            { 
                base.SetProperty<string>(ref diseaseChequeNo, value, () => this.DiseaseChequeNo);
                if (!string.IsNullOrEmpty(diseaseChequeNo) && !diseaseChequeNo.Equals(this.receipt.DiseaseChequeNo))
                {
                    this.receipt.DiseaseChequeNo = value;
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                }
            }
        }

        private bool isAnimal;

        public bool IsAnimal
        {
            get { return isAnimal; }
            set 
            { 
                base.SetProperty<bool>(ref isAnimal, value, () => this.IsAnimal);
                if (this.receipt.IsAnimal != value) this.receipt.IsAnimal = value;
            }
        }

        private bool isCommercial;

        public bool IsCommercial
        {
            get { return isCommercial; }
            set 
            { 
                base.SetProperty<bool>(ref isCommercial, value, () => this.IsCommercial);
                if (this.receipt.IsCommercial != value) this.receipt.IsCommercial = value;
            }
        }

        private bool isHealth;

        public bool IsHealth
        {
            get { return isHealth; }
            set 
            { 
                base.SetProperty<bool>(ref isHealth, value, () => this.IsHealth);
                if (this.receipt.IsHealth != value) this.receipt.IsHealth = value;
            }
        }

        private string animalNo;

        public string AnimalNo
        {
            get { return animalNo; }
            set 
            { 
                base.SetProperty<string>(ref animalNo, value, () => this.AnimalNo);
                if (!string.IsNullOrEmpty(animalNo) && !animalNo.Equals(this.receipt.AnimalNo))
                {
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    this.receipt.AnimalNo = value;
                }
            }
        }

        private string commercialNo;

        public string CommercialNo
        {
            get { return commercialNo; }
            set 
            { 
                base.SetProperty<string>(ref commercialNo, value, () => this.CommercialNo);
                if (!string.IsNullOrEmpty(commercialNo) && !commercialNo.Equals(this.receipt.CommercialNo))
                {
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    this.receipt.CommercialNo = value;
                }
            }
        }

        private string healthNo;

        public string HealthNo
        {
            get { return healthNo; }
            set 
            { 
                base.SetProperty<string>(ref healthNo, value, () => this.HealthNo);
                if (!string.IsNullOrEmpty(healthNo) && !healthNo.Equals(this.receipt.HealthNo))
                {
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    this.receipt.HealthNo = value;
                }
            }
        }

        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                base.SetProperty<string>(ref year, value, () => this.Year);
            }
        }

        #endregion

        #region View Model List Properties
        private ObservableCollection<object> companyList = new ObservableCollection<object>();
        public ObservableCollection<object> CompanyList
        {
            get { return companyList; }
            set
            {
                base.SetProperty(ref companyList, value, () => CompanyList);
            }
        }

        private ObservableCollection<object> portList = new ObservableCollection<object>();
        public ObservableCollection<object> PortList
        {
            get { return portList; }
            set
            {
                base.SetProperty(ref portList, value, () => this.PortList);
            }
        }

        private ObservableCollection<object> productList = new ObservableCollection<object>();
        public ObservableCollection<object> ProductLsit
        {
            get { return productList; }
            set
            {
                base.SetProperty(ref productList, value, () => ProductLsit);
            }
        }

        private ObservableCollection<object> clientList = new ObservableCollection<object>();
        public ObservableCollection<object> ClientList
        {
            get { return clientList; }
            set
            {
                base.SetProperty(ref clientList, value, () => ClientList);
            }
        }

        private ObservableCollection<ReceiptModel> receiptList = new ObservableCollection<ReceiptModel>();
        public ObservableCollection<ReceiptModel> ReceiptList
        {
            get { return receiptList; }
            set
            {
                base.SetProperty(ref receiptList, value, () => ReceiptList);
            }
             
        }

        private ObservableCollection<object> yearList = new ObservableCollection<object>();

        public ObservableCollection<object> YearList
        {
            get { return yearList; }
            set { base.SetProperty<ObservableCollection<object>>(ref yearList, value, () => this.YearList); }
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

        public DelegateCommand SearchReceiptCommand
        {
            get
            {
                return new DelegateCommand()
                {
                    ExecuteCommand = new Action<object>(filterReceiptByYear)
                };
            }
        }

        #endregion

        #region private methods
        /// <summary>
        /// 保存数据到数据库
        /// </summary>
        /// <param name="parameter"></param>
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
                            if (connection.GetTable<ReceiptModel>().Receipt_Insert(ref this.receipt))
                            {
                                this.SetCurrentStats(ViewModelStatus.Saved);
                            }
                        }
                        else
                        {
                            if (connection.Receipt_Update(this.receipt))
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
        /// <summary>
        /// 从数据库中删除数据
        /// </summary>
        /// <param name="parameter"></param>
        private void delete(object parameter)
        {
        }

        private void InitializePropertyList()
        {
            LinqToDB.ITable<ReceiptModel> iTable = connection.GetTable<ReceiptModel>();

            foreach (object port in iTable.GetPortList())
            {
                this.portList.Add(port);
            }
            foreach (object company in iTable.GetCompanyList())
            {
                this.companyList.Add(company);
            }
            foreach (object client in iTable.GetClientList())
            {
                this.clientList.Add(client);
            }
            foreach (object production in iTable.GetProductionList())
            {
                this.clientList.Add(production);
            }
        }

        private void filterReceiptByYear(object parameter)
        {
            if (parameter == null)
            {

                    foreach (var year in connection.GetTable<ReceiptModel>().GetYearList())
                    {
                        this.yearList.Add(year);
                    }
                
            }
            else
            {
                foreach (var receipt in connection.GetTable<ReceiptModel>().Receipt_FindByYear(parameter.ToString()))
                {
                    this.receiptList.Add(receipt);
                }
            }
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
