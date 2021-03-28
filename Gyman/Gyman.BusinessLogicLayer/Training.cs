using System;

namespace Gyman.BusinessLogicLayer
{
    public class Training
    {
        public Training() 
        {
            Start = DateTime.Now;
            End = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }
    }
}
