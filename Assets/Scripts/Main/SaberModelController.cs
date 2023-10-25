// Decompiled with JetBrains decompiler
// Type: SaberModelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberModelController : MonoBehaviour
{
  [SerializeField]
  protected SaberTrail _saberTrail;
  [SerializeField]
  protected SetSaberGlowColor[] _setSaberGlowColors;
  [SerializeField]
  protected SetSaberFakeGlowColor[] _setSaberFakeGlowColors;
  [SerializeField]
  [NullAllowed]
  protected TubeBloomPrePassLight _saberLight;
  [InjectOptional]
  protected readonly SaberModelController.InitData _initData = new SaberModelController.InitData();
  [Inject]
  protected readonly ColorManager _colorManager;

  public virtual void Init(Transform parent, Saber saber)
  {
    this.transform.SetParent(parent, false);
    this.transform.localPosition = Vector3.zero;
    this.transform.localRotation = Quaternion.identity;
    Color color = this._colorManager.ColorForSaberType(saber.saberType);
    this._saberTrail.Setup((color * this._initData.trailTintColor).linear, (IBladeMovementData) saber.movementData);
    foreach (SetSaberGlowColor setSaberGlowColor in this._setSaberGlowColors)
      setSaberGlowColor.saberType = saber.saberType;
    foreach (SetSaberFakeGlowColor saberFakeGlowColor in this._setSaberFakeGlowColors)
      saberFakeGlowColor.saberType = saber.saberType;
    if (!((Object) this._saberLight != (Object) null))
      return;
    this._saberLight.color = color;
  }

  public class InitData
  {
    public readonly Color trailTintColor;

    public InitData() => this.trailTintColor = Color.white;

    public InitData(Color trailTintColor) => this.trailTintColor = trailTintColor;
  }
}
