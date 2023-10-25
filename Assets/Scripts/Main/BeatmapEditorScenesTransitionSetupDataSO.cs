// Decompiled with JetBrains decompiler
// Type: BeatmapEditorScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class BeatmapEditorScenesTransitionSetupDataSO : SingleFixedSceneScenesTransitionSetupDataSO
{
  [CompilerGenerated]
  protected bool m_CgoStraightToEditor;

  public event System.Action<BeatmapEditorScenesTransitionSetupDataSO> didFinishEvent;

  public bool goStraightToEditor
  {
    get => this.m_CgoStraightToEditor;
    private set => this.m_CgoStraightToEditor = value;
  }

  public virtual void Init(bool goStraightToEditor)
  {
    this.goStraightToEditor = goStraightToEditor;
    this.Init((SceneSetupData) new BeatmapEditorSceneSetupData((string) null, (string) null));
  }

  public virtual void Finish()
  {
    System.Action<BeatmapEditorScenesTransitionSetupDataSO> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }
}
