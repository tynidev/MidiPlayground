using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlashCards
{
    public class Deck : IList<Card>
    {
        private List<Card> cards { get; set; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public Card this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Random rng;

        public Deck()
        {
            this.cards = cards;
            rng = new Random((int)DateTime.Now.Ticks);
        }

        public Card Pick()
        {
            //card.Picked++;
            return this.cards[rng.Next(cards.Count)];
        }

        public int IndexOf(Card item)
        {
            return this.cards.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            this.cards.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.cards.RemoveAt(index);
        }

        public void Add(Card item)
        {
            this.cards.Add(item);
        }

        public void Clear()
        {
            this.cards.Clear();
        }

        public bool Contains(Card item)
        {
            return this.cards.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            this.cards.CopyTo(array, arrayIndex);
        }

        public bool Remove(Card item)
        {
            return this.cards.Remove(item);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return this.cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.cards.GetEnumerator();
        }

        //private static Card pick(List<Card> cards)
        //{
        //    Shuffle(cards);

        //    int sumOfWeights = 0;
        //    foreach (var c in cards)
        //        sumOfWeights += c.Weight;

        //    int randomWeight = rng.Next(1, sumOfWeights);

        //    foreach (var c in cards)
        //    {
        //        randomWeight -= c.Weight;
        //        if (randomWeight <= 0)
        //            return c;
        //    }

        //    throw new Exception("Should not get here");
        //}

        //private static Random rng = new Random((int)DateTime.Now.Ticks);

        //public static void Shuffle<T>(IList<T> list)
        //{
        //    int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rng.Next(n + 1);
        //        T value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}
    }
}
