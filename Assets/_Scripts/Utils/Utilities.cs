using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        public static int[] GetRandomArrayIndexes(int length, int min, int max)//Хуета
        {
            int[] array = new int[length];

            int i = 0;
            while (!i.Equals(length))
            {
                var num = Random.Range(min, max);
                if(!array.Contains(num))
                {
                    array[i] = num;
                    i++;
                }
            }
               

            return array;
        }
    }

}
