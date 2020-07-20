using UnityEngine;

/// <summary>
/// MonoBehaviour를 상속한 클래스의 싱글톤.
/// </summary>
public class MonobehaviourSingletonClass<T> : MonoBehaviour where T : MonobehaviourSingletonClass<T>
{
    private static T _instance;
    [SerializeField] private bool _isPersistant = true;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<T>();
                    _instance.gameObject.name = _instance.GetType().Name;
                }
            }

            return _instance;
        }
    }


    public static bool HasInstance => !IsDestroyed;

    public static bool IsDestroyed => _instance == null;


    protected virtual void Awake()
    {
        if (_isPersistant)
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _instance = this as T;
        }
    }
}