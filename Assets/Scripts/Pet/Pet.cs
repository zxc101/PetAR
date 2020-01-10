using System.Collections.Generic;
using UnityEngine;

using Helpers;
using Collections;
using System.Collections;

namespace Pets
{
    public class Pet : MonoBehaviour
    {
        // Константное время для ускорения или замедления поворота
        public float TIME_ROTATE => 4;
        // Константное время для ускорения или замедления передвижения
        public float TIME_MOVE => 4;

        // Минимальная скорость
        [SerializeField] private float minSpeed;

        // Частота потребления потребностей в секунду
        [Tooltip("Потребности")]
        [SerializeField] private float timeConsumptionNeeds;

        // Потребности
        [Tooltip("Потребности")]
        public Need[] Needs;

        // Префаб для вспомогательных целей
        [Header("Вспомогательная цель")]
        [SerializeField] private Transform point;
        [SerializeField] private Sprite pointsIcon;

        [Header("Другое")]

        // Префаб для обозначения уровня глаз для нахождения стен передсобой
        [SerializeField] private Transform eye;
        
        // Скорость передвижения
        private float speedMove;
        // Скорость поворота
        private float speedRotate;

        // Стек с путём
        public Stack<Vector3> Path { get; set; }

        // Текущая потребность
        [HideInInspector] public Need Need;

        // Угол до цели, если цели нет, то угол равен нулю
        public float AngleToGoal => Goals.IsEmpty ? 0 : MathHelper.Angle(transform, Goals.First.position);
        
        //Свойства ранее перечисленных значений, их можно взять но нельзя изменить
        public float TimeConsumptionNeeds => timeConsumptionNeeds;
        public float MinSpeed => minSpeed;
        public Transform Point => point;
        public Transform Eye => eye;
        public Sprite PointsIcon => pointsIcon; 

        // Свойство отвечающее за потребности
        public Deque<Transform> Goals { get; set; }
        // Свойства которые при изменении также меняют значения у анимации
        // Свойство скорости передвижения
        public float SpeedMove { get => speedMove; set { speedMove = value; Animator.SetFloat("Speed", value); } }
        // Свойство скорости поворота
        public float SpeedRotate { get => speedRotate; set { speedRotate = value; Animator.SetFloat("Rotate", value); } }

        // Компоненты отвечающие за:
        // расположения на сцене
        public Transform Transform { get; private set; }
        // анимацию
        public Animator Animator { get; private set; }
        // коллайдер - для взаимодейсвия с объектами
        public CapsuleCollider CapsuleCollider { get; private set; }
        // физику объекта
        public Rigidbody Rigidbody { get; private set; }

        // Объекты отвечающие за:
        // анимацию
        public AnimManager AnimManager { get; private set; }
        // цели куда надо идти
        public GoalManager GoalManager { get; private set; }
        // прыжок
        public JumpManager JumpManager { get; private set; }
        // передвижение
        public MoveManager MoveManager { get; private set; }
        // потребности значения которых изменяются в процессе
        public NeedsManager NeedsManager { get; private set; }
        // Свободные действия
        public SitManager SitManager { get; private set; }
        
