using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Tyy.WordSearcher.Wpf.ViewModels;

namespace Tyy.WordSearcher.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            //绑定viewmodel
            vm = new MainViewModel();
            this.DataContext = vm;

            //程序标题
            var assembly = Assembly.GetExecutingAssembly();
            var productname = (assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true)[0] as AssemblyProductAttribute).Product;
            this.Title = productname + " V" + assembly.GetName().Version.ToString();

            //默认允许连字符
            this.cbWord.IsChecked = true;
            vm.IsAllowHyphen = true;
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.tbInputBox.Text = "";
            this.Tab.TextBoxText = "";
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            this.tbInputBox.Text = "";
            this.Tab.TextBoxText = "";
            this.tbInputBox.Text = Clipboard.GetText().Trim();
        }


        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            this.Tab.ResultIndex = 1;
        }

        private void cbWord_Checked(object sender, RoutedEventArgs e)
        {
            vm.IsAllowHyphen = true;
        }

        private void cbWord_Unchecked(object sender, RoutedEventArgs e)
        {
            vm.IsAllowHyphen = false;
        }

    }
}
