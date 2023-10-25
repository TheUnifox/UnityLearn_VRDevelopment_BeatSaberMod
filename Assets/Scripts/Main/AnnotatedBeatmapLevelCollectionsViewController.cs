// Decompiled with JetBrains decompiler
// Type: AnnotatedBeatmapLevelCollectionsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnnotatedBeatmapLevelCollectionsViewController : ViewController
{
  [SerializeField]
  protected AnnotatedBeatmapLevelCollectionsGridView _annotatedBeatmapLevelCollectionsGridView;
  [SerializeField]
  protected LoadingControl _loadingControl;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  protected int _selectedItemIndex;
  protected IReadOnlyList<IAnnotatedBeatmapLevelCollection> _annotatedBeatmapLevelCollections;

  public event System.Action didOpenBeatmapLevelCollectionsEvent;

  public event System.Action didCloseBeatmapLevelCollectionsEvent;

  public event System.Action<IAnnotatedBeatmapLevelCollection> didSelectAnnotatedBeatmapLevelCollectionEvent;

  public IAnnotatedBeatmapLevelCollection selectedAnnotatedBeatmapLevelCollection => this._annotatedBeatmapLevelCollections != null && this._annotatedBeatmapLevelCollections.Count > this._selectedItemIndex ? this._annotatedBeatmapLevelCollections[this._selectedItemIndex] : (IAnnotatedBeatmapLevelCollection) null;

  public int selectedItemIndex => this._selectedItemIndex;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this._annotatedBeatmapLevelCollectionsGridView.didOpenAnnotatedBeatmapLevelCollectionEvent += new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsGridViewOpen);
      this._annotatedBeatmapLevelCollectionsGridView.didCloseAnnotatedBeatmapLevelCollectionEvent += new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsGridViewClose);
      this._annotatedBeatmapLevelCollectionsGridView.didSelectAnnotatedBeatmapLevelCollectionEvent += new System.Action<IAnnotatedBeatmapLevelCollection>(this.HandleDidSelectAnnotatedBeatmapLevelCollection);
    }
    this._annotatedBeatmapLevelCollectionsGridView.RefreshAvailability();
    this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
  }

  public virtual void RefreshAvailability() => this._annotatedBeatmapLevelCollectionsGridView.RefreshAvailability();

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (removedFromHierarchy)
    {
      this._annotatedBeatmapLevelCollectionsGridView.didOpenAnnotatedBeatmapLevelCollectionEvent -= new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsGridViewOpen);
      this._annotatedBeatmapLevelCollectionsGridView.didCloseAnnotatedBeatmapLevelCollectionEvent -= new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsGridViewClose);
      this._annotatedBeatmapLevelCollectionsGridView.didSelectAnnotatedBeatmapLevelCollectionEvent -= new System.Action<IAnnotatedBeatmapLevelCollection>(this.HandleDidSelectAnnotatedBeatmapLevelCollection);
    }
    this._annotatedBeatmapLevelCollectionsGridView.CancelAsyncOperations();
    this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.HandleAdditionalContentModelDidInvalidateData);
  }

  public virtual void SetData(
    IReadOnlyList<IAnnotatedBeatmapLevelCollection> annotatedBeatmapLevelCollections,
    int selectedItemIndex,
    bool hideIfOneOrNoPacks)
  {
    this._annotatedBeatmapLevelCollections = annotatedBeatmapLevelCollections;
    this._annotatedBeatmapLevelCollectionsGridView.SetData(this._annotatedBeatmapLevelCollections);
    this._selectedItemIndex = selectedItemIndex;
    if (this.isInViewControllerHierarchy)
      this._annotatedBeatmapLevelCollectionsGridView.SelectAndScrollToCellWithIdx(selectedItemIndex);
    if (annotatedBeatmapLevelCollections == null || hideIfOneOrNoPacks && annotatedBeatmapLevelCollections.Count < 2)
      this._annotatedBeatmapLevelCollectionsGridView.Hide();
    else
      this._annotatedBeatmapLevelCollectionsGridView.Show();
    this._loadingControl.Hide();
  }

  public virtual void ShowLoading()
  {
    this._annotatedBeatmapLevelCollectionsGridView.Hide();
    this._loadingControl.ShowLoading();
  }

  public virtual void HandleAdditionalContentModelDidInvalidateData() => this._annotatedBeatmapLevelCollectionsGridView.RefreshAvailability();

  public virtual void HandleAnnotatedBeatmapLevelCollectionsGridViewOpen()
  {
    System.Action collectionsEvent = this.didOpenBeatmapLevelCollectionsEvent;
    if (collectionsEvent == null)
      return;
    collectionsEvent();
  }

  public virtual void HandleAnnotatedBeatmapLevelCollectionsGridViewClose()
  {
    System.Action collectionsEvent = this.didCloseBeatmapLevelCollectionsEvent;
    if (collectionsEvent == null)
      return;
    collectionsEvent();
  }

  public virtual void HandleDidSelectAnnotatedBeatmapLevelCollection(
    IAnnotatedBeatmapLevelCollection beatmapLevelCollection)
  {
    for (int index = 0; index < this._annotatedBeatmapLevelCollections.Count; ++index)
    {
      if (beatmapLevelCollection == this._annotatedBeatmapLevelCollections[index])
      {
        this._selectedItemIndex = index;
        break;
      }
    }
    System.Action<IAnnotatedBeatmapLevelCollection> levelCollectionEvent = this.didSelectAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent == null)
      return;
    levelCollectionEvent(beatmapLevelCollection);
  }
}
