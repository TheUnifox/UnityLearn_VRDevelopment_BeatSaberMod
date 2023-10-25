// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorScreenSystem
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BeatmapEditor3D
{
  public sealed class BeatmapEditorScreenSystem : MonoBehaviour
  {
    [Header("Screens")]
    [SerializeField]
    private BeatmapEditorScreen _navbarBeatmapEditorScreen;
    [SerializeField]
    private BeatmapEditorScreen _mainBeatmapEditorScreen;
    [SerializeField]
    private BeatmapEditorScreen _dialogBeatmapEditorScreen;
    [Header("Back Button")]
    [SerializeField]
    private BackButtonView _backButton;

    public BeatmapEditorScreen navbarScreen => this._navbarBeatmapEditorScreen;

    public BeatmapEditorScreen mainScreen => this._mainBeatmapEditorScreen;

    public BeatmapEditorScreen dialogScreen => this._dialogBeatmapEditorScreen;

    public event Action backButtonWasPressedEvent;

    private void Awake() => this._backButton.onClick.AddListener((UnityAction) (() =>
    {
      Action buttonWasPressedEvent = this.backButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    }));

    private void OnDestroy() => this._backButton.onClick.RemoveAllListeners();

    public void SetBackButtonIconType(BackButtonView.BackButtonType type) => this._backButton.SetBackButtonType(type);

    public void SetBackButtonIsDirtyNotification(bool isDirty) => this._backButton.SetDirtyNotification(isDirty);

    public void SetBackButton(bool visible) => this._backButton.gameObject.SetActive(visible);
  }
}
