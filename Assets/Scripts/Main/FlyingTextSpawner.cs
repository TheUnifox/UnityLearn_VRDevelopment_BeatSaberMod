// Decompiled with JetBrains decompiler
// Type: FlyingTextSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FlyingTextSpawner : MonoBehaviour, IFlyingObjectEffectDidFinishEvent
{
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
  protected float _fontSize = 5f;
  [SerializeField]
  protected bool _shake;
  [Inject]
  protected readonly FlyingTextEffect.Pool _flyingTextEffectPool;

  public virtual void SpawnText(
    Vector3 pos,
    Quaternion rotation,
    Quaternion inverseRotation,
    string text)
  {
    FlyingTextEffect flyingTextEffect = this._flyingTextEffectPool.Spawn();
    flyingTextEffect.transform.localPosition = pos;
    pos = inverseRotation * pos;
    Vector3 targetPos = rotation * new Vector3(pos.x * this._xSpread, this._targetYPos, this._targetZPos);
    flyingTextEffect.InitAndPresent(text, this._duration, targetPos, rotation, this._color, this._fontSize, this._shake);
    flyingTextEffect.didFinishEvent.Add((IFlyingObjectEffectDidFinishEvent) this);
  }

  public virtual void HandleFlyingObjectEffectDidFinish(FlyingObjectEffect flyingObjectEffect)
  {
    flyingObjectEffect.didFinishEvent.Remove((IFlyingObjectEffectDidFinishEvent) this);
    this._flyingTextEffectPool.Despawn(flyingObjectEffect as FlyingTextEffect);
  }
}
