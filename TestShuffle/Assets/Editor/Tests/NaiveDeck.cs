using System.Linq;
using UnityEngine;

namespace Finegamedesign.Utils
{
    // Most of these shuffling algorithms are biased or omit some permutations.
    // For proof, run TestDeck.
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

        public static void ShuffleSwapLessThan(T[] deck)
        {
            for (int index = deck.Length - 1; index > 0; --index)
            {
                int r = (int) Mathf.Floor(Deck<T>.Random() * index);
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }

        // This is also unbiased.  The other algorithms are biased.
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

        // Practically correct shuffling, but inefficent.
        // https://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm
        public static void ShuffleOrderBy(T[] deck)
        {
            var shuffled = deck.OrderBy(x => Deck<T>.Random());
            int index = 0;
            foreach (T number in shuffled)
            {
                deck[index++] = number;
            }
        }
    }
}
