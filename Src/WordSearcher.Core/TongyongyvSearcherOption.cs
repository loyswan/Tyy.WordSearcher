using System.Collections.Generic;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core
{
    public class TongyongyvSearcherOption
    {
        public TongyongyvSearcherOption(List<KeyCharacter> characters, bool allowHyphen = true)
        {
            _characters = characters;
            _allowHyphen = allowHyphen;
        }

        List<KeyCharacter> _characters;

        bool _allowHyphen;

        public List<KeyCharacter> SearchCharacters { get => _characters; }
        public bool AllowHyphen { get => _allowHyphen; }
    }
}