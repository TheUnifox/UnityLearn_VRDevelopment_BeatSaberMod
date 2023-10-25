// Decompiled with JetBrains decompiler
// Type: SpawnRotationChevron
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SpawnRotationChevron : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected TubeBloomPrePassLight[] _lights;
  protected Color _color;
  protected float _lightAmount;

  public override void ColorWasSet(Color color)
  {
    this._color = color;
    this.UpdateLights();
  }

  public virtual void SetLightAmount(float amount)
  {
    this._lightAmount = amount;
    this.UpdateLights();
  }

  public virtual void UpdateLights()
  {
    foreach (TubeBloomPrePassLight light in this._lights)
      light.color = this._color.ColorWithAlpha(this._lightAmount);
  }

  public class Pool : MonoMemoryPool<SpawnRotationChevron>
  {
  }
}
