namespace EE.BM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EE.BM.DAL;
    using EE.BM.Model;
    using EE.BM.Utility;
    using EE.BM.Export;
    using System.IO;

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

        #region contructors
        public ReceiptViewModel(UserModel user)
        {
            loginUser = user;
            receipt = new ReceiptModel()
            {
                SingleNo = Helper.GetTimeTicks(),
                InputerID = loginUser.ID,
                InputTime = DateTime.Now                
            };
            connection = DataModel.CreateDataConnection();

            SetCurrentStats(ViewModelStatus.NewRecord);

            //get vm lists from database
            InitializePropertyList();

            NewCommand = new DelegateCommand("NewCommand") { ExecuteCommand = new Action<object>(newReceipt) };
        }
        
        public ReceiptViewModel(UserModel user, ReceiptModel receipt):this(user)
        {
            this.receipt = receipt;
            this.loadReceipt(null);
            //set the default status
            this.SetCurrentStats(ViewModelStatus.NoChange);
        }
        #endregion

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
            set 
            {
                base.SetProperty<string>(ref inputDate, value, () => this.InputDate);
            }
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

                string date = DateTime.Parse(value).ToString("yyyy-MM-dd");

                //if (GetCurrentStatus() == ViewModelStatus.NewRecord && !connection.GetTable<ReceiptModel>().IsDuplicateDate(date))
                //{
                    base.SetProperty<string>(ref yearMonth, date, () => this.receipt.YearMonth);

                    if (!yearMonth.Equals(this.receipt.YearMonth))
                    {
                        this.receipt.YearMonth = date;

                        if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    }
                //}
                //else
                //{
                //    OutputMessage += "该日期记录已经存在，请重新选择日期！" + Environment.NewLine;
                //    //this.SetCurrentStats(ViewModelStatus.Invaild);
                //}
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
            get { return blno; }
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
                        ShowMessage("检疫费只能填写数字类型");
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
                        ShowMessage("消毒费只能填写数字类型");
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

        private string place;

        public string Place
        {
            get { return place; }
            set 
            { 
                base.SetProperty<string>(ref place, value, () => this.Place);
                if (!string.IsNullOrEmpty(place) && !place.Equals(this.receipt.Place))
                {
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    this.receipt.Place = value;
                }
            }
        }

        private string client;

        public string Client
        {
            get { return client; }
            set 
            { 
                base.SetProperty<string>(ref client, value, () => this.Client);
                if (!string.IsNullOrEmpty(client) && !client.Equals(this.receipt.Client))
                {
                    if (GetCurrentStatus() != ViewModelStatus.NewRecord) this.SetCurrentStats(ViewModelStatus.Changed);
                    this.receipt.Client = value;
                }
            }
        }

        private string singleNo;

        public string SingleNo
        {
            get { return receipt.SingleNo; }
            set 
            { 
                base.SetProperty<string>(ref singleNo, value, () => this.SingleNo);
                receipt.SingleNo = value;
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
        private DelegateCommand newCommand;
        
        /// <summary>
        /// 新建
        /// </summary>
        [PermissionAttribute(1, BM.Action.Executable)]
        [PermissionAttribute(2, BM.Action.Executable)]
        [PermissionAttribute(3, BM.Action.Visible)]
        public DelegateCommand NewCommand
        {
            get
            {
                return newCommand;
            }
            set
            {
                newCommand = value;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Visible)]
        [Permission(3, BM.Action.Invisible)]
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand("DeleteCommand")
                {
                    ExecuteCommand = new Action<object>(delete)
                };
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Executable)]
        [Permission(3, BM.Action.Invisible)]
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand("SaveCommand")
                {
                    ExecuteCommand = new Action<object>(save)
                };
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Visible)]
        [Permission(3, BM.Action.Invisible)]
        public DelegateCommand ExportToExcelCommand
        {
            get
            {
                return new DelegateCommand("ExportToExcelCommand")
                {
                    ExecuteCommand = new Action<object>(exportToExcel)
                };
            }
        }
        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Visible)]
        [Permission(3, BM.Action.Invisible)]
        public DelegateCommand SearchReceiptCommand
        {
            get
            {
                return new DelegateCommand("SearchReceiptCommand")
                {
                    ExecuteCommand = new Action<object>(filterReceiptByYear)
                };
            }
        }
        /*
        public DelegateCommand LocateReceiptCommand
        {
            get
            {
                return new DelegateCommand("LocateReceiptCommand")
                {
                };
            }
        }
         * */

        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Visible)]
        [Permission(3, BM.Action.Invisible)]
        public DelegateCommand PreviewCommand
        {
            get
            {
                return new DelegateCommand("PreviewCommand")
                {
                };
            }
        }

        [Permission(1, BM.Action.Executable)]
        [Permission(2, BM.Action.Executable)]
        [Permission(3, BM.Action.Visible)]
        public DelegateCommand CopyReceiptCommand
        {
            get
            {
                return new DelegateCommand("CopyReceiptCommand")
                {
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
                                this.ShowMessage("数据保存成功！");
                            }
                            else
                            {
                                this.ShowMessage("新建数据失败！");
                            }
                        }
                        else
                        {
                            if (connection.Receipt_Update(this.receipt))
                            {
                                this.SetCurrentStats(ViewModelStatus.Saved);



                                this.ShowMessage("数据保存成功！");
                            }
                            else
                            {
                                this.ShowMessage("数据更新失败！");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("部分数据未填写，请检查数据！");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    InitializePropertyList();
                }
            }
        }
        /// <summary>
        /// 从数据库中删除数据
        /// </summary>
        /// <param name="parameter"></param>
        private void delete(object parameter)
        {
            try
            {
                connection.GetTable<ReceiptModel>().Receipt_Delete(this.receipt.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializePropertyList()
        {
            LinqToDB.ITable<ReceiptModel> iTable = connection.GetTable<ReceiptModel>();

            this.portList.Clear();
            this.companyList.Clear();
            this.clientList.Clear();
            this.productList.Clear();

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
                this.productList.Add(production);
            }
        }

        private void filterReceiptByYear(object parameter)
        {
            if (parameter == null)
            {
                this.yearList.Clear();

                foreach (var year in connection.GetTable<ReceiptModel>().GetYearList())
                {
                    this.yearList.Add(year);
                }

            }
            else
            {
                this.receiptList.Clear();
                foreach (var receipt in connection.GetTable<ReceiptModel>().Receipt_FindByYear(parameter.ToString()))
                {
                    this.receiptList.Add(receipt);
                }
            }
        }

        private void newReceipt(object parameter)
        {
            this.InputDate = DateTime.Now.ToString();

            receipt = new ReceiptModel()
            {
                SingleNo = Helper.GetTimeTicks(),
                InputerID = loginUser.ID,
                InputTime = DateTime.Now,
                YearMonth = this.YearMonth
            };
            connection = DataModel.CreateDataConnection();

            SetCurrentStats(ViewModelStatus.NewRecord);

            #region reset
            this.Remark = string.Empty;
            this.AnimalNo = string.Empty;
            this.BLNO = string.Empty;
            this.Client = string.Empty;
            this.CommercialNo = string.Empty;
            this.Company = string.Empty;
            this.Contacter = string.Empty;
            this.Container = string.Empty;
            this.DiseaseChequeNo = string.Empty;
            this.DiseaseFee = string.Empty;
            this.DisinfectChequeNo = string.Empty;
            this.DisinfectFee = string.Empty;
            this.HealthNo = string.Empty;
            this.IsAnimal = false;
            this.IsCommercial = false;
            this.IsHealth = false;
            this.OutputMessage = string.Empty;
            this.Mobile = string.Empty;
            this.Place = string.Empty;
            this.Port = string.Empty;
            this.Production = string.Empty;
            #endregion
        }

        private void loadReceipt(object parameter)
        {
            this.InputDate = receipt.InputTime.ToString();

            #region load

            this.Remark = receipt.Remark;
            this.AnimalNo = receipt.AnimalNo;
            this.BLNO = receipt.BLNO;
            this.Client = receipt.Client;
            this.CommercialNo = receipt.CommercialNo;
            this.Company = receipt.Company;
            this.Contacter = receipt.Contacter;
            this.Container = receipt.Container;
            this.DiseaseChequeNo = receipt.DiseaseChequeNo;
            this.DiseaseFee = receipt.DiseaseFee.ToString();
            this.DisinfectChequeNo = receipt.DisinfectChequeNo;
            this.DisinfectFee = receipt.DisinfectFee.ToString();
            this.HealthNo = receipt.HealthNo;
            this.IsAnimal = receipt.IsAnimal;
            this.IsCommercial = receipt.IsCommercial;
            this.IsHealth = receipt.IsHealth;
            this.OutputMessage = string.Empty;
            this.Mobile = receipt.Mobile;
            this.Place = receipt.Place;
            this.Port = receipt.Port;
            this.Production = receipt.Production;
            #endregion
        }

        private void exportToExcel(object parameter)
        {
            string path;
            if(null == parameter)
            {
                ShowMessage("数据集为空，请先查询!");
                return;
            }
            else
            {
                path = Path.GetDirectoryName((string)parameter);
            }
            
            if (this.ReceiptList == null || this.ReceiptList.Count == 0)
            {
                ShowMessage("数据集为空，请先查询!");
                return;
            }
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Path.GetFullPath(path));
                }
            }
            catch(IOException ex)
            {
                throw ex;
            }
            try
            {
                using (ExcelExport excelComponent = new ExcelExport((string)parameter))
                {
                    string sheetName = string.Empty;
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = null;
                    for (int i = 0, rowIndex = 0; i < this.ReceiptList.Count; i++, rowIndex++)
                    {
                        DateTime date;
                        var receipt = this.ReceiptList[i];

                        if (DateTime.TryParse(receipt.YearMonth, out date))
                        {
                            var month = date.Month.ToString();
                            if (sheetName != month)
                            {
                                sheetName = month;
                                workSheet = excelComponent.CreateSheet(i, sheetName);

                                generateExcelColumn(excelComponent, workSheet);

                                rowIndex = 1;
                            }
                            generateExcelCell(rowIndex + 1, receipt, excelComponent, workSheet);
                        }
                    }
                    excelComponent.SaveAsFile();
                    ShowMessage("Excel生成成功！");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ReceiptViewModel CopyTo(ReceiptViewModel oldViewModel)
        {
            return new ReceiptViewModel(oldViewModel.GetCurrentLoginUser())
            {
                AnimalNo = oldViewModel.AnimalNo,
                BLNO = oldViewModel.BLNO,
                Client = oldViewModel.Client,
                
                CommercialNo = oldViewModel.CommercialNo,
                Company = oldViewModel.Company,
                Contacter = oldViewModel.Contacter,
                Container = oldViewModel.Container,
                DiseaseChequeNo = oldViewModel.DiseaseChequeNo,
                DiseaseFee = oldViewModel.DiseaseFee,
                DisinfectChequeNo = oldViewModel.DisinfectChequeNo,
                DisinfectFee = oldViewModel.DisinfectFee,
                HealthNo = oldViewModel.HealthNo,
                IsCommercial = oldViewModel.IsCommercial,
                IsAnimal =  oldViewModel.IsAnimal,
                IsHealth = oldViewModel.IsHealth,
                Mobile = oldViewModel.Mobile,
                Place = oldViewModel.Place,
                Port = oldViewModel.Port,
                Production = oldViewModel.Production,
                Remark = oldViewModel.Remark,
                Year = oldViewModel.Year,
                YearMonth = oldViewModel.YearMonth
            };
        }

        public ReceiptModel locateReceiptBySingleNo(object parameter)
        {
            if(parameter == null) return null;
            return connection.GetTable<ReceiptModel>().Receipt_Find((string)parameter);
        }

        private void generateExcelColumn(ExcelExport component, Microsoft.Office.Interop.Excel._Worksheet workSheet)
        {
            component.SetCells(1, 1, "日期", workSheet);
            component.SetCells(1, 2, "港口", workSheet);
            component.SetCells(1, 3, "客户", workSheet);
            component.SetCells(1, 4, "经营单位", workSheet);
            component.SetCells(1, 5, "品名", workSheet);
            component.SetCells(1, 6, "提单号", workSheet);
            component.SetCells(1, 7, "箱量", workSheet);
            component.SetCells(1, 8, "动检号", workSheet);
            component.SetCells(1, 9, "场地", workSheet);
            component.SetCells(1, 10, "商", workSheet);
            component.SetCells(1, 11, "动", workSheet);
            component.SetCells(1, 12, "卫", workSheet);
            component.SetCells(1, 13, "备注", workSheet);
            component.SetCells(1, 14, "检疫费", workSheet);
            component.SetCells(1, 15, "消毒费", workSheet);
            component.SetCells(1, 16, "检支票", workSheet);
            component.SetCells(1, 17, "消支票", workSheet);
            component.SetCells(1, 18, "联系人", workSheet);
            component.SetCells(1, 19, "联系电话", workSheet);
        }

        private void generateExcelCell(int row, ReceiptModel receipt, ExcelExport component, Microsoft.Office.Interop.Excel._Worksheet workSheet)
        {
            component.SetCells(row, 1, receipt.YearMonth, workSheet);
            component.SetCells(row, 2, receipt.Port, workSheet);
            component.SetCells(row, 3, receipt.Client, workSheet);
            component.SetCells(row, 4, receipt.Company, workSheet);
            component.SetCells(row, 5, receipt.Production, workSheet);
            component.SetCells(row, 6, receipt.BLNO, workSheet);
            component.SetCells(row, 7, receipt.Container, workSheet);
            component.SetCells(row, 8, receipt.AnimalNo, workSheet);
            component.SetCells(row, 9, receipt.Place, workSheet);
            component.SetCells(row, 10, receipt.IsCommercial ? new string(new char[] { '√' }) : "", workSheet);
            component.SetCells(row, 11, receipt.IsAnimal ? new string(new char[] { '√' }) : "", workSheet);
            component.SetCells(row, 12, receipt.IsHealth ? new string(new char[] { '√' }) : "", workSheet);
            component.SetCells(row, 13, receipt.Remark, workSheet);
            component.SetCells(row, 14, receipt.DiseaseFee.ToString(), workSheet);
            component.SetCells(row, 15, receipt.DisinfectFee.ToString(), workSheet);
            component.SetCells(row, 16, receipt.DiseaseChequeNo, workSheet);
            component.SetCells(row, 17, receipt.DisinfectChequeNo, workSheet);
            component.SetCells(row, 18, receipt.Contacter, workSheet);
            component.SetCells(row, 19, receipt.Mobile, workSheet);
        }
        #endregion

        #region public methods
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

        public void ShowMessage(string message)
        {
            OutputMessage += string.Format("{0}: {1}{2}", DateTime.Now.ToString("yy-MM-dd hh:mm:ss"), message, Environment.NewLine);
        }

        #endregion
    }
}
