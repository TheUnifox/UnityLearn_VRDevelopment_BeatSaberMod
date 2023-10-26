// Decompiled with JetBrains decompiler
// Type: MainEffectSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class MainEffectSO : PersistentScriptableObject
{
  public virtual void PreRender()
  {
  }

  public virtual void Render(RenderTexture src, RenderTexture dest, float fade)
  {
  }

  public virtual void PostRender(float fade)
  {
  }

  public virtual bool hasPostProcessEffect => false;
}
