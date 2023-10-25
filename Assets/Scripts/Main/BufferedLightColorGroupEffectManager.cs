// Decompiled with JetBrains decompiler
// Type: BufferedLightColorGroupEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BufferedLightColorGroupEffectManager : MonoBehaviour
{
  [SerializeField]
  protected LightGroup[] _lightGroups;
  [SerializeField]
  protected MaterialPropertyBlockController[] _materialPropertyBlockControllers;
  [Inject]
  protected readonly DiContainer _container;
  protected BufferedLightColorGroupEffect[] _bufferedLightColorGroupEffects;

  public virtual void Start()
  {
    this._bufferedLightColorGroupEffects = new BufferedLightColorGroupEffect[this._lightGroups.Length];
    for (int index = 0; index < this._lightGroups.Length; ++index)
      this._bufferedLightColorGroupEffects[index] = this._container.Instantiate<BufferedLightColorGroupEffect>((IEnumerable<object>) new BufferedLightColorGroupEffect.InitData[1]
      {
        new BufferedLightColorGroupEffect.InitData(this._lightGroups[index], this._materialPropertyBlockControllers[index])
      });
  }

  public virtual void OnDestroy()
  {
    if (this._bufferedLightColorGroupEffects == null)
      return;
    foreach (BufferedLightColorGroupEffect colorGroupEffect in this._bufferedLightColorGroupEffects)
      colorGroupEffect.Cleanup();
  }
}
