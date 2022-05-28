using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using Tyy.WordSearcher.Core;
using Tyy.WordSearcher.Core.Models;
using Tyy.WordSearcher.Mvvm;

namespace Tyy.WordSearcher.Wpf.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        TongyongyvSearcherLoader loader;
        TongyongyvSearcher searcher;

        List<TongyongyvCharacter> characters;
        List<TongyongyvWord> words;

        public MainViewModel()
        {
            loader = new TongyongyvSearcherLoader();
            //loader.LoadFromTxt();//加载字典
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //开始计时  
            sw.Start();
            loader.Load();
            //结束计时  
            sw.Stop();
            //获取运行时间[毫秒]  
            long times = sw.ElapsedMilliseconds;
            Console.WriteLine("执行查询总共使用了" + times + "毫秒");

            searcher = new TongyongyvSearcher(loader);

        }


        private string _inputBox;
        //输入框文本
        public string InputBox
        {
            get { return _inputBox; }
            set { SetProperty<string>(ref _inputBox, value, nameof(InputBox)); }
        }

        private string _outBox;
        //输出框文本
        public string OutBox
        {
            get { return _outBox; }
            set { SetProperty<string>(ref _outBox, value, nameof(OutBox)); }
        }

        //提示信息
        private string _tipString;
        public string TipString
        {
            get { return _tipString; }
            set { SetProperty<string>(ref _tipString, value, nameof(TipString)); }
        }
        //提示信息字体颜色  System.Windows.Media.Color
        private Color _tipColor;
        public Color TipColor
        {
            get { return _tipColor; }
            set { SetProperty<Color>(ref _tipColor, value, nameof(TipColor)); }
        }

        public bool IsAllowHyphen { get; set; }


        //转换命令
        private RelayCommand _covertCommand;
        public RelayCommand ConvertCommand
        {
            get
            {
                if (_covertCommand == null)
                {
                    _covertCommand = new RelayCommand(SearchCommand/*, () =>!string.IsNullOrEmpty(InputBox)*/);
                }
                return _covertCommand;
            }
        }

        private RelayCommand<int> _resultCommand;
        public RelayCommand<int> ResultCommand
        {
            get
            {
                if (_resultCommand == null)
                {
                    _resultCommand = new RelayCommand<int>(tabIndex =>
                    {
                        if (tabIndex == 1) OutBox = this.Result1;
                        if (tabIndex == 2) OutBox = this.Result2;
                        //if (tabIndex == 3) OutBox = this.Result3;

                        //OutBox = $"输出窗口：IsSingle = {IsSingle.ToString()}\r\nLanguege = {Languege}";
                    });
                }
                return _resultCommand;
            }
        }

        #region 字段
        private string Result1;
        private string Result2;
        //private string Result3;
        #endregion

        public void ShowInfo(string msg)
        {
            this.TipColor = Colors.Lime;
            this.TipString = msg;
        }

        public void ShowError(string msg)
        {
            this.TipColor = Colors.DarkRed;
            this.TipString = msg;
        }


        private void SearchCommand()
        {
            //设置检索参数
            var searchkey = GetInputItem();
            if (searchkey == null)
            {
                ShowError("输入内容及格式不正确！");
                return;
            }

            searcher.Option = new TongyongyvSearcherOption(searchkey, IsAllowHyphen);


            //检索结果
            words = searcher.SearchWords();
            characters = searcher.SearchCharacters(words);

            //显示结果
            StringBuilder wordstring = new StringBuilder();
            words.ForEach(word => wordstring.AppendLine(word.ToString()));
            this.Result1 = wordstring.ToString();

            var charstring = new StringBuilder();
            characters.ForEach(chr => charstring.AppendLine(chr.ToString()));
            this.Result2 = charstring.ToString();

            OutBox = this.Result1;
        }

        private List<KeyCharacter> GetInputItem()
        {
            //为空 
            if (string.IsNullOrEmpty(InputBox)) return null;
            //按行分割
            string[] keyitemstring = InputBox.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (keyitemstring.Length == 0) return null;

            var searchkey = new List<KeyCharacter>();
            //逐行验证格式
            foreach (var itemstring in keyitemstring)
            {
                if (CheckFormat(itemstring))
                {
                    var item = itemstring.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    KeyCharacter key = new KeyCharacter() { Chinese = item[1][0], TPinyin = item[0] };
                    searchkey.Add(key);
                }

            }

            return searchkey;
        }

        private bool CheckFormat(string inputBox)
        {
            string expr = @"^\w+\d[\t\s]+\w$";
            return Regex.IsMatch(inputBox, expr);
        }


    }
}
