// Decompiled with JetBrains decompiler
// Type: NoteDebrisPoolInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteDebrisPoolInstaller : ScriptableObjectInstaller
{
  [SerializeField]
  protected NoteDebris _normalNoteDebrisHDPrefab;
  [SerializeField]
  protected NoteDebris _normalNoteDebrisLWPrefab;
  [SerializeField]
  protected NoteDebris _burstSliderHeadNoteDebrisHDPrefab;
  [SerializeField]
  protected NoteDebris _burstSliderHeadNoteDebrisLWPrefab;
  [SerializeField]
  protected NoteDebris _burstSliderElementNoteHDPrefab;
  [SerializeField]
  protected NoteDebris _burstSliderElementNoteLWPrefab;
  [SerializeField]
  protected BoolSO _noteDebrisHDConditionVariable;

  public override void InstallBindings()
  {
    this.Container.BindMemoryPool<NoteDebris, NoteDebris.Pool>().WithId((object) NoteData.GameplayType.Normal).WithInitialSize(40).FromComponentInNewPrefab((bool) (ObservableVariableSO<bool>) this._noteDebrisHDConditionVariable ? (Object) this._normalNoteDebrisHDPrefab : (Object) this._normalNoteDebrisLWPrefab);
    this.Container.BindMemoryPool<NoteDebris, NoteDebris.Pool>().WithId((object) NoteData.GameplayType.BurstSliderHead).WithInitialSize(40).FromComponentInNewPrefab((bool) (ObservableVariableSO<bool>) this._noteDebrisHDConditionVariable ? (Object) this._burstSliderHeadNoteDebrisHDPrefab : (Object) this._burstSliderHeadNoteDebrisLWPrefab);
    this.Container.BindMemoryPool<NoteDebris, NoteDebris.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElement).WithInitialSize(40).FromComponentInNewPrefab((bool) (ObservableVariableSO<bool>) this._noteDebrisHDConditionVariable ? (Object) this._burstSliderElementNoteHDPrefab : (Object) this._burstSliderElementNoteLWPrefab);
  }
}