        /// <summary>
        /// Инициализация значений
        /// </summary>
        private void InitVal()
        {
            Goals = new Deque<Transform>();
        }

        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        private void InitComponents()
        {
            Transform = transform;
            Animator = GetComponent<Animator>();
            CapsuleCollider = GetComponent<CapsuleCollider>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Инициализация объектов отвечающих за какое либо действие
        /// </summary>
        private void InitManagers()
        {
            AnimManager  = new AnimManager(this);
            GoalManager  = new GoalManager(this);
            JumpManager  = new JumpManager(this);
            MoveManager  = new MoveManager(this);
            NeedsManager = new NeedsManager(this);
            SitManager   = new SitManager(this);
        }

        /// <summary>
        /// Метод который воспроизводится в начале игры
        /// </summary>
        private void Start()
        {
            // Инициализация значений
            InitVal();
            // Инициализация компонентов
            InitComponents();
            // Инициализация объектов отвечающих за какое либо действие
            InitManagers();
            // Стартуем короутину (метод который происходит с промежутком вовремени)
            StartCoroutine(NeedsManager.Consumptions());
        }
        
        /// <summary>
        /// Свойство которое выводит следующую позицию из стека пути
        /// </summary>
        public Vector3 NextPosition
        {
            get
            {
                // Если дистанция между агентом и следующим узлом меньше, чем 0.2
                if (Vector3.Distance(transform.position, Path.Peek()) < 0.2f)
                {
                    // Берётся значение и удаляется из стека
                    return Path.Pop();
                }
                // Мначе
                else
                {
                    // Берётся значение
                    return Path.Peek();
                }
            }
        }

        /// <summary>
        /// Свойство которое выводит максимальную скорость
        /// </summary>
        public float MaxSpeed
        {
            get
            {
                // Проверяется имеется ли какая либо цель,
                // если её нет, ставим ноль,
                // Если есть берём дистанцию от агента до цели
                float MAX_Speed = Goals.IsEmpty ? 0 : Vector3.Distance(transform.position, Goals.First.position);
                // Ограничиваем значение между нулём и двойкой
                MAX_Speed = Mathf.Clamp(MAX_Speed, 0, 2);
                // Выводим получившееся значение
                return MAX_Speed;
            }
        }
        
        /// <summary>
        /// Удаляет цель со сцены и из начала дека
        /// </summary>
        public void RemoveFirstGoal()
        {
            // Удаляет цель со сцены и из начала дека
            Destroy(Goals.RemoveFirst().gameObject);
        }

        /// <summary>
        /// Удаляет цель со сцены и из конца дека
        /// </summary>
        public void RemoveLastGoal()
        {
            // Удаляет цель со сцены и из конца дека
            Destroy(Goals.RemoveLast().gameObject);
        }

        /// <summary>
        /// Добавляет цель на сцену и в начала дека
        /// </summary>
        public void AddFirstGoal(Transform newPoint)
        {
            // Добавляет цель на сцену и в начала дека
            Goals.AddFirst(Instantiate(Point, newPoint.position, Quaternion.identity));
        }

        /// <summary>
        /// Добавляет цель на сцену и в начала дека
        /// </summary>
        public void AddLastGoal(Transform newPoint)
        {
            // Добавляет цель на сцену и в начала дека
            Goals.AddLast(Instantiate(Point, newPoint.position, Quaternion.identity));
        }

        /// <summary>
        /// Метод который происходит при столкновении с объектом обозначенным как тригер
        /// </summary>
        /// <param name="collider">объектом обозначенная как тригер</param>
        private void OnTriggerEnter(Collider collider)
        {
            // Если объект имее тег "Goal"
            if (collider.tag == "Goal")
            {
                // Если позиция цели равна позиции объекта к которому мы сейчас двигаемся и
                // название объекта к которому мы сейчас двигаемся равно названию объекта с которым столкнулись и
                // состояние больше критического уровня
                if (collider.transform.position == Need.prefab.position &&
                    Need.prefab.name == collider.name &&
                    Need.value < Need.criticalValue)
                {
                    // Происходит анимация соответствующая потребности
                    AnimManager.Need(Need.name, true);
                    // Состояние улучшается
                    StartCoroutine(NeedsManager.Processing());
                }
            }

            // Если объект имее тег "Point"
            if (collider.tag == "Point")
            {
                // Удаляет цель со сцены и из начала дека
                RemoveFirstGoal();
            }

            // Если потребности в чём либо нет
            if (Goals != null && !Goals.IsEmpty && collider.transform.position == Goals.Last.position)
            {
                // Удаляются все цели
                GoalManager.ClearAllGoals();
            }
        }
    }
}
