// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class LevelEditToolbarView : MonoBehaviour
  {
    [Header("Buttons")]
    [SerializeField]
    private Toggle _upLeftDirectionToggle;
    [SerializeField]
    private Toggle _upDirectionToggle;
    [SerializeField]
    private Toggle _upRightDirectionToggle;
    [SerializeField]
    private Toggle _leftDirectionToggle;
    [SerializeField]
    private Toggle _anyDirectionToggle;
    [SerializeField]
    private Toggle _rightDirectionToggle;
    [SerializeField]
    private Toggle _downLeftDirectionToggle;
    [SerializeField]
    private Toggle _downDirectionToggle;
    [SerializeField]
    private Toggle _downRightDirectionToggle;

    public event Action<NoteCutDirection> buttonPressed;

    private ToggleBinder toggleBinder { get; set; }

    public void UpdateSelection(NoteCutDirection noteCutDirection) => this.GetToggleByNoteCutDirection(noteCutDirection).SetIsOnWithoutNotify(true);

    protected void Start()
    {
      this.toggleBinder = new ToggleBinder();
      this.toggleBinder.AddBinding(this._upLeftDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.UpLeft);
      }));
      this.toggleBinder.AddBinding(this._upDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.Up);
      }));
      this.toggleBinder.AddBinding(this._upRightDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.UpRight);
      }));
      this.toggleBinder.AddBinding(this._leftDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.Left);
      }));
      this.toggleBinder.AddBinding(this._anyDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.Any);
      }));
      this.toggleBinder.AddBinding(this._rightDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.Right);
      }));
      this.toggleBinder.AddBinding(this._downLeftDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.DownLeft);
      }));
      this.toggleBinder.AddBinding(this._downDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.Down);
      }));
      this.toggleBinder.AddBinding(this._downRightDirectionToggle, true, (Action) (() =>
      {
        Action<NoteCutDirection> buttonPressed = this.buttonPressed;
        if (buttonPressed == null)
          return;
        buttonPressed(NoteCutDirection.DownRight);
      }));
    }

    protected void OnDestroy() => this.toggleBinder.ClearBindings();

    private Toggle GetToggleByNoteCutDirection(NoteCutDirection noteCutDirection)
    {
      switch (noteCutDirection)
      {
        case NoteCutDirection.Up:
          return this._upDirectionToggle;
        case NoteCutDirection.Down:
          return this._downDirectionToggle;
        case NoteCutDirection.Left:
          return this._leftDirectionToggle;
        case NoteCutDirection.Right:
          return this._rightDirectionToggle;
        case NoteCutDirection.UpLeft:
          return this._upLeftDirectionToggle;
        case NoteCutDirection.UpRight:
          return this._upRightDirectionToggle;
        case NoteCutDirection.DownLeft:
          return this._downLeftDirectionToggle;
        case NoteCutDirection.DownRight:
          return this._downRightDirectionToggle;
        case NoteCutDirection.Any:
          return this._anyDirectionToggle;
        case NoteCutDirection.None:
          return this._anyDirectionToggle;
        default:
          throw new ArgumentOutOfRangeException(nameof (noteCutDirection), (object) noteCutDirection, (string) null);
      }
    }
  }
}
