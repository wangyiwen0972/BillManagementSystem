using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EE.BM.Utility;

namespace EE.BM.View
{
    /// <summary>
    /// WindowBase.xaml 的交互逻辑
    /// </summary>
    public partial class WindowBase : Window
    {
        public WindowBase()
        {
            InitializeTheme();
            InitializeStyle();

            this.Loaded += delegate
            {
                InitializeEvent();
            };
        }

        protected virtual void MinWin()
        {
            this.WindowState = WindowState.Minimized;
        }

        public Button YesButton
        {
            get;
            set;
        }
        public Button NoButton
        {
            get;
            set;
        }

        public UserImage ImageProgress
        {
            get;
            set;
        }

        public TextBox StatusTextBox
        {
            get;
            set;
        }

        private void InitializeEvent()
        {
            ControlTemplate baseWindowTemplate = (ControlTemplate)App.Current.Resources["BaseWindowControlTemplate"];

            Border borderTitle = (Border)baseWindowTemplate.FindName("borderTitle", this);
            Button closeBtn = (Button)baseWindowTemplate.FindName("btnClose", this);
            Button minBtn = (Button)baseWindowTemplate.FindName("btnMin", this);
            YesButton = (Button)baseWindowTemplate.FindName("btnYes", this);
            NoButton = (Button)baseWindowTemplate.FindName("btnNo", this);
            ImageProgress = baseWindowTemplate.FindName("imgProgress", this) as UserImage;
            StatusTextBox = baseWindowTemplate.FindName("rtbStatus", this) as TextBox;

            minBtn.Click += delegate
            {
                MinWin();
            };

            closeBtn.Click += delegate
            {
                this.Close();
            };

            borderTitle.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            this.Title = "账单管理系统";
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public Canvas GridContent
        {
            get;
            set;
        }


        private void InitializeStyle()
        {
            this.Style = (Style)App.Current.Resources["BaseWindowStyle"];
        }

        private void InitializeTheme()
        {
            string themeName = ConfigManage.CurrentTheme;
            App.Current.Resources.MergedDictionaries.Add(Application.LoadComponent(new Uri(string.Format("../View/Theme/{0}/WindowBaseStyle.xaml", themeName), UriKind.Relative)) as ResourceDictionary);
        }

        private bool _allowSizeToContent = false;
        /// <summary>
        /// 自定义属性，用于标记该窗体是否允许按内容适应，设此属性是为了解决最大化按钮当SizeToContent属性为WidthAndHeight时不能最大化，从而最大、最小化必须变更SizeToContent的值的问题
        /// </summary>
        public bool AllowSizeToContent
        {
            get
            {
                return _allowSizeToContent;
            }
            set
            {
                this.SizeToContent = (value ? SizeToContent.WidthAndHeight : SizeToContent.Manual);
                _allowSizeToContent = value;
            }
        }

        protected void ShowProgressImage()
        {
            this.ImageProgress.Dispatcher.Invoke(new Action<UserImage>(StartProgress), this.ImageProgress);
        }

        protected void HiddenProgressImage()
        {
            this.ImageProgress.Dispatcher.Invoke(new Action<UserImage>(StopProgress), this.ImageProgress);
        }

        private void StartProgress(UserImage imgProgress)
        {
            imgProgress.Visibility = System.Windows.Visibility.Visible;
            imgProgress.StartAnimate();
        }

        private void StopProgress(UserImage imgProgress)
        {
            imgProgress.Visibility = System.Windows.Visibility.Hidden;
            imgProgress.StopAnimate();
        }

        protected Action GetPermission(object obj,IViewModel viewModel)
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
    }
}
