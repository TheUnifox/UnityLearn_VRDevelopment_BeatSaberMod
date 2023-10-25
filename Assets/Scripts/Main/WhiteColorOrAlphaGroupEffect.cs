// Decompiled with JetBrains decompiler
// Type: WhiteColorOrAlphaGroupEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class WhiteColorOrAlphaGroupEffect : LightColorGroupEffect
{
  protected readonly Color _defaultColor;

  [Inject]
  public WhiteColorOrAlphaGroupEffect(
    LightColorGroupEffect.InitData initData,
    Color defaultColor,
    LightWithIdManager lightManager,
    SongTimeTweeningManager tweeningManager,
    ColorManager colorManager,
    BeatmapCallbacksController beatmapCallbacksController,
    IBpmController bpmController)
    : base(initData, lightManager, tweeningManager, colorManager, beatmapCallbacksController, bpmController)
  {
    this._defaultColor = defaultColor;
  }

  protected override Color GetColor(
    EnvironmentColorType colorType,
    bool colorBoost,
    float brightness)
  {
    Color color = this._defaultColor;
    if (colorType == EnvironmentColorType.ColorW)
      color = this._colorManager.ColorForType(EnvironmentColorType.ColorW, false);
    return color.ColorWithAlpha(color.a * brightness);
  }
}
