// Decompiled with JetBrains decompiler
// Type: PlayerHeightSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Zenject;

public class PlayerHeightSettingsController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Button _setButton;
  [SerializeField]
  protected Vector3SO _roomCenter;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected ButtonBinder _buttonBinder;
  protected float _value;

  public event System.Action<float> valueDidChangeEvent;

  public float value => this._value;

  public virtual void Awake()
  {
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._setButton, new System.Action(this.AutoSetHeight));
  }

  public virtual void Init(float playerHeight)
  {
    this._value = playerHeight;
    this.RefreshUI();
  }

  public virtual void AutoSetHeight()
  {
    Vector3 pos;
    this._vrPlatformHelper.GetNodePose(XRNode.Head, 0, out pos, out Quaternion _);
    this._value = (float) ((double) pos.y + (double) this._roomCenter.value.y + 0.10000000149011612);
    this.RefreshUI();
    System.Action<float> valueDidChangeEvent = this.valueDidChangeEvent;
    if (valueDidChangeEvent == null)
      return;
    valueDidChangeEvent(this._value);
  }

  public virtual void RefreshUI() => this._text.text = string.Format("{0:0.0}m", (object) this._value);
}
