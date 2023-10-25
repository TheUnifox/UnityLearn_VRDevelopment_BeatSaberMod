// Decompiled with JetBrains decompiler
// Type: GameObjectTestExceptionsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTestExceptionsSO : PersistentScriptableObject
{
  public GameObjectTestExceptionsSO.GameObjectTestException[] tubeBloomPrePassLightIgnores;

  public static Dictionary<EnvironmentInfoSO, string[]> GetExceptionAsDictionary(
    GameObjectTestExceptionsSO.GameObjectTestException[] testExceptionArray)
  {
    Dictionary<EnvironmentInfoSO, string[]> exceptionAsDictionary = new Dictionary<EnvironmentInfoSO, string[]>();
    foreach (GameObjectTestExceptionsSO.GameObjectTestException testException in testExceptionArray)
      exceptionAsDictionary.Add(testException.environmentInfo, testException.sceneHierarchies);
    return exceptionAsDictionary;
  }

  [Serializable]
  public class GameObjectTestException
  {
    [SerializeField]
    protected EnvironmentInfoSO _environmentInfo;
    [SerializeField]
    [TextArea]
    protected string _exceptionNotes;
    [SerializeField]
    protected string[] _sceneHierarchies;

    public EnvironmentInfoSO environmentInfo => this._environmentInfo;

    public string[] sceneHierarchies => this._sceneHierarchies;
  }
}
