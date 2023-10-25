// Decompiled with JetBrains decompiler
// Type: BeatLine
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BeatLine : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected TubeBloomPrePassLight _tubeBloomPrePassLight;
  [SerializeField]
  protected AnimationCurve _arriveFadeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  protected AnimationCurve _jumpFadeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  protected float _alphaMul = 1f;
  [SerializeField]
  protected float _maxAlpha = 1f;
  protected readonly List<BeatLine.HighlightData> _highlights = new List<BeatLine.HighlightData>(10);
  protected Color _color;
  protected float _rotation;

  public bool isFinished => this._highlights.Count == 0;

  public float rotation => this._rotation;

  public virtual void Init(Vector3 position, float rotation)
  {
    this._rotation = rotation;
    this._tubeBloomPrePassLight.transform.localPosition = position;
    this.transform.eulerAngles = new Vector3(90f, rotation, 0.0f);
    this._tubeBloomPrePassLight.color = this._color.ColorWithAlpha(this._arriveFadeCurve.Evaluate(0.0f));
  }

  public override void ColorWasSet(Color color) => this._color = color;

  public virtual void AddHighlight(float startTime, float arriveDuration, float jumpDuration) => this._highlights.Add(new BeatLine.HighlightData()
  {
    startTime = startTime,
    arriveDuration = arriveDuration,
    halfJumpDuration = jumpDuration * 0.5f
  });

  public virtual void ManualUpdate(float songTime)
  {
    float alpha = 0.0f;
    for (int index = this._highlights.Count - 1; index >= 0; --index)
    {
      BeatLine.HighlightData highlight = this._highlights[index];
      float num = songTime - highlight.startTime;
      if ((double) num >= (double) highlight.arriveDuration + (double) highlight.halfJumpDuration)
        this._highlights.RemoveAt(index);
      else if ((double) num < (double) highlight.arriveDuration)
        alpha += this._arriveFadeCurve.Evaluate(num / highlight.arriveDuration) * this._alphaMul;
      else
        alpha += this._jumpFadeCurve.Evaluate((num - highlight.arriveDuration) / highlight.halfJumpDuration) * this._alphaMul;
    }
    if (this._highlights.Count == 0)
      return;
    if ((double) alpha > (double) this._maxAlpha)
      alpha = this._maxAlpha;
    this._tubeBloomPrePassLight.color = this._color.ColorWithAlpha(alpha);
  }

  public class Pool : MonoMemoryPool<BeatLine>
  {
  }

  public struct HighlightData
  {
    public float startTime;
    public float arriveDuration;
    public float halfJumpDuration;
  }
}
