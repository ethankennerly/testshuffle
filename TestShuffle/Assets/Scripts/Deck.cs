using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class Deck<T>
    {
        // Uniform distribution between -radius and +radius inclusive.
        public static int RandomRadius(int radius)
        {
            return UnityEngine.Random.Range(-radius, radius + 1);
        }

        // Unity Random docs says it includes 1.0, which would be out of range.
        public static float Random()
        {
            float value = UnityEngine.Random.value;
            for (int attempt = 0; value >= 1.0f && attempt < 256; ++attempt)
            {
                value = UnityEngine.Random.value;
            }
            if (value >= 1.0f)
            {
                value = 0.0f;
            }
            return value;
        }

        public static void Shuffle(T[] deck)
        {
            for (int index = deck.Length - 1; index > 0; --index)
            {
                int r = (int) Mathf.Floor(Random() * (index + 1));
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }

        public static void Shuffle(List<T> deck)
        {
            for (int index = deck.Count - 1; index > 0; --index)
            {
                int r = (int) Mathf.Floor(Random() * (index + 1));
                T swap = deck[index];
                deck[index] = deck[r];
                deck[r] = swap;
            }
        }

        private int m_Index = -1;
        private int m_Length = -1;
        private T[] m_Cards;

        public void Setup(T[] originals, int copies)
        {
            int original = originals.Length;
            m_Length = copies * original;
            m_Cards = new T[m_Length];
            for (m_Index = 0; m_Index < m_Length; ++m_Index)
            {
                m_Cards[m_Index] = originals[m_Index % original];
            }
            Shuffle(m_Cards);
            m_Index = -1;
        }

        public T NextCard()
        {
            ++m_Index;
            if (m_Length <= m_Index)
            {
                Shuffle(m_Cards);
                m_Index = 0;
            }
            return m_Cards[m_Index];
        }
    }
}
