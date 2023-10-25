// Decompiled with JetBrains decompiler
// Type: LightTranslationGroupEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LightTranslationGroupEffectManager : MonoBehaviour
{
  [Inject]
  protected readonly LightTranslationGroup[] _lightTranslationGroups;
  [Inject]
  protected readonly DiContainer _container;
  protected readonly List<LightTranslationGroupEffect> _lightTranslationGroupEffects = new List<LightTranslationGroupEffect>();

  public virtual void Start()
  {
    foreach (LightTranslationGroup translationGroup in this._lightTranslationGroups)
    {
      List<Transform> xTransforms = translationGroup.xTransforms;
      List<Transform> yTransforms = translationGroup.yTransforms;
      List<Transform> zTransforms = translationGroup.zTransforms;
      for (int index = 0; index < translationGroup.count; ++index)
        this._lightTranslationGroupEffects.Add(this._container.Instantiate<LightTranslationGroupEffect>((IEnumerable<object>) new LightTranslationGroupEffect.InitData[1]
        {
          new LightTranslationGroupEffect.InitData(translationGroup.groupId, index, translationGroup.mirrorX, translationGroup.mirrorY, translationGroup.mirrorZ, index < xTransforms.Count ? xTransforms[index] : (Transform) null, index < yTransforms.Count ? yTransforms[index] : (Transform) null, index < zTransforms.Count ? zTransforms[index] : (Transform) null, translationGroup.xTranslationLimits, translationGroup.xDistributionLimits, translationGroup.yTranslationLimits, translationGroup.yDistributionLimits, translationGroup.zTranslationLimits, translationGroup.zDistributionLimits)
        }));
    }
  }

  public virtual void OnDestroy()
  {
    foreach (LightTranslationGroupEffect translationGroupEffect in this._lightTranslationGroupEffects)
      translationGroupEffect.Cleanup();
  }
}
