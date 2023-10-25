// Decompiled with JetBrains decompiler
// Type: SetTubeBloomPrePassLightColor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SetTubeBloomPrePassLightColor : MonoBehaviour
{
  [SerializeField]
  protected ColorSO _color;
  [SerializeField]
  protected TubeBloomPrePassLight[] _tubeLights;

  public virtual void Start()
  {
    for (int index = 0; index < this._tubeLights.Length; ++index)
      this._tubeLights[index].color = (Color) this._color;
  }
}
