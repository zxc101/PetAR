using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class NodeMatrix : MonoBehaviour
    {
        /// <summary>
        /// Префаб узла
        /// </summary>
        public Transform nodeBox;
        /// <summary>
        /// Хранилище узлов на сцене
        /// </summary>
        public Transform boxesBase;
        /// <summary>
        /// Агент
        /// </summary>
        public Transform pet;
        /// <summary>
        /// Дистанция между узлами по осям X и Z
        /// </summary>
        public float wolkDistance;
        /// <summary>
        /// Дистанция прыжка
        /// </summary>
        public float jumpDistance;
        /// <summary>
        /// Величина граней матрицы в количестве узлов
        /// </summary>
        public int count;
        /// <summary>
        /// Маска которая позволяет различать поверхности карты
        /// </summary>
        public LayerMask layerMask;

        /// <summary>
        /// Свойство префаба узла
        /// </summary>
        public Transform NodeBox => nodeBox;
        /// <summary>
        /// Свойство хранилища узлов на сцене
        /// </summary>
        public Transform BoxesBase => boxesBase;
        /// <summary>
        /// Свойство агента
        /// </summary>
        public Transform Pet => pet;
        /// <summary>
        /// Свойство дистанции между узлами по осям X и Z
        /// </summary>
        public float WolkDistance => wolkDistance;
        /// <summary>
        /// Свойство дистанции прыжка
        /// </summary>
        public float JumpDistance => jumpDistance;
        /// <summary>
        /// Свойство маски которая позволяет различать поверхности карты
        /// </summary>
        public LayerMask LayerMask => layerMask;

        /// <summary>
        /// Свойство величины граней матрицы в количестве узлов
        /// </summary>
        public int Count { get => count; set => count = value; }

        /// <summary>
        /// Свойство префаба объекта на котором весит этот класс
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Свойство объекта отвечающего за перемещение
        /// </summary>
        public MoveManager MoveManager { get; private set; }
        /// <summary>
        /// Свойство объекта отвечающего за генерирование и поиск узлов
        /// </summary>
        public CreateManager CreateManager { get; private set; }

        /// <summary>
        /// Инициализация объектов отвечающих за какое либо действие
        /// </summary>
        private void InitManagers()
        {
            // Инициализация объекта отвечающего за перемещение
            MoveManager = new MoveManager(this);
            // Инициализация объекта отвечающего за генерирование и поиск узлов
            CreateManager = new CreateManager(this);
        }

        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        private void InitComponents()
        {
            // Инициализация объекта отвечающего за префаб
            Transform = transform;
        }

        /// <summary>
        /// Метод который воспроизводится в начале игры
        /// </summary>
        private void Start()
        {
            // Инициализация компонентов
            InitComponents();
            // Инициализация объектов отвечающих за какое либо действие
            InitManagers();
        }
    }
}
