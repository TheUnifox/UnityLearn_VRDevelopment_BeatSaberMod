// Decompiled with JetBrains decompiler
// Type: MirrorRendererGraphicsSettingsPresets
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class MirrorRendererGraphicsSettingsPresets : NamedPresetsSO
{
  [SerializeField]
  protected MirrorRendererGraphicsSettingsPresets.Preset[] _presets;

  public MirrorRendererGraphicsSettingsPresets.Preset[] presets => this._presets;

  public override NamedPreset[] namedPresets => (NamedPreset[]) this._presets;

  [Serializable]
  public class Preset : NamedPreset
  {
    public MirrorRendererGraphicsSettingsPresets.Preset.MirrorType mirrorType = MirrorRendererGraphicsSettingsPresets.Preset.MirrorType.RenderedMirror;
    public LayerMask reflectLayers = (LayerMask)(-1);
    public int stereoTextureWidth = 2048;
    public int stereoTextureHeight = 1024;
    public int monoTextureWidth = 256;
    public int monoTextureHeight = 256;
    public int maxAntiAliasing = 1;
    public bool enableBloomPrePassFog;

    public enum MirrorType
    {
      None,
      FakeMirror,
      RenderedMirror,
    }
  }
}
