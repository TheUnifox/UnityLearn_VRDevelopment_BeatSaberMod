// Decompiled with JetBrains decompiler
// Type: CreditsContent
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CreditsContent : MonoBehaviour
{
  [SerializeField]
  protected GameObject _normalTextPrefab;
  [SerializeField]
  protected GameObject _normalLocalizedTextPrefab;
  [SerializeField]
  protected GameObject _titleTextPrefab;
  [SerializeField]
  protected GameObject _titleLocalizedTextPrefab;
  [SerializeField]
  protected GameObject _headerTextPrefab;
  [SerializeField]
  protected GameObject _headerLocalizedTextPrefab;
  [SerializeField]
  protected int _columnCount = 3;
  [SerializeField]
  protected float _spaceHeight = 8f;
  [SerializeField]
  protected float _titleHeight = 11f;
  [Space]
  [SerializeField]
  protected Transform _contentRoot;
  [Space]
  [SerializeField]
  protected RectTransform _rootRectTransform;
  [SerializeField]
  protected TextAsset _creditsContentTextAsset;

  public GameObject normalTextPrefab => this._normalTextPrefab;

  public GameObject normalLocalizedTextPrefab => this._normalLocalizedTextPrefab;

  public GameObject titleTextPrefab => this._titleTextPrefab;

  public GameObject titleLocalizedTextPrefab => this._titleLocalizedTextPrefab;

  public GameObject headerTextPrefab => this._headerTextPrefab;

  public GameObject headerLocalizedTextPrefab => this._headerLocalizedTextPrefab;

  public int columnCount => this._columnCount;

  public float spaceHeight => this._spaceHeight;

  public float titleHeight => this._titleHeight;

  public Transform contentRoot => this._contentRoot;

  public RectTransform rootRectTransform => this._rootRectTransform;

  public TextAsset creditsContentTextAsset => this._creditsContentTextAsset;
}
