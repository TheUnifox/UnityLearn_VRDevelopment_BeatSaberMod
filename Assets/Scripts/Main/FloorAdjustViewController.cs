// Decompiled with JetBrains decompiler
// Type: FloorAdjustViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Zenject;

public class FloorAdjustViewController : ViewController
{
  [SerializeField]
  protected Vector3SO _roomCenter;
  [Space]
  [SerializeField]
  protected Button _yIncButton;
  [SerializeField]
  protected Button _yDecButton;
  [SerializeField]
  protected TextMeshProUGUI _playerHeightText;
  [Inject]
  protected IVRPlatformHelper _vrPlatformHelper;
  protected const float kMoveStep = 0.05f;
  protected const float kMinPlayerHeight = 0.5f;
  protected const float kMaxPlayerHeight = 3f;
  protected float _playerHeight;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._yIncButton, (System.Action) (() =>
    {
      Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
      roomCenter.y -= 0.05f;
      this._roomCenter.value = roomCenter;
    }));
    this.buttonBinder.AddBinding(this._yDecButton, (System.Action) (() =>
    {
      Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
      roomCenter.y += 0.05f;
      this._roomCenter.value = roomCenter;
    }));
  }

  public virtual void Update()
  {
    Vector3 pos;
    this._vrPlatformHelper.GetNodePose(XRNode.Head, 0, out pos, out Quaternion _);
    this._playerHeight = (float) ((double) pos.y + (double) this._roomCenter.value.y + 0.10000000149011612);
    if ((double) this._playerHeight < 0.5)
    {
      Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
      roomCenter.y += 0.5f - this._playerHeight;
      this._roomCenter.value = roomCenter;
      this._playerHeight = 0.5f;
    }
    else if ((double) this._playerHeight > 3.0)
    {
      Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
      roomCenter.y += 3f - this._playerHeight;
      this._roomCenter.value = roomCenter;
      this._playerHeight = 3f;
    }
    this._playerHeightText.text = Mathf.FloorToInt((float) ((double) this._playerHeight * 100.0 + 0.5)).ToString() + "cm";
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__9_0()
  {
    Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
    roomCenter.y -= 0.05f;
    this._roomCenter.value = roomCenter;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__9_1()
  {
    Vector3 roomCenter = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
    roomCenter.y += 0.05f;
    this._roomCenter.value = roomCenter;
  }
}
