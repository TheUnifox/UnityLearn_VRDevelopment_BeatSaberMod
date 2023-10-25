// Decompiled with JetBrains decompiler
// Type: CreateServerFormController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CreateServerFormController : MonoBehaviour
{
  protected const int kMinPlayers = 2;
  protected const int kMaxPlayers = 5;
  [SerializeField]
  protected FormattedFloatListSettingsController _maxPlayersList;
  protected bool _netDiscoverable;

  public CreateServerFormData formData => new CreateServerFormData()
  {
    maxPlayers = (int) Mathf.Clamp(this._maxPlayersList.value, 2f, 5f),
    netDiscoverable = this._netDiscoverable,
    difficulties = BeatmapDifficultyMask.All,
    modifiers = GameplayModifierMask.All,
    songPacks = SongPackMask.all,
    gameplayServerMode = GameplayServerMode.Managed,
    songSelectionMode = SongSelectionMode.OwnerPicks,
    gameplayServerControlSettings = GameplayServerControlSettings.All
  };

  public virtual void Setup(int selectedNumberOfPlayers, bool netDiscoverable)
  {
    this._maxPlayersList.SetValue((float) selectedNumberOfPlayers);
    this._netDiscoverable = netDiscoverable;
  }
}
