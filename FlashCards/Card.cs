using System;

namespace FlashCards
{
    public class Card
    {
        public int correctAnswers = 0;

        public Card(object card)
        {
            this.Object = card;
        }

        private object Object;
        public object Get()
        {
            return this.Object;
        }

        public void RecordAnswer(bool correct, int difficulty = 1)
        {
            //card.Weight = (((card.Picked - 1) * card.Weight) + (int)watch.ElapsedMilliseconds) / (card.Picked);

            if (correct)
                this.correctAnswers += 1 * difficulty;
            else
                this.correctAnswers -= 1 * difficulty;
        }
    }
}
