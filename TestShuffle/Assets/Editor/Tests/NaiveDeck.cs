using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class NaiveDeck<T>
    {
        // https://blog.codinghorror.com/the-danger-of-naivete/
        public static void ShuffleSwapAny(T[] deck)
        {
            for (int index = 0, length = deck.Length; index < length; ++index)
            {
                int r = (int) Mathf.Floor(Deck<T>.Random() * length);
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }

        public static void ShuffleSwapUpTo(T[] deck)
        {
            for (int index = 0, length = deck.Length; index < length; ++index)
            {
                int r = (int) Mathf.Floor(Deck<T>.Random() * (index + 1));
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }

        public static void ShuffleModulo(T[] deck)
        {
            for (int index = 0, length = deck.Length; index < length; ++index)
            {
                int r = (index + UnityEngine.Random.Range(1, length)) % length;
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }
    }
}
