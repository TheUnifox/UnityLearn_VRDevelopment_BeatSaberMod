// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapEditorViewRelationData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  [Serializable]
  public class BeatmapEditorViewRelationData
  {
    [SerializeField]
    [NullAllowed]
    private UnityEngine.Object _parent;
    [SerializeField]
    private List<BeatmapEditorView> _children = new List<BeatmapEditorView>();

    public UnityEngine.Object parent => this._parent;

    public List<BeatmapEditorView> children => this._children;

    public bool hasChildren => this._children != null && this._children.Count > 0;

    public void AddChild(BeatmapEditorView child) => this._children.Add(child);

    public void AddChildIfNotExists(BeatmapEditorView child)
    {
      if (this._children.Contains(child))
        return;
      this.AddChild(child);
    }

    public void RemoveChild(BeatmapEditorView child)
    {
      this._children.Remove(child);
      this._children = this._children.Where<BeatmapEditorView>((Func<BeatmapEditorView, bool>) (c => (UnityEngine.Object) c != (UnityEngine.Object) null)).ToList<BeatmapEditorView>();
    }

    public void SetParent(UnityEngine.Object parent) => this._parent = parent;

    public void AddChildToParent(BeatmapEditorView childView)
    {
      if (this._parent == (UnityEngine.Object) null)
        return;
      if (this._parent is BeatmapEditorView parent1)
      {
        parent1.AddChildView(childView);
      }
      else
      {
        if (!(this._parent is BeatmapEditorViewController parent))
          return;
        parent.AddChildView(childView);
      }
    }

    public void RemoveChildViewFromParent(BeatmapEditorView childView)
    {
      if (this._parent == (UnityEngine.Object) null)
        return;
      if (this._parent is BeatmapEditorView parent1)
      {
        parent1.RemoveChildView(childView);
      }
      else
      {
        if (!(this._parent is BeatmapEditorViewController parent))
          return;
        parent.RemoveChildView(childView);
      }
    }

    public void ClearEmptyChildren()
    {
      if (!this.hasChildren)
        return;
      this._children = this._children.Where<BeatmapEditorView>((Func<BeatmapEditorView, bool>) (child => (UnityEngine.Object) child != (UnityEngine.Object) null)).ToList<BeatmapEditorView>();
    }

    public void WillActivateChildren()
    {
      for (int index = 0; index < this._children.Count; ++index)
        this._children[index].__WillActivate();
    }

    public void WillDeactivateChildren()
    {
      for (int index = 0; index < this._children.Count; ++index)
        this._children[index].__WillDeactivate();
    }

    public void ActivateChildren()
    {
      for (int index = 0; index < this._children.Count; ++index)
        this._children[index].__Activate();
    }

    public void DeactivateChildren()
    {
      for (int index = 0; index < this._children.Count; ++index)
        this._children[index].__Deactivate();
    }
  }
}
