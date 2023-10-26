// Decompiled with JetBrains decompiler
// Type: HMUI.UIItemsList`1
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HMUI
{
  public class UIItemsList<T> : MonoBehaviour where T : MonoBehaviour
  {
    [SerializeField]
    protected T _prefab;
    [SerializeField]
    protected Transform _itemsContainer;
    [SerializeField]
    protected bool _insertInTheBeginning;
    [Inject]
    protected DiContainer _container;
    protected List<T> _items = new List<T>();

    public IEnumerable<T> items => (IEnumerable<T>) this._items;

    public virtual void SetData(int numberOfElements, UIItemsList<T>.DataCallback dataCallback)
    {
      for (int index = 0; index < numberOfElements; ++index)
      {
        T component;
        if (index > this._items.Count - 1)
        {
          GameObject gameObject = this._container.InstantiatePrefab((Object) this._prefab);
          gameObject.SetActive(true);
          component = gameObject.GetComponent<T>();
          component.transform.SetParent(this._itemsContainer, false);
          this._items.Add(component);
        }
        else
        {
          component = this._items[index];
          component.gameObject.SetActive(true);
        }
        if (this._insertInTheBeginning)
          component.transform.SetAsFirstSibling();
        dataCallback(index, component);
      }
      for (int index = numberOfElements; index < this._items.Count; ++index)
        this._items[index].gameObject.SetActive(false);
    }

    public delegate void DataCallback(int idx, T item);
  }
}
