// Decompiled with JetBrains decompiler
// Type: StartMiddleEndButtonBackgroundController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class StartMiddleEndButtonBackgroundController : MonoBehaviour
{
  [SerializeField]
  protected Sprite _startSprite;
  [SerializeField]
  protected Sprite _middleSprite;
  [SerializeField]
  protected Sprite _endSprite;
  [SerializeField]
  protected ImageView _image;

  public virtual void SetStartSprite() => this._image.sprite = this._startSprite;

  public virtual void SetMiddleSprite() => this._image.sprite = this._middleSprite;

  public virtual void SetEndSprite() => this._image.sprite = this._endSprite;
}
