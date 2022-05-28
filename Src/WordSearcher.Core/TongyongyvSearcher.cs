using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyy.WordSearcher.Core.Dictionary;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core
{
    public class TongyongyvSearcher
    {
        private WordDictionary _tongyongyuWords;//词汇
        private CharacterDictionary _tongyongyvCharacters;//单字
        private CharWordDictionary _Wordindex;//索引字典

        //检索器配置
        public TongyongyvSearcherOption Option { get; set; }

        public TongyongyvSearcher(WordDictionary tongyongyuWords,
                                  CharacterDictionary tongyongyvCharacters,
                                  CharWordDictionary wordindex)
        {
            _tongyongyuWords = tongyongyuWords;
            _tongyongyvCharacters = tongyongyvCharacters;
            _Wordindex = wordindex;
        }

        public TongyongyvSearcher(TongyongyvSearcherLoader loader)
        {
            _tongyongyuWords = loader.WordDictionary;
            _tongyongyvCharacters = loader.CharacterDictionary;
            _Wordindex = loader.SearchDictionary;
        }

        public List<TongyongyvWord> SearchWords()
        {
            List<TongyongyvWord> words;

            //第一步 获取词汇列表  (依赖索引）

            List<TongyongyvWord> wordlist = new List<TongyongyvWord>();//用于存储查询到的词汇，后面用于筛选
            //循环获取要查询的每个字的词汇列表
            foreach (var character in this.Option.SearchCharacters)
            {
                var list = _Wordindex.GetWords(character);

                list.ForEach(wd =>
                {
                    if (!Option.AllowHyphen && wd.HasHyphen)//词汇包含连字符但设置不允许连字符 跳过
                    {
                        return;
                    }
                    if (wordlist.Contains(wd))  //list已添加过,不重复添加 跳过
                    {
                        return;
                    }
                    wordlist.Add(wd);
                });
            }

            //逐个词汇拆分单字，判断是否在输入查询字中，如果不在则舍弃  
            words = new List<TongyongyvWord>(wordlist);
            foreach (var wd in wordlist)
            {
                foreach (var c in wd.Characters)
                {
                    if (!this.Option.SearchCharacters.Contains(c)) //输入字符不包含，移除不符条件的词汇
                    {
                        words.Remove(wd);
                        break;
                    }
                }
            }

            return words;
        }


        public List<TongyongyvCharacter> SearchCharacters(List<TongyongyvWord> searchWords = null)
        {
            List<TongyongyvCharacter> characters = new List<TongyongyvCharacter>();


            List<TongyongyvWord> words = searchWords ?? SearchWords();
            //对输出词汇中所有单字进行合并返回
            var keycharacter = new HashSet<KeyCharacter>();
            words.ForEach(wd => wd.Characters.ForEach(kc => keycharacter.Add(kc)));

            foreach (var kc in keycharacter)
            {
                characters.Add(_tongyongyvCharacters.TryGetBestMatchValue(kc));
            }

            return characters;
        }

    }
}
