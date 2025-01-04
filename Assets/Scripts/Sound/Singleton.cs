using System.Reflection;
using UnityEngine;

public class SingletonBehivour<T> : EventBehaviour where T : EventBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
    }
}

public class SingletonDontDestroy<T> : EventBehaviour where T : EventBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance => _instance ??= FindObjectOfType<T>();

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}


public class PopupSingleton<T> : EventBehaviour where T : EventBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            bool isDontDestroy = false;
            var att = typeof(T).GetCustomAttribute(typeof(ContDestroyOnloadAttribute));
            if (att != null)
            {
                isDontDestroy = (att as ContDestroyOnloadAttribute).isTrue;
            }

            var popup = Instantiate(Resources.Load<GameObject>("Popup/" + typeof(T))).GetComponent<T>();
            if (!isDontDestroy)
                popup.transform.SetParent(UIManager.Instance.transform);
            else popup.transform.SetParent(UIManagerDontDestroyOnload.Instance.transform);
            popup.transform.localScale = Vector3.one;
            popup.transform.localPosition = Vector3.zero;
            instance = popup;
            return instance;
        }
    }
}