using Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class MainManager : MonoBehaviour
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        [SerializeField] private Pet pet;

        /// <summary>
        /// Метод который воспроизводится в начале игры
        /// </summary>
        private void Start()
        {
            // Запуск основной короутины для агента
            StartCoroutine(CFixedUpdate());
        }

        /// <summary>
        /// Короутина которая воспраизводится с промежутком во времени,
        /// в данном случае в зависимости от того, сколько физических
        /// кадров в секунду установлено в настройках времени
        /// 
        /// Данная короутина отвечает за действия агента
        /// </summary>
        /// <returns>Ждёт определённое время</returns>
        private IEnumerator CFixedUpdate()
        {
            // Зацикливаем данный метод
            while (true)
            {
                // Если агент активирован
                if (pet.isActiveAndEnabled)
                {
                    // Происходит поиск цели
                    pet.GoalManager.ChangeGoal();
                    // Если цель есть
                    if (!pet.Goals.IsEmpty)
                    {
                        // Находим кратчайший путь к нему
                        pet.Path = Pathfinder.FindPath(pet.Transform.position, pet.Goals.First.position);
                        // Если путь равен нулю или его просто нет
                        if (pet.Path == null || pet.Path.Count == 0)
                        {
                            // Останавливаем агента
                            pet.MoveManager.Stop();
                            // Ждём
                            yield return new WaitForFixedUpdate();
                        }
                        // Иначе
                        else
                        {
                            // Запускаем и ждём ответа от короутины отвечающей за передвижение
                            yield return StartCoroutine(pet.MoveManager.Start());
                        }
                    }
                    // Иначе
                    else
                    {
                        // Останавливаем агента
                        pet.MoveManager.Stop();
                        // Ждём
                        yield return new WaitForFixedUpdate();
                    }
                }
                else
                {
                    // Ждём
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        /// <summary>
        /// Отрисовывает только на сцене игрового движка
        /// </summary>
        private void OnDrawGizmos()
        {
            // Если путь инициализирован
            if (pet.Path != null)
            {
                // Берём красный цвет
                Gizmos.color = Color.red;
                // Переносим путь из стека в массив
                Vector3[] gizmosPath = pet.Path.ToArray();
                // Проходит по всем узлам пути
                for (int i = 0; i < gizmosPath.Length; i++)
                {
                    // Отрисовываем на данном месте кубик с размером 0.05 по x, y и z
                    Gizmos.DrawCube(gizmosPath[i], Vector3.one * 0.05f);
                }
            }
        }
    }
}
