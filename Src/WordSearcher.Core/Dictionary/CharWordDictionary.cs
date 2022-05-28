using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core.Dictionary
{
    public class CharWordDictionary
    {
        //key 为汉字
        //value 为 包含 字 的所有词汇
        internal Dictionary<KeyCharacter, HashSet<TongyongyvWord>> words = new Dictionary<KeyCharacter, HashSet<TongyongyvWord>>();

        //用来记录已经添加了的词汇，防止重复添加
        internal HashSet<string> wordList = new HashSet<string>();

        //添加一个词汇到索引中
        public void Add(TongyongyvWord tyyWord)
        {
            //索引中已存在，不添加
            if (wordList.Contains(tyyWord.Word))
            {
                return;
            }
            //添加到索引
            wordList.Add(tyyWord.Word);

            //在每个字中 添加 
            foreach (var c in tyyWord.Characters)
            {
                if (words.ContainsKey(c))
                {
                    if (!words[c].Contains(tyyWord))
                        words[c].Add(tyyWord);
                }
                else
                {
                    words.Add(c, new HashSet<TongyongyvWord> { tyyWord });
                }
            }
        }


        //返回 包含 字 的词汇列表
        public List<TongyongyvWord> GetWords(KeyCharacter c)
        {
            var list = new List<TongyongyvWord>();
            if (words.TryGetValue(c, out HashSet<TongyongyvWord> set))
            {
                list.AddRange(set);
            }
            return list;
        }

        internal void Clear()
        {
            this.words.Clear();
            this.wordList.Clear();
        }
    }
}
