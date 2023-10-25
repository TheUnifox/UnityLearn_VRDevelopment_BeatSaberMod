// Decompiled with JetBrains decompiler
// Type: BTSCharacter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MaterialPropertyBlockController))]
[RequireComponent(typeof (BTSCharacterMaterialSwitcher))]
public class BTSCharacter : MonoBehaviour
{
  [SerializeField]
  protected string _characterName;
  [Space]
  [SerializeField]
  protected Animator _animator;
  [SerializeField]
  protected BTSCharacterMaterialSwitcher _btsCharacterMaterialSwitcher;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [SerializeField]
  protected Transform _headTransform;

  public string characterName => this._characterName;

  public MaterialPropertyBlockController materialPropertyBlockController => this._materialPropertyBlockController;

  public Animator animator => this._animator;

  public Transform headTransform => this._headTransform;

  public virtual void SetAlternativeAnimationAndMaterial(
    AnimationClip animation,
    bool alternativeMaterialOn)
  {
    this._btsCharacterMaterialSwitcher.SwapMaterials(alternativeMaterialOn);
    AnimatorOverrideController overrideController = new AnimatorOverrideController(this._animator.runtimeAnimatorController);
    List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();
    foreach (AnimationClip animationClip in overrideController.animationClips)
      keyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, animation));
    overrideController.ApplyOverrides((IList<KeyValuePair<AnimationClip, AnimationClip>>) keyValuePairList);
    this._animator.runtimeAnimatorController = (RuntimeAnimatorController) overrideController;
  }
}
