using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nodes;

namespace Pets
{
    public class GoalManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;

        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public GoalManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Метод отвечающий за поиск цели
        /// </summary>
        public void ChangeGoal()
        {
            // Если не было найдено объектов которые требуются или потребности попросту нет
            if (FindMinValInNeed() == null)
            {
                // Выходим из метода
                return;
            }
            else
            {
                // Если у объекта для удовлетворения потребностей c минимальным значением, значение меньше критического значения
                if (FindMinValInNeed().value < FindMinValInNeed().criticalValue)
                {
                    // Если текущая нужда агента не равна значению активного объекта для удовлетворения потребностей с минимальным значением и
                    // если агент сейчас не восстанавливает данную потребность
                    if (pet.Need != FindMinValInNeed() && !pet.AnimManager.IsNeed(pet.Need.name))
                    {
                        // Задаёт текущей потребности агента как объект для удовлетворения потребностей c минимальным значением
                        pet.Need = FindMinValInNeed();
                        // Обновляет список целей ставя текущую потребность как основную цель
                        ChangeMainGoal(pet.Need.prefab);
                    }
                }
            }
        }

        /// <summary>
        /// Находим активный объект для удовлетворения потребностей с минимальным значением
        /// </summary>
        /// <returns>Объект для удовлетворения потребностей c минимальным значением</returns>
        private Need FindMinValInNeed()
        {
            // Создаём пустой результат
            Need res = null;
            // Создаём и инициализируем активные объекты для удовлетворения потребностей
            List<Need> activeNeeds = new List<Need>();
            // Проходим по всем потребностям
            for(int i = 0; i < pet.Needs.Length; i++)
            {
                // Если объект для удовлетворения потребности активен
                if (pet.Needs[i].prefab.gameObject.activeSelf)
                {
                    // добавляем его в лист
                    activeNeeds.Add(pet.Needs[i]);
                }
            }
            // Проходим по всем активным объектам для удовлетворения потребностей
            for (int i = 0; i < activeNeeds.Count; i++)
            {
                // Если это первая потребность
                if(i == 0)
                {
                    // Результат инициализируем как первый активный объект для удовлетворения потребностей
                    res = activeNeeds[0];
                }
                // Иначе
                else
                {
                    // Если значение следующей потребности ниже текущей
                    if(activeNeeds[i].value < res.value)
                    {
                        // Выбераем её как текущую
                        res = activeNeeds[i];
                    }
                }
            }
            // Выводим получившуюся потребность
            return res;
        }

        /// <summary>
        /// Удаляет все цели
        /// </summary>
        public void ClearAllGoals()
        {
            // Если есть цель 
            if (pet.Goals != null)
            {
                // Удаляет все цели из коллекции
                RemoveGoals(pet.Goals.Count);
            }
        }

        /// <summary>
        /// Удаляет все вспомогательные цели
        /// </summary>
        public void RemoveAllHalperGoals()
        {
            // Удаляет все цели кроме последней
            RemoveGoals(pet.Goals.Count - 1);
        }

        /// <summary>
        /// Обновляет текущую главную цель
        /// </summary>
        /// <param name="newPoint">Новая цель</param>
        public void ChangeMainGoal(Transform newPoint)
        {
            // Если цель есть
            if (pet.Goals != null && !pet.Goals.IsEmpty)
                // Удаляет основную цель
                pet.RemoveLastGoal();
            // Добавляет новую цель
            pet.AddLastGoal(newPoint);
        }

        /// <summary>
        /// Удаляет текущую основную цель
        /// </summary>
        public void RemoveMainGoal()
        {
            // Если цель есть
            if (pet.Goals != null && !pet.Goals.IsEmpty)
            {
                // Если позиция основной цели занимает дествительно основная цель
                if (pet.Goals.Last.position == pet.Need.prefab.position)
                {
                    // Удаляет эту цель
                    pet.RemoveLastGoal();
                }
            }
        }

        /// <summary>
        /// Метод отвечающий за выбор новой цели
        /// </summary>
        /// <param name="nodeList">Лист узлов</param>
        /// <returns></returns>
        private Vector3 SelectNewGoal(List<Node> nodeList)
        {
            // Берёт рандомный узел и представленного листа
            return nodeList[Random.Range(0, nodeList.Count)].position;
        }

        /// <summary>
        /// Удаляет все узлы
        /// </summary>
        /// <param name="count">количество удаляемых узлов</param>
        private void RemoveGoals(int count)
        {
            // Проходит по указанному количеству узлов
            for (int i = 0; i < count; i++)
            {
                // Удаляет из начала дека
                pet.RemoveFirstGoal();
            }
        }
    }
}
