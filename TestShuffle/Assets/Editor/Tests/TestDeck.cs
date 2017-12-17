using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class TestDeck
    {
        private static Dictionary<int, int> s_HashPermutationIndexes = new Dictionary<int, int>();

        private bool m_Verbose = true;

        // Copied from:
        // https://stackoverflow.com/questions/3404715/c-sharp-hashcode-for-array-of-ints
        private static int GetHashCode(int[] numbers)
        {
            int length = numbers.Length;
            int code = length;
            for (int i = 0; i < length; ++i)
            {
                code = unchecked(314159 * code + numbers[i]);
            }
            return code;
        }

        // Would be faster to memoize.
        // Memoize by length of permutation and array values.
        // Copied from:
        // http://www.geekviewpoint.com/java/numbers/permutation_index
        private static int PermutationIndex(int[] permutation)
        {
            int hashCode = GetHashCode(permutation);
            if (s_HashPermutationIndexes.ContainsKey(hashCode))
            {
                return s_HashPermutationIndexes[hashCode];
            }
            int index = 0;
            // position 1 is paired with factor 0 and so is skipped
            int position = 2;
            int factor = 1;
            for (int p = permutation.Length - 2; p >= 0; p--)
            {
                int successors = 0;
                for (int q = p + 1; q < permutation.Length; q++)
                {
                   if (permutation[p] > permutation[q])
                   {
                       successors++;
                   }
                }
                index += (successors * factor);
                factor *= position;
                position++;
            }
            s_HashPermutationIndexes[hashCode] = index;
            return index;
        }

        // 3 integers representing 3 bits:  0, 1, 2.
        // Shift 3 bits to take up 3 bits total.
        // There are 6-permutations of 3 numbers.
        // Only need to look at first 2.
        // Shift first by 1 bit.
        // Combine with bitwise or of next.
        // In a perfect shuffle, odds are 1/6 for each permutation.
        // Precompute confidence interval above 99.999% (6 standard deviations).
        // Test if within confidence interval.
        // https://onlinecourses.science.psu.edu/stat100/node/56
        private void AssertShuffle(Action<int[]> shuffle, int numCards, string message)
        {
            int numPermutations = numCards;
            for (int c = 2; c < numCards; ++c)
            {
                numPermutations *= c;
            }
            int expectedCount = 100000;
            int numSamples = numPermutations * expectedCount;
            int[] cards = new int[numCards];
            int[] counts = new int[numPermutations];
            for (int sample = 0; sample < numSamples; ++sample)
            {
                for (int c = 0; c < numCards; ++c)
                {
                    cards[c] = c;
                }
                shuffle(cards);
                int index = PermutationIndex(cards);
                counts[index]++;
            }
            if (m_Verbose)
            {
                string[] countStrings = new string[numPermutations];
                for (int p = 0; p < numPermutations; ++p)
                {
                    countStrings[p] = counts[p].ToString();
                }
                Debug.Log("TestDeck.AssertShuffle3: Counts ["
                    + string.Join(", ", countStrings)
                    + "]. " + message
                    + " num cards " + numCards);
            }
            float standardDeviations = 6.0f;
            float proportion = 1.0f / numPermutations;
            float confidenceMargin = numSamples * standardDeviations * Mathf.Sqrt(
                (proportion * (1.0f - proportion))
                / numSamples
            );
            float minConfidence = expectedCount - confidenceMargin;
            float maxConfidence = expectedCount + confidenceMargin;
            for (int p = 0; p < numPermutations; ++p)
            {
                int count = counts[p];
                Assert.IsTrue(count >= minConfidence && count <= maxConfidence,
                    count + " is out of confidence interval [" + minConfidence + ", " + maxConfidence + "].  num cards " + numCards + " message " + message);
            }
        }

        private void AssertShuffle(Action<int[]> shuffle, int minCards, int maxCards, string message)
        {
            for (int numCards = minCards; numCards <= maxCards; ++numCards)
            {
                AssertShuffle(shuffle, numCards, message);
            }
        }

        [Test]
        public void TestNaiveShuffle3()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleSwapAny, 3, 4, "NaiveDeck");
        }

        [Test]
        public void TestShuffle3()
        {
            AssertShuffle(Deck<int>.Shuffle, 3, 4, "Deck");
        }

        [Test]
        public void TestNaiveShuffleUpTo3()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleSwapUpTo, 3, 4, "NaiveDeck SwapUpTo");
        }
    }
}
