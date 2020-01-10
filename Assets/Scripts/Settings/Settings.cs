using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс позволяет соединять записывающие и читающие структуры
/// </summary>
public class Settings : MonoBehaviour
{
    /// <summary>
    /// Значения узла
    /// </summary>
    [SerializeField] private NodeValues node;

    /// <summary>
    /// Метод который воспроизводится в начале игры
    /// </summary>
    private void Start()
    {
        // Метод который инициализирует узловые настроеки
        InitNodeSettings();
    }

    /// <summary>
    /// Метод который инициализирует узловые настроеки
    /// </summary>
    private void InitNodeSettings()
    {
        // Инициализируем базу для хранения узлов
        NodeSetting.boxesBase = node.boxesBase;
        // Инициализируем положение персонажа
        NodeSetting.pet = node.pet;

        // Инициализируем цвет сырого узла
        NodeSetting.rawNodeColor = node.rawNodeColor;
        // Инициализируем цвет готового узла
        NodeSetting.nodeColor = node.nodeColor;

        // Инициализируем дистанцию между узлами
        NodeSetting.wolkDistance = node.wolkDistance;
        // Инициализируем высоту прыжка агента
        NodeSetting.jumpDistance = node.jumpDistance;

        // Инициализируем количество узлов по граням матрицы
        NodeSetting.count = node.count;

        // Инициализируем слой поверхоностей карты генерируемой игроком
        NodeSetting.layerMask = node.layerMask;
    }
}
