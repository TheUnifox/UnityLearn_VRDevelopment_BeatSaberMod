// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EnvironmentDefinitionsListSO
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EnvironmentDefinitionsListSO : PersistentScriptableObject
  {
    [SerializeField]
    private EnvironmentTracksDefinitionSO[] _environmentTracksDefinitions;
    private Dictionary<EnvironmentInfoSO, EnvironmentTracksDefinitionSO> _environmentTracksTypeMap;

    public EnvironmentTracksDefinitionSO this[EnvironmentInfoSO type]
    {
      get
      {
        if (this._environmentTracksTypeMap == null)
        {
          this._environmentTracksTypeMap = new Dictionary<EnvironmentInfoSO, EnvironmentTracksDefinitionSO>(this._environmentTracksDefinitions.Length);
          foreach (EnvironmentTracksDefinitionSO tracksDefinition in this._environmentTracksDefinitions)
            this._environmentTracksTypeMap[tracksDefinition.environmentInfo] = tracksDefinition;
        }
        EnvironmentTracksDefinitionSO tracksDefinitionSo;
        this._environmentTracksTypeMap.TryGetValue(type, out tracksDefinitionSo);
        return tracksDefinitionSo;
      }
    }
  }
}
