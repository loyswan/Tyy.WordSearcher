using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tyy.WordSearcher.Core.Models
{
    public struct TongyongyvWord : IEquatable<TongyongyvWord>
    {
        public TongyongyvWord(string word, string tpinyin, string tongyongyv)
        {
            Word = word;
            TPinyin = tpinyin;
            Tongyongyv = tongyongyv;

            //初始化汉字列表
            Characters = new List<KeyCharacter>();
            for (int i = 0; i < Length; i++)
            {
                Characters.Add(new KeyCharacter() { Chinese = ChineseArray[i], TPinyin = TPinyinArray[i] });
            }
        }

        //
        internal TongyongyvWord(string[] data)
        {
            byte cs = (byte)data.Length;
            Word = data[0];
            TPinyin = data[1];
            Tongyongyv = data[2];

            //初始化汉字列表
            Characters = new List<KeyCharacter>();
            for (byte i = 3; i < cs ;)
            {
                Characters.Add(new KeyCharacter() { Chinese = data[i][0], TPinyin = data[i + 1] });
                i += 2;
            }
        }

        public string Word { get; private set; }

        public string TPinyin { get; private set; }

        public string Tongyongyv { get; private set; }

        //汉字列表
        public List<KeyCharacter> Characters { get; private set; }


        public bool HasHyphen => Tongyongyv.Contains('-');

        public int Length => Word.Length;

        public char[] ChineseArray => Word.ToCharArray();

        public string[] TPinyinArray => Regex.Split(TPinyin, @"(?<=[12345])")//分割并保留数字
                .Where(s => !string.IsNullOrEmpty(s)).ToArray(); //去除空字符串



        public override bool Equals(object obj)
        {
            return obj is TongyongyvWord word && Equals(word);
        }

        public bool Equals(TongyongyvWord other)
        {
            return Word == other.Word;
        }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }

        public static bool operator ==(TongyongyvWord left, TongyongyvWord right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TongyongyvWord left, TongyongyvWord right)
        {
            return !(left == right);
        }


        public override string ToString()
        {
            return Word + "\t" + TPinyin + "\t" + Tongyongyv;
        }


    }

}
