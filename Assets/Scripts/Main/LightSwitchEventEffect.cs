// Decompiled with JetBrains decompiler
// Type: LightSwitchEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class LightSwitchEventEffect : MonoBehaviour
{
  [SerializeField]
  protected ColorSO _lightColor0;
  [SerializeField]
  protected ColorSO _lightColor1;
  [SerializeField]
  protected ColorSO _highlightColor0;
  [SerializeField]
  protected ColorSO _highlightColor1;
  [SerializeField]
  protected ColorSO _lightColor0Boost;
  [SerializeField]
  protected ColorSO _lightColor1Boost;
  [SerializeField]
  protected ColorSO _highlightColor0Boost;
  [SerializeField]
  protected ColorSO _highlightColor1Boost;
  [SerializeField]
  protected float _offColorIntensity;
  [SerializeField]
  protected bool _lightOnStart;
  [SerializeField]
  protected int _lightsID;
  [SerializeField]
  protected BasicBeatmapEventType _event;
  [Inject]
  protected readonly LightWithIdManager _lightManager;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly SongTimeTweeningManager _tweeningManager;
  [Inject]
  protected readonly ColorManager _colorManager;
  protected ColorTween _colorTween;
  protected Color _alternativeFromColor;
  protected Color _alternativeToColor;
  protected bool _usingBoostColors;
  protected const float kFlashAndFadeDuration = 1.5f;
  protected const float kHighlightDuration = 0.6f;
  protected BeatmapDataCallbackWrapper _colorChangeBeatmapDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _colorBoostBeatmapDataCallbackWrapper;

  public int lightsId => this._lightsID;

  public virtual void Awake()
  {
    this._usingBoostColors = false;
    Color color1 = this._lightOnStart ? (Color) this._lightColor0 : this._lightColor0.color.ColorWithAlpha(this._offColorIntensity);
    Color color2 = this._lightOnStart ? (Color) this._lightColor0Boost : this._lightColor0Boost.color.ColorWithAlpha(this._offColorIntensity);
    this._colorTween = new ColorTween(color1, color1, new System.Action<Color>(this.SetColor), 0.0f, EaseType.Linear);
    this.SetupTweenAndSaveOtherColors(color1, color1, color2, color2);
  }

  public virtual void Start()
  {
    this._colorTween.ForceOnUpdate();
    this._colorChangeBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleColorChangeBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._event));
    this._colorBoostBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ColorBoostBeatmapEventData>(new BeatmapDataCallback<ColorBoostBeatmapEventData>(this.HandleColorBoostBeatmapEvent));
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
    {
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._colorBoostBeatmapDataCallbackWrapper);
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._colorChangeBeatmapDataCallbackWrapper);
    }
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void HandleColorChangeBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    int beatmapEventValue = basicBeatmapEventData.value;
    float floatValue = basicBeatmapEventData.floatValue;
    switch (beatmapEventValue)
    {
      case -1:
      case 3:
      case 7:
      case 11:
        Color highlightColor1 = this.GetHighlightColor(beatmapEventValue, this._usingBoostColors);
        Color fromColor1 = highlightColor1.ColorWithAlpha(highlightColor1.a * floatValue);
        Color toColor1 = this.GetNormalColor(beatmapEventValue, this._usingBoostColors).ColorWithAlpha(this._offColorIntensity * floatValue);
        Color highlightColor2 = this.GetHighlightColor(beatmapEventValue, !this._usingBoostColors);
        Color alternativeFromColor1 = highlightColor2.ColorWithAlpha(highlightColor2.a * floatValue);
        Color alternativeToColor1 = this.GetNormalColor(beatmapEventValue, !this._usingBoostColors).ColorWithAlpha(this._offColorIntensity * floatValue);
        this.SetupTweenAndSaveOtherColors(fromColor1, toColor1, alternativeFromColor1, alternativeToColor1);
        this._colorTween.duration = 1.5f;
        this._colorTween.easeType = EaseType.OutExpo;
        this._tweeningManager.RestartTween((Tween) this._colorTween, (object) this);
        break;
      case 0:
        this._colorTween.Kill();
        Color color1 = this._colorTween.toValue.ColorWithAlpha(this._offColorIntensity * floatValue);
        Color color2 = this._alternativeToColor.ColorWithAlpha(this._offColorIntensity * floatValue);
        this.SetupTweenAndSaveOtherColors(color1, color1, color2, color2);
        this.SetColor(color1);
        this.CheckNextEventForFade(basicBeatmapEventData);
        break;
      case 1:
      case 4:
      case 5:
      case 8:
      case 9:
      case 12:
        this._colorTween.Kill();
        Color normalColor1 = this.GetNormalColor(beatmapEventValue, this._usingBoostColors);
        Color color3 = normalColor1.ColorWithAlpha(normalColor1.a * floatValue);
        Color normalColor2 = this.GetNormalColor(beatmapEventValue, !this._usingBoostColors);
        Color color4 = normalColor2.ColorWithAlpha(normalColor2.a * floatValue);
        this.SetupTweenAndSaveOtherColors(color3, color3, color4, color4);
        this.SetColor(color3);
        this.CheckNextEventForFade(basicBeatmapEventData);
        break;
      case 2:
      case 6:
      case 10:
        Color highlightColor3 = this.GetHighlightColor(beatmapEventValue, this._usingBoostColors);
        Color fromColor2 = highlightColor3.ColorWithAlpha(highlightColor3.a * floatValue);
        Color normalColor3 = this.GetNormalColor(beatmapEventValue, this._usingBoostColors);
        Color toColor2 = normalColor3.ColorWithAlpha(normalColor3.a * floatValue);
        Color highlightColor4 = this.GetHighlightColor(beatmapEventValue, !this._usingBoostColors);
        Color alternativeFromColor2 = highlightColor4.ColorWithAlpha(highlightColor4.a * floatValue);
        Color normalColor4 = this.GetNormalColor(beatmapEventValue, !this._usingBoostColors);
        Color alternativeToColor2 = normalColor4.ColorWithAlpha(normalColor4.a * floatValue);
        this.SetupTweenAndSaveOtherColors(fromColor2, toColor2, alternativeFromColor2, alternativeToColor2);
        this._colorTween.duration = 0.6f;
        this._colorTween.easeType = EaseType.OutCubic;
        this._tweeningManager.RestartTween((Tween) this._colorTween, (object) this);
        break;
    }
  }

  public virtual void HandleColorBoostBeatmapEvent(ColorBoostBeatmapEventData eventData)
  {
    bool boostColorsAreOn = eventData.boostColorsAreOn;
    if (boostColorsAreOn == this._usingBoostColors)
      return;
    this.SetupTweenAndSaveOtherColors(this._alternativeFromColor, this._alternativeToColor, this._colorTween.fromValue, this._colorTween.toValue);
    this._usingBoostColors = boostColorsAreOn;
    this._colorTween.ForceOnUpdate();
  }

  public virtual void SetupTweenAndSaveOtherColors(
    Color fromColor,
    Color toColor,
    Color alternativeFromColor,
    Color alternativeToColor)
  {
    this._alternativeFromColor = alternativeFromColor;
    this._alternativeToColor = alternativeToColor;
    this._colorTween.fromValue = fromColor;
    this._colorTween.toValue = toColor;
  }

  public virtual void CheckNextEventForFade(BasicBeatmapEventData basicBeatmapEventData)
  {
    BasicBeatmapEventData sameTypeEventData = basicBeatmapEventData.nextSameTypeEventData;
    if (sameTypeEventData == null || !sameTypeEventData.HasLightFadeEventDataValue())
      return;
    float floatValue1 = basicBeatmapEventData.floatValue;
    int beatmapEventValue1 = basicBeatmapEventData.value;
    float floatValue2 = sameTypeEventData.floatValue;
    int beatmapEventValue2 = sameTypeEventData.value;
    Color normalColor1 = this.GetNormalColor(beatmapEventValue2, this._usingBoostColors);
    Color color1 = normalColor1.ColorWithAlpha(normalColor1.a * floatValue2);
    Color normalColor2 = this.GetNormalColor(beatmapEventValue2, !this._usingBoostColors);
    Color color2 = normalColor2.ColorWithAlpha(normalColor2.a * floatValue2);
    Color fromColor = this._colorTween.toValue;
    Color alternativeFromColor = this._alternativeToColor;
    if (beatmapEventValue1 == 0)
    {
      fromColor = color1.ColorWithAlpha(0.0f);
      alternativeFromColor = color2.ColorWithAlpha(0.0f);
    }
    else if (!basicBeatmapEventData.HasFixedDurationLightSwitchEventDataValue())
    {
      Color normalColor3 = this.GetNormalColor(beatmapEventValue1, this._usingBoostColors);
      fromColor = normalColor3.ColorWithAlpha(normalColor3.a * floatValue1);
      Color normalColor4 = this.GetNormalColor(beatmapEventValue1, !this._usingBoostColors);
      alternativeFromColor = normalColor4.ColorWithAlpha(normalColor4.a * floatValue1);
    }
    this.SetupTweenAndSaveOtherColors(fromColor, color1, alternativeFromColor, color2);
    this._colorTween.SetStartTimeAndEndTime(basicBeatmapEventData.time, sameTypeEventData.time);
    this._colorTween.easeType = EaseType.Linear;
    this._tweeningManager.ResumeTween((Tween) this._colorTween, (object) this);
  }

  public virtual Color GetNormalColor(int beatmapEventValue, bool colorBoost)
  {
    switch (BeatmapEventDataLightsExtensions.GetLightColorTypeFromEventDataValue(beatmapEventValue))
    {
      case EnvironmentColorType.Color0:
        return !colorBoost ? this._lightColor0.color : this._lightColor0Boost.color;
      case EnvironmentColorType.Color1:
        return !colorBoost ? this._lightColor1.color : this._lightColor1Boost.color;
      case EnvironmentColorType.ColorW:
        return this._colorManager.ColorForType(EnvironmentColorType.ColorW, colorBoost);
      default:
        return this._lightColor0.color;
    }
  }

  public virtual Color GetHighlightColor(int beatmapEventValue, bool colorBoost)
  {
    switch (BeatmapEventDataLightsExtensions.GetLightColorTypeFromEventDataValue(beatmapEventValue))
    {
      case EnvironmentColorType.Color0:
        return !colorBoost ? this._highlightColor0.color : this._highlightColor0Boost.color;
      case EnvironmentColorType.Color1:
        return !colorBoost ? this._highlightColor1.color : this._highlightColor1Boost.color;
      case EnvironmentColorType.ColorW:
        return this._colorManager.ColorForType(EnvironmentColorType.ColorW, colorBoost);
      default:
        return this._highlightColor0.color;
    }
  }

  public virtual void SetColor(Color color) => this._lightManager.SetColorForId(this._lightsID, color);
}
