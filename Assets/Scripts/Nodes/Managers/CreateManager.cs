using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

namespace Nodes
{
    public class CreateManager
    {
        /// <summary>
        /// Класс со значениями матрицы
        /// </summary>
        private NodeMatrix matrix;

        /// <summary>
        /// Конструктор для инициализации матрицы
        /// </summary>
        /// <param name="_matrix">Матрица</param>
        public CreateManager(NodeMatrix _matrix)
        {
            // Инициализируем матрицу
            matrix = _matrix;
        }

        /// <summary>
        /// Метод создающий узлы 
        /// </summary>
        public void Create()
        {
            // Если количество Raycast-ов чётное количество
            if (NodeSetting.count % 2 == 0)
            {
                // Создаём и расставляем узлы в чётном виде (агент, по осям x и z, находится в середине между четырьмя центральными узлами)
                CreateEvenCountNodes();
            }
            // Иначе, если количество Raycast-ов не чётное количество
            else
            {
                // Создаём и расставляем узлы в не чётном виде (Центральный узел находится ровно над агентом)
                CreateOddCountNodes();
            }
        }

        public void DestroyAllNodes()
        {
            NodeList.nodeList.Clear();
            NodeList.rawNodeList.Clear();
        }

        /// <summary>
        /// Создаём и расставляем узлы в чётном виде (агент, по осям x и z, находится в середине между четырьмя центральными узлами) 
        /// </summary>
        private void CreateEvenCountNodes()
        {
            // Проходим от отрицательного значения заданного количества делённого пополам до положительного значения заданного количества делённого пополам
            for (int i = -(NodeSetting.count / 2); i < NodeSetting.count / 2; i++)
            {
                // Проходим от отрицательного значения заданного количества делённого пополам до положительного значения заданного количества делённого пополам
                for (int j = -(NodeSetting.count / 2); j < NodeSetting.count / 2; j++)
                {
                    // Позиция рассчитывается следующим образом
                    // Берём позицию столкновения с маской и добавляем её в базу
                    AddNodes(RaycastAll(Helper.VectorCreater(matrix.Transform.position.x + NodeSetting.wolkDistance * i + NodeSetting.wolkDistance * 0.5f,
                                                             matrix.Transform.position.y,
                                                             matrix.Transform.position.z + NodeSetting.wolkDistance * j + NodeSetting.wolkDistance * 0.5f)), i, j);
                }
            }
        }

        /// <summary>
        /// Создаём и расставляем узлы в не чётном виде (Центральный узел находится ровно над агентом)
        /// </summary>
        private void CreateOddCountNodes()
        {
            // Проходим от отрицательного значения заданного количества делённого пополам до положительного значения заданного количества делённого пополам
            for (int i = -(NodeSetting.count / 2); i <= NodeSetting.count / 2; i++)
            {
                // Проходим от отрицательного значения заданного количества делённого пополам до положительного значения заданного количества делённого пополам
                for (int j = -(NodeSetting.count / 2); j <= NodeSetting.count / 2; j++)
                {
                    // Позиция рассчитывается следующим образом
                    // Берём позицию столкновения с маской и добавляем её в базу
                    AddNodes(RaycastAll(Helper.VectorCreater(matrix.Transform.position.x + NodeSetting.wolkDistance * i,
                                                             matrix.Transform.position.y,
                                                             matrix.Transform.position.z + NodeSetting.wolkDistance * j)), i, j);
                }
            }
        }

        /// <summary>
        /// Берём позиции сталкивающиеся с маской (заменить на Frame)
        /// </summary>
        /// <param name="position">Позиция начала луча</param>
        /// <returns>Все найденные плоскости на пути луча</returns>
        private RaycastHit[] RaycastAll(Vector3 position)
        {
            // Берём все найденные плоскости на пути луча с началом в заданной позиции и направленном вниз на условно бесконечную дистанцию
            return Physics.RaycastAll(position, Vector3.down, Mathf.Infinity, NodeSetting.layerMask);
        }

        /// <summary>
        /// Добавляем узлы
        /// </summary>
        /// <param name="hits">Столкнувшиеся Node</param>
        /// <param name="X">нумерация по x</param>
        /// <param name="Z">нумерация по z</param>
        private void AddNodes(RaycastHit[] hits, float X, float Z)
        {
            // Проходим по всем плоскостям с которыми столкнулись
            for (int i = 0; i < hits.Length; i++)
            {
                // Обновляем или создаём узел с заданной позицией
                NodeList.Update(Helper.VectorCreater(matrix.Transform.position.x + NodeSetting.wolkDistance * X,
                                                     hits[i].transform.position.y,
                                                     matrix.Transform.position.z + NodeSetting.wolkDistance * Z));
            }
        }
    }
}
