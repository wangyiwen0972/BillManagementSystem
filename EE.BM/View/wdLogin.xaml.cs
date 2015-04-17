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
using EE.BM;
namespace EE.BM.View
{
    /// <summary>
    /// wdLogin.xaml 的交互逻辑
    /// </summary>
    public partial class wdLogin : WindowBase
    {
        public wdLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                base.ShowProgressImage();

                LoginViewModel loginVM = (this.DataContext as LoginViewModel);

                loginVM.LoginCommand.Execute(this.txtPassword);
                if (loginVM.GetCurrentLoginUser() != null)
                {
                    ReceiptViewModel receiptVM = new ReceiptViewModel(loginVM.GetCurrentLoginUser());

                    wdMain mainWindow = new wdMain()
                    {
                        DataContext = receiptVM
                    };
                    
                    mainWindow.Closing += (o,er)=>
                    {
                        if (MessageBoxResult.No == MessageBox.Show(this, "是否确定退出程序？", "退出", MessageBoxButton.YesNo))
                        {
                            er.Cancel = true;
                        }
                        else
                        {
                            App.Current.MainWindow.Close();
                        }
                    };
                    mainWindow.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
            finally
            {
                base.HiddenProgressImage();
            }
        }


    }
}
