// Decompiled with JetBrains decompiler
// Type: ResultsEnvironmentManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultsEnvironmentManager : MonoBehaviour
{
  [SerializeField]
  protected ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword[] _resultEnvironmentControllersWithKeyword;
  protected string _currentShownKeyword;

  public virtual BaseResultsEnvironmentController GetResultEnvironmentControllerForKeyword(
    string keyword)
  {
    return ((IEnumerable<ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword>) this._resultEnvironmentControllersWithKeyword).FirstOrDefault<ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword>((Func<ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword, bool>) (controllerWithKeyword => controllerWithKeyword.keyword == keyword))?.resultsEnvironmentController;
  }

  public virtual void ShowResultForKeyword(string keyword, bool immediately = false)
  {
    foreach (ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword controllerWithKeyword in this._resultEnvironmentControllersWithKeyword)
    {
      if (keyword == controllerWithKeyword.keyword)
        controllerWithKeyword.resultsEnvironmentController.Activate(immediately);
    }
    this._currentShownKeyword = keyword;
  }

  public virtual void HideResultForKeyword(string keyword, bool immediately = false)
  {
    foreach (ResultsEnvironmentManager.ResultEnvironmentControllerWithKeyword controllerWithKeyword in this._resultEnvironmentControllersWithKeyword)
    {
      if (keyword == controllerWithKeyword.keyword)
        controllerWithKeyword.resultsEnvironmentController.Deactivate(immediately);
    }
  }

  [Serializable]
  public class ResultEnvironmentControllerWithKeyword
  {
    [SerializeField]
    protected string _keyword;
    [SerializeField]
    protected BaseResultsEnvironmentController _resultsEnvironmentController;

    public string keyword => this._keyword;

    public BaseResultsEnvironmentController resultsEnvironmentController => this._resultsEnvironmentController;
  }
}
