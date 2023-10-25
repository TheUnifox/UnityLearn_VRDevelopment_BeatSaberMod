// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapEditorView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.Views
{
  [ExecuteAlways]
  public class BeatmapEditorView : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorViewRelationData _relationData;
    private bool _active;

    public void AddChildView(BeatmapEditorView view) => this._relationData.AddChildIfNotExists(view);

    public void RemoveChildView(BeatmapEditorView view) => this._relationData.RemoveChild(view);

    public void __WillActivate()
    {
      if (this._relationData.hasChildren)
        this._relationData.WillActivateChildren();
      this.WillActivate();
    }

    public void __Activate()
    {
      this.DidActivate();
      this._active = true;
      if (!this._relationData.hasChildren)
        return;
      this._relationData.ActivateChildren();
    }

    public void __WillDeactivate()
    {
      if (this._relationData.hasChildren)
        this._relationData.WillDeactivateChildren();
      this.WillDeactivate();
    }

    public void __Deactivate()
    {
      this.DidDeactivate();
      this._active = false;
      if (!this._relationData.hasChildren)
        return;
      this._relationData.DeactivateChildren();
    }

    protected void Update()
    {
      if (!this._active)
        return;
      this.ViewUpdate();
    }

    protected virtual void WillActivate()
    {
    }

    protected virtual void DidActivate()
    {
    }

    protected virtual void WillDeactivate()
    {
    }

    protected virtual void DidDeactivate()
    {
    }

    protected virtual void ViewUpdate()
    {
    }
  }
}
