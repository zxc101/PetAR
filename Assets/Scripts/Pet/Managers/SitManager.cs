using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class SitManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;
        
        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public SitManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Короутина которая воспраизводится с промежутком во времени,
        /// в данном случае это одна секунда
        /// 
        /// Отвечет за бездействие
        /// </summary>
        /// <returns>Ждёт секунду</returns>
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Sit");
        }
    }
}
