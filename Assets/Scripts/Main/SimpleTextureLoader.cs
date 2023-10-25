// Decompiled with JetBrains decompiler
// Type: SimpleTextureLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class SimpleTextureLoader
{
  private static HMCache<string, Texture2D> _cache = new HMCache<string, Texture2D>(100);

  public static void LoadTexture(
    string filePath,
    bool useCache,
    System.Action<Texture2D> finishedCallback)
  {
    PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(SimpleTextureLoader.LoadTextureCoroutine(filePath, useCache, finishedCallback));
  }

  public static IEnumerator LoadTextureCoroutine(
    string filePath,
    bool useCache,
    System.Action<Texture2D> finishedCallback)
  {
    if (useCache && SimpleTextureLoader._cache.IsInCache(filePath))
    {
      System.Action<Texture2D> action = finishedCallback;
      if (action != null)
        action(SimpleTextureLoader._cache.GetFromCache(filePath));
    }
    else
    {
      using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(FileHelpers.GetEscapedURLForFilePath(filePath)))
      {
        yield return (object) uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
        {
          System.Action<Texture2D> action = finishedCallback;
          if (action != null)
            action((Texture2D) null);
        }
        else
        {
          Texture2D content = DownloadHandlerTexture.GetContent(uwr);
          if (useCache)
            SimpleTextureLoader._cache.PutToCache(filePath, content);
          System.Action<Texture2D> action = finishedCallback;
          if (action != null)
            action(content);
        }
      }
    }
  }
}
