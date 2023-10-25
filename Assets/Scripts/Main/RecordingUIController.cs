// Decompiled with JetBrains decompiler
// Type: RecordingUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class RecordingUIController : MonoBehaviour
{
  [SerializeField]
  protected GameObject _circle;
  [Space]
  [SerializeField]
  protected float _updateTimeSpan = 0.75f;
  [InjectOptional]
  protected readonly RecordingUIController.InitData _initData;
  protected float _lastUpdateTime;

  [Inject]
  public virtual void Init()
  {
    if (this._initData == null || !this._initData.recordingEnabled)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
      this._circle.SetActive(true);
      this._lastUpdateTime = Time.time;
    }
  }

  public virtual void Update()
  {
    if ((double) Time.time - (double) this._lastUpdateTime < (double) this._updateTimeSpan)
      return;
    this._circle.SetActive(!this._circle.activeSelf);
    this._lastUpdateTime = Time.time;
  }

  public class InitData
  {
    public readonly bool recordingEnabled;

    public InitData(bool recordingEnabled) => this.recordingEnabled = recordingEnabled;
  }
}
