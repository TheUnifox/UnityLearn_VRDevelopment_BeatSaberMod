// Decompiled with JetBrains decompiler
// Type: ScoreModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreModel
{
  public const int kMaxMultiplier = 8;
  public const float kMaxDistanceForDistanceToCenterScore = 0.3f;
  [DoesNotRequireDomainReloadInit]
  public static readonly HashSet<NoteData.ScoringType> fullScoreScoringTypes = new HashSet<NoteData.ScoringType>()
  {
    NoteData.ScoringType.Normal,
    NoteData.ScoringType.SliderHead,
    NoteData.ScoringType.SliderTail
  };
  [DoesNotRequireDomainReloadInit]
  protected static readonly Dictionary<NoteData.ScoringType, ScoreModel.NoteScoreDefinition> _scoreDefinitions = new Dictionary<NoteData.ScoringType, ScoreModel.NoteScoreDefinition>()
  {
    {
      NoteData.ScoringType.Ignore,
      (ScoreModel.NoteScoreDefinition) null
    },
    {
      NoteData.ScoringType.NoScore,
      new ScoreModel.NoteScoreDefinition(0, 0, 0, 0, 0, 0)
    },
    {
      NoteData.ScoringType.Normal,
      new ScoreModel.NoteScoreDefinition(15, 0, 70, 0, 30, 0)
    },
    {
      NoteData.ScoringType.SliderHead,
      new ScoreModel.NoteScoreDefinition(15, 0, 70, 30, 30, 0)
    },
    {
      NoteData.ScoringType.SliderTail,
      new ScoreModel.NoteScoreDefinition(15, 70, 70, 0, 30, 0)
    },
    {
      NoteData.ScoringType.BurstSliderHead,
      new ScoreModel.NoteScoreDefinition(15, 0, 70, 0, 0, 0)
    },
    {
      NoteData.ScoringType.BurstSliderElement,
      new ScoreModel.NoteScoreDefinition(0, 0, 0, 0, 0, 20)
    }
  };
  protected const int kMaxBeforeCutScore = 70;
  public const int kMaxCenterDistanceCutScore = 15;
  protected const int kMaxAfterCutScore = 30;
  [DoesNotRequireDomainReloadInit]
  protected static readonly ScoreMultiplierCounter _scoreMultiplierCounter = new ScoreMultiplierCounter();

  public static ScoreModel.NoteScoreDefinition GetNoteScoreDefinition(
    NoteData.ScoringType scoringType)
  {
    return ScoreModel._scoreDefinitions[scoringType];
  }

  public static int ComputeMaxMultipliedScoreForBeatmap(IReadonlyBeatmapData beatmapData)
  {
    IEnumerable<NoteData> beatmapDataItems1 = beatmapData.GetBeatmapDataItems<NoteData>(0);
    IEnumerable<SliderData> beatmapDataItems2 = beatmapData.GetBeatmapDataItems<SliderData>(0);
    List<ScoreModel.MaxScoreCounterElement> scoreCounterElementList = new List<ScoreModel.MaxScoreCounterElement>(1000);
    foreach (NoteData noteData in beatmapDataItems1)
    {
      if (noteData.scoringType != NoteData.ScoringType.Ignore && noteData.scoringType != NoteData.ScoringType.NoScore)
        scoreCounterElementList.Add(new ScoreModel.MaxScoreCounterElement(noteData.scoringType, noteData.time));
    }
    foreach (SliderData sliderData in beatmapDataItems2)
    {
      if (sliderData.sliderType == SliderData.Type.Burst)
      {
        for (int index = 1; index < sliderData.sliceCount; ++index)
        {
          float t = (float) index / (float) (sliderData.sliceCount - 1);
          scoreCounterElementList.Add(new ScoreModel.MaxScoreCounterElement(NoteData.ScoringType.BurstSliderElement, Mathf.LerpUnclamped(sliderData.time, sliderData.tailTime, t)));
        }
      }
    }
    scoreCounterElementList.Sort();
    int multipliedScoreForBeatmap = 0;
    ScoreModel._scoreMultiplierCounter.Reset();
    foreach (ScoreModel.MaxScoreCounterElement scoreCounterElement in scoreCounterElementList)
    {
      ScoreModel._scoreMultiplierCounter.ProcessMultiplierEvent(ScoreMultiplierCounter.MultiplierEventType.Positive);
      multipliedScoreForBeatmap += scoreCounterElement.noteScoreDefinition.maxCutScore * ScoreModel._scoreMultiplierCounter.multiplier;
    }
    return multipliedScoreForBeatmap;
  }

  public static int GetModifiedScoreForGameplayModifiersScoreMultiplier(
    int multipliedScore,
    float gameplayModifiersScoreMultiplier)
  {
    return Mathf.FloorToInt((float) multipliedScore * gameplayModifiersScoreMultiplier);
  }

  public class NoteScoreDefinition
  {
    public readonly int maxCenterDistanceCutScore;
    public readonly int minBeforeCutScore;
    public readonly int maxBeforeCutScore;
    public readonly int minAfterCutScore;
    public readonly int maxAfterCutScore;
    public readonly int fixedCutScore;

    public int maxCutScore => this.maxCenterDistanceCutScore + this.maxBeforeCutScore + this.maxAfterCutScore + this.fixedCutScore;

    public int executionOrder => this.maxCutScore;

    public NoteScoreDefinition(
      int maxCenterDistanceCutScore,
      int minBeforeCutScore,
      int maxBeforeCutScore,
      int minAfterCutScore,
      int maxAfterCutScore,
      int fixedCutScore)
    {
      this.maxCenterDistanceCutScore = maxCenterDistanceCutScore;
      this.minBeforeCutScore = minBeforeCutScore;
      this.maxBeforeCutScore = maxBeforeCutScore;
      this.minAfterCutScore = minAfterCutScore;
      this.maxAfterCutScore = maxAfterCutScore;
      this.fixedCutScore = fixedCutScore;
    }
  }

  public class MaxScoreCounterElement : IComparable<ScoreModel.MaxScoreCounterElement>
  {
    public readonly ScoreModel.NoteScoreDefinition noteScoreDefinition;
    protected readonly float time;

    public MaxScoreCounterElement(NoteData.ScoringType scoringType, float time)
    {
      this.time = time;
      this.noteScoreDefinition = ScoreModel.GetNoteScoreDefinition(scoringType);
    }

    public virtual int CompareTo(ScoreModel.MaxScoreCounterElement other)
    {
      int num = this.time.CompareTo(other.time);
      return num == 0 ? this.noteScoreDefinition.executionOrder.CompareTo(other.noteScoreDefinition.executionOrder) : num;
    }
  }
}
