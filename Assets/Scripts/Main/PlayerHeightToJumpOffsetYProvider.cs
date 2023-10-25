// Decompiled with JetBrains decompiler
// Type: PlayerHeightToJumpOffsetYProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class PlayerHeightToJumpOffsetYProvider : IJumpOffsetYProvider, IInitializable, IDisposable
{
  [Inject]
  protected readonly PlayerHeightDetector _playerHeightDetector;
  protected float _jumpOffsetY;

  public float jumpOffsetY => this._jumpOffsetY;

  public virtual void Initialize()
  {
    this._jumpOffsetY = PlayerHeightToJumpOffsetYProvider.JumpOffsetYForPlayerHeight(this._playerHeightDetector.playerHeight);
    this._playerHeightDetector.playerHeightDidChangeEvent += new System.Action<float>(this.HandlePlayerHeightDidChange);
    this.HandlePlayerHeightDidChange(this._playerHeightDetector.playerHeight);
  }

  public virtual void Dispose()
  {
    if (!((UnityEngine.Object) this._playerHeightDetector != (UnityEngine.Object) null))
      return;
    this._playerHeightDetector.playerHeightDidChangeEvent -= new System.Action<float>(this.HandlePlayerHeightDidChange);
  }

  public virtual void HandlePlayerHeightDidChange(float playerHeight) => this._jumpOffsetY = PlayerHeightToJumpOffsetYProvider.JumpOffsetYForPlayerHeight(playerHeight);

  public static float JumpOffsetYForPlayerHeight(float playerHeight) => Mathf.Clamp((float) (((double) playerHeight - 1.7999999523162842) * 0.5), -0.2f, 0.6f);
}
