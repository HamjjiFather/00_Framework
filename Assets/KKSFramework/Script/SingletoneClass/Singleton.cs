/// <summary>
/// MonoBehaviour를 상속하지 않은 일반 클래스의 싱글톤 디자인.
/// </summary>
public class Singleton<T> where T : new()
{
    private static readonly object _lock = new object();
    private static T _this;

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_this == null) _this = new T();
                return _this;
            }
        }
    }
}