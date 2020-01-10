using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данная структура позволяет брать данные об узлах
/// </summary>
public struct NodeSetting
{
    // База для хранения узлов
    public static Transform boxesBase;
    // Положение персонажа
    public static Transform pet;

    // Цвет сырого узла
    public static Color rawNodeColor;
    // Цвет готового узла
    public static Color nodeColor;

    // Дистанция между узлами
    public static float wolkDistance;
    // Высота прыжка агента
    public static float jumpDistance;

    // Количество узлов по граням матрицы
    public static int count;

    // Слой поверхоностей карты генерируемой игроком
    public static LayerMask layerMask;
}
