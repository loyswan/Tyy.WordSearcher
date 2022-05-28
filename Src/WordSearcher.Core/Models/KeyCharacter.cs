using System;

namespace Tyy.WordSearcher.Core.Models
{
    public struct KeyCharacter : IEquatable<KeyCharacter>
    {
        public char Chinese { get; set; }

        public string TPinyin { get; set; }

        public override bool Equals(object obj)
        {
            return obj is KeyCharacter character &&
                   Chinese == character.Chinese &&
                   TPinyin == character.TPinyin;
        }

        public bool Equals(KeyCharacter other)
        {
            return Chinese == other.Chinese &&
                   TPinyin == other.TPinyin;
        }

        public override int GetHashCode()
        {
            return Chinese.GetHashCode() ^ TPinyin.GetHashCode();
        }

        public override string ToString()
        {
            return this.Chinese + "\t" + this.TPinyin;
        }

        public static bool operator ==(KeyCharacter left, KeyCharacter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KeyCharacter left, KeyCharacter right)
        {
            return !(left == right);
        }
    }

}
