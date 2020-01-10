using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

public class PlaneСorrector
{
    private Transform point1;
    private Transform point2;
    private Transform point3;
    
    public Transform Base { get; set; }

    public TrackableHit Hit    { private get; set; }
    public Transform    Point1 { set { point1 = value; SetPoint(point1); } }
    public Transform    Point2 { set { point2 = value; SetPoint(point2); } }
    public Transform    Point3 { set { point3 = value; SetPoint(point3); } }

    public void AddPlane(Transform transform)
    {
        Position(transform);
        Rotation(transform);
        Scale(transform);
        HidePoints();
    }

    private void Position(Transform plane)
    {
        // Находим центр по формуле деления отрезка пополам
        plane.position = Helper.VectorCreater((point1.position.x + point3.position.x) / 2,
                                                  (point1.position.y + point3.position.y) / 2,
                                                  (point1.position.z + point3.position.z) / 2);
    }

    private void Rotation(Transform plane)
    {
        // Запоминаем текущий поворот первой точки
        var rotation1 = point1.rotation;
        // Поворачиваем первую точку в сторону третьей
        point1.LookAt(point2);
        // Созданной поверхности задаётся поворот, такой же что и у первой точки
        plane.rotation = point1.rotation;
        // Возвращаем точку в исходное положение
        point1.rotation = rotation1;

        // Если она вертикальна
        if ((Hit.Trackable as DetectedPlane).PlaneType == DetectedPlaneType.Vertical)
        {
            // Поворачиваем данную проскость по оси z на 90 градусов
            plane.Rotate(plane.rotation.x, plane.rotation.y, plane.rotation.z + 90);
        }
    }

    private void Scale(Transform plane)
    {
        plane.localScale = Helper.VectorCreater(Vector3.Distance(point3.position, point2.position),
                                                    plane.localScale.y,
                                                    Vector3.Distance(point1.position, point2.position));
    }

    // Метод для установки точки
    private void SetPoint(Transform point)
    {
        // Устанавливаем позицию точки
        point.position = Hit.Pose.position;
        // её поворот
        point.rotation = Hit.Pose.rotation;
        // и активируем
        point.gameObject.SetActive(true);
    }

    // Скрываем все точки
    public void HidePoints()
    {
        // Скрываем первую точку
        point1.gameObject.SetActive(false);
        // Скрываем вторую точку
        point2.gameObject.SetActive(false);
        // Скрываем третью точку
        point3.gameObject.SetActive(false);
    }
}
