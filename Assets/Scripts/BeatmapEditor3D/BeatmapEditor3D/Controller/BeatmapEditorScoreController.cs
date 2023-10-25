// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorScoreController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorScoreController : IScoreController
  {
    public event Action<int, int> scoreDidChangeEvent;

    public event Action<int, float> multiplierDidChangeEvent;

    public event Action<ScoringElement> scoringForNoteStartedEvent;

    public event Action<ScoringElement> scoringForNoteFinishedEvent;

    public int multipliedScore => 0;

    public int modifiedScore => 0;

    public int immediateMaxPossibleMultipliedScore => 0;

    public int immediateMaxPossibleModifiedScore => 0;

    public void SetEnabled(bool enabled)
    {
    }
  }
}
