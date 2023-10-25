// Decompiled with JetBrains decompiler
// Type: MultiplayerSpectatingSpotPickerViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;
using Zenject;

public class MultiplayerSpectatingSpotPickerViewController : MonoBehaviour
{
  [SerializeField]
  protected StepValuePicker _stepValuePicker;
  [Inject]
  protected readonly MultiplayerSpectatorController _spectatorController;

  public virtual void Start()
  {
    this.RefreshSpectatingSpotName();
    this._spectatorController.spectatingSpotDidChangeEvent += new System.Action<IMultiplayerSpectatingSpot>(this.HandleSpectatingSpotDidChangeEvent);
    this._stepValuePicker.incButtonWasPressedEvent += new System.Action(this.HandleIncButtonWasPressed);
    this._stepValuePicker.decButtonWasPressedEvent += new System.Action(this.HandleDecButtonWasPressed);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._spectatorController != (UnityEngine.Object) null)
      this._spectatorController.spectatingSpotDidChangeEvent -= new System.Action<IMultiplayerSpectatingSpot>(this.HandleSpectatingSpotDidChangeEvent);
    if (!((UnityEngine.Object) this._stepValuePicker != (UnityEngine.Object) null))
      return;
    this._stepValuePicker.incButtonWasPressedEvent -= new System.Action(this.HandleIncButtonWasPressed);
    this._stepValuePicker.decButtonWasPressedEvent -= new System.Action(this.HandleDecButtonWasPressed);
  }

  public virtual void HandleSpectatingSpotDidChangeEvent(IMultiplayerSpectatingSpot spectatingSpot) => this.RefreshSpectatingSpotName();

  public virtual void RefreshSpectatingSpotName() => this._stepValuePicker.text = this._spectatorController.currentSpot?.spotName ?? Localization.Get("LABEL_GRANDSTAND");

  public virtual void HandleIncButtonWasPressed() => this._spectatorController.SwitchToNext();

  public virtual void HandleDecButtonWasPressed() => this._spectatorController.SwitchToPrev();
}
