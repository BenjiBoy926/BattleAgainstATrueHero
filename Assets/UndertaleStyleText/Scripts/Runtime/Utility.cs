using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UndertaleStyleText
{
    public static class Utility
    {
        public static T GetValueOrDefault<T>(this T[] array, int index)
        {
            if (index >= 0 && index < array.Length) return array[index];
            else return default;
        }   
    }
}
