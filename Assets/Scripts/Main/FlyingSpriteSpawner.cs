// Decompiled with JetBrains decompiler
// Type: FlyingSpriteSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FlyingSpriteSpawner : MonoBehaviour, IFlyingObjectEffectDidFinishEvent
{
  [SerializeField]
  protected Sprite _sprite;
  [SerializeField]
  protected Material _material;
  [SerializeField]
  protected float _duration = 1f;
  [SerializeField]
  protected float _xSpread = 1.15f;
  [SerializeField]
  protected float _targetYPos = 1.3f;
  [SerializeField]
  protected float _targetZPos = 10f;
  [SerializeField]
  protected Color _color = new Color(0.0f, 0.75f, 1f);
  [SerializeField]
  protected bool _shake;
  [Inject]
  protected readonly FlyingSpriteEffect.Pool _flyingSpriteEffectPool;

  public virtual void SpawnFlyingSprite(
    Vector3 pos,
    Quaternion rotation,
    Quaternion inverseRotation)
  {
    FlyingSpriteEffect flyingSpriteEffect = this._flyingSpriteEffectPool.Spawn();
    flyingSpriteEffect.didFinishEvent.Add((IFlyingObjectEffectDidFinishEvent) this);
    flyingSpriteEffect.transform.localPosition = pos;
    pos = inverseRotation * pos;
    flyingSpriteEffect.InitAndPresent(this._duration, rotation * new Vector3(Mathf.Sign(pos.x) * this._xSpread, this._targetYPos, this._targetZPos), rotation, this._sprite, this._material, this._color, this._shake);
  }

  public virtual void HandleFlyingObjectEffectDidFinish(FlyingObjectEffect flyingObjectEffect)
  {
    flyingObjectEffect.didFinishEvent.Remove((IFlyingObjectEffectDidFinishEvent) this);
    this._flyingSpriteEffectPool.Despawn(flyingObjectEffect as FlyingSpriteEffect);
  }
}
