using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    [Header("Singleton")]
    [SerializeField]
    private bool destroyOnLoad = true;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        Instance = this as T;
    }
}