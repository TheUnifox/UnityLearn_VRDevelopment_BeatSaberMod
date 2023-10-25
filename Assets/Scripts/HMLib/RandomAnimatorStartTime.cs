// Decompiled with JetBrains decompiler
// Type: RandomAnimatorStartTime
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class RandomAnimatorStartTime : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;
  [SerializeField]
  protected string _stateName = "Idle";

  public virtual void Start() => this._animator.Play(this._stateName, 0, Random.Range(0.0f, 1f));
}
