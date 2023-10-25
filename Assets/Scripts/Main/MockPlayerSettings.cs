// Decompiled with JetBrains decompiler
// Type: MockPlayerSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class MockPlayerSettings
{
  [SerializeField]
  protected string _userName;
  [SerializeField]
  protected string _userId;
  [SerializeField]
  protected int _sortIndex;
  [SerializeField]
  protected float _latency;
  [SerializeField]
  protected bool _autoConnect;
  [SerializeField]
  protected bool _inactiveByDefault;
  [SerializeField]
  protected MockPlayerMovementType _movementType;
  [SerializeField]
  protected string _recodingFile;
  [SerializeField]
  protected float _aiCubeHitChance;
  [Space]
  [SerializeField]
  protected bool _leftHanded;
  [SerializeField]
  protected Color _saberAColor;
  [SerializeField]
  protected Color _saberBColor;
  [SerializeField]
  protected Color _obstaclesColor;

  public string userName
  {
    get => this._userName;
    set => this._userName = value;
  }

  public string userId
  {
    get => this._userId;
    set => this._userId = value;
  }

  public int sortIndex
  {
    get => this._sortIndex;
    set => this._sortIndex = value;
  }

  public float latency
  {
    get => this._latency;
    set => this._latency = value;
  }

  public bool autoConnect
  {
    get => this._autoConnect;
    set => this._autoConnect = value;
  }

  public bool inactiveByDefault
  {
    get => this._inactiveByDefault;
    set => this._inactiveByDefault = value;
  }

  public MockPlayerMovementType movementType
  {
    get => this._movementType;
    set => this._movementType = value;
  }

  public string recodingFile
  {
    get => this._recodingFile;
    set => this._recodingFile = value;
  }

  public float aiCubeHitChance
  {
    get => this._aiCubeHitChance;
    set => this._aiCubeHitChance = value;
  }

  public bool leftHanded
  {
    get => this._leftHanded;
    set => this._leftHanded = value;
  }

  public Color saberAColor
  {
    get => this._saberAColor;
    set => this._saberAColor = value;
  }

  public Color saberBColor
  {
    get => this._saberBColor;
    set => this._saberBColor = value;
  }

  public Color obstaclesColor
  {
    get => this._obstaclesColor;
    set => this._obstaclesColor = value;
  }
}
