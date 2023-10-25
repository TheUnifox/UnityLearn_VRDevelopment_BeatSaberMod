// Decompiled with JetBrains decompiler
// Type: CenterStageLobbyViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class CenterStageLobbyViewController : ViewController
{
  [SerializeField]
  protected BeatmapSelectionView _beatmapSelectionView;
  [SerializeField]
  protected ModifiersSelectionView _modifiersSelectionView;

  public virtual void SetLevelGameplaySetupData(ILevelGameplaySetupData levelGameplaySetupData)
  {
    this._beatmapSelectionView.SetBeatmap(levelGameplaySetupData.beatmapLevel);
    this._modifiersSelectionView.SetGameplayModifiers(levelGameplaySetupData.gameplayModifiers);
  }
}
