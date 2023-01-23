using UnityEngine;

namespace Model
{
    public class Ticker
    {
        private float lastTickTime;
        private float totalAdjustment;
        private readonly LocalTime localTime;

        public Ticker(LocalTime localTime)
        {
            this.localTime = localTime;
        }
    
        public int Current = 0;

        public void Adjust(float adjustment)
        {
            totalAdjustment += adjustment;
        }

        public int TryTick()
        {
            var time = GetAdjTime();
            var ticked = Mathf.FloorToInt((time - lastTickTime) / Constants.TickIntervalS);
            if (ticked > 0)
            {
                lastTickTime += ticked * Constants.TickIntervalS;
                Current += (int)ticked;

                return (int)ticked;
            }

            return 0;
        }

        private float GetAdjTime()
        {
            return localTime.Current + totalAdjustment;
        }
    }
}
