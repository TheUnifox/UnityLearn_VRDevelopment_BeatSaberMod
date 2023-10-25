// Decompiled with JetBrains decompiler
// Type: ExtensionMethods
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
  public static bool ContainsLayer(this LayerMask layerMask, int layer) => (layerMask.value & 1 << layer) != 0;

  public static Coroutine StartUniqueCoroutine(this MonoBehaviour m, Func<IEnumerator> func)
  {
    m.StopCoroutine(func.Method.Name);
    return m.StartCoroutine(func.Method.Name);
  }

  public static Coroutine StartUniqueCoroutine<T>(
    this MonoBehaviour m,
    Func<T, IEnumerator> func,
    T value)
  {
    m.StopCoroutine(func.Method.Name);
    return m.StartCoroutine(func.Method.Name, (object) value);
  }

  public static void StopUniqueCoroutine(this MonoBehaviour m, Func<IEnumerator> func) => m.StopCoroutine(func.Method.Name);

  public static void StopUniqueCoroutine<T>(this MonoBehaviour m, Func<T, IEnumerator> func) => m.StopCoroutine(func.Method.Name);

  public static bool IsDescendantOf(this Transform transform, Transform parent)
  {
    while ((UnityEngine.Object) transform != (UnityEngine.Object) null && (UnityEngine.Object) transform != (UnityEngine.Object) parent)
      transform = transform.parent;
    return (UnityEngine.Object) transform == (UnityEngine.Object) parent;
  }

  public static void SetLocalPositionAndRotation(this Transform tr, Vector3 pos, Quaternion rot)
  {
    tr.localPosition = pos;
    tr.localRotation = rot;
  }

  public static string GetPath(this Transform current) => (UnityEngine.Object) current.parent == (UnityEngine.Object) null ? "/ " + current.name : current.parent.GetPath() + " / " + current.name;

  public static Quaternion Reflect(this Quaternion source, Vector3 normal) => Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));

  public static Texture2D CreateTexture2D(
    this RenderTexture renderTexture,
    TextureFormat textureFormat = TextureFormat.RGB24)
  {
    GL.Flush();
    Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);
    RenderTexture.active = renderTexture;
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTexture.width, (float) renderTexture.height), 0, 0);
    texture2D.Apply();
    RenderTexture.active = (RenderTexture) null;
    GL.Flush();
    return texture2D;
  }

  public static Vector2 Rotate(this Vector2 vector, float rads) => new Vector2((float) ((double) vector.x * (double) Mathf.Cos(rads) - (double) vector.y * (double) Mathf.Sin(rads)), (float) ((double) vector.x * (double) Mathf.Sin(rads) + (double) vector.y * (double) Mathf.Cos(rads)));
}
