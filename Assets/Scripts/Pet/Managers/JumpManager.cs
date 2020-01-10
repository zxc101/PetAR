using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class JumpManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;

        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public JumpManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Метод обрабатывающий прыжок агента
        /// </summary>
        /// <param name="direction">направление прыжка</param>
        public void Jump(DirectionY direction)
        {
            // Чтобы иметь большую живость задаём угол при котором персонаж уже может прыгать
            // Если абсолютное значение этого угола больше четырнадцати градусов
            if (Mathf.Abs(pet.AngleToGoal) > 14)
            {
                // Поворачиваем агента в сторону узла куда надо будет прыгнуть
                RotateBeforeJump();
            }
            // Иначе
            else
            {
                // Агент совершает прыжок в заданном направлении
                pet.AnimManager.Jump(direction, true);
            }
        }

        /// <summary>
        /// Поворот перед прыжком
        /// </summary>
        private void RotateBeforeJump()
        {
            // Скорость поворота агента равна линейной интерполяции скорости поворота агента и угла поворота до следующего узла, по времени заданному константой в секундах
            pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, pet.AngleToGoal, Time.fixedDeltaTime * pet.TIME_ROTATE);
            // Скорость передвижения агента равна линейной интерполяции скорости передвижения агента и нуля, по времени заданному константой в секундах
            pet.SpeedMove = Mathf.Lerp(pet.SpeedMove, 0, Time.fixedDeltaTime * pet.TIME_MOVE);
        }
    }
}
