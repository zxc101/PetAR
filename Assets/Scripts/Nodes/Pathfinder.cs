using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class Pathfinder
    {
        /// <summary>
        /// Поиск кратчайшего пути построенного по алгоритму AStar
        /// </summary>
        /// <param name="startPos">Стартовая позиция</param>
        /// <param name="targetPos">Позиция цели</param>
        /// <returns></returns>
        public static Stack<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            // Находим узел находящийся ближе всего к стартовой позиции
            Node startNode = NodeList.NearestNode(startPos);
            if (startNode == null) return null;
            // Находим узел находящийся ближе всего к позиции цели
            Node targetNode = NodeList.NearestNode(targetPos);
            if (targetNode == null) return null;

            // Создаём коллекцию рассматриваемых узлов, по которым осуществляется поиск кратчайшего пути
            Heap<Node> openSet = new Heap<Node>(NodeList.nodeList.Count);
            // Инициализируем коллекция закрытых узлов, которые уже были рассмотренны при поиске кратчайшего пути
            HashSet<Node> closedSet = new HashSet<Node>();

            // Добавляем в коллекцию рассматриваемых узлов стартовую позицию
            openSet.Add(startNode);

            // Пока коллекция не пустая
            while (openSet.Count > 0)
            {
                // Берём подходящий узел из рассматриваемых узлов и записываем как текущий рассматриваемый узел
                Node currentNode = openSet.RemoveFirst();
                // Добавляем этот узел в коллекцию закрытых узлов
                closedSet.Add(currentNode);

                // Если текущий узел равен узлу, который ближе всего находится к позиции цели
                if (currentNode == targetNode)
                {
                    // Записываем в стек узлы рассмотренные в обратном порядке, если таковых нету, прото создаём стек
                    Stack<Vector3> path = RetracePath(startNode, targetNode) ?? new Stack<Vector3>();
                    // Если количество элементов в коллекции найденных узлов равен нулю
                    if (path.Count == 0)
                    {
                        // Под конец в коллекцию найденых узлов добавляем узел который ближе всего к стартовой позиции
                        path.Push(startPos);
                    }
                    // Выводим этот путь
                    return path;
                }

                // Берём соседей текущего узла
                List<Node> neighbors = currentNode.neighborsList;

                // Рассматриваем соседей текущего узла
                for (int i = 0; i < neighbors.Count; i++)
                {
                    // Если соседний узел уже находится в закрытом листе
                    if (closedSet.Contains(neighbors[i]))
                    {
                        // Рассматриваем других соседей
                        continue;
                    }

                    // Приплюсовываем к дистанции текущего пути, дистанцию от текущего узла, до аперируемого соседа
                    float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbors[i]);
                    // Если это значение меньше чем путь от соседа до текущего узла или он не находится в отркытом листе
                    if (newMovementCostToNeighbor < neighbors[i].gCost || !openSet.Contains(neighbors[i]))
                    {
                        // Записываем его в дистанцию пути от начала до данного соседа
                        neighbors[i].gCost = newMovementCostToNeighbor;
                        // Записываем для данного соседа дистанцию от данного соседа до позиции цели
                        neighbors[i].hCost = GetDistance(neighbors[i], targetNode);
                        // Записываем данного соседа как следующий узел в пути
                        neighbors[i].parent = currentNode;

                        // Если данного узла нету в открытом листе
                        if (!openSet.Contains(neighbors[i]))
                        {
                            // Записываем его туда
                            openSet.Add(neighbors[i]);
                        }
                    }
                }
            }
            // Если путь не удалось найти выводим null
            return null;
        }

        /// <summary>
        /// Данный метод переворачивает получившийся лист и вывдит путь в качестве стека
        /// </summary>
        /// <param name="startNode">Начальный узел</param>
        /// <param name="endNode">Узел цели</param>
        /// <returns>Путь</returns>
        private static Stack<Vector3> RetracePath(Node startNode, Node endNode)
        {
            // Создаём стек для узлов которые будут использоваться в качестве пути
            Stack<Vector3> path = new Stack<Vector3>();
            // В текущий узел записываем последний узел
            Node currentNode = endNode;

            // Пока текущий узел не является начальным узлом, то есть не пройдёт весь путь
            while (currentNode != startNode)
            {
                // В стек добавляется текущий узел
                path.Push(currentNode.position);
                // В текущий узел записывается следующий узел
                currentNode = currentNode.parent;
            }

            // Выводим путь
            return path;
        }

        /// <summary>
        /// Находит дистанцию от первого узла, до второго узла
        /// </summary>
        /// <param name="nodeA">Первый узел</param>
        /// <param name="nodeB">Второй узел</param>
        /// <returns>Дистанция между узлами</returns>
        private static float GetDistance(Node nodeA, Node nodeB)
        {
            // Выводим дистанцию от первого пути до последнего
            return Vector3.Distance(nodeA.position, nodeB.position);
        }
    }
}
