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
using Microsoft.Win32;
namespace EE.BM.View
{
    /// <summary>
    /// wdMain.xaml 的交互逻辑
    /// </summary>
    public partial class wdMain : EE.BM.View.WindowBase
    {
        public wdMain()
        {
            InitializeComponent();

            this.dpYearMonth.IsTodayHighlighted = false;
            this.dpYearMonth.DisplayDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));


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
    }
}
