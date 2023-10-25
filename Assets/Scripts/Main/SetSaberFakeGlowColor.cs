// Decompiled with JetBrains decompiler
// Type: SetSaberFakeGlowColor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SetSaberFakeGlowColor : MonoBehaviour
{
  [SerializeField]
  protected Color _tintColor;
  [Space]
  [SerializeField]
  [NullAllowed]
  protected SaberTypeObject _saberTypeObject;
  [SerializeField]
  protected Parametric3SliceSpriteController _parametric3SliceSprite;
  [Inject]
  protected ColorManager _colorManager;
  protected SaberType _saberType;

  public SaberType saberType
  {
    set
    {
      this._saberTypeObject = (SaberTypeObject) null;
      this._saberType = value;
      this.SetColors();
    }
  }

  public virtual void Start()
  {
    if ((Object) this._saberTypeObject != (Object) null)
      this._saberType = this._saberTypeObject.saberType;
    this.SetColors();
  }

  public virtual void SetColors()
  {
    this._parametric3SliceSprite.color = this._colorManager.ColorForSaberType(this._saberType) * this._tintColor;
    this._parametric3SliceSprite.Refresh();
  }
}
