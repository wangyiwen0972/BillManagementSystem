﻿using System;
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

            this.dpYearMonth.IsTodayHighlighted = false;
            this.dpYearMonth.DisplayDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));

            this.Loaded += (s, e) =>
                {
                    reciptVM = this.DataContext as ReceiptViewModel;


                    Action action = Helper.GetActionFromPermission(reciptVM.NewCommand, reciptVM.GetCurrentLoginUser());
                    this.btnNew.Visibility = action == Action.Invisible ? Visibility.Collapsed : Visibility.Visible;
                    this.btnNew.IsEnabled = action == Action.Executable ? true : false;

                    

                };
            //System.Drawing.Bitmap bitmap = Properties.Resources.Folder_generic;

            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            ////ImageBrush imageBrush = new ImageBrush();
            //ImageSourceConverter convert = new ImageSourceConverter();

            ////imageBrush.ImageSource = convert.ConvertFrom(stream) as ImageSource;

            //Image imgNew = new Image()
            //{
            //    Source = convert.ConvertFrom(stream) as ImageSource
            //};
            //btnNew.Content = imgNew;
            
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
            //ReceiptViewModel context = this.DataContext as ReceiptViewModel;
            (sender as Button).CaptureMouse();
            //context.SaveCommand.Execute(null);
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
            //MessageBox.Show("当前数据被清空");
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
    }
}
