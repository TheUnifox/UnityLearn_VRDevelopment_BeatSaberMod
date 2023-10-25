// Decompiled with JetBrains decompiler
// Type: WhiteColorOrAlphaGroupEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WhiteColorOrAlphaGroupEffectManager : MonoBehaviour
{
  [SerializeField]
  protected LightGroup[] _lightGroup;
  [SerializeField]
  protected Color _color;
  [Inject]
  protected readonly DiContainer _container;
  protected readonly List<WhiteColorOrAlphaGroupEffect> _whiteColorOrAlphaEffects = new List<WhiteColorOrAlphaGroupEffect>();

  public virtual void Start()
  {
    foreach (LightGroup lightGroup in this._lightGroup)
    {
      for (int elementId = 0; elementId < lightGroup.numberOfElements; ++elementId)
        this._whiteColorOrAlphaEffects.Add(this._container.Instantiate<WhiteColorOrAlphaGroupEffect>((IEnumerable<object>) new object[2]
        {
          (object) new LightColorGroupEffect.InitData(lightGroup.groupId, elementId, lightGroup.startLightId + elementId),
          (object) this._color
        }));
    }
  }

  public virtual void OnDestroy()
  {
    foreach (LightColorGroupEffect colorOrAlphaEffect in this._whiteColorOrAlphaEffects)
      colorOrAlphaEffect.Cleanup();
  }
}
