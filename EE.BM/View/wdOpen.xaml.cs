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

namespace EE.BM.View
{
    /// <summary>
    /// wdOpen.xaml 的交互逻辑
    /// </summary>
    public partial class wdOpen : WindowBase
    {
        public wdOpen()
        {
            InitializeComponent();

            this.txtSingleNo.Focus();
        }

        public string SearchText
        {
            get { return txtSingleNo.Text; }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtSingleNo.Text))
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("请填写唯一单号");
            }
        }
    }
}
