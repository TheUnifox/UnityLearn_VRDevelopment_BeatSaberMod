// Decompiled with JetBrains decompiler
// Type: NoteCutScoreSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteCutScoreSpawner : MonoBehaviour
{
  [SerializeField]
  protected FlyingScoreSpawner _flyingScoreSpawner;
  [Inject]
  protected readonly IScoreController _scoreController;

  public virtual void Start() => this._scoreController.scoringForNoteStartedEvent += new System.Action<ScoringElement>(this.HandleScoringForNoteStarted);

  public virtual void OnDestroy()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.scoringForNoteStartedEvent -= new System.Action<ScoringElement>(this.HandleScoringForNoteStarted);
  }

  public virtual void HandleScoringForNoteStarted(ScoringElement scoringElement)
  {
    if (!(scoringElement is GoodCutScoringElement cutScoringElement))
      return;
    this._flyingScoreSpawner.SpawnFlyingScore(cutScoringElement.cutScoreBuffer, new Color(0.8f, 0.8f, 0.8f));
  }
}
