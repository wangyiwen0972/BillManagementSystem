using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EE.BM.Utility;
namespace EE.BM.View
{
    /// <summary>
    /// userToolBar.xaml 的交互逻辑
    /// </summary>
    public partial class userToolBar : UserControl
    {
        private ReceiptViewModel reciptVM;

        public userToolBar()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                InitializePermissionForComponenet();
            };  
        }



        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            wdOpen openWindow = new wdOpen()
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };

            if (openWindow.ShowDialog() == true)
            {
                var specifiedReceipt = reciptVM.locateReceiptBySingleNo(openWindow.SearchText);

                var newReceiptDetail = new userDetail()
                {
                    DataContext = new ReceiptViewModel(reciptVM.GetCurrentLoginUser(), specifiedReceipt)
                };

                WindowBase window = new WindowBase()
                {
                    Content = newReceiptDetail,
                    Width = 1024,
                    Height = newReceiptDetail.Height
                };
                window.Show();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.reciptVM.SaveCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializePermissionForComponenet()
        {
            reciptVM = this.DataContext as ReceiptViewModel;

            //get permission for the commands
            //create, modify, delete, copy commands
            this.SetEnableVisible(this.btnNew, this.GetPermission(reciptVM.NewCommand, reciptVM));
            this.SetEnableVisible(this.btnSave, this.GetPermission(reciptVM.SaveCommand, reciptVM));
            this.SetEnableVisible(this.btnDelete, this.GetPermission(reciptVM.DeleteCommand, reciptVM));
            this.SetEnableVisible(this.btnCopy, this.GetPermission(reciptVM.DeleteCommand, reciptVM));
            //print, preview commands
            this.SetEnableVisible(this.btnPrint, this.GetPermission(reciptVM.DeleteCommand, reciptVM));
            this.SetEnableVisible(this.btnPreview, this.GetPermission(reciptVM.PreviewCommand, reciptVM));
            //export to excel
            //this.SetEnableVisible(this.btnExport, this.GetPermission(reciptVM.ExportToExcelCommand, reciptVM));

        }

        protected Action GetPermission(object obj, IViewModel viewModel)
        {
            return Helper.GetActionFromPermission(obj, viewModel.GetCurrentLoginUser());
        }

        protected void SetEnableVisible(Control control, Action action)
        {
            control.Visibility = action == Action.Invisible ? Visibility.Collapsed : Visibility.Visible;
            control.IsEnabled = action == Action.Executable ? true : false;
            if (action != Action.Executable)
            {
                control.ToolTip = "您当前的用户组无法执行该操作！";
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            switch (reciptVM.Status.ToLower())
            {
                case "nochanged":
                case "saved":
                    {
                        MessageBoxResult result = MessageBox.Show("确认删除当前数据？", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                this.reciptVM.DeleteCommand.Execute(null);
                                this.reciptVM.NewCommand.Execute(null);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    }
                default:
                    {
                        MessageBox.Show("请先保存数据！");
                        break;
                    }
            }

            
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            switch (reciptVM.Status.ToLower())
            {
                case "nochanged":
                case "saved":
                    {
                        var newReceiptVM = ReceiptViewModel.CopyTo(this.reciptVM);
                        var window = new wdMain()
                        {
                            DataContext = newReceiptVM
                        };
                        window.Show();
                        break;
                    }
                default:
                    {
                        MessageBox.Show("当前数据不完整或未保存！不能复制该数据！");
                        break;
                    }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("当前数据将被清空，是否继续？","新建", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.reciptVM.NewCommand.Execute(null);
            }
        }
    }
}
