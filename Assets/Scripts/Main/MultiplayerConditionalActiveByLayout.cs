// Decompiled with JetBrains decompiler
// Type: MultiplayerConditionalActiveByLayout
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConditionalActiveByLayout : MonoBehaviour
{
  [SerializeField]
  protected MultiplayerConditionalActiveByLayout.Condition _condition;
  [SerializeField]
  protected MultiplayerPlayerLayout _layout;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;

  public virtual void Start()
  {
    if (this._layoutProvider.layout != MultiplayerPlayerLayout.NotDetermined)
      this.HandlePlayersLayoutWasCalculated(this._layoutProvider.layout, this._layoutProvider.activePlayerSpotsCount);
    else
      this._layoutProvider.playersLayoutWasCalculatedEvent += new System.Action<MultiplayerPlayerLayout, int>(this.HandlePlayersLayoutWasCalculated);
  }

  public virtual void HandlePlayersLayoutWasCalculated(
    MultiplayerPlayerLayout layout,
    int playersCount)
  {
    this.gameObject.SetActive(this._condition == MultiplayerConditionalActiveByLayout.Condition.ShowIf && layout == this._layout || this._condition == MultiplayerConditionalActiveByLayout.Condition.HideIf && layout != this._layout);
  }

  public enum Condition
  {
    ShowIf,
    HideIf,
  }
}
