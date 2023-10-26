// Decompiled with JetBrains decompiler
// Type: FadeOutInstantly
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;
using Zenject;

public class FadeOutInstantly : MonoBehaviour
{
  [Inject]
  private FadeInOutController _fadeInOut;

  protected void Start() => this._fadeInOut.FadeOutInstant();
}
