// Decompiled with JetBrains decompiler
// Type: LightRotationGroupEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LightRotationGroupEffectManager : MonoBehaviour
{
  [Inject]
  protected readonly LightRotationGroup[] _lightRotationGroups;
  [Inject]
  protected readonly DiContainer _container;
  protected readonly List<LightRotationGroupEffect> _lightRotationGroupEffects = new List<LightRotationGroupEffect>();

  public virtual void Start()
  {
    foreach (LightRotationGroup lightRotationGroup in this._lightRotationGroups)
    {
      List<Transform> xTransforms = lightRotationGroup.xTransforms;
      List<Transform> yTransforms = lightRotationGroup.yTransforms;
      List<Transform> zTransforms = lightRotationGroup.zTransforms;
      for (int index = 0; index < xTransforms.Count; ++index)
        this._lightRotationGroupEffects.Add(this._container.Instantiate<LightRotationGroupEffect>((IEnumerable<object>) new LightRotationGroupEffect.InitData[1]
        {
          new LightRotationGroupEffect.InitData(lightRotationGroup.groupId, index, LightAxis.X, lightRotationGroup.mirrorX, xTransforms[index])
        }));
      for (int index = 0; index < yTransforms.Count; ++index)
        this._lightRotationGroupEffects.Add(this._container.Instantiate<LightRotationGroupEffect>((IEnumerable<object>) new LightRotationGroupEffect.InitData[1]
        {
          new LightRotationGroupEffect.InitData(lightRotationGroup.groupId, index, LightAxis.Y, lightRotationGroup.mirrorY, yTransforms[index])
        }));
      for (int index = 0; index < zTransforms.Count; ++index)
        this._lightRotationGroupEffects.Add(this._container.Instantiate<LightRotationGroupEffect>((IEnumerable<object>) new LightRotationGroupEffect.InitData[1]
        {
          new LightRotationGroupEffect.InitData(lightRotationGroup.groupId, index, LightAxis.Z, lightRotationGroup.mirrorZ, zTransforms[index])
        }));
    }
  }

  public virtual void OnDestroy()
  {
    foreach (LightRotationGroupEffect rotationGroupEffect in this._lightRotationGroupEffects)
      rotationGroupEffect.Cleanup();
  }
}
