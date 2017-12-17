using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class TestDeck
    {
        private const int kMinCards = 3;
        private const int kMaxCards = 4;

        private static Dictionary<int, int> s_HashPermutationIndexes =
            new Dictionary<int, int>();

        private static bool s_Verbose = true;

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

        // Returns error message.
        //
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
        private static string DescribeShuffleError(Action<int[]> shuffle,
            int numCards, string message, int skipPermutations = 0)
        {
            int numPermutations = numCards;
            for (int c = 2; c < numCards; ++c)
            {
                numPermutations *= c;
            }
            int includedPermutations = numPermutations - skipPermutations;
            int expectedCount = 100000;
            int numSamples = includedPermutations * expectedCount;
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
            string log = "";
            if (s_Verbose)
            {
                string[] countStrings = new string[numPermutations];
                for (int p = 0; p < numPermutations; ++p)
                {
                    countStrings[p] = counts[p].ToString();
                }
                log = "TestDeck.DescribeShuffleError: " + message
                    + ", " + numCards + " cards."
                    + " Counts [" + string.Join(", ", countStrings) + "].";
                Debug.Log(log);
            }
            float proportion = 1.0f / includedPermutations;
            string confidenceMessage = DescribeLackOfConfidence(proportion,
                numSamples, counts, skipPermutations);
            if (!string.IsNullOrEmpty(confidenceMessage))
            {
                return "\n" + message + ", " + numCards + " cards. "
                    + confidenceMessage
                    + "].\n" + log;
            }
            return "";
        }

        private static string DescribeLackOfConfidence(float proportion,
            int numSamples, int[] counts, int skipPermutations)
        {
            int numPermutations = counts.Length;
            float confidenceMargin = CalculateConfidenceMargin(
                proportion, numSamples);
            int expectedCount = (int)Mathf.Round(numSamples * proportion);
            float minConfidence = expectedCount - confidenceMargin;
            float maxConfidence = expectedCount + confidenceMargin;
            for (int p = skipPermutations; p < numPermutations; ++p)
            {
                int count = counts[p];
                bool isExpected = count >= minConfidence
                    && count <= maxConfidence;
                if (!isExpected)
                {
                    return count + " is out of confidence interval ["
                        + minConfidence + ", " + maxConfidence + "]";
                }
            }
            return null;
        }

        private static float CalculateConfidenceMargin(float proportion,
            int numSamples)
        {
            float standardDeviations = 6.0f;
            float confidenceMargin = numSamples * standardDeviations
                * Mathf.Sqrt(
                    (proportion * (1.0f - proportion))
                    / numSamples
                );
            return confidenceMargin;
        }

        private void AssertShuffle(Action<int[]> shuffle, int minCards,
            int maxCards, string message, int skipPermutations = 0)
        {
            string errorMessages = "";
            for (int numCards = minCards; numCards <= maxCards; ++numCards)
            {
                errorMessages += DescribeShuffleError(shuffle, numCards,
                    message, skipPermutations);
            }
            Assert.IsTrue(errorMessages == "", errorMessages);
        }

        [Test]
        public void TestShuffle()
        {
            AssertShuffle(Deck<int>.Shuffle, kMinCards, kMaxCards,
                "Deck.Shuffle");
        }

        [Test]
        public void TestShuffleUntilChanged()
        {
            AssertShuffle(Deck<int>.ShuffleUntilChanged, kMinCards, kMaxCards,
                "Deck.ShuffleUntilChanged", 1);
        }

        [Test]
        public void TestNaiveShuffle()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleSwapAny, kMinCards, kMaxCards,
                "NaiveDeck");
        }

        [Test]
        public void TestNaiveShuffleSwapLessThan()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleSwapLessThan, kMinCards, kMaxCards,
                "NaiveDeck.ShuffleSwapLessThan", 1);
        }

        [Test]
        public void TestNaiveShuffleUpTo()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleSwapUpTo, kMinCards, kMaxCards,
                "NaiveDeck.ShuffleSwapUpTo");
        }

        [Test]
        public void TestNaiveShuffleModulo()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleModulo, kMinCards, kMaxCards,
                "NaiveDeck.ShuffleSwapModulo");
        }

        [Test]
        public void TestNaiveShuffleOrderBy()
        {
            AssertShuffle(NaiveDeck<int>.ShuffleOrderBy, kMinCards, kMaxCards,
                "NaiveDeck.ShuffleOrderBy");
        }
    }
}
