using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jigsaw
{
    class GetRandom
    {
        public static Random globalRandomCentor = GenerateNewRandomGenerator();
        public static Random GenerateNewRandomGenerator()
        {
      
            globalRandomCentor = new Random((int)DateTime.Now.Ticks);
            return globalRandomCentor;
        }

        public static int GetRandonInt()
        {
            return globalRandomCentor.Next(10);
        }

        public static int GetRandonInt(int max)
        {
            return globalRandomCentor.Next(max);
        }

        public static int GetRandonInt(int min,int max)
        {
            return globalRandomCentor.Next(max-min)+min;
        }

        public static float GetRandonInt(float min, float max)
        {
            return (float)globalRandomCentor.NextDouble()*(max-min) + min;
        }
    }
}
