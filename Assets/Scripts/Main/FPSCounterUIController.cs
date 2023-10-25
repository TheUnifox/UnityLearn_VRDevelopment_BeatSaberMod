// Decompiled with JetBrains decompiler
// Type: FPSCounterUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof (FPSCounter))]
public class FPSCounterUIController : MonoBehaviour
{
  [SerializeField]
  protected float _uiUpdateTimeInterval = 0.5f;
  [SerializeField]
  protected TextMeshProUGUI _currentFPSText;
  [SerializeField]
  protected TextMeshProUGUI _lowestFPSText;
  [SerializeField]
  protected TextMeshProUGUI _highestFPSText;
  [SerializeField]
  protected TextMeshProUGUI _droppedFramesText;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  protected FPSCounter _fpsCounter;
  protected float _timeToUpdateUI;

  public virtual void Awake()
  {
    this._fpsCounter = this.GetComponent<FPSCounter>();
    this._fpsCounter.enabled = false;
  }

  public virtual IEnumerator Start()
  {
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    yield return (object) null;
    this._fpsCounter.enabled = true;
  }

  public virtual void LateUpdate()
  {
    this._timeToUpdateUI -= Time.unscaledDeltaTime;
    if ((double) this._timeToUpdateUI > 0.0)
      return;
    this._timeToUpdateUI = this._uiUpdateTimeInterval;
    this._currentFPSText.text = this._fpsCounter.currentFPS.ToString();
    TextMeshProUGUI lowestFpsText = this._lowestFPSText;
    int num = this._fpsCounter.lowestFPS;
    string str1 = num.ToString();
    lowestFpsText.text = str1;
    TextMeshProUGUI highestFpsText = this._highestFPSText;
    num = this._fpsCounter.highestFPS;
    string str2 = num.ToString();
    highestFpsText.text = str2;
    TextMeshProUGUI droppedFramesText = this._droppedFramesText;
    num = this._fpsCounter.droppedFrames;
    string str3 = num.ToString();
    droppedFramesText.text = str3;
  }
}
