// Decompiled with JetBrains decompiler
// Type: BTSCharacterResultAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BTSCharacterResultAnimationController : MonoBehaviour
{
  [SerializeField]
  protected MaterialPropertyBlockColorSetter _rimLightColorSetter;
  [SerializeField]
  protected MaterialPropertyBlockFloatAnimator _rimLightIntensityAnimator;
  [SerializeField]
  protected MaterialPropertyBlockFloatAnimator _rimLightEdgeStartAnimator;
  [Space]
  [SerializeField]
  protected GameObject _collidersGameObject;

  public virtual void SetCharacter(BTSCharacter btsCharacter)
  {
    this._rimLightColorSetter.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._rimLightIntensityAnimator.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._rimLightEdgeStartAnimator.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._collidersGameObject.SetActive(true);
  }

  public virtual void StopAnimation() => this._collidersGameObject.SetActive(false);
}
