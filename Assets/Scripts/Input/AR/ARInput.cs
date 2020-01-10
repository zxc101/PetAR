using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Helpers;
using Pets;
using Nodes;

public class ARInput : MonoBehaviour
{
    // Камера
    [SerializeField] private Camera ARCamera;

    // Кнопка для перехода в создания карты
    [SerializeField] private RectTransform createMap_BTN;
    // Кнопка для перехода в игру
    [SerializeField] private RectTransform play_BTN;
    // Лист потребностей
    [SerializeField] private RectTransform needsScroll;
    // Статистика потребностей
    [SerializeField] private RectTransform stats;
    // Кнопка с триггером создания или удаления плоскостей
    [SerializeField] private RectTransform createMapTrigger_BTN;

    /// <summary>
    /// Класс со значениями матрицы
    /// </summary>
    [SerializeField] private NodeMatrix matrix;

    // Точки для построения плоскостей для генерирования карты
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;

    // Префаб который будем размножать для создания плоскостей из которых состоит карта
    [SerializeField] private Transform plane;
    // Хранилище плоскостей
    [SerializeField] private Transform planeBase;

    // Агент
    [SerializeField] private Pet pet;

    // Хранилище в котором хранятся все цели агента
    [SerializeField] private Transform goalsBase;

    // Предназначен для выявления какого рода был клик по экрану
    private Touch touch;
    // Для хранения данных об объекте с которым произошло столкновение
    private TrackableHit hit;
    // Фильтр касаний
    private TrackableHitFlags filter;
    // Булевый показатель - идёт ли сейчас создание или удаление плоскостей
    private bool isCreateMap;
    // Корректор созданной плоскости
    private PlaneСorrector planeСorrector = new PlaneСorrector();

    // В начале создания сцены
    private void Start()
    {
        // Задаём значения фильтра
        filter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;
        planeСorrector.Base = planeBase;
        CreateMap();
        CreateMapTrigger();
    }

    // Непрекращающийся метод который воспроизводится каждый кадр
    private void Update()
    {
        // Проверяем было ли нажатие
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Проверяем было ли это нажатие на UI (к примеру на кнопку)
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        // Находим позицию нажатия и проверяем его с помощью фильтра
        if (Frame.Raycast(touch.position.x, touch.position.y, filter, out hit))
        {
            // Проверяем не было ли это нажатие на плоскость созданную ARCore и повёрнута ли эта плоскость не в сторону камеры
            if (!((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(ARCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0))
            {
                // Если мы нажали на плоскость
                if (hit.Trackable is DetectedPlane)
                {
                    // Если в данный момент прои сходит игра
                    if (Helper.isPlay)
                    {
                        // Если игрок активирован
                        if (pet.gameObject.activeSelf)
                        {
                            if (Helper.ChoicesNeeds.tag == "Point")
                            {
                                // Генерирует дополнительную точку назначения
                                CreatePoint();
                            }
                            else
                            {
                                // Активирует удавлетворитель потребности
                                ActivateNeed();
                            }
                        }
                        // Иначе
                        else
                        {
                            // Активирует агента
                            ActivatePet();
                        }
                    }
                    // Иначе
                    else
                    {
                        if (isCreateMap)
                        {
                            planeСorrector.Hit = hit;
                            // Если все точки не активны
                            if (!point1.gameObject.activeSelf && !point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                            {
                                // Ставим первую точку
                                planeСorrector.Point1 = point1;
                            }
                            // Если активна только первая
                            else if (point1.gameObject.activeSelf && !point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                            {
                                // Ставим вторую точку
                                planeСorrector.Point2 = point2;
                            }
                            // Если активны только первые две точки
                            else if (point1.gameObject.activeSelf && point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                            {
                                // Ставим третью точку
                                planeСorrector.Point3 = point3;
                                // Создаём новую поверхность
                                planeСorrector.AddPlane(Instantiate(plane, planeBase).transform);
                            }
                        }
                        else
                        {
                            DestroyPlane();
                        }
                    }
                }
            }
        }
    }

    // Метод срабатывающий при нажатии кнопки CreateMap
    public void CreateMap()
    {
        // Сообщаем, что мы приостанавливаем игру
        Helper.isPlay = false;
        // Метод срабатывающий как тригер для создания карты и продолжения игры
        CreateMap_Play_Trigger(Helper.isPlay);
    }

    // Метод срабатывающий при нажатии кнопки Play
    public void Play()
    {
        // Сообщаем, что мы начинаем или продолжаем игру
        Helper.isPlay = true;
        // Метод срабатывающий как тригер для создания карты и продолжения игры
        CreateMap_Play_Trigger(Helper.isPlay);
        matrix.CreateManager.DestroyAllNodes();
        matrix.CreateManager.Create();
        planeСorrector.HidePoints();
    }

    // Метод срабатывающий как тригер для создания карты и продолжения игры
    private void CreateMap_Play_Trigger(bool isPlay)
    {
        // Скрываем или показываем кнопку Play
        play_BTN.gameObject.SetActive(!isPlay);
        // Скрываем или показываем кнопку CreateMap
        createMap_BTN.gameObject.SetActive(isPlay);
        // Скрываем или показываем лист потребностей
        needsScroll.gameObject.SetActive(isPlay);
        // Скрываем или показываем статистику
        stats.gameObject.SetActive(isPlay);
        createMapTrigger_BTN.gameObject.SetActive(!isPlay);
        foreach(Transform child in planeBase)
        {
            child.GetComponent<Plane>().TriggerAlphaPlanes();
        }
    }

    public void CreateMapTrigger()
    {
        isCreateMap = !isCreateMap;
        if (isCreateMap)
        {
            createMapTrigger_BTN.GetComponent<Image>().color = Helper.VectorCreater(0, 1, 0, 1);
        }
        else
        {
            createMapTrigger_BTN.GetComponent<Image>().color = Helper.VectorCreater(1, 0, 0, 1);
        }
    }

    private void CreatePoint()
    {
        // К его целям добавляется новая цель
        pet.Goals.AddFirst(Instantiate(pet.Point, goalsBase));
        // Задаются её координаты
        pet.Goals.First.position = hit.Pose.position;
        // И поворот
        pet.Goals.First.Rotate(0, 180, 0, Space.Self);
    }

    private void ActivateNeed()
    {
        // Задаётся данному объекту координаты
        Helper.ChoicesNeeds.position = hit.Pose.position;
        // Поворот
        Helper.ChoicesNeeds.Rotate(0, 180, 0, Space.Self);
        // Он активируется
        Helper.ChoicesNeeds.gameObject.SetActive(true);
    }

    private void ActivatePet()
    {
        // Питомцу задаётся позиция
        pet.transform.position = hit.Pose.position;
        // Поворот
        pet.transform.Rotate(0, 180, 0, Space.Self);
        // Он активируется
        pet.transform.gameObject.SetActive(true);
    }

    private void DestroyPlane()
    {
        Ray hitInfo = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
        RaycastHit hit;
        if (Physics.Raycast(hitInfo, out hit, NodeSetting.layerMask) &&
            hit.transform.GetComponent<Plane>())
        {
            hit.transform.GetComponent<Plane>().DestroyObj();
        }
    }
}
