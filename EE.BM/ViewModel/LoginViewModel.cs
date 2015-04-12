using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.DAL;
using EE.BM.Model;
using EE.BM.Utility;
using LinqToDB.DataProvider.Access;
using System.Windows.Controls;
namespace EE.BM
{
    public class LoginViewModel:NotificationObject
    {
        #region private variables
        private BMConnection dbConnection = null;
        private UserModel loginUser = null;

        private string userName = null;
        private string userPassword = null;
        #endregion

        public LoginViewModel()
        {
            dbConnection = DataModel.CreateDataConnection();
        }

        #region Commands
        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand()
                {
                    ExecuteCommand = new Action<object>(Login)
                };
            }
        }

        public DelegateCommand RegisterCommand
        {
            get
            {
                return new DelegateCommand()
                {
                    ExecuteCommand = new Action<object>(Register)
                };
            }
        }

        #endregion

        #region View Model Properties

        public string UserName
        {
            get { return this.userName; }
            set
            {
                base.SetProperty<string>(ref userName, value, () => this.UserName);
            }
        }

        public string UserPassword
        {
            get { return this.userPassword; }
            set
            {
                base.SetProperty<string>(ref userPassword, value, () => this.UserPassword);
            }
        }

        #endregion

        #region private methods
        private void Login(object parameter)
        {
            PasswordBox pb = parameter as PasswordBox;

            UserPassword = Helper.Encrypt(pb.Password);

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPassword))
            {
                throw new Exception("用户名、密码不许为空。");
            }

            UserModel userModel = dbConnection.GetTable<UserModel>().Find(UserName, UserPassword);

            if (userModel == null)
            {
                throw new Exception("用户名、密码错误。请联系管理员。");
            }
        }

        private void Register(object parameter)
        {
            PasswordBox pb = parameter as PasswordBox;

            try
            {
                
            }
            catch
            {
            }
        }
        #endregion
    }
}
