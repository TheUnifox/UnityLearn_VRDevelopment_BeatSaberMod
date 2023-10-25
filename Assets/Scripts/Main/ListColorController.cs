// Decompiled with JetBrains decompiler
// Type: ListColorController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class ListColorController : IncDecColorController
{
  private int _idx;
  private int _numberOfElements;

  protected abstract bool GetInitValues(out int idx, out int numberOfElements);

  protected abstract void ApplyValue(int idx);

  protected abstract Color ColorForValue(int idx);

  protected void OnEnable()
  {
    if (!this.GetInitValues(out this._idx, out this._numberOfElements))
      return;
    this.RefreshUI();
  }

  private void RefreshUI()
  {
    this.color = this.ColorForValue(this._idx);
    this.enableDec = this._idx > 0;
    this.enableInc = this._idx < this._numberOfElements - 1;
  }

  public void Refresh(bool applyValue)
  {
    if (!this.GetInitValues(out this._idx, out this._numberOfElements))
      return;
    this.RefreshUI();
    if (!applyValue)
      return;
    this.ApplyValue(this._idx);
  }

  protected override void IncButtonPressed()
  {
    if (this._idx < this._numberOfElements - 1)
      ++this._idx;
    this.RefreshUI();
    this.ApplyValue(this._idx);
  }

  protected override void DecButtonPressed()
  {
    if (this._idx > 0)
      --this._idx;
    this.RefreshUI();
    this.ApplyValue(this._idx);
  }
}
