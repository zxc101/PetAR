using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class AnimManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;

        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public AnimManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Анимация прыжка
        /// </summary>
        /// <param name="direction">Направление</param>
        /// <param name="isJump">Начало или конец прыжка</param>
        public void Jump(DirectionY direction, bool isJump)
        {
            // Смотрим какое у нас направление прыжка
            switch (direction)
            {
                // Если это направление вверх
                case DirectionY.Up:
                    // Запускаем или останавливаем анимацию прыжка вверх
                    pet.Animator.SetBool("IsJumpUp", isJump);
                    break;
                // Если это направление вниз
                case DirectionY.Down:
                    // Запускаем или останавливаем анимацию прыжка вниз
                    pet.Animator.SetBool("IsJumpDown", isJump);
                    break;
            }
        }

        /// <summary>
        /// Запускаем анимацию действия
        /// </summary>
        /// <param name="name">название действия</param>
        /// <param name="isNeed">запуск или остановка</param>
        public void Need(string name, bool isNeed)
        {
            // Запускаем анимацию действия
            pet.Animator.SetBool(name, isNeed);
        }

        /// <summary>
        /// Нужно ли в данный момент воспроизводить анимацию действия
        /// </summary>
        /// <param name="name">Название действия</param>
        /// <returns>Нужно ли в данный момент воспроизводить анимацию действия</returns>
        public bool IsNeed(string name)
        {
            return pet.Animator.GetBool(name);
        }
    }
}
