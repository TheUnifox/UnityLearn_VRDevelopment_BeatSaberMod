// Decompiled with JetBrains decompiler
// Type: GridView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridView : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _contentTransform;
  [CompilerGenerated]
  protected GridView.GridViewCellsEnumerator m_CcellsEnumerator;
  protected GridView.IDataSource _dataSource;
  protected int _columnCount;
  protected int _rowCount;
  protected readonly Dictionary<MonoBehaviour, Queue<MonoBehaviour>> _availableCellsPerPrefabDictionary = new Dictionary<MonoBehaviour, Queue<MonoBehaviour>>();
  protected readonly Dictionary<MonoBehaviour, List<MonoBehaviour>> _spawnedCellsPerPrefabDictionary = new Dictionary<MonoBehaviour, List<MonoBehaviour>>();

  public GridView.IDataSource dataSource => this._dataSource;

  public GridView.GridViewCellsEnumerator cellsEnumerator
  {
    get => this.m_CcellsEnumerator;
    private set => this.m_CcellsEnumerator = value;
  }

  public int rowCount => this._rowCount;

  public int columnCount => this._columnCount;

  public virtual void SetDataSource(GridView.IDataSource newDataSource, bool reloadData)
  {
    if (this.cellsEnumerator == null)
      this.cellsEnumerator = new GridView.GridViewCellsEnumerator(this);
    this._dataSource = newDataSource;
    if (!reloadData)
      return;
    this.ReloadData();
  }

  public virtual void ReloadData()
  {
    float cellWidth = this._dataSource.GetCellWidth();
    float cellHeight = this._dataSource.GetCellHeight();
    int numberOfCells = this._dataSource.GetNumberOfCells();
    this._columnCount = Mathf.FloorToInt(this._contentTransform.rect.width / cellWidth);
    this._rowCount = Mathf.CeilToInt((float) numberOfCells / (float) this._columnCount);
    foreach (MonoBehaviour key in this._spawnedCellsPerPrefabDictionary.Keys)
    {
      List<MonoBehaviour> spawnedCellsPerPrefab = this._spawnedCellsPerPrefabDictionary[key];
      foreach (MonoBehaviour monoBehaviour in spawnedCellsPerPrefab)
      {
        monoBehaviour.gameObject.SetActive(false);
        this._availableCellsPerPrefabDictionary[key].Enqueue(monoBehaviour);
      }
      spawnedCellsPerPrefab.Clear();
    }
    for (int idx = 0; idx < numberOfCells; ++idx)
    {
      int num1 = idx % this._columnCount;
      int num2 = idx / this._columnCount;
      RectTransform transform = (RectTransform) this._dataSource.CellForIdx(this, idx).transform;
      transform.anchorMin = new Vector2(0.0f, 1f);
      transform.anchorMax = new Vector2(0.0f, 1f);
      transform.pivot = new Vector2(0.0f, 1f);
      transform.anchoredPosition = new Vector2((float) num1 * cellWidth, (float) num2 * -cellHeight);
    }
  }

  public virtual List<MonoBehaviour> GetActiveCellsForIdentifier(MonoBehaviour prefab)
  {
    List<MonoBehaviour> cellsForIdentifier;
    if (!this._spawnedCellsPerPrefabDictionary.TryGetValue(prefab, out cellsForIdentifier))
      this._spawnedCellsPerPrefabDictionary[prefab] = cellsForIdentifier = new List<MonoBehaviour>();
    return cellsForIdentifier;
  }

  public virtual T GetReusableCellView<T>(MonoBehaviour prefab) where T : MonoBehaviour
  {
    Queue<MonoBehaviour> monoBehaviourQueue;
    if (!this._availableCellsPerPrefabDictionary.TryGetValue(prefab, out monoBehaviourQueue))
      this._availableCellsPerPrefabDictionary[prefab] = monoBehaviourQueue = new Queue<MonoBehaviour>();
    T reusableCellView = monoBehaviourQueue.Count <= 0 ? Object.Instantiate<MonoBehaviour>(prefab, (Transform) this._contentTransform) as T : monoBehaviourQueue.Dequeue() as T;
    List<MonoBehaviour> monoBehaviourList;
    if (!this._spawnedCellsPerPrefabDictionary.TryGetValue(prefab, out monoBehaviourList))
      this._spawnedCellsPerPrefabDictionary[prefab] = monoBehaviourList = new List<MonoBehaviour>();
    monoBehaviourList.Add((MonoBehaviour) reusableCellView);
    reusableCellView.gameObject.SetActive(true);
    return reusableCellView;
  }

  public interface IDataSource
  {
    int GetNumberOfCells();

    float GetCellWidth();

    float GetCellHeight();

    MonoBehaviour CellForIdx(GridView gridView, int idx);
  }

  public class GridViewCellsEnumerator : IEnumerable<MonoBehaviour>, IEnumerable
  {
    protected readonly GridView _gridView;

    public virtual IEnumerator<MonoBehaviour> GetEnumerator()
    {
      foreach (MonoBehaviour key in this._gridView._spawnedCellsPerPrefabDictionary.Keys)
      {
        foreach (MonoBehaviour monoBehaviour in this._gridView._spawnedCellsPerPrefabDictionary[key])
          yield return monoBehaviour;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public GridViewCellsEnumerator(GridView gridView) => this._gridView = gridView;
  }
}
