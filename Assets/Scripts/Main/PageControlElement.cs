// Decompiled with JetBrains decompiler
// Type: PageControlElement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class PageControlElement : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _rectTransform;
  [SerializeField]
  protected ImageView _imageView;
  [Space]
  [SerializeField]
  protected Color _selectedColor = Color.red;
  [SerializeField]
  protected Color _unselectedColor = Color.blue;

  public RectTransform rectTransform => this._rectTransform;

  public virtual void SetSelected(bool isSelected) => this._imageView.color = isSelected ? this._selectedColor : this._unselectedColor;
}
