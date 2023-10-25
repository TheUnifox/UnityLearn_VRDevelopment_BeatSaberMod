// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EnvironmentTracksDefinitionSO
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace BeatmapEditor3D
{
  public class EnvironmentTracksDefinitionSO : PersistentScriptableObject
  {
    [SerializeField]
    private EnvironmentInfoSO _environmentInfo;
    [Space]
    [SerializeField]
    [NullAllowed]
    private EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] _basicEventTrackInfos;
    [Space]
    [SerializeField]
    [NullAllowed]
    private EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo[] _eventBoxGroupPageInfos;
    private Dictionary<BasicBeatmapEventType, EnvironmentTracksDefinitionSO.BasicEventTrackInfo> _beatmapTypeToTrackInfoMap;
    private Dictionary<EventTrackDefinitionSO, EnvironmentTracksDefinitionSO.BasicEventTrackInfo[]> _trackDefinitionToTrackInfoListMap;
    private List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>[] _trackPageToTrackInfoListMap;
    private List<TrackToolbarType>[] _trackPageToTrackToolbarTypeMap;
    private EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] _visibleTrackInfos;
    private Dictionary<int, int> _groupIdToPageMap;
    private Dictionary<int, EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> _groupIdToTrackMap;
    private Dictionary<int, List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>> _spawnableEventBoxGroupTracksByPageMap;

    public EnvironmentInfoSO environmentInfo => this._environmentInfo;

    public EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] basicEventTrackInfos => this._basicEventTrackInfos;

    public EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] visibleTrackInfos
    {
      get
      {
        if (this._visibleTrackInfos == null || this._visibleTrackInfos.Length == 0)
          this._visibleTrackInfos = ((IEnumerable<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) this._basicEventTrackInfos).Where<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>((Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, bool>) (info => info.trackDefinition.visible)).ToArray<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>();
        return this._visibleTrackInfos;
      }
    }

    public EnvironmentTracksDefinitionSO.BasicEventTrackInfo this[BasicBeatmapEventType type]
    {
      get
      {
        if (this._beatmapTypeToTrackInfoMap == null)
          this._beatmapTypeToTrackInfoMap = ((IEnumerable<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) this._basicEventTrackInfos).ToDictionary<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, BasicBeatmapEventType, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>((Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, BasicBeatmapEventType>) (trackInfo => trackInfo.basicBeatmapEventType), (Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) (trackInfo => trackInfo));
        EnvironmentTracksDefinitionSO.BasicEventTrackInfo basicEventTrackInfo;
        this._beatmapTypeToTrackInfoMap.TryGetValue(type, out basicEventTrackInfo);
        return basicEventTrackInfo;
      }
    }

    public EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] this[
      EventTrackDefinitionSO trackDefinition]
    {
      get
      {
        if (this._trackDefinitionToTrackInfoListMap == null)
          this._trackDefinitionToTrackInfoListMap = ((IEnumerable<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) this.visibleTrackInfos).GroupBy<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EventTrackDefinitionSO>((Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EventTrackDefinitionSO>) (trackInfo => trackInfo.trackDefinition)).ToDictionary<IGrouping<EventTrackDefinitionSO, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>, EventTrackDefinitionSO, EnvironmentTracksDefinitionSO.BasicEventTrackInfo[]>((Func<IGrouping<EventTrackDefinitionSO, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>, EventTrackDefinitionSO>) (group => group.Key), (Func<IGrouping<EventTrackDefinitionSO, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>, EnvironmentTracksDefinitionSO.BasicEventTrackInfo[]>) (group => group.ToArray<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>()));
        return this._trackDefinitionToTrackInfoListMap[trackDefinition];
      }
    }

    public int pageCount => this.beatmapTypeToTrackInfoMap.Length;

    public List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo> this[
      EnvironmentTracksDefinitionSO.BasicEventTrackPage page]
    {
            get
            {
                return this.beatmapTypeToTrackInfoMap[(int)page];
            }
    }

    public List<TrackToolbarType> GetToolbarTypesOnPage(
      EnvironmentTracksDefinitionSO.BasicEventTrackPage page)
    {
      if (this._trackPageToTrackToolbarTypeMap == null)
      {
        this._trackPageToTrackToolbarTypeMap = new List<TrackToolbarType>[2];
        for (int index = 0; index < 2; ++index)
          this._trackPageToTrackToolbarTypeMap[index] = this.beatmapTypeToTrackInfoMap[index].GroupBy<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, TrackToolbarType>((Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, TrackToolbarType>) (info => info.trackToolbarType)).Select<IGrouping<TrackToolbarType, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>, TrackToolbarType>((Func<IGrouping<TrackToolbarType, EnvironmentTracksDefinitionSO.BasicEventTrackInfo>, TrackToolbarType>) (group => group.Key)).ToList<TrackToolbarType>();
      }
      return this._trackPageToTrackToolbarTypeMap[(int) page];
    }

    public EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo[] eventBoxGroupPageInfos => this._eventBoxGroupPageInfos;

    public IReadOnlyDictionary<int, int> groupIdToPageMap
    {
      get
      {
        if (this._groupIdToPageMap == null)
        {
          this._groupIdToPageMap = new Dictionary<int, int>();
          for (int index = 0; index < this._eventBoxGroupPageInfos.Length; ++index)
          {
            foreach (EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo boxGroupTrackInfo in this._eventBoxGroupPageInfos[index].eventBoxGroupTrackInfos)
              this._groupIdToPageMap[boxGroupTrackInfo.lightGroup.groupId] = index;
          }
        }
        return (IReadOnlyDictionary<int, int>) this._groupIdToPageMap;
      }
    }

    public IReadOnlyDictionary<int, EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> groupIdToTrackInfo
    {
      get
      {
        if (this._groupIdToTrackMap == null)
        {
          this._groupIdToTrackMap = new Dictionary<int, EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo>();
          foreach (EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo boxGroupPageInfo in this._eventBoxGroupPageInfos)
          {
            foreach (EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo boxGroupTrackInfo in boxGroupPageInfo.eventBoxGroupTrackInfos)
              this._groupIdToTrackMap[boxGroupTrackInfo.lightGroup.groupId] = boxGroupTrackInfo;
          }
        }
        return (IReadOnlyDictionary<int, EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo>) this._groupIdToTrackMap;
      }
    }

    public List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> GetSpawnableEventBoxGroupTracks(
      int pageId)
    {
      if (this._spawnableEventBoxGroupTracksByPageMap == null)
      {
        this._spawnableEventBoxGroupTracksByPageMap = new Dictionary<int, List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>>();
        for (int key = 0; key < this._eventBoxGroupPageInfos.Length; ++key)
          this._spawnableEventBoxGroupTracksByPageMap[key] = this._eventBoxGroupPageInfos[key].eventBoxGroupTrackInfos.Select<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>((Func<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) (trackInfo => new EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack()
          {
            lightGroup = trackInfo.lightGroup,
            groupName = trackInfo.groupName,
            tracksCount = (trackInfo.showColorTrack ? 1 : 0) + (trackInfo.showRotationTrack ? 1 : 0) + (trackInfo.showTranslationTrack ? 1 : 0),
            eventBoxGroupTracks = this.GetSpawnableEventBoxGroupTypeTracksForTrackInfo(trackInfo)
          })).ToList<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>();
      }
      return !this._spawnableEventBoxGroupTracksByPageMap.ContainsKey(pageId) ? new List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>() : this._spawnableEventBoxGroupTracksByPageMap[pageId];
    }

    public List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack> GetSpawnableEventBoxGroupTypeTracksForTrackInfo(
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo trackInfo)
    {
      List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack> tracksForTrackInfo = new List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack>();
      if (trackInfo.showColorTrack)
        tracksForTrackInfo.Add(new EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack()
        {
          lightGroup = trackInfo.lightGroup,
          groupName = "C",
          trackType = EventBoxGroupEditorData.EventBoxGroupType.Color
        });
      if (trackInfo.showRotationTrack)
        tracksForTrackInfo.Add(new EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack()
        {
          lightGroup = trackInfo.lightGroup,
          groupName = "R",
          trackType = EventBoxGroupEditorData.EventBoxGroupType.Rotation
        });
      if (trackInfo.showTranslationTrack)
        tracksForTrackInfo.Add(new EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack()
        {
          lightGroup = trackInfo.lightGroup,
          groupName = "T",
          trackType = EventBoxGroupEditorData.EventBoxGroupType.Translation
        });
      return tracksForTrackInfo;
    }

    private List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>[] beatmapTypeToTrackInfoMap
    {
      get
      {
        if (this._trackPageToTrackInfoListMap == null)
        {
          this._trackPageToTrackInfoListMap = new List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>[2];
          for (int index = 0; index < 2; ++index)
            this._trackPageToTrackInfoListMap[index] = new List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>();
          foreach (EnvironmentTracksDefinitionSO.BasicEventTrackInfo visibleTrackInfo in this.visibleTrackInfos)
            this._trackPageToTrackInfoListMap[(int) visibleTrackInfo.basicEventTrackPage].Add(visibleTrackInfo);
        }
        return this._trackPageToTrackInfoListMap;
      }
    }

    public enum BasicEventTrackPage
    {
      Page1,
      Page2,
      Count,
    }

    public enum OverrideDefaultLightAxis
    {
      NoOverride,
      X,
      Y,
      Z,
    }

    [Serializable]
    public class BasicEventTrackInfo
    {
      [SerializeField]
      private string _trackName;
      [SerializeField]
      private BasicBeatmapEventType _beatmapEventType;
      [SerializeField]
      private TrackToolbarType _trackToolbarType;
      [SerializeField]
      private EventTrackDefinitionSO _trackDefinition;
      [SerializeField]
      private EnvironmentTracksDefinitionSO.BasicEventTrackPage _basicEventTrackPage;

      public string trackName => this._trackName;

      public BasicBeatmapEventType basicBeatmapEventType => this._beatmapEventType;

      public TrackToolbarType trackToolbarType => this._trackToolbarType;

      public EventTrackDefinitionSO trackDefinition => this._trackDefinition;

      public EnvironmentTracksDefinitionSO.BasicEventTrackPage basicEventTrackPage => this._basicEventTrackPage;
    }

    [Serializable]
    public class EventBoxGroupPageInfo
    {
      [SerializeField]
      private string _eventBoxGroupPageName;
      [SerializeField]
      private List<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> _eventBoxGroupTrackInfos;

      public string eventBoxGroupPageName => this._eventBoxGroupPageName;

      public List<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> eventBoxGroupTrackInfos => this._eventBoxGroupTrackInfos;
    }

    [Serializable]
    public class EventBoxGroupTrackInfo
    {
      [SerializeField]
      private string _groupName;
      [SerializeField]
      private LightGroupSO _lightGroup;
      [Header("Color Track")]
      [SerializeField]
      private bool _showColorTrack = true;
      [Header("Rotation Track")]
      [SerializeField]
      [FormerlySerializedAs("_showRotationTrack")]
      private bool _showRotationXTrack;
      [SerializeField]
      [FormerlySerializedAs("_showRotationTrack")]
      private bool _showRotationYTrack;
      [SerializeField]
      [FormerlySerializedAs("_showRotationTrack")]
      private bool _showRotationZTrack;
      [SerializeField]
      private EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis _overrideDefaultRotationAxis;
      [Header("Translation Track")]
      [SerializeField]
      private bool _showTranslationXTrack;
      [SerializeField]
      private bool _showTranslationYTrack;
      [SerializeField]
      private bool _showTranslationZTrack;
      [SerializeField]
      private EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis _overrideDefaultTranslationAxis;
      [Space]
      [SerializeField]
      private bool _enableDuplicate = true;
      [SerializeField]
      private LightGroupSO[] _targetLightGroups;

      public string groupName => this._groupName;

      public LightGroupSO lightGroup => this._lightGroup;

      public bool showColorTrack => this._showColorTrack;

      public bool showRotationTrack => this._showRotationXTrack || this._showRotationYTrack || this._showRotationZTrack;

      public bool showTranslationTrack => this._showTranslationXTrack || this._showTranslationYTrack || this._showTranslationZTrack;

      public bool showRotationXTrack => this._showRotationXTrack;

      public bool showRotationYTrack => this._showRotationYTrack;

      public bool showRotationZTrack => this._showRotationZTrack;

      public EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis overrideDefaultRotationAxis => this._overrideDefaultRotationAxis;

      public bool showTranslationXTrack => this._showTranslationXTrack;

      public bool showTranslationYTrack => this._showTranslationYTrack;

      public bool showTranslationZTrack => this._showTranslationZTrack;

      public EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis overrideDefaultTranslationAxis => this._overrideDefaultTranslationAxis;

      public bool enableDuplicate => this._enableDuplicate;

      public LightGroupSO[] targetLightGroups => this._targetLightGroups;
    }

    public class SpawnableEventBoxGroupTypeTrack
    {
      public LightGroupSO lightGroup;
      public string groupName;
      public EventBoxGroupEditorData.EventBoxGroupType trackType;
    }

    public class SpawnableEventBoxGroupTrack
    {
      public LightGroupSO lightGroup;
      public string groupName;
      public int tracksCount;
      public List<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTypeTrack> eventBoxGroupTracks;
    }
  }
}
