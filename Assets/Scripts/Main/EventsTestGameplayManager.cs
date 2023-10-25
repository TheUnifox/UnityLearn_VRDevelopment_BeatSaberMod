// Decompiled with JetBrains decompiler
// Type: EventsTestGameplayManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class EventsTestGameplayManager : MonoBehaviour
{
  [SerializeField]
  protected bool _moveTime;
  [SerializeField]
  protected bool _spawnTestBox;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly BeatmapData _beatmapData;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected BasicBeatmapEventType _basicBeatmapEventType;
  protected float _floatValue = 1f;
  protected const int kNumberOfLightGroups = 20;
  protected readonly BeatmapEventDataBoxGroupList[] _beatmapEventDataBoxGroupLists = new BeatmapEventDataBoxGroupList[20];
  protected bool[] groupState = new bool[20];
  protected readonly Dictionary<KeyCode, BasicBeatmapEventType> _beatmapEventTypeBindings = new Dictionary<KeyCode, BasicBeatmapEventType>()
  {
    {
      KeyCode.Alpha1,
      BasicBeatmapEventType.Event0
    },
    {
      KeyCode.Alpha2,
      BasicBeatmapEventType.Event1
    },
    {
      KeyCode.Alpha3,
      BasicBeatmapEventType.Event2
    },
    {
      KeyCode.Alpha4,
      BasicBeatmapEventType.Event3
    },
    {
      KeyCode.Alpha5,
      BasicBeatmapEventType.Event4
    },
    {
      KeyCode.Alpha6,
      BasicBeatmapEventType.Event5
    },
    {
      KeyCode.Alpha7,
      BasicBeatmapEventType.Event6
    },
    {
      KeyCode.Alpha8,
      BasicBeatmapEventType.Event7
    },
    {
      KeyCode.Alpha9,
      BasicBeatmapEventType.Event8
    },
    {
      KeyCode.Alpha0,
      BasicBeatmapEventType.Event9
    },
    {
      KeyCode.Q,
      BasicBeatmapEventType.Event10
    },
    {
      KeyCode.W,
      BasicBeatmapEventType.Event11
    },
    {
      KeyCode.E,
      BasicBeatmapEventType.Event12
    },
    {
      KeyCode.R,
      BasicBeatmapEventType.Event13
    },
    {
      KeyCode.T,
      BasicBeatmapEventType.Event14
    },
    {
      KeyCode.Y,
      BasicBeatmapEventType.Event15
    },
    {
      KeyCode.U,
      BasicBeatmapEventType.Event16
    },
    {
      KeyCode.I,
      BasicBeatmapEventType.Event17
    }
  };
  protected readonly Dictionary<KeyCode, int> _intBindings = new Dictionary<KeyCode, int>()
  {
    {
      KeyCode.Alpha1,
      0
    },
    {
      KeyCode.Alpha2,
      1
    },
    {
      KeyCode.Alpha3,
      2
    },
    {
      KeyCode.Alpha4,
      3
    },
    {
      KeyCode.Alpha5,
      4
    },
    {
      KeyCode.Alpha6,
      5
    },
    {
      KeyCode.Alpha7,
      6
    },
    {
      KeyCode.Alpha8,
      7
    },
    {
      KeyCode.Alpha9,
      8
    },
    {
      KeyCode.Alpha0,
      9
    },
    {
      KeyCode.Q,
      10
    },
    {
      KeyCode.W,
      11
    },
    {
      KeyCode.E,
      12
    },
    {
      KeyCode.R,
      13
    },
    {
      KeyCode.T,
      14
    },
    {
      KeyCode.Y,
      15
    },
    {
      KeyCode.U,
      16
    },
    {
      KeyCode.I,
      17
    },
    {
      KeyCode.O,
      18
    },
    {
      KeyCode.P,
      19
    }
  };
  protected readonly Dictionary<KeyCode, int> _beatmapValuesBindings = new Dictionary<KeyCode, int>()
  {
    {
      KeyCode.Keypad0,
      0
    },
    {
      KeyCode.Keypad1,
      1
    },
    {
      KeyCode.Keypad2,
      2
    },
    {
      KeyCode.Keypad3,
      3
    },
    {
      KeyCode.Keypad4,
      4
    },
    {
      KeyCode.Keypad5,
      5
    },
    {
      KeyCode.Keypad6,
      6
    },
    {
      KeyCode.Keypad7,
      7
    },
    {
      KeyCode.Keypad8,
      8
    },
    {
      KeyCode.Keypad9,
      9
    }
  };
  protected readonly Dictionary<KeyCode, float> _floatValuesBindings = new Dictionary<KeyCode, float>()
  {
    {
      KeyCode.Keypad0,
      0.0f
    },
    {
      KeyCode.Keypad1,
      0.1f
    },
    {
      KeyCode.Keypad2,
      0.2f
    },
    {
      KeyCode.Keypad3,
      0.3f
    },
    {
      KeyCode.Keypad4,
      0.4f
    },
    {
      KeyCode.Keypad5,
      0.5f
    },
    {
      KeyCode.Keypad6,
      0.6f
    },
    {
      KeyCode.Keypad7,
      0.7f
    },
    {
      KeyCode.Keypad8,
      0.8f
    },
    {
      KeyCode.Keypad9,
      0.9f
    }
  };
  protected bool _rotatingLasers;

  public virtual void Start()
  {
    for (int groupId = 0; groupId < 20; ++groupId)
      this._beatmapEventDataBoxGroupLists[groupId] = new BeatmapEventDataBoxGroupList(groupId, this._beatmapData, (IBeatToTimeConvertor) new EventsTestGameplayManager.MockBeatToTimeConvertor(60f))
      {
        updateBeatmapDataOnInsert = true
      };
  }

  public virtual void Update()
  {
    foreach (KeyValuePair<KeyCode, BasicBeatmapEventType> eventTypeBinding in this._beatmapEventTypeBindings)
    {
      if (Input.GetKeyDown(eventTypeBinding.Key))
        this._basicBeatmapEventType = eventTypeBinding.Value;
    }
    if (Input.GetKey(KeyCode.LeftControl))
    {
      foreach (KeyValuePair<KeyCode, float> floatValuesBinding in this._floatValuesBindings)
      {
        if (Input.GetKeyDown(floatValuesBinding.Key))
          this._floatValue = floatValuesBinding.Value;
      }
    }
    else
    {
      foreach (KeyValuePair<KeyCode, int> beatmapValuesBinding in this._beatmapValuesBindings)
      {
        if (Input.GetKeyDown(beatmapValuesBinding.Key))
          this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, this._basicBeatmapEventType, beatmapValuesBinding.Value, this._floatValue));
      }
    }
    if (Input.GetKey(KeyCode.B))
    {
      foreach (KeyValuePair<KeyCode, int> intBinding in this._intBindings)
      {
        if (Input.GetKeyDown(intBinding.Key))
          this.AddEventsForLightGroup(intBinding.Value);
      }
    }
    if (Input.GetKey(KeyCode.N))
    {
      foreach (KeyValuePair<KeyCode, int> intBinding in this._intBindings)
      {
        if (Input.GetKeyDown(intBinding.Key))
          this.AddToggleEventsForLightGroup(intBinding.Value, EnvironmentColorType.Color0);
      }
    }
    if (Input.GetKey(KeyCode.M))
    {
      foreach (KeyValuePair<KeyCode, int> intBinding in this._intBindings)
      {
        if (Input.GetKeyDown(intBinding.Key))
          this.AddToggleEventsForLightGroup(intBinding.Value, EnvironmentColorType.Color1);
      }
    }
    if (Input.GetKey(KeyCode.V))
    {
      foreach (KeyValuePair<KeyCode, int> intBinding in this._intBindings)
      {
        if (Input.GetKeyDown(intBinding.Key))
          this.AddInstantToggleEventsForLightGroup(intBinding.Value);
      }
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      int num = 0;
      this._rotatingLasers = !this._rotatingLasers;
      if (this._rotatingLasers)
        num = 4;
      this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event12, num, 1f));
      this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event13, num, 1f));
    }
    if (this._moveTime)
      this._beatmapCallbacksController.ManualUpdate(this._audioTimeSource.songTime);
    if (!this._spawnTestBox)
      return;
    this._spawnTestBox = false;
    this.AddTestBox();
  }

  public virtual void AddEventsForLightGroup(int lightGroupId)
  {
    LightGroup lightGroup = UnityEngine.Object.FindObjectOfType<LightColorGroupEffectManager>().lightGroups.ToList<LightGroup>().Find((Predicate<LightGroup>) (x => x.groupId == lightGroupId));
    if ((UnityEngine.Object) lightGroup == (UnityEngine.Object) null)
      return;
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup1 = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightColorBeatmapEventDataBox[1]
    {
      new LightColorBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 1f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[4]
      {
        new LightColorBaseData(0.0f, BeatmapEventTransitionType.Interpolate, EnvironmentColorType.Color0, 0.0f, 0),
        new LightColorBaseData(1f, BeatmapEventTransitionType.Interpolate, EnvironmentColorType.Color0, 1f, 0),
        new LightColorBaseData(2f, BeatmapEventTransitionType.Instant, EnvironmentColorType.Color0, 1f, 0),
        new LightColorBaseData(3f, BeatmapEventTransitionType.Interpolate, EnvironmentColorType.Color0, 1f, 0)
      })
    });
    this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup1);
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup2 = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightRotationBeatmapEventDataBox[2]
    {
      new LightRotationBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 3f, BeatmapEventDataBox.DistributionParamType.Wave, LightAxis.X, false, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[2]
      {
        new LightRotationBaseData(0.0f, false, EaseType.None, -90f, 0, LightRotationDirection.Automatic),
        new LightRotationBaseData(1f, false, EaseType.InOutQuad, 0.0f, 0, LightRotationDirection.Automatic)
      }),
      new LightRotationBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 1f, BeatmapEventDataBox.DistributionParamType.Wave, LightAxis.Y, false, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[2]
      {
        new LightRotationBaseData(0.0f, false, EaseType.None, -90f, 0, LightRotationDirection.Automatic),
        new LightRotationBaseData(3f, false, EaseType.InOutQuad, 0.0f, 0, LightRotationDirection.Automatic)
      })
    });
    this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup2);
  }

  public virtual void AddInstantToggleEventsForLightGroup(int lightGroupId)
  {
    LightGroup lightGroup = ((IEnumerable<LightGroup>) UnityEngine.Object.FindObjectsOfType<LightGroup>()).ToList<LightGroup>().Find((Predicate<LightGroup>) (x => x.groupId == lightGroupId));
    if ((UnityEngine.Object) lightGroup == (UnityEngine.Object) null)
      return;
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightColorBeatmapEventDataBox[1]
    {
      new LightColorBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 1f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[1]
      {
        new LightColorBaseData(0.0f, BeatmapEventTransitionType.Instant, EnvironmentColorType.Color0, this.groupState[lightGroupId] ? 0.0f : 1f, 0)
      })
    });
    this.groupState[lightGroupId] = !this.groupState[lightGroupId];
    this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup);
  }

  public virtual void AddToggleEventsForLightGroup(int lightGroupId, EnvironmentColorType color)
  {
    LightGroup lightGroup = UnityEngine.Object.FindObjectOfType<LightColorGroupEffectManager>().lightGroups.ToList<LightGroup>().Find((Predicate<LightGroup>) (x => x.groupId == lightGroupId));
    if ((UnityEngine.Object) lightGroup == (UnityEngine.Object) null)
      return;
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightColorBeatmapEventDataBox[1]
    {
      new LightColorBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 1f, BeatmapEventDataBox.DistributionParamType.Wave, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[3]
      {
        new LightColorBaseData(0.0f, BeatmapEventTransitionType.Interpolate, color, this.groupState[lightGroupId] ? 1f : 0.0f, 0),
        new LightColorBaseData(1f, BeatmapEventTransitionType.Interpolate, color, this.groupState[lightGroupId] ? 0.0f : 1f, 0),
        new LightColorBaseData(2f, BeatmapEventTransitionType.Instant, color, this.groupState[lightGroupId] ? 0.0f : 1f, 0)
      })
    });
    this.groupState[lightGroupId] = !this.groupState[lightGroupId];
    this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup);
  }

  public virtual void AddTestBox()
  {
    foreach (LightGroup lightGroup in (IEnumerable<LightGroup>) UnityEngine.Object.FindObjectOfType<LightColorGroupEffectManager>().lightGroups)
    {
      BeatmapEventDataBoxGroup beatmapEventDataBoxGroup1 = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightColorBeatmapEventDataBox[1]
      {
        new LightColorBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 2f, BeatmapEventDataBox.DistributionParamType.Step, 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, true, EaseType.Linear, (IReadOnlyList<LightColorBaseData>) new LightColorBaseData[2]
        {
          new LightColorBaseData(0.0f, BeatmapEventTransitionType.Instant, EnvironmentColorType.Color0, 0.0f, 0),
          new LightColorBaseData(2f, BeatmapEventTransitionType.Instant, EnvironmentColorType.Color1, 1f, 0)
        })
      });
      this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup1);
      BeatmapEventDataBoxGroup beatmapEventDataBoxGroup2 = new BeatmapEventDataBoxGroup(this._audioTimeSource.songTime, (IReadOnlyCollection<BeatmapEventDataBox>) new LightRotationBeatmapEventDataBox[1]
      {
        new LightRotationBeatmapEventDataBox(new IndexFilter(0, lightGroup.numberOfElements - 1, lightGroup.numberOfElements, IndexFilter.IndexFilterRandomType.NoRandom, 0, 1, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None), 0.0f, BeatmapEventDataBox.DistributionParamType.Wave, LightAxis.X, false, 360f, BeatmapEventDataBox.DistributionParamType.Wave, false, EaseType.Linear, (IReadOnlyList<LightRotationBaseData>) new LightRotationBaseData[2]
        {
          new LightRotationBaseData(0.0f, false, EaseType.None, 170f, 0, LightRotationDirection.Clockwise),
          new LightRotationBaseData(3f, false, EaseType.InOutQuad, 170f, 0, LightRotationDirection.Clockwise)
        })
      });
      this._beatmapEventDataBoxGroupLists[lightGroup.groupId].Insert(beatmapEventDataBoxGroup2);
    }
  }

  public class MockBeatToTimeConvertor : IBeatToTimeConvertor
  {
    protected readonly float _bpm;

    public MockBeatToTimeConvertor(float bpm) => this._bpm = bpm;

    public virtual float ConvertBeatToTime(float beat) => (float) ((double) beat / (double) this._bpm * 60.0);
  }
}
