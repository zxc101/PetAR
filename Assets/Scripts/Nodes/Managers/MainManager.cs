using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class MainManager : MonoBehaviour
    {
        /// <summary>
        /// Класс со значениями матрицы
        /// </summary>
        [SerializeField] private NodeMatrix matrix;
        /// <summary>
        /// Надо ли рисовать узлы на карте сцены движка
        /// </summary>
        [SerializeField] private bool isDrawNodes;

        /// <summary>
        /// Метод который воспроизводится в начале игры
        /// </summary>
        private void Start()
        {
            // Задаём начальную позицию матрицы
            matrix.Transform.position = matrix.Pet.position + Vector3.up * 5;
            // Создаём ближайшие к агенту узлы
            matrix.CreateManager.Create();
            // Начинаем корутину отвечающую за действия воспроизводимые матрицей
            StartCoroutine(MainDoes());
        }

        /// <summary>
        /// Короутина которая воспраизводится с промежутком во времени,
        /// в данном случае в зависимости от того, сколько физических
        /// кадров в секунду установлено в настройках времени
        /// 
        /// Данный метод отвечает за действия воспроизводимые матрицей
        /// </summary>
        /// <returns>Ждёт определённое время</returns>
        private IEnumerator MainDoes()
        {
            // Зацикливаем данный метод
            while (true)
            {
                // Если матрица активна
                if (matrix.isActiveAndEnabled)
                {
                    // Передвигаем её
                    matrix.MoveManager.Move();
                }
                // Ждём
                yield return new WaitForFixedUpdate();
            }
        }

        public void RebuildNodes()
        {
            matrix.CreateManager.DestroyAllNodes();
            matrix.CreateManager.Create();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RebuildNodes();
            }
        }

        /// <summary>
        /// Отрисовывает только на сцене игрового движка
        /// </summary>
        private void OnDrawGizmos()
        {
            // Если надо отрисовывать
            if (isDrawNodes)
            {
                // Если чистый лист с узлами не пуст
                if (NodeList.nodeList != null)
                {
                    // Берём жёлтый цвет
                    Gizmos.color = Color.yellow;
                    // Проходим по каждому узлу
                    for (int i = 0; i < NodeList.nodeList.Count; i++)
                    {
                        // Отрисовываем на данном месте кубик с размером 0.05 по x, y и z
                        Gizmos.DrawCube(NodeList.nodeList[i].position, Vector3.one * 0.05f);
                    }
                }

                // Если сырой лист с узлами не пуст
                if (NodeList.rawNodeList != null)
                {
                    // Берём зелёный цвет
                    Gizmos.color = Color.green;
                    // Проходим по каждому узлу
                    for (int i = 0; i < NodeList.rawNodeList.Count; i++)
                    {
                        // Отрисовываем на данном месте кубик с размером 0.05 по x, y и z
                        Gizmos.DrawCube(NodeList.rawNodeList[i].position, Vector3.one * 0.05f);
                    }
                }
            }
        }
    }
}
