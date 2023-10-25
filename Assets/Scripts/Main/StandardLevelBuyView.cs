// Decompiled with JetBrains decompiler
// Type: StandardLevelBuyView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class StandardLevelBuyView : MonoBehaviour
{
  [SerializeField]
  protected LevelBar _levelBar;
  [SerializeField]
  protected Button _buyButton;

  public Button buyButton => this._buyButton;

  public virtual void SetContent(IPreviewBeatmapLevel previewBeatmapLevel) => this._levelBar.Setup(previewBeatmapLevel);
}
