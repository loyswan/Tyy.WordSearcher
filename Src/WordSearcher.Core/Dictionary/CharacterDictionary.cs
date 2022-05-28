using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core.Dictionary
{
    public class CharacterDictionary
    {
        internal Dictionary<KeyCharacter, List<TongyongyvCharacter>> _tongyongyvCharacters;

        public CharacterDictionary()
        {
            _tongyongyvCharacters = new Dictionary<KeyCharacter, List<TongyongyvCharacter>>();
        }

        public List<TongyongyvCharacter> this[KeyCharacter key]
        {
            get => _tongyongyvCharacters[key];
            set => _tongyongyvCharacters[key] = value;
        }

        public ICollection<KeyCharacter> Keys => _tongyongyvCharacters.Keys;

        public ICollection<List<TongyongyvCharacter>> Values => _tongyongyvCharacters?.Values;

        public int Count => _tongyongyvCharacters.Count;

        //添加 一个 单字 到字典
        public void Add(TongyongyvCharacter character)
        {
            Add(character.Character, character);
        }

        //添加 一堆 单字 到字典
        public void Add(List<TongyongyvCharacter> characters)
        {
            foreach (var character in characters)
            {
                Add(character.Character, character);
            }
        }


        public void Add(KeyCharacter key, TongyongyvCharacter character)
        {
            if (this.Keys.Contains(key))
            {
                var list = _tongyongyvCharacters[key];
                if (list.Contains(character)) { return; }
                list.Add(character);
                return;
            }
            _tongyongyvCharacters.Add(key, new List<TongyongyvCharacter> { character });
        }

        public bool ContainsKey(KeyCharacter key)
        {
            return _tongyongyvCharacters.ContainsKey(key);
        }

        public bool Remove(KeyCharacter key)
        {
            return this._tongyongyvCharacters.Remove(key);
        }


        //获取 单字 列表
        public bool TryGetValue(KeyCharacter key, out List<TongyongyvCharacter> value)
        {
            return _tongyongyvCharacters.TryGetValue(key, out value);
        }


        //获取 最佳 单字
        public TongyongyvCharacter TryGetBestMatchValue(KeyCharacter key)
        {
            if (TryGetValue(key, out List<TongyongyvCharacter> list))
            {
                //如果有2个以上单字，取带圆括号的单字
                if (list.Count > 1)
                {
                    foreach (var c in list)
                    {
                        if (c.Type.StartsWith("(") && c.Type.EndsWith(")"))
                        {
                            return c; //返回带圆括号的单字
                        }
                    }
                }
                //如果都不带圆括号，或者仅有一个单字
                return list[0]; //返回第一个单字
            }
            //未找到符合的单字 
            return TongyongyvCharacter.Empty; //返回空
        }

        public void Clear()
        {
            _tongyongyvCharacters.Clear();
        }
    }
}
