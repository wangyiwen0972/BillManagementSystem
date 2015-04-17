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
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        private void ComboBox_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
