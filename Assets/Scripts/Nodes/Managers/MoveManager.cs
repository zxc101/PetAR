using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

namespace Nodes
{
    public class MoveManager
    {
        // Класс со значениями матрицы
        private NodeMatrix matrix;

        /// <summary>
        /// Конструктор для инициализации матрицы
        /// </summary>
        /// <param name="_matrix">Матрица</param>
        public MoveManager(NodeMatrix _matrix)
        {
            // Инициализируем матрицу
            matrix = _matrix;
        }

        /// <summary>
        /// Метод передвигающий матрицу
        /// </summary>
        public void Move()
        {
            // Берём дистанцию между агентом и матрицей
            Vector3 petDistance = NodeSetting.pet.position - matrix.Transform.position;

            // Если дистанция между агентом и матрицей равна или превышает дистанцию между узлами по оси X
            if (Mathf.Abs(NodeSetting.pet.position.x - matrix.Transform.position.x) >= NodeSetting.wolkDistance)
            {
                // Передвигаем её по оси X приплюсовывая к её позиции вектор отвечающий только за ось X умноженный на дистанцию между узлами
                // и умноженную на дистанцию между агентом и матрицей делёную на абсолютное её значение
                matrix.Transform.position += Vector3.right * NodeSetting.wolkDistance * (petDistance.x / Mathf.Abs(petDistance.x));
                // Создаём или обновляем в данной позиции узлы которые матрица смогла найти
                matrix.CreateManager.Create();
            }

            // Если дистанция между агентом и матрицей равна или превышает дистанцию между узлами по оси Z
            if (Mathf.Abs(NodeSetting.pet.position.z - matrix.Transform.position.z) >= NodeSetting.wolkDistance)
            {
                // Передвигаем её по оси Z приплюсовывая к её позиции вектор отвечающий только за ось Z умноженный на дистанцию между узлами
                // и умноженную на дистанцию между агентом и матрицей делёную на абсолютное её значение
                matrix.Transform.position += Vector3.forward * NodeSetting.wolkDistance * (petDistance.z / Mathf.Abs(petDistance.z));
                // Создаём или обновляем в данной позиции узлы которые матрица смогла найти
                matrix.CreateManager.Create();
            }
            
            // Выставляем значения позиции
            matrix.Transform.position = Helper.VectorCreater(matrix.Transform.position.x,
                                                             NodeSetting.pet.position.y + 5,
                                                             matrix.Transform.position.z);
        }
    }
}
