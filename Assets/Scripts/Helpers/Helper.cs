using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class Helper
    {
        /// <summary>
        /// Булевый показатель - идёт ли сейчас игра или игрок в данный момент находится на создании карты
        /// </summary>
        public static bool isPlay;

        public static Transform ChoicesNeeds;

        private static Vector4 helpVector;

        /// <summary>
        /// Собирает вектор трёхмерного пространства
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <param name="z">W</param>
        /// <returns>Получившийся вектор</returns>
        public static Vector4 VectorCreater(float x, float y, float z, float w)
        {
            helpVector.x = x;
            helpVector.y = y;
            helpVector.z = z;
            helpVector.w = w;
            return helpVector;
        }

        /// <summary>
        /// Собирает вектор трёхмерного пространства
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>Получившийся вектор</returns>
        public static Vector3 VectorCreater(float x, float y, float z)
        {
            helpVector.x = x;
            helpVector.y = y;
            helpVector.z = z;
            return helpVector;
        }

        /// <summary>
        /// Собирает вектор двумерного пространства
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Получившийся вектор</returns>
        public static Vector2 VectorCreater(float x, float y)
        {
            helpVector.x = x;
            helpVector.y = y;
            return helpVector;
        }

        /// <summary>
        /// Находит значение по проценту и обозначенному максимуму
        /// </summary>
        /// <param name="max">максимальное значение</param>
        /// <param name="percent">текущий процент</param>
        /// <returns>Текущее значение</returns>
        public static float PercentToValue(int percent, float max)
        {
            float res = -1;
            if (percent >= 0 && percent <= 100)
            {
                res = percent * max / 100;
            }
            return res;
        }

        /// <summary>
        /// Находит процент по значению и обозначенному максимуму
        /// </summary>
        /// <param name="max">максимальное значение</param>
        /// <param name="percent">текущий значение</param>
        /// <returns>Текущей процент</returns>
        public static float ValueToPercent(float val, float max)
        {
            float res = -1;
            if (val <= max)
            {
                res = val * 100 / max;
            }
            return res;
        }
    }
}
