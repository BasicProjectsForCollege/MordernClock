using System.Windows.Media.Animation;

namespace MordernClock
{
    public class EasingFunction : IEasingFunction
    {
        
        public EasingMode EasingMode { get; set; } = EasingMode.EaseInOut;

        
        private static float EaseInOutExpo(float x)
        {
            if (x == 0f) return 0f;
            if (x == 1f) return 1f;
            if (x < 0.5f)
                return (float)(Math.Pow(2, 20 * x - 10) / 2);
            else
                return (float)((2 - Math.Pow(2, -20 * x + 10)) / 2);
        }

        
        public double Ease(double t)
        {
            float x = (float)t;

            return EaseInOutExpo(x);
            
        }
    }
}