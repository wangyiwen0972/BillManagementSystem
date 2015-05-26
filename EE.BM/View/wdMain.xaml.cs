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
using System.Windows.Shapes;
using EE.BM.DAL;
using EE.BM.Utility;
using Microsoft.Win32;
using System.Reflection;
using System.Globalization;

namespace EE.BM.View
{
    /// <summary>
    /// wdMain.xaml 的交互逻辑
    /// </summary>
    public partial class wdMain : EE.BM.View.WindowBase
    {
        private ReceiptViewModel reciptVM = null;
        public wdMain()
        {
            InitializeComponent();

            

            this.Loaded += (s, e) =>
                {
                    InitializePermissionForComponenet();
                    InitializeCultureForUI();
                    this.userReceiptDetail.DataContext = reciptVM;
                };            
        }

        /// <summary>
        /// initialize permission for each component
        /// </summary>
        private void InitializePermissionForComponenet()
        {
            reciptVM = this.DataContext as ReceiptViewModel;

            //export to excel
            base.SetEnableVisible(this.btnExport, base.GetPermission(reciptVM.ExportToExcelCommand, reciptVM));

        }

        private void InitializeCultureForUI()
        {
            if (reciptVM == null) reciptVM = this.DataContext as ReceiptViewModel;

            List<object> searchKeyList = new List<object>();

            ResXDataAccess resource = reciptVM.ResxDataAccess;

            foreach (KeyValuePair<string, string> keyValue in resource.Load(CultureInfo.CurrentCulture.DisplayName))
            {
                if (keyValue.Key.IndexOf("search", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    resource.TryGetValue(
                }
            }
            

        }

        private void GetEnableOrVisible(Control control, Action action)
        {
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            //ToolBar toolBar = sender as ToolBar;
            //var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            //if (overflowGrid != null)
            //{
            //    overflowGrid.Visibility = Visibility.Collapsed;
            //}
            //var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            //if (mainPanelBorder != null)
            //{
            //    mainPanelBorder.Margin = new Thickness(0);
            //}
        }

        private void ComboBox_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (this.DataContext as ReceiptViewModel).SearchReceiptCommand.Execute((sender as ComboBox).SelectedValue.ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.reciptVM.SaveCommand.Execute(null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ReceiptViewModel).SearchReceiptCommand.Execute(null);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedValue != null)
            {
                try
                {
                    (this.DataContext as ReceiptViewModel).SearchReceiptCommand.Execute((sender as ComboBox).SelectedValue.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Excel文件(*.xls)|*.xls"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    (this.DataContext as ReceiptViewModel).ExportToExcelCommand.Execute(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("当前数据将被清空，是否继续？") == MessageBoxResult.Yes)
            {
                this.reciptVM.NewCommand.Execute(null);
            }
        }

        private void Binding(int number, int currentSize)
        {
            (DataContext as ReceiptViewModel).SearchReceiptCommand.Execute(cbYear.Text);
            
            int count = (DataContext as ReceiptViewModel).ReceiptList.Count;          //获取记录总数  
            int pageSize = 0;            //pageSize表示总页数  
            if (count % number == 0)
            {
                pageSize = count / number;
            }
            else
            {
                pageSize = count / number + 1;
            }
            tbkTotal.Text = pageSize.ToString();

            tbkCurrentsize.Text = currentSize.ToString();

            dgReceipt.ItemsSource = (DataContext as ReceiptViewModel).ReceiptList.Take(number * currentSize).Skip(number * (currentSize - 1));        //重新绑定dataGrid1  
        }



        //分页方法写好了 接下来就是响应下一页，上一页，和跳转页面的事件了  

        //先定义一个常量  

        const int Num = 12;  //表示每页显示12条记录  

        //上一页事件   

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int currentsize = int.Parse(tbkCurrentsize.Text); //获取当前页数  
            if (currentsize > 1)
            {
                Binding(Num, currentsize - 1);   //调用分页方法  
            }
        }


        //下一页事件  
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int total = int.Parse(tbkTotal.Text); //总页数  
            int currentsize = int.Parse(tbkCurrentsize.Text); //当前页数  
            if (currentsize < total)
            {
                Binding(Num, currentsize + 1);   //调用分页方法  
            }
        }


        //跳转事件  
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            int pageNum = int.Parse(tbxPageNum.Text);
            int total = int.Parse(tbkTotal.Text); //总页数  
            if (pageNum >= 1 && pageNum <= total)
            {
                Binding(Num, pageNum);     //调用分页方法  
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            wdOpen openWindow = new wdOpen()
            {
                Owner = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };
            openWindow.StatusTextBox.Visibility = System.Windows.Visibility.Hidden;

            if (openWindow.ShowDialog() == true)
            {
                var specifiedReceipt = reciptVM.locateReceiptBySingleNo(openWindow.SearchText);

                var newReceiptDetail = new userDetail()
                {
                    DataContext = new ReceiptViewModel(reciptVM.GetCurrentLoginUser(),specifiedReceipt)
                };

                WindowBase window = new WindowBase()
                {
                    Content = newReceiptDetail,
                    Width = 1024,
                    Height = newReceiptDetail.Height,
                    Owner = this,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };
                
                window.Show();
            }
        }  
    }
}
