// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorScreen
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorScreen : MonoBehaviour
  {
    private BeatmapEditorViewController _rootBeatmapEditorViewController;
    private bool _isBeingDestroyed;
    private RectTransform _rectTransform;

    public bool isBeingDestroyed => this._isBeingDestroyed;

    public RectTransform rectTransform
    {
      get
      {
        if ((Object) this._rectTransform == (Object) null)
          this._rectTransform = (RectTransform) this.transform;
        return this._rectTransform;
      }
    }

    public void SetRootViewController(
      BeatmapEditorViewController newBeatmapEditorViewController)
    {
      if ((Object) newBeatmapEditorViewController == (Object) this._rootBeatmapEditorViewController)
        return;
      this.SwapViewControllers(newBeatmapEditorViewController);
    }

    private void SwapViewControllers(BeatmapEditorViewController newRootViewController)
    {
      BeatmapEditorViewController editorViewController = this._rootBeatmapEditorViewController;
      this._rootBeatmapEditorViewController = newRootViewController;
      if ((Object) newRootViewController != (Object) null)
      {
        newRootViewController.__Init(this, (BeatmapEditorViewController) null);
        if (!newRootViewController.active)
          newRootViewController.active = true;
        newRootViewController.__Activate(true, false);
        newRootViewController.__ResetRectTransformPosition();
      }
      if ((Object) editorViewController != (Object) null)
        editorViewController.__Deactivate(true, false, false);
      if ((Object) newRootViewController != (Object) null)
        newRootViewController.__ResetRectTransformPosition();
      if (!((Object) editorViewController != (Object) null))
        return;
      editorViewController.__ResetRectTransformPosition();
      editorViewController.DeactivateGameObject();
      editorViewController.__ResetViewController();
    }

    protected void OnDestroy() => this._isBeingDestroyed = true;
  }
}
