using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Services
{
    public class DeterministicRandomSorter
    {
        private readonly int _seed;

        public DeterministicRandomSorter(int seed)
        {
            _seed = PseudoRandomFunction(seed);
        }
        
        public int PseudoRandomFunction(int a)
        {
            // http://burtleburtle.net/bob/hash/integer.html
            a = (a ^ 61) ^ (a >> 16);
            a = a + (a << 3);
            a = a ^ (a >> 4);
            a = a * 0x27d4eb2d;
            a = a ^ (a >> 15);
            return a;
        }

        public void Shuffle<T>(int arraySeed, ref T[] array)
        {
            var r = new Random(_seed ^ PseudoRandomFunction(arraySeed));
            int n = array.Count();
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }
    }
}
