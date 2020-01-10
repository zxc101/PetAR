using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class Node : IHeapItem<Node>
    {
        /// <summary>
        /// Количество узлов требующихся для обозначения, что этот узел уже не сырой
        /// </summary>
        public const int MAX_NEIGHBORS = 8;

        /// <summary>
        /// Позиция узла
        /// </summary>
        public Vector3 position { get; private set; }
        /// <summary>
        /// Лист с соседними узлами
        /// </summary>
        public List<Node> neighborsList = new List<Node>();

        /// <summary>
        /// Родительский узел, используется для алгоритма поиска кратчайшего пути
        /// </summary>
        public Node parent;

        /// <summary>
        /// Дистанция пути от начала до данного узла
        /// </summary>
        public float gCost;
        /// <summary>
        /// Длинна от конца до данного узла
        /// </summary>
        public float hCost;

        /// <summary>
        /// Цвет узла
        /// </summary>
        private Color color;
        /// <summary>
        /// Индекс в куче
        /// </summary>
        private int heapIndex;

        /// <summary>
        /// Суммарное значение дистанций пути от начала до данного узла и до конца пути
        /// </summary>
        public float fCost { get => gCost + hCost; }
        /// <summary>
        /// Является ли этот узел чистым
        /// </summary>
        public bool IsClean { get; private set; }
        /// <summary>
        /// Публичное свойство показывающие индекс в куче
        /// </summary>
        public int HeapIndex
        {
            get => heapIndex;
            set { heapIndex = value; }
        }

        /// <summary>
        /// Конструктор узла
        /// </summary>
        /// <param name="_position">Позиция</param>
        public Node(Vector3 _position)
        {
            // Задаёт позицию узла
            position = _position;
            // Отмечает узел как сырой
            IsClean = false;
        }

        /// <summary>
        /// Метод для соединения соседних узлов
        /// </summary>
        /// <param name="neighborNode">Соседний узел</param>
        public void ConnectNode(Node neighborNode)
        {
            // Если в листе соседних узлов нету заданного узла
            if (!neighborsList.Exists(x => x.position == neighborNode.position))
            {
                // Добавляем заданный узел в лист
                neighborsList.Add(neighborNode);
            }

            // Если количество соседних узлов равно количеству достаточному для выведения данного узла из листа сырых
            if (neighborsList.Count == MAX_NEIGHBORS)
            {
                // Ставим данный узел как готовый
                IsClean = true;
            }
        }

        /// <summary>
        /// Метод для удаления из соседних узлов
        /// </summary>
        public void DisconnectNode()
        {
            // Проходим по всему листу с соседними узлами
            for (int i = 0; i < neighborsList.Count; i++)
            {
                // Если выбранный узел имеет в себе текущий узел
                if (neighborsList[i].neighborsList.Exists(x => x.position == position))
                {
                    // Удаляем текущий узел из листа соседей, соседнего узла
                    neighborsList[i].neighborsList.Remove(this);
                    // Делаем текущий узел сырым
                    IsClean = false;
                }
            }
        }

        /// <summary>
        /// Сравниваем два узла на дальность
        /// </summary>
        /// <param name="nodeToCompare"></param>
        /// <returns></returns>
        public int CompareTo(Node nodeToCompare)
        {
            // Берём сравнение cуммарного значения дистанций пути от начала до данного узла и до соседнего узла
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            // Если данное сравнение равно нулю
            if (compare == 0)
            {
                // Сравниваем оба узла по дальности к концу
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            // Выводим его отрицательное значение
            return -compare;
        }
    }
}
