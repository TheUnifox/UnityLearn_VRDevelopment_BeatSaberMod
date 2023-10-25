// Decompiled with JetBrains decompiler
// Type: MissionStageLockView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class MissionStageLockView : MonoBehaviour
{
  [SerializeField]
  protected TMP_Text _text;
  [SerializeField]
  protected RectTransform _rectTransform;
  protected float _dstPosY;
  protected float _animationDuration;
  protected float _startAnimationTime;

  public virtual void UpdateLocalPositionY(float dstPosY, bool animated, float animationDuration)
  {
    this._dstPosY = dstPosY;
    if (!animated)
    {
      this._rectTransform.localPosition = new Vector3(0.0f, dstPosY, 0.0f);
    }
    else
    {
      this._startAnimationTime = Time.time;
      this._animationDuration = animationDuration;
      this.enabled = true;
    }
  }

  public virtual void Update()
  {
    float time = Time.time;
    float y;
    if ((double) time > (double) this._startAnimationTime + (double) this._animationDuration || (double) this._animationDuration == 0.0)
    {
      y = this._dstPosY;
      this.enabled = false;
    }
    else
      y = Mathf.Lerp(this._rectTransform.transform.localPosition.y, this._dstPosY, (time - this._startAnimationTime) / this._animationDuration);
    this._rectTransform.transform.localPosition = new Vector3(0.0f, y, 0.0f);
  }

  public virtual void UpdateStageLockText(string text) => this._text.text = text;
}
