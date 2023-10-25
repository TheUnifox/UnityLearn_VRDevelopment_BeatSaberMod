// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapEditorFoldableView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class BeatmapEditorFoldableView : MonoBehaviour
  {
    [SerializeField]
    private Button _foldButton;
    [SerializeField]
    private LayoutElement _viewLayoutElement;
    [SerializeField]
    private RectTransform _viewTransform;
    [SerializeField]
    private RectTransform _contentTransform;
    [Header("Folded icons")]
    [SerializeField]
    private GameObject _foldedIcon;
    [SerializeField]
    private GameObject _unfoldedIcon;
    [Space]
    [SerializeField]
    private bool _startFolded = true;
    [SerializeField]
    private float _foldedHeight = 50f;
    private bool _isFolded;
    private float _openHeight;

    public event Action foldStateChangedEvent;

    protected void Start()
    {
      this._openHeight = this._viewLayoutElement.preferredHeight;
      this._isFolded = this._startFolded;
      this.SetState();
    }

    protected void OnEnable() => this._foldButton.onClick.AddListener(new UnityAction(this.HandleFoldButtonOnClick));

    protected void OnDisable() => this._foldButton.onClick.RemoveListener(new UnityAction(this.HandleFoldButtonOnClick));

    private void HandleFoldButtonOnClick()
    {
      this._isFolded = !this._isFolded;
      this.SetState();
      Action stateChangedEvent = this.foldStateChangedEvent;
      if (stateChangedEvent == null)
        return;
      stateChangedEvent();
    }

    private void SetState()
    {
      this._contentTransform.gameObject.SetActive(!this._isFolded);
      this._foldedIcon.SetActive(this._isFolded);
      this._unfoldedIcon.SetActive(!this._isFolded);
      this._viewLayoutElement.preferredHeight = this._isFolded ? this._foldedHeight : this._openHeight;
      LayoutRebuilder.MarkLayoutForRebuild(this._viewTransform);
    }
  }
}
