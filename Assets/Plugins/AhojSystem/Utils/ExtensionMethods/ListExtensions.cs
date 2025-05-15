using System;
using System.Collections.Generic;

namespace Plugins.AhojSystem.Tools.Utils.ExtensionMethods {
    public static class ListExtensions {
        public static T Choice<T>(this List<T> array) {
            if (array.Count == 0) throw new InvalidOperationException("array is empty");
            var rnd = new Random();
            var index = rnd.Next(0, array.Count);
            return array[index];
        }

        // Listの中身をランダムに並び替えて新しいListを返すメソッド
        public static List<T> Shuffle<T>(this List<T> list) {
            var rnd = new Random();
            for (var i = 0; i < list.Count; i++) {
                var temp = list[i];
                var randomIndex = rnd.Next(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }
    }
}