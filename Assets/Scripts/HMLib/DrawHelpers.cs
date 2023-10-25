// Decompiled with JetBrains decompiler
// Type: DrawHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class DrawHelpers
{
  public static void DrawTexture(
    Texture texture,
    float x,
    float y,
    float w,
    float h,
    Material mat,
    float sx = 0.0f,
    float sy = 0.0f,
    float sw = 1f,
    float sh = 1f)
  {
    x /= (float) Screen.width;
    w /= (float) Screen.width;
    y /= (float) Screen.height;
    h /= (float) Screen.height;
    GL.PushMatrix();
    GL.LoadOrtho();
    mat.mainTexture = texture;
    mat.SetPass(0);
    GL.Begin(7);
    GL.TexCoord2(sx, sy);
    GL.Vertex3(x, y, 0.0f);
    GL.TexCoord2(sx + sw, sy);
    GL.Vertex3(x + w, y, 0.0f);
    GL.TexCoord2(sx + sw, sy + sw);
    GL.Vertex3(x + w, y + h, 0.0f);
    GL.TexCoord2(sx, sy + sw);
    GL.Vertex3(x, y + h, 0.0f);
    GL.End();
    GL.PopMatrix();
  }
}
