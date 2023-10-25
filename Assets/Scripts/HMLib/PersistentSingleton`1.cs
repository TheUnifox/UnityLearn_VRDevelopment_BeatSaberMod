// Decompiled with JetBrains decompiler
// Type: PersistentSingleton`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
  protected static T _instance;
  [DoesNotRequireDomainReloadInit]
  protected static object _lock = new object();
  protected static bool _applicationIsQuitting = false;

  public static T instance
  {
    get
    {
      if (PersistentSingleton<T>._applicationIsQuitting)
      {
        Debug.LogWarning((object) ("[Singleton] Instance '" + (object) typeof (T) + "' already destroyed on application quit. Won't create again - returning null."));
        return default (T);
      }
      lock (PersistentSingleton<T>._lock)
      {
        if ((Object) PersistentSingleton<T>._instance == (Object) null)
        {
          PersistentSingleton<T>._instance = (T) Object.FindObjectOfType(typeof (T));
          if (Object.FindObjectsOfType(typeof (T)).Length > 1)
          {
            Debug.LogError((object) "[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
            return PersistentSingleton<T>._instance;
          }
          if ((Object) PersistentSingleton<T>._instance == (Object) null)
          {
            GameObject target = new GameObject();
            PersistentSingleton<T>._instance = target.AddComponent<T>();
            target.name = typeof (T).ToString();
            Object.DontDestroyOnLoad((Object) target);
          }
        }
        return PersistentSingleton<T>._instance;
      }
    }
  }

  public static void TouchInstance()
  {
    int num = (Object) PersistentSingleton<T>.instance == (Object) null ? 1 : 0;
  }

  public static bool IsSingletonAvailable => !PersistentSingleton<T>._applicationIsQuitting && (Object) PersistentSingleton<T>._instance != (Object) null;

  public virtual void OnEnable() => Object.DontDestroyOnLoad((Object) this);

  protected virtual void OnDestroy() => PersistentSingleton<T>._applicationIsQuitting = true;
}
