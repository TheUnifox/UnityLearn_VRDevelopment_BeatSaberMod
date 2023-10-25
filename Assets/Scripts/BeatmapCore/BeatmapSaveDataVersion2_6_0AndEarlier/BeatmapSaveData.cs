using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapSaveDataVersion2_6_0AndEarlier
{
    [Serializable]
    public class BeatmapSaveData
    {
        public string version { get { return this._version; }}
        public List<BeatmapSaveData.EventData> events { get { return this._events; }}
        public List<BeatmapSaveData.NoteData> notes
        {
            get
            {
                return this._notes;
            }
        }

        // Token: 0x170000A0 RID: 160
        // (get) Token: 0x060001C6 RID: 454 RVA: 0x00005E17 File Offset: 0x00004017
        public List<BeatmapSaveData.SliderData> sliders
        {
            get
            {
                return this._sliders;
            }
        }

        // Token: 0x170000A1 RID: 161
        // (get) Token: 0x060001C7 RID: 455 RVA: 0x00005E1F File Offset: 0x0000401F
        public List<BeatmapSaveData.WaypointData> waypoints
        {
            get
            {
                return this._waypoints;
            }
        }

        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x060001C8 RID: 456 RVA: 0x00005E27 File Offset: 0x00004027
        public List<BeatmapSaveData.ObstacleData> obstacles
        {
            get
            {
                return this._obstacles;
            }
        }

        // Token: 0x170000A3 RID: 163
        // (get) Token: 0x060001C9 RID: 457 RVA: 0x00005E2F File Offset: 0x0000402F
        public BeatmapSaveData.SpecialEventKeywordFiltersData specialEventsKeywordFilters
        {
            get
            {
                return this._specialEventsKeywordFilters;
            }
        }

        // Token: 0x060001CA RID: 458 RVA: 0x00005E37 File Offset: 0x00004037
        public BeatmapSaveData(List<BeatmapSaveData.EventData> events, List<BeatmapSaveData.NoteData> notes, List<BeatmapSaveData.SliderData> sliders, List<BeatmapSaveData.WaypointData> waypoints, List<BeatmapSaveData.ObstacleData> obstacles, BeatmapSaveData.SpecialEventKeywordFiltersData specialEventsKeywordFilters)
        {
            this._version = "2.6.0";
            this._events = events;
            this._notes = notes;
            this._sliders = sliders;
            this._waypoints = waypoints;
            this._obstacles = obstacles;
            this._specialEventsKeywordFilters = specialEventsKeywordFilters;
        }

        // Token: 0x060001CB RID: 459 RVA: 0x00004EE7 File Offset: 0x000030E7
        public virtual string SerializeToJSONString()
        {
            return JsonUtility.ToJson(this);
        }

        // Token: 0x060001CC RID: 460 RVA: 0x00005E78 File Offset: 0x00004078
        public static BeatmapSaveData DeserializeFromJSONString(string stringData)
        {
            BeatmapSaveData beatmapSaveData = null;
            try
            {
                beatmapSaveData = JsonUtility.FromJson<BeatmapSaveData>(stringData);
            }
            catch (Exception arg)
            {
                Debug.LogWarning(string.Format("Exception in BeatmapSaveData json loading:\n{0}", arg));
            }
            if (beatmapSaveData != null)
            {
                if (!string.IsNullOrEmpty(beatmapSaveData.version))
                {
                    Version version = new Version(beatmapSaveData.version);
                    Version value = new Version("2.5.0");
                    if (version.CompareTo(value) < 0)
                    {
                        BeatmapSaveData.ConvertBeatmapSaveDataPreV2_5_0(beatmapSaveData);
                    }
                }
                else
                {
                    BeatmapSaveData.ConvertBeatmapSaveDataPreV2_5_0(beatmapSaveData);
                }
            }
            return beatmapSaveData;
        }

        // Token: 0x060001CD RID: 461 RVA: 0x00005F04 File Offset: 0x00004104
        public static void ConvertBeatmapSaveDataPreV2_5_0(BeatmapSaveData beatmapSaveData)
        {
            List<BeatmapSaveData.EventData> list = new List<BeatmapSaveData.EventData>(beatmapSaveData.events.Count);
            foreach (BeatmapSaveData.EventData eventData in beatmapSaveData.events)
            {
                EventData eventDataToReturn = eventData;
                if (eventData.type == BeatmapSaveData.BeatmapEventType.Event10)
                {
                    eventDataToReturn = new BeatmapSaveData.EventData(eventData.time, BeatmapSaveData.BeatmapEventType.BpmChange, eventData.value, eventData.floatValue);
                }
                if (eventData.type == BeatmapSaveData.BeatmapEventType.BpmChange)
                {
                    if (eventData.value != 0)
                    {
                        eventDataToReturn = new BeatmapSaveData.EventData(eventData.time, eventData.type, eventData.value, (float)eventData.value);
                    }
                }
                else
                {
                    eventDataToReturn = new BeatmapSaveData.EventData(eventData.time, eventData.type, eventData.value, 1f);
                }
                list.Add(eventDataToReturn);
            }
            beatmapSaveData._events = list;
        }

        // Token: 0x060001CE RID: 462 RVA: 0x00005FEC File Offset: 0x000041EC
        public static HashSet<BeatmapSaveData.BeatmapEventType> GetSpecialEventTypes()
        {
            return new HashSet<BeatmapSaveData.BeatmapEventType>
            {
                BeatmapSaveData.BeatmapEventType.Special0,
                BeatmapSaveData.BeatmapEventType.Special1,
                BeatmapSaveData.BeatmapEventType.Special2,
                BeatmapSaveData.BeatmapEventType.Special3
            };
        }

        // Token: 0x040001A0 RID: 416
        protected const string kCurrentVersion = "2.6.0";

        // Token: 0x040001A1 RID: 417
        [SerializeField]
        protected string _version;

        // Token: 0x040001A2 RID: 418
        [SerializeField]
        protected List<BeatmapSaveData.EventData> _events;

        // Token: 0x040001A3 RID: 419
        [SerializeField]
        protected List<BeatmapSaveData.NoteData> _notes;

        // Token: 0x040001A4 RID: 420
        [SerializeField]
        protected List<BeatmapSaveData.SliderData> _sliders;

        // Token: 0x040001A5 RID: 421
        [SerializeField]
        protected List<BeatmapSaveData.WaypointData> _waypoints;

        // Token: 0x040001A6 RID: 422
        [SerializeField]
        protected List<BeatmapSaveData.ObstacleData> _obstacles;

        // Token: 0x040001A7 RID: 423
        [SerializeField]
        protected BeatmapSaveData.SpecialEventKeywordFiltersData _specialEventsKeywordFilters;

        // Token: 0x040001A8 RID: 424
        public const BeatmapSaveData.BeatmapEventType kColorBoostEventType = BeatmapSaveData.BeatmapEventType.Event5;

        // Token: 0x040001A9 RID: 425
        public const BeatmapSaveData.BeatmapEventType kLegacyBPMEventType = BeatmapSaveData.BeatmapEventType.Event10;

        // Token: 0x040001AA RID: 426
        public const BeatmapSaveData.BeatmapEventType kBPMEventType = BeatmapSaveData.BeatmapEventType.BpmChange;

        // Token: 0x040001AB RID: 427
        public const BeatmapSaveData.BeatmapEventType kEarlyRotationEventType = BeatmapSaveData.BeatmapEventType.Event14;

        // Token: 0x040001AC RID: 428
        public const BeatmapSaveData.BeatmapEventType kLateRotationEventType = BeatmapSaveData.BeatmapEventType.Event15;

        // Token: 0x02000069 RID: 105
        public enum BeatmapEventType
        {
            // Token: 0x040001AE RID: 430
            Event0,
            // Token: 0x040001AF RID: 431
            Event1,
            // Token: 0x040001B0 RID: 432
            Event2,
            // Token: 0x040001B1 RID: 433
            Event3,
            // Token: 0x040001B2 RID: 434
            Event4,
            // Token: 0x040001B3 RID: 435
            Event5,
            // Token: 0x040001B4 RID: 436
            Event6,
            // Token: 0x040001B5 RID: 437
            Event7,
            // Token: 0x040001B6 RID: 438
            Event8,
            // Token: 0x040001B7 RID: 439
            Event9,
            // Token: 0x040001B8 RID: 440
            Event10,
            // Token: 0x040001B9 RID: 441
            Event11,
            // Token: 0x040001BA RID: 442
            Event12,
            // Token: 0x040001BB RID: 443
            Event13,
            // Token: 0x040001BC RID: 444
            Event14,
            // Token: 0x040001BD RID: 445
            Event15,
            // Token: 0x040001BE RID: 446
            Event16,
            // Token: 0x040001BF RID: 447
            Event17,
            // Token: 0x040001C0 RID: 448
            VoidEvent = -1,
            // Token: 0x040001C1 RID: 449
            Special0 = 40,
            // Token: 0x040001C2 RID: 450
            Special1,
            // Token: 0x040001C3 RID: 451
            Special2,
            // Token: 0x040001C4 RID: 452
            Special3,
            // Token: 0x040001C5 RID: 453
            BpmChange = 100
        }

        // Token: 0x0200006A RID: 106
        [Serializable]
        public class EventData : BeatmapSaveDataItem
        {
            // Token: 0x170000A4 RID: 164
            // (get) Token: 0x060001CF RID: 463 RVA: 0x00006017 File Offset: 0x00004217
            public override float time
            {
                get
                {
                    return this._time;
                }
            }

            // Token: 0x170000A5 RID: 165
            // (get) Token: 0x060001D0 RID: 464 RVA: 0x0000601F File Offset: 0x0000421F
            public BeatmapSaveData.BeatmapEventType type
            {
                get
                {
                    return this._type;
                }
            }

            // Token: 0x170000A6 RID: 166
            // (get) Token: 0x060001D1 RID: 465 RVA: 0x00006027 File Offset: 0x00004227
            public int value
            {
                get
                {
                    return this._value;
                }
            }

            // Token: 0x170000A7 RID: 167
            // (get) Token: 0x060001D2 RID: 466 RVA: 0x0000602F File Offset: 0x0000422F
            public float floatValue
            {
                get
                {
                    return this._floatValue;
                }
            }

            // Token: 0x060001D3 RID: 467 RVA: 0x00006037 File Offset: 0x00004237
            public EventData(float time, BeatmapSaveData.BeatmapEventType type, int value, float floatValue)
            {
                this._time = time;
                this._type = type;
                this._value = value;
                this._floatValue = floatValue;
            }

            // Token: 0x040001C6 RID: 454
            [SerializeField]
            protected float _time;

            // Token: 0x040001C7 RID: 455
            [SerializeField]
            protected BeatmapSaveData.BeatmapEventType _type;

            // Token: 0x040001C8 RID: 456
            [SerializeField]
            protected int _value;

            // Token: 0x040001C9 RID: 457
            [SerializeField]
            protected float _floatValue;
        }

        // Token: 0x0200006B RID: 107
        public enum BeatmapObjectType
        {
            // Token: 0x040001CB RID: 459
            Note,
            // Token: 0x040001CC RID: 460
            Obstacle = 2,
            // Token: 0x040001CD RID: 461
            Slider
        }

        // Token: 0x0200006C RID: 108
        public enum NoteType
        {
            // Token: 0x040001CF RID: 463
            NoteA,
            // Token: 0x040001D0 RID: 464
            NoteB,
            // Token: 0x040001D1 RID: 465
            GhostNote,
            // Token: 0x040001D2 RID: 466
            Bomb,
            // Token: 0x040001D3 RID: 467
            None = -1
        }

        // Token: 0x0200006D RID: 109
        public enum ColorType
        {
            // Token: 0x040001D5 RID: 469
            ColorA,
            // Token: 0x040001D6 RID: 470
            ColorB,
            // Token: 0x040001D7 RID: 471
            None = -1
        }

        // Token: 0x0200006E RID: 110
        public enum SliderType
        {
            // Token: 0x040001D9 RID: 473
            Normal,
            // Token: 0x040001DA RID: 474
            Burst
        }

        // Token: 0x0200006F RID: 111
        [Serializable]
        public class NoteData : BeatmapSaveDataItem
        {
            // Token: 0x170000A8 RID: 168
            // (get) Token: 0x060001D4 RID: 468 RVA: 0x0000605C File Offset: 0x0000425C
            public override float time
            {
                get
                {
                    return this._time;
                }
            }

            // Token: 0x170000A9 RID: 169
            // (get) Token: 0x060001D5 RID: 469 RVA: 0x00006064 File Offset: 0x00004264
            public int lineIndex
            {
                get
                {
                    return this._lineIndex;
                }
            }

            // Token: 0x170000AA RID: 170
            // (get) Token: 0x060001D6 RID: 470 RVA: 0x0000606C File Offset: 0x0000426C
            public NoteLineLayer lineLayer
            {
                get
                {
                    return this._lineLayer;
                }
            }

            // Token: 0x170000AB RID: 171
            // (get) Token: 0x060001D7 RID: 471 RVA: 0x00006074 File Offset: 0x00004274
            public BeatmapSaveData.NoteType type
            {
                get
                {
                    return this._type;
                }
            }

            // Token: 0x170000AC RID: 172
            // (get) Token: 0x060001D8 RID: 472 RVA: 0x0000607C File Offset: 0x0000427C
            public NoteCutDirection cutDirection
            {
                get
                {
                    return this._cutDirection;
                }
            }

            // Token: 0x060001D9 RID: 473 RVA: 0x00006084 File Offset: 0x00004284
            public NoteData(float time, int lineIndex, NoteLineLayer lineLayer, BeatmapSaveData.NoteType type, NoteCutDirection cutDirection)
            {
                this._time = time;
                this._lineIndex = lineIndex;
                this._lineLayer = lineLayer;
                this._type = type;
                this._cutDirection = cutDirection;
            }

            // Token: 0x040001DB RID: 475
            [SerializeField]
            protected float _time;

            // Token: 0x040001DC RID: 476
            [SerializeField]
            protected int _lineIndex;

            // Token: 0x040001DD RID: 477
            [SerializeField]
            protected NoteLineLayer _lineLayer;

            // Token: 0x040001DE RID: 478
            [SerializeField]
            protected BeatmapSaveData.NoteType _type;

            // Token: 0x040001DF RID: 479
            [SerializeField]
            protected NoteCutDirection _cutDirection;
        }

        // Token: 0x02000070 RID: 112
        [Serializable]
        public class WaypointData : BeatmapSaveDataItem
        {
            // Token: 0x170000AD RID: 173
            // (get) Token: 0x060001DA RID: 474 RVA: 0x000060B1 File Offset: 0x000042B1
            public override float time
            {
                get
                {
                    return this._time;
                }
            }

            // Token: 0x170000AE RID: 174
            // (get) Token: 0x060001DB RID: 475 RVA: 0x000060B9 File Offset: 0x000042B9
            public int lineIndex
            {
                get
                {
                    return this._lineIndex;
                }
            }

            // Token: 0x170000AF RID: 175
            // (get) Token: 0x060001DC RID: 476 RVA: 0x000060C1 File Offset: 0x000042C1
            public NoteLineLayer lineLayer
            {
                get
                {
                    return this._lineLayer;
                }
            }

            // Token: 0x170000B0 RID: 176
            // (get) Token: 0x060001DD RID: 477 RVA: 0x000060C9 File Offset: 0x000042C9
            public OffsetDirection offsetDirection
            {
                get
                {
                    return this._offsetDirection;
                }
            }

            // Token: 0x060001DE RID: 478 RVA: 0x000060D1 File Offset: 0x000042D1
            public WaypointData(float time, int lineIndex, NoteLineLayer lineLayer, OffsetDirection offsetDirection)
            {
                this._time = time;
                this._lineIndex = lineIndex;
                this._lineLayer = lineLayer;
                this._offsetDirection = offsetDirection;
            }

            // Token: 0x040001E0 RID: 480
            [SerializeField]
            protected float _time;

            // Token: 0x040001E1 RID: 481
            [SerializeField]
            protected int _lineIndex;

            // Token: 0x040001E2 RID: 482
            [SerializeField]
            protected NoteLineLayer _lineLayer;

            // Token: 0x040001E3 RID: 483
            [SerializeField]
            protected OffsetDirection _offsetDirection;
        }

        // Token: 0x02000071 RID: 113
        [Serializable]
        public class SliderData : BeatmapSaveDataItem
        {
            // Token: 0x170000B1 RID: 177
            // (get) Token: 0x060001DF RID: 479 RVA: 0x000060F6 File Offset: 0x000042F6
            public override float time
            {
                get
                {
                    return this._headTime;
                }
            }

            // Token: 0x170000B2 RID: 178
            // (get) Token: 0x060001E0 RID: 480 RVA: 0x000060FE File Offset: 0x000042FE
            public BeatmapSaveData.ColorType colorType
            {
                get
                {
                    return this._colorType;
                }
            }

            // Token: 0x170000B3 RID: 179
            // (get) Token: 0x060001E1 RID: 481 RVA: 0x00006106 File Offset: 0x00004306
            public int headLineIndex
            {
                get
                {
                    return this._headLineIndex;
                }
            }

            // Token: 0x170000B4 RID: 180
            // (get) Token: 0x060001E2 RID: 482 RVA: 0x0000610E File Offset: 0x0000430E
            public NoteLineLayer headLineLayer
            {
                get
                {
                    return this._headLineLayer;
                }
            }

            // Token: 0x170000B5 RID: 181
            // (get) Token: 0x060001E3 RID: 483 RVA: 0x00006116 File Offset: 0x00004316
            public float headControlPointLengthMultiplier
            {
                get
                {
                    return this._headControlPointLengthMultiplier;
                }
            }

            // Token: 0x170000B6 RID: 182
            // (get) Token: 0x060001E4 RID: 484 RVA: 0x0000611E File Offset: 0x0000431E
            public NoteCutDirection headCutDirection
            {
                get
                {
                    return this._headCutDirection;
                }
            }

            // Token: 0x170000B7 RID: 183
            // (get) Token: 0x060001E5 RID: 485 RVA: 0x00006126 File Offset: 0x00004326
            public float tailTime
            {
                get
                {
                    return this._tailTime;
                }
            }

            // Token: 0x170000B8 RID: 184
            // (get) Token: 0x060001E6 RID: 486 RVA: 0x0000612E File Offset: 0x0000432E
            public int tailLineIndex
            {
                get
                {
                    return this._tailLineIndex;
                }
            }

            // Token: 0x170000B9 RID: 185
            // (get) Token: 0x060001E7 RID: 487 RVA: 0x00006136 File Offset: 0x00004336
            public NoteLineLayer tailLineLayer
            {
                get
                {
                    return this._tailLineLayer;
                }
            }

            // Token: 0x170000BA RID: 186
            // (get) Token: 0x060001E8 RID: 488 RVA: 0x0000613E File Offset: 0x0000433E
            public float tailControlPointLengthMultiplier
            {
                get
                {
                    return this._tailControlPointLengthMultiplier;
                }
            }

            // Token: 0x170000BB RID: 187
            // (get) Token: 0x060001E9 RID: 489 RVA: 0x00006146 File Offset: 0x00004346
            public NoteCutDirection tailCutDirection
            {
                get
                {
                    return this._tailCutDirection;
                }
            }

            // Token: 0x170000BC RID: 188
            // (get) Token: 0x060001EA RID: 490 RVA: 0x0000614E File Offset: 0x0000434E
            public SliderMidAnchorMode sliderMidAnchorMode
            {
                get
                {
                    return this._sliderMidAnchorMode;
                }
            }

            // Token: 0x060001EB RID: 491 RVA: 0x00006158 File Offset: 0x00004358
            public SliderData(BeatmapSaveData.ColorType colorType, float headTime, int headLineIndex, NoteLineLayer headLineLayer, float headControlPointLengthMultiplier, NoteCutDirection headCutDirection, float tailTime, int tailLineIndex, NoteLineLayer tailLineLayer, float tailControlPointLengthMultiplier, NoteCutDirection tailCutDirection, SliderMidAnchorMode sliderMidAnchorMode)
            {
                this._colorType = colorType;
                this._headTime = headTime;
                this._headLineIndex = headLineIndex;
                this._headLineLayer = headLineLayer;
                this._headControlPointLengthMultiplier = headControlPointLengthMultiplier;
                this._headCutDirection = headCutDirection;
                this._tailTime = tailTime;
                this._tailLineIndex = tailLineIndex;
                this._tailLineLayer = tailLineLayer;
                this._tailControlPointLengthMultiplier = tailControlPointLengthMultiplier;
                this._tailCutDirection = tailCutDirection;
                this._sliderMidAnchorMode = sliderMidAnchorMode;
            }

            // Token: 0x040001E4 RID: 484
            [SerializeField]
            protected BeatmapSaveData.ColorType _colorType;

            // Token: 0x040001E5 RID: 485
            [SerializeField]
            protected float _headTime;

            // Token: 0x040001E6 RID: 486
            [SerializeField]
            protected int _headLineIndex;

            // Token: 0x040001E7 RID: 487
            [SerializeField]
            protected NoteLineLayer _headLineLayer;

            // Token: 0x040001E8 RID: 488
            [SerializeField]
            protected float _headControlPointLengthMultiplier;

            // Token: 0x040001E9 RID: 489
            [SerializeField]
            protected NoteCutDirection _headCutDirection;

            // Token: 0x040001EA RID: 490
            [SerializeField]
            protected float _tailTime;

            // Token: 0x040001EB RID: 491
            [SerializeField]
            protected int _tailLineIndex;

            // Token: 0x040001EC RID: 492
            [SerializeField]
            protected NoteLineLayer _tailLineLayer;

            // Token: 0x040001ED RID: 493
            [SerializeField]
            protected float _tailControlPointLengthMultiplier;

            // Token: 0x040001EE RID: 494
            [SerializeField]
            protected NoteCutDirection _tailCutDirection;

            // Token: 0x040001EF RID: 495
            [SerializeField]
            protected SliderMidAnchorMode _sliderMidAnchorMode;
        }

        // Token: 0x02000072 RID: 114
        [Serializable]
        public class ObstacleData : BeatmapSaveDataItem
        {
            // Token: 0x170000BD RID: 189
            // (get) Token: 0x060001EC RID: 492 RVA: 0x000061C8 File Offset: 0x000043C8
            public override float time
            {
                get
                {
                    return this._time;
                }
            }

            // Token: 0x170000BE RID: 190
            // (get) Token: 0x060001ED RID: 493 RVA: 0x000061D0 File Offset: 0x000043D0
            public int lineIndex
            {
                get
                {
                    return this._lineIndex;
                }
            }

            // Token: 0x170000BF RID: 191
            // (get) Token: 0x060001EE RID: 494 RVA: 0x000061D8 File Offset: 0x000043D8
            public BeatmapSaveData.ObstacleType type
            {
                get
                {
                    return this._type;
                }
            }

            // Token: 0x170000C0 RID: 192
            // (get) Token: 0x060001EF RID: 495 RVA: 0x000061E0 File Offset: 0x000043E0
            public float duration
            {
                get
                {
                    return this._duration;
                }
            }

            // Token: 0x170000C1 RID: 193
            // (get) Token: 0x060001F0 RID: 496 RVA: 0x000061E8 File Offset: 0x000043E8
            public int width
            {
                get
                {
                    return this._width;
                }
            }

            // Token: 0x060001F1 RID: 497 RVA: 0x000061F0 File Offset: 0x000043F0
            public ObstacleData(float time, int lineIndex, BeatmapSaveData.ObstacleType type, float duration, int width)
            {
                this._time = time;
                this._lineIndex = lineIndex;
                this._type = type;
                this._duration = duration;
                this._width = width;
            }

            // Token: 0x040001F0 RID: 496
            [SerializeField]
            protected float _time;

            // Token: 0x040001F1 RID: 497
            [SerializeField]
            protected int _lineIndex;

            // Token: 0x040001F2 RID: 498
            [SerializeField]
            protected BeatmapSaveData.ObstacleType _type;

            // Token: 0x040001F3 RID: 499
            [SerializeField]
            protected float _duration;

            // Token: 0x040001F4 RID: 500
            [SerializeField]
            protected int _width;
        }

        // Token: 0x02000073 RID: 115
        public enum ObstacleType
        {
            // Token: 0x040001F6 RID: 502
            FullHeight,
            // Token: 0x040001F7 RID: 503
            Top,
            // Token: 0x040001F8 RID: 504
            Free
        }

        // Token: 0x02000074 RID: 116
        [Serializable]
        public class SpecialEventKeywordFiltersData
        {
            // Token: 0x170000C2 RID: 194
            // (get) Token: 0x060001F2 RID: 498 RVA: 0x0000621D File Offset: 0x0000441D
            public List<BeatmapSaveData.SpecialEventsForKeyword> keywords
            {
                get
                {
                    return this._keywords;
                }
            }

            // Token: 0x060001F3 RID: 499 RVA: 0x00006225 File Offset: 0x00004425
            public SpecialEventKeywordFiltersData(List<BeatmapSaveData.SpecialEventsForKeyword> keywords)
            {
                this._keywords = keywords;
            }

            // Token: 0x040001F9 RID: 505
            [SerializeField]
            protected List<BeatmapSaveData.SpecialEventsForKeyword> _keywords;
        }

        // Token: 0x02000075 RID: 117
        [Serializable]
        public class SpecialEventsForKeyword
        {
            // Token: 0x170000C3 RID: 195
            // (get) Token: 0x060001F4 RID: 500 RVA: 0x00006234 File Offset: 0x00004434
            public string keyword
            {
                get
                {
                    return this._keyword;
                }
            }

            // Token: 0x170000C4 RID: 196
            // (get) Token: 0x060001F5 RID: 501 RVA: 0x0000623C File Offset: 0x0000443C
            public List<BeatmapSaveData.BeatmapEventType> specialEvents
            {
                get
                {
                    return this._specialEvents;
                }
            }

            // Token: 0x060001F6 RID: 502 RVA: 0x00006244 File Offset: 0x00004444
            public SpecialEventsForKeyword(string keyword, List<BeatmapSaveData.BeatmapEventType> specialEvents)
            {
                this._keyword = keyword;
                this._specialEvents = specialEvents;
            }

            // Token: 0x040001FA RID: 506
            [SerializeField]
            protected string _keyword;

            // Token: 0x040001FB RID: 507
            [SerializeField]
            protected List<BeatmapSaveData.BeatmapEventType> _specialEvents;
        }
    }
}