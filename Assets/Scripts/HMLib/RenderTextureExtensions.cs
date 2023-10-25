// Decompiled with JetBrains decompiler
// Type: RenderTextureExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public static class RenderTextureExtensions
{
  public static Texture2D GetTexture2D(this RenderTexture rt)
  {
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = rt;
    Texture2D texture2D = new Texture2D(rt.width, rt.height);
    texture2D.wrapMode = rt.wrapMode;
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), 0, 0);
    texture2D.Apply();
    RenderTexture.active = active;
    return texture2D;
  }
}
