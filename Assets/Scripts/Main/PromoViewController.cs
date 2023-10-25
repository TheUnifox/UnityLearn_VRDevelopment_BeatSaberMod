// Decompiled with JetBrains decompiler
// Type: PromoViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PromoViewController : ViewController
{
  [SerializeField]
  protected PromoViewController.ButtonPromoTypePair[] _elements;

  public event System.Action<PromoViewController, IAnnotatedBeatmapLevelCollection, IPreviewBeatmapLevel> promoButtonWasPressedEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    foreach (PromoViewController.ButtonPromoTypePair element in this._elements)
    {
      PromoViewController.ButtonPromoTypePair item = element;
      this.buttonBinder.AddBinding(item.button, (System.Action) (() =>
      {
        System.Action<PromoViewController, IAnnotatedBeatmapLevelCollection, IPreviewBeatmapLevel> buttonWasPressedEvent = this.promoButtonWasPressedEvent;
        if (buttonWasPressedEvent == null)
          return;
        buttonWasPressedEvent(this, item.annotatedBeatmapLevelCollection, (IPreviewBeatmapLevel) item.beatmapLevel);
      }));
    }
  }

  [Serializable]
  public class ButtonPromoTypePair
  {
    public Button button;
    [NullAllowed]
    public PreviewBeatmapLevelPackSO previewLevelPack;
    [NullAllowed]
    public BeatmapLevelPackSO levelPack;
    [NullAllowed]
    public BeatmapLevelSO beatmapLevel;
    protected IAnnotatedBeatmapLevelCollection _annotatedBeatmapLevelCollection;

    public IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection
    {
      get
      {
        if (this._annotatedBeatmapLevelCollection == null)
        {
          if ((UnityEngine.Object) this.previewLevelPack != (UnityEngine.Object) null)
            this._annotatedBeatmapLevelCollection = (IAnnotatedBeatmapLevelCollection) this.previewLevelPack;
          else if ((UnityEngine.Object) this.levelPack != (UnityEngine.Object) null)
            this._annotatedBeatmapLevelCollection = (IAnnotatedBeatmapLevelCollection) this.levelPack;
        }
        return this._annotatedBeatmapLevelCollection;
      }
    }
  }
}
