// Decompiled with JetBrains decompiler
// Type: BTSStarTextEffectsManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class BTSStarTextEffectsManager : MonoBehaviour
{
  [SerializeField]
  protected BTSStarTextEventEffect _btsStarTextEventEffect;
  [Inject]
  protected readonly BTSStarTextEffectController.Pool _btsStarTextEffectControllerPool;

  public virtual void Start() => this._btsStarTextEventEffect.startStarTextAnimationEvent += new System.Action<Sprite, Transform, float>(this.HandleBTSStarTextEventEffect);

  public virtual void OnDestroy() => this._btsStarTextEventEffect.startStarTextAnimationEvent -= new System.Action<Sprite, Transform, float>(this.HandleBTSStarTextEventEffect);

  public virtual void HandleBTSStarTextEventEffect(
    Sprite sprite,
    Transform parentTransform,
    float desiredAnimationLength)
  {
    this.StartCoroutine(this.DespawnEffectDelayed(this._btsStarTextEffectControllerPool.Spawn(sprite, parentTransform, desiredAnimationLength)));
  }

  public virtual IEnumerator DespawnEffectDelayed(BTSStarTextEffectController effectController)
  {
    yield return (object) new WaitForSeconds(effectController.animationDuration);
    this._btsStarTextEffectControllerPool.Despawn(effectController);
  }
}
