using System.Collections;
using UnityEngine;
using Helpers;

namespace Pets
{
    public class MoveManager
    {
        /// <summary>
        /// Класс со значениями агента
        /// </summary>
        private Pet pet;

        /// <summary>
        /// Конструктор для инициализации агента
        /// </summary>
        /// <param name="_pet">Агент</param>
        public MoveManager(Pet _pet)
        {
            // инициализация агента
            pet = _pet;
        }

        /// <summary>
        /// Короутина которая воспраизводится с промежутком во времени,
        /// в данном случае в зависимости от того, сколько физических
        /// кадров в секунду установлено в настройках времени
        /// 
        /// Отвечет за передвижение агента
        /// </summary>
        /// <returns>Ждёт определённое время</returns>
        public IEnumerator Start()
        {
            // Если пути нету, агент ждёт
            if (pet.Path == null) yield return new WaitForFixedUpdate();

            // Задаётся позиция следующего узла по оси y
            float hight = pet.NextPosition.y;
            // Если позиция агента меньше чем позиция следующего узла по оси y плюс/минус 0.2
            if (pet.Transform.position.y < hight + 0.1f && pet.Transform.position.y < hight - 0.1f)
            {
                // Агент прыгает вверх
                pet.JumpManager.Jump(DirectionY.Up);
            }
            // иначе, если позиция агента больше чем позиция следующего узла по оси y плюс/минус 0.2
            else if (pet.Transform.position.y > hight + 0.1f && pet.Transform.position.y > hight - 0.1f)
            {
                // Агент прыгает вниз
                pet.JumpManager.Jump(DirectionY.Down);
            }
            // иначе
            else
            {
                // Агент перемещается
                Move(hight);
            }
            // Ждём
            yield return new WaitForFixedUpdate();
        }

        /// <summary>
        /// Метод для остановки агента
        /// </summary>
        public void Stop()
        {
            pet.SpeedMove = 0;
            pet.SpeedRotate = 0;
        }

        /// <summary>
        /// Метод для перемещения агента
        /// </summary>
        /// <param name="hight">значение для стабилизации по оси y</param>
        private void Move(float hight)
        {
            // Стабилизируем по оси y
            StabilizationY(hight);
            // Корректируем скорость
            CorrectMoveSpeed();
        }

        /// <summary>
        /// Метод для определения, есть ли впереди агента препятствие
        /// </summary>
        /// <param name="hit">Инициализирующийся объект при столкновении</param>
        /// <param name="direction">Направление</param>
        /// <returns>Есть ли впереди агента препятствие</returns>
        private bool IsDirectionWall(out RaycastHit hit, Vector3 direction)
        {
            // Определяем, есть ли впереди агента препятствие
            return Physics.Raycast(pet.Eye.position, direction, out hit, 1, NodeSetting.layerMask);
        }

        /// <summary>
        /// Метод для стабилизации агента после прыжка
        /// </summary>
        /// <param name="hight">Позиция по оси y</param>
        private void StabilizationY(float hight)
        {
            // Выключаем анимацию прыжка вверх
            pet.AnimManager.Jump(DirectionY.Up, false);
            // Выключаем анимацию прыжка вниз
            pet.AnimManager.Jump(DirectionY.Down, false);

            // Используем данную позицию для агента
            pet.transform.position = Helper.VectorCreater(pet.transform.position.x,
                                                          hight,
                                                          pet.transform.position.z);
        }

        /// <summary>
        /// Корректируем скорость агента
        /// </summary>
        private void CorrectMoveSpeed()
        {
            // Поворачиваем объект
            // Если абсолютное значение агента по x и по z равняется 0.7 идёт по диагонали
            // или абсолютное значение агента по x и по z равняется 1 идёт по горизонтали или вертикали
            if ((Mathf.Abs(pet.transform.forward.x) == 0.7f && Mathf.Abs(pet.transform.forward.z) == 0.7f) ||
                (Mathf.Abs(pet.transform.forward.x) == 1 && Mathf.Abs(pet.transform.forward.z) == 1))
            {
                // Скорость поворота равна нулю
                pet.SpeedRotate = 0;
            }
            // Иначе
            else
            {
                // Скорость поворота агента равна линейной интерполяции скорости поворота агента и угла поворота до следующего узла, по времени заданному константой в секундах
                pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, MathHelper.Angle(pet.Transform, pet.NextPosition), Time.fixedDeltaTime * pet.TIME_ROTATE);
            }

            // Move speed

            // Чем больше угол между направлением агента и позицией следующего узла пути, тем меньше скорость при слишком большом угле скорость нулевая
            // Чем ближе объект к его Need, тем меньше скорость при очень близком сближении скорость равна 0

            // Чем дальше объект тем больше скорость
            // Чем больше угол, тем меньше скорость

            pet.SpeedMove = Mathf.Clamp((Mathf.Clamp(MathHelper.DistanceXZ(pet.Transform.position, pet.Goals.First.position), 0, 1) - Mathf.Abs(MathHelper.Angle(pet.Transform, pet.NextPosition) / 180)) * 2, pet.MinSpeed, 2);
        }
    }
}
