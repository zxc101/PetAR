using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

namespace Pets
{
    public class NeedsManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;

        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public NeedsManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Короутина повторяющаяся раз в промежуток времени, при котором,
        /// с каждым шагом значение увеличивается, состояние улучшается
        /// 
        /// Данная короутина увеличивает значение состояния агента в заданной потребности
        /// </summary>
        /// <returns>Ждёт определённый промежуток времени, при котором,
        /// с каждым шагом значение увеличивается, состояние улучшается</returns>
        public IEnumerator Processing()
        {
            // Если значение состояния агента по данной потребности меньше её максимального значения
            while (pet.Need.value < pet.Need.maxValue)
            {
                // Ждёт определённый промежуток времени, при котором,
                // с каждым шагом значение увеличивается, состояние улучшается
                yield return new WaitForSeconds(pet.Need.processingTime);
                // Значение состояния увеличивается
                pet.Need.value++;
            }
            // Анимация прекращается
            pet.AnimManager.Need(pet.Need.name, false);
        }

        /// <summary>
        /// Короутина повторяющаяся раз в заданную частоту потребления потребностей в секунду
        /// 
        /// Данная короутина уменьшает значение состояния агента в заданной потребности
        /// </summary>
        /// <returns>Частота потребления потребностей в секунду</returns>
        public IEnumerator Consumptions()
        {
            // Зацикливаем короутину
            while (true)
            {
                // Через частоту потребления потребностей в секунду
                yield return new WaitForSeconds(pet.TimeConsumptionNeeds);
                // Если мы сейчас в игре
                if (Helper.isPlay)
                {
                    // Проходим по всем потребностям
                    for (int i = 0; i < pet.Needs.Length; i++)
                    {
                        // Если он сейчас не восстанавливает данную потребность и значение больше нуля
                        if (!pet.AnimManager.IsNeed(pet.Needs[i].name) &&
                            pet.Needs[i].value > 0)
                        {
                            // Происходит уменьшение значения на заданный шаг, с которым значение уменьшается
                            pet.Needs[i].value -= pet.Needs[i].stepConsumption;
                        }
                    }
                }
                
            }
        }
    }
}
