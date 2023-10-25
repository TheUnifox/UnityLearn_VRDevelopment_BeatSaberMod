// Decompiled with JetBrains decompiler
// Type: MultiplierValuesRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplierValuesRecorder : MonoBehaviour
{
  [Inject]
  protected IScoreController _scoreController;
  [Inject]
  protected AudioTimeSyncController _audioTimeSyncController;
  protected List<MultiplierValuesRecorder.MultiplierValue> _multiplierValues = new List<MultiplierValuesRecorder.MultiplierValue>(1000);

  public List<MultiplierValuesRecorder.MultiplierValue> multiplierValues => this._multiplierValues;

  public virtual void Start() => this._scoreController.multiplierDidChangeEvent += new System.Action<int, float>(this.HandleScoreControllerMultiplierDidChange);

  public virtual void OnDestroy()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.multiplierDidChangeEvent -= new System.Action<int, float>(this.HandleScoreControllerMultiplierDidChange);
  }

  public virtual void HandleScoreControllerMultiplierDidChange(
    int multiplier,
    float multiplierProgress)
  {
    if (this._multiplierValues.Count > 0 && this._multiplierValues[this._multiplierValues.Count - 1].multiplier == multiplier)
      return;
    this._multiplierValues.Add(new MultiplierValuesRecorder.MultiplierValue(multiplier, this._audioTimeSyncController.songTime));
  }

  public struct MultiplierValue
  {
    public readonly int multiplier;
    public readonly float time;

    public MultiplierValue(int multiplier, float time)
    {
      this.multiplier = multiplier;
      this.time = time;
    }
  }
}
