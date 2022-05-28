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

namespace Tyy.WordSearcher.Wpf
{
    /// <summary>
    /// TabControl.xaml 的交互逻辑
    /// </summary>
    public partial class TabControl : UserControl
    {
        public TabControl()
        {
            InitializeComponent();
            ucRoot.DataContext = this;
        }

        #region 标签
        /// <summary>
        /// 标签1
        /// </summary>
        public string Button1Content
        {
            get { return (string)GetValue(Button1ContentProperty); }
            set { SetValue(Button1ContentProperty, value); }
        }
        public static readonly DependencyProperty Button1ContentProperty =
            DependencyProperty.Register("Button1Content", typeof(string), typeof(TabControl), new PropertyMetadata("Tab1"));


        /// <summary>
        /// 标签2
        /// </summary>
        public string Button2Content
        {
            get { return (string)GetValue(Button2ContentProperty); }
            set { SetValue(Button2ContentProperty, value); }
        }
        public static readonly DependencyProperty Button2ContentProperty =
            DependencyProperty.Register("Button2Content", typeof(string), typeof(TabControl), new PropertyMetadata("Tab2"));

        /// <summary>
        /// 标签3
        /// </summary>
        //public string Button3Content
        //{
        //    get { return (string)GetValue(Button3ContentProperty); }
        //    set { SetValue(Button3ContentProperty, value); }
        //}
        //public static readonly DependencyProperty Button3ContentProperty =
        //    DependencyProperty.Register("Button3Content", typeof(string), typeof(TextTabControl), new PropertyMetadata("Tab3"));

        #endregion

        #region 复制按钮
        /// <summary>
        /// 复制按钮文本
        /// </summary>
        public string ButtonCopy { get { return (string)GetValue(ButtonCopyProperty); } set { SetValue(ButtonCopyProperty, value); } }
        public static readonly DependencyProperty ButtonCopyProperty = DependencyProperty.Register("ButtonCopy", typeof(string), typeof(TabControl), new PropertyMetadata("Copy"));
        /// <summary>
        /// 复制按钮图片
        /// </summary>
        public ImageSource ButtonCopyImage { get { return (ImageSource)GetValue(ButtonCopyImageProperty); } set { SetValue(ButtonCopyImageProperty, value); } }
        public static readonly DependencyProperty ButtonCopyImageProperty = DependencyProperty.Register(nameof(ButtonCopyImage), typeof(ImageSource), typeof(TabControl));

        #endregion


        /// <summary>
        /// 文本框内容
        /// </summary>
        public string TextBoxText { get { return (string)GetValue(TextBoxTextProperty); } set { SetValue(TextBoxTextProperty, value); } }
        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register(nameof(TextBoxText), typeof(string), typeof(TabControl), new PropertyMetadata(""));


        /// <summary>
        /// Tab标签选中序号 值 1，2
        /// </summary>
        public int ResultIndex { get { return (int)GetValue(ResultIndexProperty); } set { SetValue(ResultIndexProperty, value); } }
        public static readonly DependencyProperty ResultIndexProperty = DependencyProperty.Register("ResultIndex", typeof(int), typeof(TabControl), new PropertyMetadata(1, ResultIndexChangedCallback));

        private static void ResultIndexChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabControl TextTab = d as TabControl;
            if ((int)e.NewValue == 1)
                TextTab.rb1.IsChecked = true;
            else if ((int)e.NewValue == 2)
                TextTab.rb2.IsChecked = true;
            //else if ((int)e.NewValue == 3)
            //    TextTab.rb3.IsChecked = true;
            else
                TextTab.rb1.IsChecked = true;
        }


        /// <summary>
        /// 切换Tab1命令
        /// </summary>
        public ICommand SwitchTabCommand { get { return (ICommand)GetValue(SwitchTabCommandProperty); } set { SetValue(SwitchTabCommandProperty, value); } }
        public static readonly DependencyProperty SwitchTabCommandProperty = DependencyProperty.Register("SwitchTabCommand", typeof(ICommand), typeof(TabControl), new PropertyMetadata(null));



        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBoxText))
            {

                //MessageBox.Show(_showLang == "Cn" ? "没有要复制的内容！" : "Nothing to copy!", _showLang == "Cn" ? "错误" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(this.TextBoxText, TextDataFormat.Text);
                //MessageBox.Show(_showLang == "Cn" ? "复制成功" : "Success", _showLang == "Cn" ? "复制成功" : "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), _showLang == "Cn" ? "复制出错" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tab_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Name == "rb1")
            {
                ResultIndex = 1;
            }
            if (rb.Name == "rb2")
            {
                ResultIndex = 2;
            }
            //if (rb.Name == "rb3")
            //{
            //    ResultIndex = 3;
            //}
        }
    }
}
