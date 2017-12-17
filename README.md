# Test shuffling algoritms

Tests randomness of a shuffling algorithm with Unity C#.

### Installation

1. Clone or copy this repo.
1. Open this project or scene in Unity.
1. Open test runner window.
1. Run all tests.

Example run:

## Naive

Reproduces results of:
<https://blog.codinghorror.com/the-danger-of-naivete/>

        TestNaiveShuffle (1.828s)
        ---
        NaiveDeck, 3 cards. 88833 is out of confidence interval [98267.95, 101732]].
        TestDeck.DescribeShuffleError: NaiveDeck, 3 cards. Counts [88833, 111143, 111116, 111056, 88972, 88880].
        NaiveDeck, 4 cards. 93520 is out of confidence interval [98142.59, 101857.4]].
        TestDeck.DescribeShuffleError: NaiveDeck, 4 cards. Counts [93520, 93595, 93792, 131766, 103014, 84620, 93195, 140173, 131336, 131230, 102913, 102792, 103041, 103310, 84652, 103529, 103516, 93445, 75028, 83975, 84491, 75281, 93586, 94200].
          Expected: True
          But was:  False
        ---
        at Finegamedesign.Utils.TestDeck.AssertShuffle (System.Action`1 shuffle, Int32 minCards, Int32 maxCards, System.String message, Int32 skipPermutations) [0x0003f] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:168
        at Finegamedesign.Utils.TestDeck.TestNaiveShuffle () [0x00027] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:188
        ---
        TestDeck.DescribeShuffleError: NaiveDeck, 3 cards. Counts [88833, 111143, 111116, 111056, 88972, 88880].
        TestDeck.DescribeShuffleError: NaiveDeck, 4 cards. Counts [93520, 93595, 93792, 131766, 103014, 84620, 93195, 140173, 131336, 131230, 102913, 102792, 103041, 103310, 84652, 103529, 103516, 93445, 75028, 83975, 84491, 75281, 93586, 94200].

## Naive Modulo

        TestNaiveShuffleModulo (1.318s)
        ---
        NaiveDeck.ShuffleSwapModulo, 3 cards. 0 is out of confidence interval [98267.95, 101732]].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapModulo, 3 cards. Counts [0, 225389, 224571, 0, 0, 150040].
        NaiveDeck.ShuffleSwapModulo, 4 cards. 88414 is out of confidence interval [98142.59, 101857.4]].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapModulo, 4 cards. Counts [88414, 0, 0, 266638, 207583, 0, 0, 326572, 266401, 0, 0, 177456, 207556, 0, 0, 178007, 207501, 0, 0, 147798, 147828, 0, 0, 178246].
          Expected: True
          But was:  False
         ---
         at Finegamedesign.Utils.TestDeck.AssertShuffle (System.Action`1 shuffle, Int32 minCards, Int32 maxCards, System.String message, Int32 skipPermutations) [0x0003f] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:168
         at Finegamedesign.Utils.TestDeck.TestNaiveShuffleModulo () [0x00027] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:209
         ---
         TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapModulo, 3 cards. Counts [0, 225389, 224571, 0, 0, 150040].
         TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapModulo, 4 cards. Counts [88414, 0, 0, 266638, 207583, 0, 0, 326572, 266401, 0, 0, 177456, 207556, 0, 0, 178007, 207501, 0, 0, 147798, 147828, 0, 0, 178246].

## Naive Order By

Correct, but slower.  In this small sample, about 5-times slower.

Discussed here:
<https://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm>

        TestNaiveShuffleOrderBy (8.032s)
        ---
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleOrderBy, 3 cards. Counts [99738, 100009, 100481, 100426, 99654, 99692].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleOrderBy, 4 cards. Counts [99989, 100251, 99863, 99620, 99959, 99639, 99929, 99916, 99786, 100024, 99832, 100546, 100497, 99737, 100444, 99980, 99955, 99759, 100148, 99489, 100495, 99914, 100607, 99621].

## Naive Shuffle Swap Less Than

Naive way to try to shuffle with the original order omitted.  It omits some other permutations too.

        TesttNaiveShuffleSwapLessThan (1.417s)
        ---
        NaiveDeck.ShuffleSwapLessThan, 3 cards. 0 is out of confidence interval [98302.95, 101697.1]].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapLessThan, 3 cards. Counts [0, 0, 0, 250189, 249811, 0].
        NaiveDeck.ShuffleSwapLessThan, 4 cards. 0 is out of confidence interval [98144.34, 101855.7]].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapLessThan, 4 cards. Counts [0, 0, 0, 0, 0, 0, 0, 0, 0, 382453, 383517, 0, 0, 383358, 0, 0, 0, 383411, 383465, 0, 0, 0, 383796, 0].
        Expected: True
        But was:  False
        ---
        at Finegamedesign.Utils.TestDeck.AssertShuffle (System.Action`1 shuffle, Int32 minCards, Int32 maxCards, System.String message, Int32 skipPermutations) [0x0003f] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:168
        at Finegamedesign.Utils.TestDeck.TestNaiveShuffleSwapLessThan () [0x00027] in C:\Users\Ethan\workspace\testshuffle\TestShuffle\Assets\Editor\Tests\TestDeck.cs:195
        ---
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapLessThan, 3 cards. Counts [0, 0, 0, 250189, 249811, 0].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapLessThan, 4 cards. Counts [0, 0, 0, 0, 0, 0, 0, 0, 0, 382453, 383517, 0, 0, 383358, 0, 0, 0, 383411, 383465, 0, 0, 0, 383796, 0].

## Shuffle Up To

Correct.

        TestNaiveShuffleUpTo (1.778s)
        ---
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapUpTo, 3 cards. Counts [100176, 100140, 99479, 99796, 100133, 100276].
        TestDeck.DescribeShuffleError: NaiveDeck.ShuffleSwapUpTo, 4 cards. Counts [100451, 99508, 99852, 99815, 99618, 100426, 100217, 100161, 99945, 100211, 99883, 99954, 100306, 100410, 100316, 99658, 99729, 99663, 99824, 100224, 100001, 99481, 100092, 100255].

## Shuffle

Standard shuffle.

        TestShuffle (1.530s)
        ---
        TestDeck.DescribeShuffleError: Deck.Shuffle, 3 cards. Counts [100147, 99887, 99828, 99877, 100406, 99855].
        TestDeck.DescribeShuffleError: Deck.Shuffle, 4 cards. Counts [99568, 99734, 100179, 100150, 100315, 100434, 99874, 100015, 99459, 100204, 99795, 100121, 100095, 100022, 100545, 99748, 99898, 100360, 99934, 100235, 99762, 99269, 99920, 100364].

## Shuffle Until Changed

Omits the original permutation.  All others are equally likely.

        TestShuffleUntilChanged (2.826s)
        ---
        TestDeck.DescribeShuffleError: Deck.ShuffleUntilChanged, 3 cards. Counts [0, 100127, 99839, 100137, 99911, 99986].
        TestDeck.DescribeShuffleError: Deck.ShuffleUntilChanged, 4 cards. Counts [0, 100508, 100447, 100260, 99893, 100385, 100501, 100174, 99836, 99176, 99816, 100165, 99714, 99795, 100016, 100230, 99614, 99592, 99939, 99858, 99884, 100070, 99974, 100153].


