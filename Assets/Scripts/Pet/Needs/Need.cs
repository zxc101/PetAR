using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Need
{
    // Префаб потребности
    public Transform prefab;
    // Картинка
    public Sprite icon;
    // Имя потребности
    public string name;
    // Нужда в потребности
    public float value;
    // Максимальное значение потребности
    public float maxValue;
    // Шаг, с которым значение уменьшается, состояние ухудшается
    public float stepConsumption;
    // Значение отвечающее за критичную нужду в потребности
    public float criticalValue;
    // Промежуток времени, при котором, с каждым шагом значение увеличивается, состояние улучшается
    public float processingTime;
}
