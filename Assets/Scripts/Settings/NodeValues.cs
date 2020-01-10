using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данная структура позволяет записывать данные об узлах
/// </summary>
[System.Serializable]
public struct NodeValues
{
    // База для хранения узлов
    public Transform boxesBase;
    // Положение персонажа
    public Transform pet;

    // Цвет сырого узла
    public Color rawNodeColor;
    // Цвет готового узла
    public Color nodeColor;

    // Дистанция между узлами
    public float wolkDistance;
    // Высота прыжка агента
    public float jumpDistance;

    // Количество узлов по граням матрицы
    public int count;

    // Слой поверхоностей карты генерируемой игроком
    public LayerMask layerMask;
}
