using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class NodeList
    {
        /// <summary>
        /// Лист с сырыми узлами (сырой лист)
        /// </summary>
        public static List<Node> rawNodeList = new List<Node>();
        /// <summary>
        /// Лист с готовыми узлами (чистый лист)
        /// </summary>
        public static List<Node> nodeList = new List<Node>();

        /// <summary>
        /// Находит ближайший узел или создаёт узел соответствующий заданной позиции
        /// </summary>
        /// <param name="position">Проверяемая позиция</param>
        /// <returns>Узел соответствующий заданной позиции</returns>
        public static Node NearestNode(Vector3 position)
        {
            Node nearestNode = null;
            if (nodeList.Count != 0)
            {
                // Берёт первый узел
                nearestNode = nodeList[0];
                // Проходится по всем готовым узлам
                for (int i = 0; i < nodeList.Count; i++)
                {
                    // Если дистанция от i-того узла, до заданного узла меньше чем при сравнении с предыдущим подходящим узлом
                    if (Vector3.Distance(nodeList[i].position, position) < Vector3.Distance(nearestNode.position, position))
                    {
                        // Заменяем старый узел на новый
                        nearestNode = nodeList[i];
                    }
                }
            }
            // В конце возвращаем получившийся узел если такой есть
            return nearestNode;
        }

        /// <summary>
        /// Берёт всех соседей того узла, который находится на заданной позиции
        /// </summary>
        /// <param name="position">Проверяемая позиция</param>
        /// <returns>Лист из соседей</returns>
        public static List<Node> GetNeighbors(Vector3 position)
        {
            // Берёт всех соседей того узла, который находится на заданной позиции
            return NearestNode(position).neighborsList;
        }

        /// <summary>
        /// Добавляет Node
        /// </summary>
        /// <param name="position">Позиция добавления Node</param>
        public static void Update(Vector3 position)
        {
            // Ищем узел по позиции
            Node node = rawNodeList.Find(x => x.position == position);

            // Если этот узел найден
            if (node != null)
            {
                // Если соседей данного узла меньше чем минимальное количество требуемое для прохождения в сырой лист
                if (node.neighborsList.Count < Node.MAX_NEIGHBORS)
                {
                    // Отрываем этому узлу все связи
                    node.DisconnectNode();
                    // Удаляем из сырого листа
                    rawNodeList.Remove(node);
                    // Обновляем заданную позицию
                    Update(position);
                    // Выходим из метода
                    return;
                }
            }
            // Иначе
            else
            {
                // Ищем узел в чистом листе по заданной позиции
                node = nodeList.Find(x => x.position == position);
                // Если соседей данного узла меньше чем минимальное количество требуемое для прохождения в чистый лист
                if (node != null && node.neighborsList.Count < Node.MAX_NEIGHBORS)
                {
                    // Отрываем этому узлу все связи
                    node.DisconnectNode();
                    // Удаляем из сырого листа
                    nodeList.Remove(node);
                    // Обновляем заданную позицию
                    Update(position);
                    // Выходим из метода
                    return;
                }
            }

            // Если нету в nodeList и в rawNodeList то ...
            if (!nodeList.Exists(x => x.position == position) &&
                !rawNodeList.Exists(x => x.position == position))
            {
                // Создаём узел по заданной позиции
                BuildNode(position);
            }
        }

        /// <summary>
        /// Метод создающий узел по позиции
        /// </summary>
        /// <param name="position">Позиция</param>
        private static void BuildNode(Vector3 position)
        {
            // Создаём новый узел
            Node node = new Node(position);

            // Находим соседей по всем направлениям (по вертикали, горизонтали и диагоналям), а также в позициях подними
            FindNeighbors(node, Vector3.forward);
            FindNeighbors(node, Vector3.forward + Vector3.right);
            FindNeighbors(node, Vector3.right);
            FindNeighbors(node, Vector3.right + Vector3.back);
            FindNeighbors(node, Vector3.back);
            FindNeighbors(node, Vector3.back + Vector3.left);
            FindNeighbors(node, Vector3.left);
            FindNeighbors(node, Vector3.left + Vector3.forward);

            // Добавляем данный узел в сырой лист
            AddNodeToList(ref rawNodeList, node);
        }

        /// <summary>
        /// Метод для поиска соседних узлов
        /// </summary>
        /// <param name="node">проверяемый узел</param>
        /// <param name="direction">направление проверки</param>
        private static void FindNeighbors(Node node, Vector3 direction)
        {
            // Для хранения данных об объекте с которым произошло столкновение
            RaycastHit hit;

            // Берём соседнюю позицию
            Vector3 sidePosition = node.position + direction * NodeSetting.wolkDistance;

            // Если не сталкивается со стенкой
            if (!Physics.Raycast(node.position, direction, out hit, NodeSetting.wolkDistance, NodeSetting.layerMask))
            {
                // Проверяем на присутствие узла в сыром листе с определённой стороны
                Node neighborNode = rawNodeList.Find(x => x.position == sidePosition);

                // Если соседа нет ищем в чистом листе
                if (neighborNode == null)
                {
                    neighborNode = nodeList.Find(x => x.position == sidePosition);
                }

                // Если всё же найден
                if (neighborNode != null)
                {
                    // Записываем в соседей
                    ConnectSideNodes(node, neighborNode);
                }
                // Если сбоку не найден, то
                else
                {
                    // ищем сбоку - ниже и проделываем тоже самое
                    ConnectSideDownNodes(sidePosition, node, neighborNode);
                }
            }
            else
            {
                // Если узел присутствует в чистом листе
                if (nodeList.Exists(x => x.position == node.position))
                {
                    // Удаляем из чистого листа
                    nodeList.Remove(node);
                    // Добавляем в сырой лист
                    AddNodeToList(ref rawNodeList, node);
                }

                // Если сквозь стенку есть узел стоящий в nodeList
                Node neighborNode = nodeList.Find(x => x.position == sidePosition);

                // Удаляем и его
                if (neighborNode != null)
                {
                    // Из чистого листа
                    nodeList.Remove(neighborNode);
                    // Добавляем в сырой лист
                    AddNodeToList(ref rawNodeList, neighborNode);
                }
            }
        }

        /// <summary>
        /// Проверка двух узлов сбоку вниз и их соединение
        /// </summary>
        /// <param name="sidePosition">боковая позиция</param>
        /// <param name="node1">первый узел</param>
        /// <param name="node2">второй узел</param>
        private static void ConnectSideDownNodes(Vector3 sidePosition, Node node1, Node node2)
        {
            // Для хранения данных об объекте с которым произошло столкновение
            RaycastHit hit;

            // Если произошло столкновение с плоскостью от боковой позиции в низ на дистанцию прыжка
            if (Physics.Raycast(sidePosition, Vector3.down, out hit, NodeSetting.jumpDistance, NodeSetting.layerMask))
            {
                // Боковой позиции по Y задаём позицию столкновения по Y
                sidePosition.y = hit.transform.position.y;
                // Ищем узел в сыром листе
                node2 = rawNodeList.Find(x => x.position == sidePosition);

                // Если не нашли
                if (node2 == null)
                {
                    // Ищем в чистом листе
                    node2 = nodeList.Find(x => x.position == sidePosition);
                }

                // Если узел всё таки был найден
                if (node2 != null)
                {
                    // Каждому узлу указываем что второй является соседом
                    ConnectSideNodes(node1, node2);
                }
            }
        }

        /// <summary>
        /// Проверка двух узлов и их соединение
        /// </summary>
        /// <param name="node1">первый узел</param>
        /// <param name="node2">второй узел</param>
        private static void ConnectSideNodes(Node node1, Node node2)
        {
            // Записываем в соседей первого узла второй узел
            node1.ConnectNode(node2);

            // Если у первого узла присутствуют все стороны, но он всё ещё отсутствует в чистом листе
            if (node1.IsClean && !nodeList.Exists(n => n == node1))
            {
                // Удаляем его из сырого листа
                rawNodeList.Remove(node1);
                // Добавляем первый узел в чистый лист
                AddNodeToList(ref nodeList, node1);
            }

            // Записываем в соседей второго узла первый узел
            node2.ConnectNode(node1);

            // Если у второго узла присутствуют все стороны, но он всё ещё отсутствует в чистом листе
            if (node2.IsClean && !nodeList.Exists(n => n == node2))
            {
                // Удаляем второй узел из сырого листа
                rawNodeList.Remove(node2);
                // Добавляем второй узел в чистый лист
                AddNodeToList(ref nodeList, node2);
            }
        }

        /// <summary>
        /// Добавление узла в лист
        /// </summary>
        /// <param name="list">лист</param>
        /// <param name="node">узел</param>
        private static void AddNodeToList(ref List<Node> list, Node node)
        {
            // Если проверяемый лист слишком большой, больше 1000 элементов
            if (list.Count > 1000)
            {
                // Удаляем дальний узел
                list.Remove(list.Find(x => Vector3.Distance(x.position, node.position) > 1));
            }

            // Добавляем новый узел в оперируемый лист
            list.Add(node);
        }
    }
}
