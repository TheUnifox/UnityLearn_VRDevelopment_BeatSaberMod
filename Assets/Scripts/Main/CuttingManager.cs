// Decompiled with JetBrains decompiler
// Type: CuttingManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class CuttingManager : MonoBehaviour
{
  [SerializeField]
  protected SaberManager _saberManager;
  [Inject]
  protected readonly NoteCutter _noteCutter;

  public virtual void OnEnable() => this._saberManager.didUpdateSaberPositionsEvent += new System.Action<Saber, Saber>(this.HandleSaberManagerDidUpdateSaberPositions);

  public virtual void OnDisable() => this._saberManager.didUpdateSaberPositionsEvent -= new System.Action<Saber, Saber>(this.HandleSaberManagerDidUpdateSaberPositions);

  public virtual void HandleSaberManagerDidUpdateSaberPositions(Saber leftSaber, Saber rightSaber)
  {
    if (this._noteCutter == null)
      return;
    this._noteCutter.Cut(leftSaber);
    this._noteCutter.Cut(rightSaber);
  }
}
