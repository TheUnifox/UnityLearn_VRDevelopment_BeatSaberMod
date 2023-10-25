// Decompiled with JetBrains decompiler
// Type: MenuShockwave
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using VRUIControls;

public class MenuShockwave : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _shockwavePS;
  [SerializeField]
  protected VRPointer _vrPointer;
  [SerializeField]
  protected Signal[] _buttonClickEvents;
  protected ParticleSystem.EmitParams _shockwavePSEmitParams;

  public virtual void Awake() => this._shockwavePSEmitParams = new ParticleSystem.EmitParams();

  public virtual void OnEnable()
  {
    for (int index = 0; index < this._buttonClickEvents.Length; ++index)
      this._buttonClickEvents[index].Subscribe(new System.Action(this.HandleButtonClickEvent));
  }

  public virtual void OnDisable()
  {
    for (int index = 0; index < this._buttonClickEvents.Length; ++index)
      this._buttonClickEvents[index].Unsubscribe(new System.Action(this.HandleButtonClickEvent));
  }

  public virtual void HandleButtonClickEvent() => this.SpawnShockwave(this._vrPointer.cursorPosition);

  public virtual void SpawnShockwave(Vector3 pos)
  {
    if (!this.isActiveAndEnabled)
      return;
    this._shockwavePSEmitParams.position = pos;
    this._shockwavePS.Emit(this._shockwavePSEmitParams, 1);
  }
}
