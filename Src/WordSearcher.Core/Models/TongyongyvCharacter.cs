using System;

namespace Tyy.WordSearcher.Core.Models
{
    public struct TongyongyvCharacter : IEquatable<TongyongyvCharacter>
    {
        public static readonly TongyongyvCharacter Empty;

        public KeyCharacter Character { get; set; }

        public string Type { get; set; }

        public string Flag { get; set; }

        public string Tongyongyv { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TongyongyvCharacter character && Equals(character);
        }

        public bool Equals(TongyongyvCharacter other)
        {
            return Character.Equals(other.Character);
        }

        public override int GetHashCode()
        {
            return Character.GetHashCode();
        }

        public static bool operator ==(TongyongyvCharacter left, TongyongyvCharacter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TongyongyvCharacter left, TongyongyvCharacter right)
        {
            return !(left == right);
        }



        public override string ToString()
        {
            return Character.ToString();
        }

        public string ToString(bool full = true)
        {
            return Character.ToString() + "\t" + Type + "\t" + Flag + "\t" + Tongyongyv + "\t" + Description;
        }


    }

}
