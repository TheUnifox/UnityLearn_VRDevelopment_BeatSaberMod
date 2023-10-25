// Decompiled with JetBrains decompiler
// Type: BTSStarTextEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BTSStarTextEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BTSStarTextEventEffect.StarTextSprite[] _starTextSprites;
  [SerializeField]
  protected BTSStarTextEventEffect.StartTextPosition[] _startTextPositions;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected const BasicBeatmapEventType kStarTextAppearEventType = BasicBeatmapEventType.Special1;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;
  protected readonly Dictionary<int, BTSStarTextEventEffect.StarTextSprite> _idsToStarTextSpriteDictionary = new Dictionary<int, BTSStarTextEventEffect.StarTextSprite>();
  protected readonly Dictionary<int, Transform> _idsToStarTextPositionDictionary = new Dictionary<int, Transform>();

  public event System.Action<Sprite, Transform, float> startStarTextAnimationEvent;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(BasicBeatmapEventType.Special1));
    foreach (BTSStarTextEventEffect.StarTextSprite starTextSprite in this._starTextSprites)
      this._idsToStarTextSpriteDictionary.Add(starTextSprite.id, starTextSprite);
    foreach (BTSStarTextEventEffect.StartTextPosition startTextPosition in this._startTextPositions)
      this._idsToStarTextPositionDictionary.Add(startTextPosition.id, startTextPosition.transform);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if ((double) Mathf.Abs(basicBeatmapEventData.time - this._audioTimeSource.songTime) > (double) Time.deltaTime)
      return;
    int textId = BTSStarTextEffectValueParser.GetTextId(basicBeatmapEventData.value);
    int positionId = BTSStarTextEffectValueParser.GetPositionId(basicBeatmapEventData.value);
    BTSStarTextEventEffect.StarTextSprite starTextSprite;
    if (!this._idsToStarTextSpriteDictionary.TryGetValue(textId, out starTextSprite))
    {
      Debug.LogError((object) "Non-existing text id requested.");
    }
    else
    {
      Transform transform;
      if (!this._idsToStarTextPositionDictionary.TryGetValue(positionId, out transform))
      {
        Debug.LogError((object) "Non-existing position id requested.");
      }
      else
      {
        System.Action<Sprite, Transform, float> textAnimationEvent = this.startStarTextAnimationEvent;
        if (textAnimationEvent == null)
          return;
        textAnimationEvent(starTextSprite.starTextSprite, transform, starTextSprite.animationLength);
      }
    }
  }

  [Serializable]
  public class StarTextSprite
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected Sprite _starTextSprite;
    [SerializeField]
    protected float _animationLength;

    public int id => this._id;

    public Sprite starTextSprite => this._starTextSprite;

    public float animationLength => this._animationLength;
  }

  [Serializable]
  public class StartTextPosition
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected Transform _transform;

    public int id => this._id;

    public Transform transform => this._transform;
  }
}
