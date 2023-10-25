// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapObjectEditGridView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class BeatmapObjectEditGridView : MonoBehaviour
  {
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private BeatmapObjectEditGridCellView[] _cells;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    private const int kColumnsCount = 4;
    private const int kRowsCount = 3;
    private readonly List<BeatmapObjectEditGridCellView> _highlightedCells = new List<BeatmapObjectEditGridCellView>();

    public event Action<int, int> pointerDownEvent;

    public event Action<int, int> pointerUpEvent;

    public event Action<int, int> pointerEnterEvent;

    public event Action<int, int> pointerExitEvent;

    protected void Start()
    {
      foreach (BeatmapObjectEditGridCellView cell in this._cells)
      {
        cell.pointerDownEvent += new Action<int, int>(this.HandleSongElementGridCellViewPointerDown);
        cell.pointerUpEvent += new Action<int, int>(this.HandleSongElementGridCellViewPointerUp);
        cell.pointerEnterEvent += new Action<int, int>(this.HandleSongElementGridCellViewPointerEnter);
        cell.pointerExitEvent += new Action<int, int>(this.HandleSongElementGridCellViewPointerExit);
      }
    }

    protected void OnDestroy()
    {
      foreach (BeatmapObjectEditGridCellView cell in this._cells)
      {
        cell.pointerDownEvent -= new Action<int, int>(this.HandleSongElementGridCellViewPointerDown);
        cell.pointerUpEvent -= new Action<int, int>(this.HandleSongElementGridCellViewPointerUp);
        cell.pointerEnterEvent -= new Action<int, int>(this.HandleSongElementGridCellViewPointerEnter);
        cell.pointerExitEvent -= new Action<int, int>(this.HandleSongElementGridCellViewPointerExit);
      }
    }

    public void Show() => this.SetVisible(!this._beatmapEditorSettingsDataModel.zenMode);

    public void Hide() => this.SetVisible(false);

    public void UpdateHighlightedArea(Vector2Int startCellCoords, Vector2Int endCellCoords)
    {
      this.ResetHighlights();
      int num1 = Mathf.Min(startCellCoords.x, endCellCoords.x);
      int num2 = Mathf.Max(startCellCoords.x, endCellCoords.x);
      int num3 = Mathf.Min(startCellCoords.y, endCellCoords.y);
      int num4 = Mathf.Max(startCellCoords.y, endCellCoords.y);
      for (int column = num1; column <= num2; ++column)
      {
        for (int row = num3; row <= num4; ++row)
          this.HighlightCell(column, row);
      }
    }

    public void ResetHighlights()
    {
      foreach (BeatmapObjectEditGridCellView highlightedCell in this._highlightedCells)
        highlightedCell.SetHighlighted(false);
      this._highlightedCells.Clear();
    }

    public void HighlightCell(int column, int row)
    {
      BeatmapObjectEditGridCellView cellByCoords = this.GetCellByCoords(column, row);
      cellByCoords.SetHighlighted(true);
      this._highlightedCells.Add(cellByCoords);
    }

    public void ResetHighlight(int column, int row) => this.GetCellByCoords(column, row).SetHighlighted(false);

    private void HandleSongElementGridCellViewPointerEnter(int x, int y)
    {
      Action<int, int> pointerEnterEvent = this.pointerEnterEvent;
      if (pointerEnterEvent == null)
        return;
      pointerEnterEvent(x, y);
    }

    private void HandleSongElementGridCellViewPointerExit(int x, int y)
    {
      Action<int, int> pointerExitEvent = this.pointerExitEvent;
      if (pointerExitEvent == null)
        return;
      pointerExitEvent(x, y);
    }

    private void HandleSongElementGridCellViewPointerDown(int x, int y)
    {
      Action<int, int> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(x, y);
    }

    private void HandleSongElementGridCellViewPointerUp(int x, int y)
    {
      Action<int, int> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(x, y);
    }

    private BeatmapObjectEditGridCellView GetCellByCoords(int column, int row) => this._cells[row * 4 + column];

    private void SetVisible(bool visible) => this._canvas.enabled = visible;
  }
}
