using System.Collections.Generic;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core.Dictionary
{
    public class WordDictionary : Dictionary<string, TongyongyvWord>
    {
        public TongyongyvWord Add(string word, string tpinyin, string tongyongyv)
        {
            var tw = new TongyongyvWord(word, tpinyin, tongyongyv);
            Add(word, tw);
            return tw;
        }
    }
}
