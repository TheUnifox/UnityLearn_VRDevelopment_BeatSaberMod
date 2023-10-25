using System;
using System.Collections.Generic;
using BeatmapSaveDataVersion2_6_0AndEarlier;
using UnityEngine;

namespace BeatmapSaveDataVersion3
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class BeatmapSaveData
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00004E54 File Offset: 0x00003054
		public BeatmapSaveData(List<BeatmapSaveData.BpmChangeEventData> bpmEvents, List<BeatmapSaveData.RotationEventData> rotationEvents, List<BeatmapSaveData.ColorNoteData> colorNotes, List<BeatmapSaveData.BombNoteData> bombNotes, List<BeatmapSaveData.ObstacleData> obstacles, List<BeatmapSaveData.SliderData> sliders, List<BeatmapSaveData.BurstSliderData> burstSliders, List<BeatmapSaveData.WaypointData> waypoints, List<BeatmapSaveData.BasicEventData> basicBeatmapEvents, List<BeatmapSaveData.ColorBoostEventData> colorBoostBeatmapEvents, List<BeatmapSaveData.LightColorEventBoxGroup> lightColorEventBoxGroups, List<BeatmapSaveData.LightRotationEventBoxGroup> lightRotationEventBoxGroups, List<BeatmapSaveData.LightTranslationEventBoxGroup> lightTranslationEventBoxGroups, BeatmapSaveData.BasicEventTypesWithKeywords basicEventTypesWithKeywords, bool useNormalEventsAsCompatibleEvents)
		{
			this.version = "3.2.0";
			this.bpmEvents = bpmEvents;
			this.rotationEvents = rotationEvents;
			this.colorNotes = colorNotes;
			this.bombNotes = bombNotes;
			this.obstacles = obstacles;
			this.sliders = sliders;
			this.burstSliders = burstSliders;
			this.waypoints = waypoints;
			this.basicBeatmapEvents = basicBeatmapEvents;
			this.colorBoostBeatmapEvents = colorBoostBeatmapEvents;
			this.lightColorEventBoxGroups = lightColorEventBoxGroups;
			this.lightRotationEventBoxGroups = lightRotationEventBoxGroups;
			this.lightTranslationEventBoxGroups = lightTranslationEventBoxGroups;
			this.basicEventTypesWithKeywords = basicEventTypesWithKeywords;
			this.useNormalEventsAsCompatibleEvents = useNormalEventsAsCompatibleEvents;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004EE7 File Offset: 0x000030E7
		public virtual string SerializeToJSONString()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004EF0 File Offset: 0x000030F0
		public static BeatmapSaveData DeserializeFromJSONString(string stringData)
		{
			if (BeatmapSaveDataHelpers.GetVersion(stringData).CompareTo(BeatmapSaveData.version2_6_0) <= 0)
			{
				return BeatmapSaveData.ConvertBeatmapSaveData(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.DeserializeFromJSONString(stringData));
			}
			BeatmapSaveData result = null;
			try
			{
				result = JsonUtility.FromJson<BeatmapSaveData>(stringData);
			}
			catch (Exception arg)
			{
				Debug.LogWarning(string.Format("Exception in BeatmapSaveData json loading:\n{0}", arg));
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004F4C File Offset: 0x0000314C
		public static BeatmapSaveData ConvertBeatmapSaveData(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData beatmapSaveData)
		{
			List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.EventData> events = beatmapSaveData.events;
			List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData> notes = beatmapSaveData.notes;
			List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleData> list = beatmapSaveData.obstacles;
			List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SliderData> list2 = beatmapSaveData.sliders;
			List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.WaypointData> list3 = beatmapSaveData.waypoints;
			if (!BeatmapSaveData.BeatmapSaveDataAreSorted(events))
			{
				events.Sort();
			}
			if (!BeatmapSaveData.BeatmapSaveDataAreSorted(notes))
			{
				notes.Sort();
			}
			if (!BeatmapSaveData.BeatmapSaveDataAreSorted(list2))
			{
				list2.Sort();
			}
			if (!BeatmapSaveData.BeatmapSaveDataAreSorted(list3))
			{
				list3.Sort();
			}
			if (!BeatmapSaveData.BeatmapSaveDataAreSorted(list))
			{
				list.Sort();
			}
			List<BeatmapSaveData.ColorNoteData> list4 = new List<BeatmapSaveData.ColorNoteData>(notes.Count);
			List<BeatmapSaveData.BombNoteData> list5 = new List<BeatmapSaveData.BombNoteData>(200);
			foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteData noteData in notes)
			{
				switch (noteData.type)
				{
				case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.NoteA:
				case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.NoteB:
					list4.Add(new BeatmapSaveData.ColorNoteData(noteData.time, noteData.lineIndex, (int)noteData.lineLayer, BeatmapSaveData.GetNoteColorType(noteData.type), noteData.cutDirection, 0));
					break;
				case BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.Bomb:
					list5.Add(new BeatmapSaveData.BombNoteData(noteData.time, noteData.lineIndex, (int)noteData.lineLayer));
					break;
				}
			}
			List<BeatmapSaveData.ObstacleData> list6 = new List<BeatmapSaveData.ObstacleData>(list.Count);
			foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleData obstacleData in list)
			{
				list6.Add(new BeatmapSaveData.ObstacleData(obstacleData.time, obstacleData.lineIndex, BeatmapSaveData.GetLayerForObstacleType(obstacleData.type), obstacleData.duration, obstacleData.width, BeatmapSaveData.GetHeightForObstacleType(obstacleData.type)));
			}
			List<BeatmapSaveData.SliderData> list7 = new List<BeatmapSaveData.SliderData>(list2.Count);
			foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SliderData sliderData in list2)
			{
				list7.Add(new BeatmapSaveData.SliderData(BeatmapSaveData.GetNoteColorType(sliderData.colorType), sliderData.time, sliderData.headLineIndex, (int)sliderData.headLineLayer, sliderData.headControlPointLengthMultiplier, sliderData.headCutDirection, sliderData.tailTime, sliderData.tailLineIndex, (int)sliderData.tailLineLayer, sliderData.tailControlPointLengthMultiplier, sliderData.tailCutDirection, sliderData.sliderMidAnchorMode));
			}
			List<BeatmapSaveData.WaypointData> list8 = new List<BeatmapSaveData.WaypointData>(list3.Count);
			foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.WaypointData waypointData in list3)
			{
				list8.Add(new BeatmapSaveData.WaypointData(waypointData.time, waypointData.lineIndex, (int)waypointData.lineLayer, waypointData.offsetDirection));
			}
			List<BeatmapSaveData.BpmChangeEventData> list9 = new List<BeatmapSaveData.BpmChangeEventData>(100);
			List<BeatmapSaveData.BasicEventData> list10 = new List<BeatmapSaveData.BasicEventData>((events != null) ? events.Count : 0);
			List<BeatmapSaveData.RotationEventData> list11 = new List<BeatmapSaveData.RotationEventData>(100);
			List<BeatmapSaveData.ColorBoostEventData> list12 = new List<BeatmapSaveData.ColorBoostEventData>(100);
			if (events != null)
			{
				foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.EventData eventData in events)
				{
                    BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType type = eventData.type;
					if (type <= BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType.Event14)
					{
						if (type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType.Event5)
						{
							list12.Add(new BeatmapSaveData.ColorBoostEventData(eventData.time, eventData.value == 1));
							continue;
						}
						if (type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType.Event14)
						{
							list11.Add(new BeatmapSaveData.RotationEventData(eventData.time, BeatmapSaveData.ExecutionTime.Early, BeatmapSaveData.SpawnRotationForEventValue(eventData.value)));
							continue;
						}
					}
					else
					{
						if (type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType.Event15)
						{
							list11.Add(new BeatmapSaveData.RotationEventData(eventData.time, BeatmapSaveData.ExecutionTime.Late, BeatmapSaveData.SpawnRotationForEventValue(eventData.value)));
							continue;
						}
						if (type == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType.BpmChange)
						{
							list9.Add(new BeatmapSaveData.BpmChangeEventData(eventData.time, eventData.floatValue));
							continue;
						}
					}
					list10.Add(new BeatmapSaveData.BasicEventData(eventData.time, eventData.type, eventData.value, eventData.floatValue));
				}
			}
			List<BeatmapSaveData.LightColorEventBoxGroup> list13 = new List<BeatmapSaveData.LightColorEventBoxGroup>();
			List<BeatmapSaveData.LightRotationEventBoxGroup> list14 = new List<BeatmapSaveData.LightRotationEventBoxGroup>();
			List<BeatmapSaveData.LightTranslationEventBoxGroup> list15 = new List<BeatmapSaveData.LightTranslationEventBoxGroup>();
            BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventKeywordFiltersData specialEventsKeywordFilters = beatmapSaveData.specialEventsKeywordFilters;
			int? num;
			if (specialEventsKeywordFilters == null)
			{
				num = null;
			}
			else
			{
				List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventsForKeyword> keywords = specialEventsKeywordFilters.keywords;
				num = ((keywords != null) ? new int?(keywords.Count) : null);
			}
			List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword> list16 = new List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword>(num ?? 0);
			if (specialEventsKeywordFilters != null && specialEventsKeywordFilters.keywords != null)
			{
				foreach (BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.SpecialEventsForKeyword specialEventsForKeyword in specialEventsKeywordFilters.keywords)
				{
					list16.Add(new BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword(specialEventsForKeyword.keyword, specialEventsForKeyword.specialEvents));
				}
			}
			BeatmapSaveData.BasicEventTypesWithKeywords basicEventTypesWithKeywords = new BeatmapSaveData.BasicEventTypesWithKeywords(list16);
			return new BeatmapSaveData(list9, list11, list4, list5, list6, list7, new List<BeatmapSaveData.BurstSliderData>(), list8, list10, list12, list13, list14, list15, basicEventTypesWithKeywords, true);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005488 File Offset: 0x00003688
		private static BeatmapSaveData.NoteColorType GetNoteColorType(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType noteType)
		{
			if (noteType == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.NoteType.NoteB)
			{
				return BeatmapSaveData.NoteColorType.ColorB;
			}
			return BeatmapSaveData.NoteColorType.ColorA;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005488 File Offset: 0x00003688
		private static BeatmapSaveData.NoteColorType GetNoteColorType(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ColorType colorType)
		{
			if (colorType == BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ColorType.ColorB)
			{
				return BeatmapSaveData.NoteColorType.ColorB;
			}
			return BeatmapSaveData.NoteColorType.ColorA;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005491 File Offset: 0x00003691
		private static int GetHeightForObstacleType(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType obstacleType)
		{
			if (obstacleType != BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType.Top)
			{
				return 5;
			}
			return 3;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000549A File Offset: 0x0000369A
		private static int GetLayerForObstacleType(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType obstacleType)
		{
			if (obstacleType != BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType.Top)
			{
				return 0;
			}
			return 2;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005488 File Offset: 0x00003688
		private static BeatmapSaveData.SliderType GetSliderType(BeatmapSaveData.SliderType sliderType)
		{
			if (sliderType == BeatmapSaveData.SliderType.Burst)
			{
				return BeatmapSaveData.SliderType.Burst;
			}
			return BeatmapSaveData.SliderType.Normal;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000054A3 File Offset: 0x000036A3
		private static float SpawnRotationForEventValue(int index)
		{
			if (index >= 0 && index < BeatmapSaveData._spawnRotations.Length)
			{
				return BeatmapSaveData._spawnRotations[index];
			}
			return 0f;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000054C0 File Offset: 0x000036C0
		private static bool BeatmapSaveDataAreSorted(IReadOnlyList<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveDataItem> beatmapSaveData)
		{
			if (beatmapSaveData == null)
			{
				return true;
			}
			for (int i = 1; i < beatmapSaveData.Count; i++)
			{
				if (beatmapSaveData[i].time < beatmapSaveData[i - 1].time)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000109 RID: 265
		protected const string kCurrentVersion = "3.2.0";

		// Token: 0x0400010A RID: 266
		public string version;

		// Token: 0x0400010B RID: 267
		public List<BeatmapSaveData.BpmChangeEventData> bpmEvents;

		// Token: 0x0400010C RID: 268
		public List<BeatmapSaveData.RotationEventData> rotationEvents;

		// Token: 0x0400010D RID: 269
		public List<BeatmapSaveData.ColorNoteData> colorNotes;

		// Token: 0x0400010E RID: 270
		public List<BeatmapSaveData.BombNoteData> bombNotes;

		// Token: 0x0400010F RID: 271
		public List<BeatmapSaveData.ObstacleData> obstacles;

		// Token: 0x04000110 RID: 272
		public List<BeatmapSaveData.SliderData> sliders;

		// Token: 0x04000111 RID: 273
		public List<BeatmapSaveData.BurstSliderData> burstSliders;

		// Token: 0x04000112 RID: 274
		public List<BeatmapSaveData.WaypointData> waypoints;

		// Token: 0x04000113 RID: 275
		public List<BeatmapSaveData.BasicEventData> basicBeatmapEvents;

		// Token: 0x04000114 RID: 276
		public List<BeatmapSaveData.ColorBoostEventData> colorBoostBeatmapEvents;

		// Token: 0x04000115 RID: 277
		public List<BeatmapSaveData.LightColorEventBoxGroup> lightColorEventBoxGroups;

		// Token: 0x04000116 RID: 278
		public List<BeatmapSaveData.LightRotationEventBoxGroup> lightRotationEventBoxGroups;

		// Token: 0x04000117 RID: 279
		public List<BeatmapSaveData.LightTranslationEventBoxGroup> lightTranslationEventBoxGroups;

		// Token: 0x04000118 RID: 280
		public BeatmapSaveData.BasicEventTypesWithKeywords basicEventTypesWithKeywords;

		// Token: 0x04000119 RID: 281
		public bool useNormalEventsAsCompatibleEvents;

        // Token: 0x0400011A RID: 282
        [DoesNotRequireDomainReloadInit]
        protected static readonly Version version2_6_0 = new Version("2.6.0");

        // Token: 0x0400011B RID: 283
        [DoesNotRequireDomainReloadInit]
        protected static readonly float[] _spawnRotations = new float[]
		{
			-60f,
			-45f,
			-30f,
			-15f,
			15f,
			30f,
			45f,
			60f
		};

		// Token: 0x02000041 RID: 65
		[Serializable]
		public abstract class BeatmapSaveDataItem : IComparable<BeatmapSaveData.BeatmapSaveDataItem>
		{
			// Token: 0x17000044 RID: 68
			// (get) Token: 0x06000147 RID: 327 RVA: 0x00005529 File Offset: 0x00003729
			public float beat
			{
				get
				{
					return this.b;
				}
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00005531 File Offset: 0x00003731
			protected BeatmapSaveDataItem(float beat)
			{
				this.b = beat;
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00005540 File Offset: 0x00003740
			public int CompareTo(BeatmapSaveData.BeatmapSaveDataItem other)
			{
				return this.beat.CompareTo(other.beat);
			}

			// Token: 0x0400011C RID: 284
			[SerializeField]
			private float b;
		}

		// Token: 0x02000042 RID: 66
		[Serializable]
		public class BasicEventData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000045 RID: 69
			// (get) Token: 0x0600014A RID: 330 RVA: 0x00005561 File Offset: 0x00003761
			public BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType eventType
			{
				get
				{
					return this.et;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600014B RID: 331 RVA: 0x00005569 File Offset: 0x00003769
			public int value
			{
				get
				{
					return this.i;
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x0600014C RID: 332 RVA: 0x00005571 File Offset: 0x00003771
			public float floatValue
			{
				get
				{
					return this.f;
				}
			}

			// Token: 0x0600014D RID: 333 RVA: 0x00005579 File Offset: 0x00003779
			public BasicEventData(float beat, BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType eventType, int value, float floatValue) : base(beat)
			{
				this.et = eventType;
				this.i = value;
				this.f = floatValue;
			}

			// Token: 0x0400011D RID: 285
			[SerializeField]
			protected BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType et;

			// Token: 0x0400011E RID: 286
			[SerializeField]
			protected int i;

			// Token: 0x0400011F RID: 287
			[SerializeField]
			protected float f;
		}

		// Token: 0x02000043 RID: 67
		[Serializable]
		public class ColorBoostEventData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000048 RID: 72
			// (get) Token: 0x0600014E RID: 334 RVA: 0x00005598 File Offset: 0x00003798
			public bool boost
			{
				get
				{
					return this.o;
				}
			}

			// Token: 0x0600014F RID: 335 RVA: 0x000055A0 File Offset: 0x000037A0
			public ColorBoostEventData(float beat, bool boost) : base(beat)
			{
				this.o = boost;
			}

			// Token: 0x04000120 RID: 288
			[SerializeField]
			public bool o;
		}

		// Token: 0x02000044 RID: 68
		[Serializable]
		public class BpmChangeEventData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x06000150 RID: 336 RVA: 0x000055B0 File Offset: 0x000037B0
			public float bpm
			{
				get
				{
					return this.m;
				}
			}

			// Token: 0x06000151 RID: 337 RVA: 0x000055B8 File Offset: 0x000037B8
			public BpmChangeEventData(float beat, float bpm) : base(beat)
			{
				this.m = bpm;
			}

			// Token: 0x04000121 RID: 289
			[SerializeField]
			public float m;
		}

		// Token: 0x02000045 RID: 69
		public enum ExecutionTime
		{
			// Token: 0x04000123 RID: 291
			Early,
			// Token: 0x04000124 RID: 292
			Late
		}

		// Token: 0x02000046 RID: 70
		[Serializable]
		public class RotationEventData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000152 RID: 338 RVA: 0x000055C8 File Offset: 0x000037C8
			public BeatmapSaveData.ExecutionTime executionTime
			{
				get
				{
					return this.e;
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000153 RID: 339 RVA: 0x000055D0 File Offset: 0x000037D0
			public float rotation
			{
				get
				{
					return this.r;
				}
			}

			// Token: 0x06000154 RID: 340 RVA: 0x000055D8 File Offset: 0x000037D8
			public RotationEventData(float beat, BeatmapSaveData.ExecutionTime executionTime, float rotation) : base(beat)
			{
				this.e = executionTime;
				this.r = rotation;
			}

			// Token: 0x04000125 RID: 293
			[SerializeField]
			public BeatmapSaveData.ExecutionTime e;

			// Token: 0x04000126 RID: 294
			[SerializeField]
			public float r;
		}

		// Token: 0x02000047 RID: 71
		[Serializable]
		public class BasicEventTypesWithKeywords
		{
			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000155 RID: 341 RVA: 0x000055EF File Offset: 0x000037EF
			public List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword> data
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x06000156 RID: 342 RVA: 0x000055F7 File Offset: 0x000037F7
			public BasicEventTypesWithKeywords(List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword> data)
			{
				this.d = data;
			}

			// Token: 0x04000127 RID: 295
			[SerializeField]
			protected List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword> d;

			// Token: 0x02000048 RID: 72
			[Serializable]
			public class BasicEventTypesForKeyword
			{
				// Token: 0x1700004D RID: 77
				// (get) Token: 0x06000157 RID: 343 RVA: 0x00005606 File Offset: 0x00003806
				public string keyword
				{
					get
					{
						return this.k;
					}
				}

				// Token: 0x1700004E RID: 78
				// (get) Token: 0x06000158 RID: 344 RVA: 0x0000560E File Offset: 0x0000380E
				public List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType> eventTypes
				{
					get
					{
						return this.e;
					}
				}

				// Token: 0x06000159 RID: 345 RVA: 0x00005616 File Offset: 0x00003816
				public BasicEventTypesForKeyword(string keyword, List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType> eventTypes)
				{
					this.k = keyword;
					this.e = eventTypes;
				}

				// Token: 0x04000128 RID: 296
				[SerializeField]
				protected string k;

				// Token: 0x04000129 RID: 297
				[SerializeField]
				protected List<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.BeatmapEventType> e;
			}
		}

		// Token: 0x02000049 RID: 73
		public enum EnvironmentColorType
		{
			// Token: 0x0400012B RID: 299
			Color0,
			// Token: 0x0400012C RID: 300
			Color1,
			// Token: 0x0400012D RID: 301
			ColorWhite
		}

		// Token: 0x0200004A RID: 74
		public enum TransitionType
		{
			// Token: 0x0400012F RID: 303
			Instant,
			// Token: 0x04000130 RID: 304
			Interpolate,
			// Token: 0x04000131 RID: 305
			Extend
		}

		// Token: 0x0200004B RID: 75
		public enum Axis
		{
			// Token: 0x04000133 RID: 307
			X,
			// Token: 0x04000134 RID: 308
			Y,
			// Token: 0x04000135 RID: 309
			Z
		}

		// Token: 0x0200004C RID: 76
		public enum EaseType
		{
			// Token: 0x04000137 RID: 311
			None = -1,
			// Token: 0x04000138 RID: 312
			Linear,
			// Token: 0x04000139 RID: 313
			InQuad,
			// Token: 0x0400013A RID: 314
			OutQuad,
			// Token: 0x0400013B RID: 315
			InOutQuad
		}

		// Token: 0x0200004D RID: 77
		[Serializable]
		public abstract class EventBox
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x0600015A RID: 346 RVA: 0x0000562C File Offset: 0x0000382C
			public BeatmapSaveData.IndexFilter indexFilter
			{
				get
				{
					return this.f;
				}
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x0600015B RID: 347 RVA: 0x00005634 File Offset: 0x00003834
			public float beatDistributionParam
			{
				get
				{
					return this.w;
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x0600015C RID: 348 RVA: 0x0000563C File Offset: 0x0000383C
			public BeatmapSaveData.EventBox.DistributionParamType beatDistributionParamType
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x0600015D RID: 349 RVA: 0x00005644 File Offset: 0x00003844
			protected EventBox(BeatmapSaveData.IndexFilter indexFilter, float beatDistributionParam, BeatmapSaveData.EventBox.DistributionParamType beatDistributionParamType)
			{
				this.f = indexFilter;
				this.w = beatDistributionParam;
				this.d = beatDistributionParamType;
			}

			// Token: 0x0400013C RID: 316
			[SerializeField]
			protected BeatmapSaveData.IndexFilter f;

			// Token: 0x0400013D RID: 317
			[SerializeField]
			protected float w;

			// Token: 0x0400013E RID: 318
			[SerializeField]
			protected BeatmapSaveData.EventBox.DistributionParamType d;

			// Token: 0x0200004E RID: 78
			public enum DistributionParamType
			{
				// Token: 0x04000140 RID: 320
				Wave = 1,
				// Token: 0x04000141 RID: 321
				Step
			}
		}

		// Token: 0x0200004F RID: 79
		[Flags]
		[Serializable]
		public enum IndexFilterRandomType
		{
			// Token: 0x04000143 RID: 323
			NoRandom = 0,
			// Token: 0x04000144 RID: 324
			KeepOrder = 1,
			// Token: 0x04000145 RID: 325
			RandomElements = 2
		}

		// Token: 0x02000050 RID: 80
		[Flags]
		[Serializable]
		public enum IndexFilterLimitAlsoAffectsType
		{
			// Token: 0x04000147 RID: 327
			None = 0,
			// Token: 0x04000148 RID: 328
			Duration = 1,
			// Token: 0x04000149 RID: 329
			Distribution = 2
		}

		// Token: 0x02000051 RID: 81
		[Serializable]
		public class IndexFilter
		{
			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600015E RID: 350 RVA: 0x00005661 File Offset: 0x00003861
			public BeatmapSaveData.IndexFilter.IndexFilterType type
			{
				get
				{
					return this.f;
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x0600015F RID: 351 RVA: 0x00005669 File Offset: 0x00003869
			public int param0
			{
				get
				{
					return this.p;
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x06000160 RID: 352 RVA: 0x00005671 File Offset: 0x00003871
			public int param1
			{
				get
				{
					return this.t;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000161 RID: 353 RVA: 0x00005679 File Offset: 0x00003879
			public bool reversed
			{
				get
				{
					return this.r != 0;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000162 RID: 354 RVA: 0x00005684 File Offset: 0x00003884
			public int chunks
			{
				get
				{
					return this.c;
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000163 RID: 355 RVA: 0x0000568C File Offset: 0x0000388C
			public float limit
			{
				get
				{
					return this.l;
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x06000164 RID: 356 RVA: 0x00005694 File Offset: 0x00003894
			public BeatmapSaveData.IndexFilterLimitAlsoAffectsType limitAlsoAffectsType
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x06000165 RID: 357 RVA: 0x0000569C File Offset: 0x0000389C
			public BeatmapSaveData.IndexFilterRandomType random
			{
				get
				{
					return this.n;
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x06000166 RID: 358 RVA: 0x000056A4 File Offset: 0x000038A4
			public int seed
			{
				get
				{
					return this.s;
				}
			}

			// Token: 0x06000167 RID: 359 RVA: 0x000056AC File Offset: 0x000038AC
			public IndexFilter(BeatmapSaveData.IndexFilter.IndexFilterType type, int param0, int param1, bool reversed, BeatmapSaveData.IndexFilterRandomType random, int seed, int chunks, float limit, BeatmapSaveData.IndexFilterLimitAlsoAffectsType limitAlsoAffectsType)
			{
				this.f = type;
				this.p = param0;
				this.t = param1;
				this.r = (reversed ? 1 : 0);
				this.c = chunks;
				this.l = limit;
				this.d = limitAlsoAffectsType;
				this.n = random;
				this.s = seed;
			}

			// Token: 0x06000168 RID: 360 RVA: 0x0000570C File Offset: 0x0000390C
			public IndexFilter(BeatmapSaveData.IndexFilter other)
			{
				this.f = other.f;
				this.p = other.p;
				this.t = other.t;
				this.r = other.r;
				this.c = other.chunks;
				this.l = other.limit;
				this.d = other.limitAlsoAffectsType;
				this.n = other.random;
				this.s = other.seed;
			}

			// Token: 0x06000169 RID: 361 RVA: 0x0000578C File Offset: 0x0000398C
			public static BeatmapSaveData.IndexFilter CreateDivisionIndexFilter(int numberOfSections, int divisionIdx, bool reversed)
			{
				return new BeatmapSaveData.IndexFilter(BeatmapSaveData.IndexFilter.IndexFilterType.Division, numberOfSections, divisionIdx, reversed, BeatmapSaveData.IndexFilterRandomType.NoRandom, 0, 0, 0f, BeatmapSaveData.IndexFilterLimitAlsoAffectsType.None);
			}

			// Token: 0x0600016A RID: 362 RVA: 0x000057AC File Offset: 0x000039AC
			public static BeatmapSaveData.IndexFilter CreateStepFilter(int offset, int step, bool reversed)
			{
				return new BeatmapSaveData.IndexFilter(BeatmapSaveData.IndexFilter.IndexFilterType.StepAndOffset, offset, step, reversed, BeatmapSaveData.IndexFilterRandomType.NoRandom, 0, 0, 0f, BeatmapSaveData.IndexFilterLimitAlsoAffectsType.None);
			}

			// Token: 0x0600016B RID: 363 RVA: 0x000057CC File Offset: 0x000039CC
			public static BeatmapSaveData.IndexFilter CreateForExtension()
			{
				return new BeatmapSaveData.IndexFilter(BeatmapSaveData.IndexFilter.IndexFilterType.Division, 1, 0, false, BeatmapSaveData.IndexFilterRandomType.NoRandom, 0, 0, 0f, BeatmapSaveData.IndexFilterLimitAlsoAffectsType.None);
			}

			// Token: 0x0400014A RID: 330
			[SerializeField]
			protected BeatmapSaveData.IndexFilter.IndexFilterType f;

			// Token: 0x0400014B RID: 331
			[SerializeField]
			protected int p;

			// Token: 0x0400014C RID: 332
			[SerializeField]
			protected int t;

			// Token: 0x0400014D RID: 333
			[SerializeField]
			protected int r;

			// Token: 0x0400014E RID: 334
			[SerializeField]
			protected int c;

			// Token: 0x0400014F RID: 335
			[SerializeField]
			protected BeatmapSaveData.IndexFilterRandomType n;

			// Token: 0x04000150 RID: 336
			[SerializeField]
			protected int s;

			// Token: 0x04000151 RID: 337
			[SerializeField]
			protected float l;

			// Token: 0x04000152 RID: 338
			[SerializeField]
			protected BeatmapSaveData.IndexFilterLimitAlsoAffectsType d;

			// Token: 0x02000052 RID: 82
			public enum IndexFilterType
			{
				// Token: 0x04000154 RID: 340
				Division = 1,
				// Token: 0x04000155 RID: 341
				StepAndOffset
			}
		}

		// Token: 0x02000053 RID: 83
		[Serializable]
		public class LightColorEventBox : BeatmapSaveData.EventBox
		{
			// Token: 0x1700005B RID: 91
			// (get) Token: 0x0600016C RID: 364 RVA: 0x000057EB File Offset: 0x000039EB
			public float brightnessDistributionParam
			{
				get
				{
					return this.r;
				}
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x0600016D RID: 365 RVA: 0x000057F3 File Offset: 0x000039F3
			public BeatmapSaveData.EventBox.DistributionParamType brightnessDistributionParamType
			{
				get
				{
					return this.t;
				}
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x0600016E RID: 366 RVA: 0x000057FB File Offset: 0x000039FB
			public bool brightnessDistributionShouldAffectFirstBaseEvent
			{
				get
				{
					return this.b == 1;
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x0600016F RID: 367 RVA: 0x00005806 File Offset: 0x00003A06
			public BeatmapSaveData.EaseType brightnessDistributionEaseType
			{
				get
				{
					return this.i;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000170 RID: 368 RVA: 0x0000580E File Offset: 0x00003A0E
			public List<BeatmapSaveData.LightColorBaseData> lightColorBaseDataList
			{
				get
				{
					return this.e;
				}
			}

			// Token: 0x06000171 RID: 369 RVA: 0x00005816 File Offset: 0x00003A16
			public LightColorEventBox(BeatmapSaveData.IndexFilter indexFilter, float beatDistributionParam, BeatmapSaveData.EventBox.DistributionParamType beatDistributionParamType, float brightnessDistributionParam, bool brightnessDistributionShouldAffectFirstBaseEvent, BeatmapSaveData.EventBox.DistributionParamType brightnessDistributionParamType, BeatmapSaveData.EaseType brightnessDistributionEaseType, List<BeatmapSaveData.LightColorBaseData> lightColorBaseDataList) : base(indexFilter, beatDistributionParam, beatDistributionParamType)
			{
				this.r = brightnessDistributionParam;
				this.t = brightnessDistributionParamType;
				this.e = lightColorBaseDataList;
				this.b = (brightnessDistributionShouldAffectFirstBaseEvent ? 1 : 0);
				this.i = brightnessDistributionEaseType;
			}

			// Token: 0x04000156 RID: 342
			[SerializeField]
			protected float r;

			// Token: 0x04000157 RID: 343
			[SerializeField]
			protected BeatmapSaveData.EventBox.DistributionParamType t;

			// Token: 0x04000158 RID: 344
			[SerializeField]
			protected int b;

			// Token: 0x04000159 RID: 345
			[SerializeField]
			protected BeatmapSaveData.EaseType i;

			// Token: 0x0400015A RID: 346
			[SerializeField]
			protected List<BeatmapSaveData.LightColorBaseData> e;
		}

		// Token: 0x02000054 RID: 84
		[Serializable]
		public class LightColorBaseData
		{
			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000172 RID: 370 RVA: 0x0000584F File Offset: 0x00003A4F
			public float beat
			{
				get
				{
					return this.b;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x06000173 RID: 371 RVA: 0x00005857 File Offset: 0x00003A57
			public BeatmapSaveData.TransitionType transitionType
			{
				get
				{
					return this.i;
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x06000174 RID: 372 RVA: 0x0000585F File Offset: 0x00003A5F
			public BeatmapSaveData.EnvironmentColorType colorType
			{
				get
				{
					return this.c;
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000175 RID: 373 RVA: 0x00005867 File Offset: 0x00003A67
			public float brightness
			{
				get
				{
					return this.s;
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000176 RID: 374 RVA: 0x0000586F File Offset: 0x00003A6F
			public int strobeBeatFrequency
			{
				get
				{
					return this.f;
				}
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00005877 File Offset: 0x00003A77
			public LightColorBaseData(float beat, BeatmapSaveData.TransitionType transitionType, BeatmapSaveData.EnvironmentColorType colorType, float brightness, int strobeFrequency)
			{
				this.b = beat;
				this.i = transitionType;
				this.c = colorType;
				this.s = brightness;
				this.f = strobeFrequency;
			}

			// Token: 0x0400015B RID: 347
			[SerializeField]
			protected float b;

			// Token: 0x0400015C RID: 348
			[SerializeField]
			protected BeatmapSaveData.TransitionType i;

			// Token: 0x0400015D RID: 349
			[SerializeField]
			protected BeatmapSaveData.EnvironmentColorType c;

			// Token: 0x0400015E RID: 350
			[SerializeField]
			protected float s;

			// Token: 0x0400015F RID: 351
			[SerializeField]
			protected int f;
		}

		// Token: 0x02000055 RID: 85
		[Serializable]
		public class LightRotationEventBox : BeatmapSaveData.EventBox
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000178 RID: 376 RVA: 0x000058A4 File Offset: 0x00003AA4
			public float rotationDistributionParam
			{
				get
				{
					return this.s;
				}
			}

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000179 RID: 377 RVA: 0x000058AC File Offset: 0x00003AAC
			public BeatmapSaveData.EventBox.DistributionParamType rotationDistributionParamType
			{
				get
				{
					return this.t;
				}
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x0600017A RID: 378 RVA: 0x000058B4 File Offset: 0x00003AB4
			public BeatmapSaveData.Axis axis
			{
				get
				{
					return this.a;
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x0600017B RID: 379 RVA: 0x000058BC File Offset: 0x00003ABC
			public bool flipRotation
			{
				get
				{
					return this.r == 1;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x0600017C RID: 380 RVA: 0x000058C7 File Offset: 0x00003AC7
			public bool rotationDistributionShouldAffectFirstBaseEvent
			{
				get
				{
					return this.b == 1;
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x0600017D RID: 381 RVA: 0x000058D2 File Offset: 0x00003AD2
			public BeatmapSaveData.EaseType rotationDistributionEaseType
			{
				get
				{
					return this.i;
				}
			}

			// Token: 0x1700006B RID: 107
			// (get) Token: 0x0600017E RID: 382 RVA: 0x000058DA File Offset: 0x00003ADA
			public IReadOnlyList<BeatmapSaveData.LightRotationBaseData> lightRotationBaseDataList
			{
				get
				{
					return this.l;
				}
			}

			// Token: 0x0600017F RID: 383 RVA: 0x000058E4 File Offset: 0x00003AE4
			public LightRotationEventBox(BeatmapSaveData.IndexFilter indexFilter, float beatDistributionParam, BeatmapSaveData.EventBox.DistributionParamType beatDistributionParamType, float rotationDistributionParam, BeatmapSaveData.EventBox.DistributionParamType rotationDistributionParamType, bool rotationDistributionShouldAffectFirstBaseEvent, BeatmapSaveData.EaseType rotationDistributionEaseType, BeatmapSaveData.Axis axis, bool flipRotation, List<BeatmapSaveData.LightRotationBaseData> lightRotationBaseDataList) : base(indexFilter, beatDistributionParam, beatDistributionParamType)
			{
				this.s = rotationDistributionParam;
				this.t = rotationDistributionParamType;
				this.a = axis;
				this.l = lightRotationBaseDataList;
				this.r = (flipRotation ? 1 : 0);
				this.b = (rotationDistributionShouldAffectFirstBaseEvent ? 1 : 0);
				this.i = rotationDistributionEaseType;
			}

			// Token: 0x04000160 RID: 352
			[SerializeField]
			protected float s;

			// Token: 0x04000161 RID: 353
			[SerializeField]
			protected BeatmapSaveData.EventBox.DistributionParamType t;

			// Token: 0x04000162 RID: 354
			[SerializeField]
			protected BeatmapSaveData.Axis a;

			// Token: 0x04000163 RID: 355
			[SerializeField]
			protected int r;

			// Token: 0x04000164 RID: 356
			[SerializeField]
			protected int b;

			// Token: 0x04000165 RID: 357
			[SerializeField]
			protected BeatmapSaveData.EaseType i;

			// Token: 0x04000166 RID: 358
			[SerializeField]
			protected List<BeatmapSaveData.LightRotationBaseData> l;
		}

		// Token: 0x02000056 RID: 86
		[Serializable]
		public class LightRotationBaseData
		{
			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000180 RID: 384 RVA: 0x0000593E File Offset: 0x00003B3E
			public float beat
			{
				get
				{
					return this.b;
				}
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x06000181 RID: 385 RVA: 0x00005946 File Offset: 0x00003B46
			public bool usePreviousEventRotationValue
			{
				get
				{
					return this.p == 1;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000182 RID: 386 RVA: 0x00005951 File Offset: 0x00003B51
			public BeatmapSaveData.EaseType easeType
			{
				get
				{
					return this.e;
				}
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x06000183 RID: 387 RVA: 0x00005959 File Offset: 0x00003B59
			public int loopsCount
			{
				get
				{
					return this.l;
				}
			}

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000184 RID: 388 RVA: 0x00005961 File Offset: 0x00003B61
			public float rotation
			{
				get
				{
					return this.r;
				}
			}

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x06000185 RID: 389 RVA: 0x00005969 File Offset: 0x00003B69
			public BeatmapSaveData.LightRotationBaseData.RotationDirection rotationDirection
			{
				get
				{
					return this.o;
				}
			}

			// Token: 0x06000186 RID: 390 RVA: 0x00005971 File Offset: 0x00003B71
			public LightRotationBaseData(float beat, bool usePreviousEventRotationValue, BeatmapSaveData.EaseType easeType, int loopsCount, float rotation, BeatmapSaveData.LightRotationBaseData.RotationDirection rotationDirection)
			{
				this.b = beat;
				this.p = (usePreviousEventRotationValue ? 1 : 0);
				this.e = easeType;
				this.l = loopsCount;
				this.r = rotation;
				this.o = rotationDirection;
			}

			// Token: 0x04000167 RID: 359
			[SerializeField]
			protected float b;

			// Token: 0x04000168 RID: 360
			[SerializeField]
			protected int p;

			// Token: 0x04000169 RID: 361
			[SerializeField]
			protected BeatmapSaveData.EaseType e;

			// Token: 0x0400016A RID: 362
			[SerializeField]
			protected int l;

			// Token: 0x0400016B RID: 363
			[SerializeField]
			protected float r;

			// Token: 0x0400016C RID: 364
			[SerializeField]
			protected BeatmapSaveData.LightRotationBaseData.RotationDirection o;

			// Token: 0x02000057 RID: 87
			public enum RotationDirection
			{
				// Token: 0x0400016E RID: 366
				Automatic,
				// Token: 0x0400016F RID: 367
				Clockwise,
				// Token: 0x04000170 RID: 368
				Counterclockwise
			}
		}

		// Token: 0x02000058 RID: 88
		[Serializable]
		public class LightTranslationEventBox : BeatmapSaveData.EventBox
		{
			// Token: 0x17000072 RID: 114
			// (get) Token: 0x06000187 RID: 391 RVA: 0x000059AC File Offset: 0x00003BAC
			public float gapDistributionParam
			{
				get
				{
					return this.s;
				}
			}

			// Token: 0x17000073 RID: 115
			// (get) Token: 0x06000188 RID: 392 RVA: 0x000059B4 File Offset: 0x00003BB4
			public BeatmapSaveData.EventBox.DistributionParamType gapDistributionParamType
			{
				get
				{
					return this.t;
				}
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000189 RID: 393 RVA: 0x000059BC File Offset: 0x00003BBC
			public BeatmapSaveData.Axis axis
			{
				get
				{
					return this.a;
				}
			}

			// Token: 0x17000075 RID: 117
			// (get) Token: 0x0600018A RID: 394 RVA: 0x000059C4 File Offset: 0x00003BC4
			public bool flipTranslation
			{
				get
				{
					return this.r == 1;
				}
			}

			// Token: 0x17000076 RID: 118
			// (get) Token: 0x0600018B RID: 395 RVA: 0x000059CF File Offset: 0x00003BCF
			public bool gapDistributionShouldAffectFirstBaseEvent
			{
				get
				{
					return this.b == 1;
				}
			}

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x0600018C RID: 396 RVA: 0x000059DA File Offset: 0x00003BDA
			public BeatmapSaveData.EaseType gapDistributionEaseType
			{
				get
				{
					return this.i;
				}
			}

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x0600018D RID: 397 RVA: 0x000059E2 File Offset: 0x00003BE2
			public IReadOnlyList<BeatmapSaveData.LightTranslationBaseData> lightTranslationBaseDataList
			{
				get
				{
					return this.l;
				}
			}

			// Token: 0x0600018E RID: 398 RVA: 0x000059EC File Offset: 0x00003BEC
			public LightTranslationEventBox(BeatmapSaveData.IndexFilter indexFilter, float beatDistributionPara, BeatmapSaveData.EventBox.DistributionParamType beatDistributionParamType, float gapDistributionParam, BeatmapSaveData.EventBox.DistributionParamType gapDistributionParamType, bool gapDistributionShouldAffectFirstBaseEvent, BeatmapSaveData.EaseType gapDistributionEaseType, BeatmapSaveData.Axis axis, bool flipTranslation, List<BeatmapSaveData.LightTranslationBaseData> lightTranslationBaseDataList) : base(indexFilter, beatDistributionPara, beatDistributionParamType)
			{
				this.s = gapDistributionParam;
				this.t = gapDistributionParamType;
				this.a = axis;
				this.r = (flipTranslation ? 1 : 0);
				this.b = (gapDistributionShouldAffectFirstBaseEvent ? 1 : 0);
				this.i = gapDistributionEaseType;
				this.l = lightTranslationBaseDataList;
			}

			// Token: 0x04000171 RID: 369
			[SerializeField]
			protected float s;

			// Token: 0x04000172 RID: 370
			[SerializeField]
			protected BeatmapSaveData.EventBox.DistributionParamType t;

			// Token: 0x04000173 RID: 371
			[SerializeField]
			protected BeatmapSaveData.Axis a;

			// Token: 0x04000174 RID: 372
			[SerializeField]
			protected int r;

			// Token: 0x04000175 RID: 373
			[SerializeField]
			protected int b;

			// Token: 0x04000176 RID: 374
			[SerializeField]
			protected BeatmapSaveData.EaseType i;

			// Token: 0x04000177 RID: 375
			[SerializeField]
			protected List<BeatmapSaveData.LightTranslationBaseData> l;
		}

		// Token: 0x02000059 RID: 89
		[Serializable]
		public class LightTranslationBaseData
		{
			// Token: 0x17000079 RID: 121
			// (get) Token: 0x0600018F RID: 399 RVA: 0x00005A46 File Offset: 0x00003C46
			public float beat
			{
				get
				{
					return this.b;
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000190 RID: 400 RVA: 0x00005A4E File Offset: 0x00003C4E
			public bool usePreviousEventTranslationValue
			{
				get
				{
					return this.p == 1;
				}
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000191 RID: 401 RVA: 0x00005A59 File Offset: 0x00003C59
			public BeatmapSaveData.EaseType easeType
			{
				get
				{
					return this.e;
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000192 RID: 402 RVA: 0x00005A61 File Offset: 0x00003C61
			public float translation
			{
				get
				{
					return this.t;
				}
			}

			// Token: 0x06000193 RID: 403 RVA: 0x00005A69 File Offset: 0x00003C69
			public LightTranslationBaseData(float beat, bool usePreviousEventTranslationValue, BeatmapSaveData.EaseType easeType, float translation)
			{
				this.b = beat;
				this.p = (usePreviousEventTranslationValue ? 1 : 0);
				this.e = easeType;
				this.t = translation;
			}

			// Token: 0x04000178 RID: 376
			[SerializeField]
			protected float b;

			// Token: 0x04000179 RID: 377
			[SerializeField]
			protected int p;

			// Token: 0x0400017A RID: 378
			[SerializeField]
			protected BeatmapSaveData.EaseType e;

			// Token: 0x0400017B RID: 379
			[SerializeField]
			protected float t;
		}

		// Token: 0x0200005A RID: 90
		[Serializable]
		public abstract class EventBoxGroup : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x1700007D RID: 125
			// (get) Token: 0x06000194 RID: 404 RVA: 0x00005A94 File Offset: 0x00003C94
			public int groupId
			{
				get
				{
					return this.g;
				}
			}

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000195 RID: 405
			public abstract IReadOnlyList<BeatmapSaveData.EventBox> baseEventBoxes { get; }

			// Token: 0x06000196 RID: 406 RVA: 0x00005A9C File Offset: 0x00003C9C
			protected EventBoxGroup(float beat, int groupId) : base(beat)
			{
				this.g = groupId;
			}

			// Token: 0x0400017C RID: 380
			[SerializeField]
			private int g;
		}

		// Token: 0x0200005B RID: 91
		[Serializable]
		public class EventBoxGroup<T> : BeatmapSaveData.EventBoxGroup
		{
			// Token: 0x1700007F RID: 127
			// (get) Token: 0x06000197 RID: 407 RVA: 0x00005AAC File Offset: 0x00003CAC
			public override IReadOnlyList<BeatmapSaveData.EventBox> baseEventBoxes
			{
				get
				{
					return (IReadOnlyList<BeatmapSaveData.EventBox>)this.e;
				}
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x06000198 RID: 408 RVA: 0x00005AB9 File Offset: 0x00003CB9
			public IReadOnlyList<T> eventBoxes
			{
				get
				{
					return this.e;
				}
			}

			// Token: 0x06000199 RID: 409 RVA: 0x00005AC1 File Offset: 0x00003CC1
			protected EventBoxGroup(float beat, int groupId, List<T> eventBoxes) : base(beat, groupId)
			{
				this.e = eventBoxes;
			}

			// Token: 0x0400017D RID: 381
			[SerializeField]
			protected List<T> e;
		}

		// Token: 0x0200005C RID: 92
		[Serializable]
		public class LightColorEventBoxGroup : BeatmapSaveData.EventBoxGroup<BeatmapSaveData.LightColorEventBox>
		{
			// Token: 0x0600019A RID: 410 RVA: 0x00005AD2 File Offset: 0x00003CD2
			public LightColorEventBoxGroup(float beat, int groupId, List<BeatmapSaveData.LightColorEventBox> eventBoxes) : base(beat, groupId, eventBoxes)
			{
			}

			// Token: 0x0600019B RID: 411 RVA: 0x00005AE0 File Offset: 0x00003CE0
			public virtual BeatmapSaveData.LightColorEventBoxGroup CopyWith(float? newBeat = null, int? newGroupId = null)
			{
				return new BeatmapSaveData.LightColorEventBoxGroup(newBeat ?? base.beat, newGroupId ?? base.groupId, this.e);
			}
		}

		// Token: 0x0200005D RID: 93
		[Serializable]
		public class LightRotationEventBoxGroup : BeatmapSaveData.EventBoxGroup<BeatmapSaveData.LightRotationEventBox>
		{
			// Token: 0x0600019C RID: 412 RVA: 0x00005B2C File Offset: 0x00003D2C
			public LightRotationEventBoxGroup(float beat, int groupId, List<BeatmapSaveData.LightRotationEventBox> eventBoxes) : base(beat, groupId, eventBoxes)
			{
			}

			// Token: 0x0600019D RID: 413 RVA: 0x00005B38 File Offset: 0x00003D38
			public virtual BeatmapSaveData.LightRotationEventBoxGroup CopyWith(float? newBeat = null, int? newGroupId = null)
			{
				return new BeatmapSaveData.LightRotationEventBoxGroup(newBeat ?? base.beat, newGroupId ?? base.groupId, this.e);
			}
		}

		// Token: 0x0200005E RID: 94
		[Serializable]
		public class LightTranslationEventBoxGroup : BeatmapSaveData.EventBoxGroup<BeatmapSaveData.LightTranslationEventBox>
		{
			// Token: 0x0600019E RID: 414 RVA: 0x00005B84 File Offset: 0x00003D84
			public LightTranslationEventBoxGroup(float beat, int groupId, List<BeatmapSaveData.LightTranslationEventBox> eventBoxes) : base(beat, groupId, eventBoxes)
			{
			}

			// Token: 0x0600019F RID: 415 RVA: 0x00005B90 File Offset: 0x00003D90
			public virtual BeatmapSaveData.LightTranslationEventBoxGroup CopyWith(float? newBeat = null, int? newGroupId = null)
			{
				return new BeatmapSaveData.LightTranslationEventBoxGroup(newBeat ?? base.beat, newGroupId ?? base.groupId, this.e);
			}
		}

		// Token: 0x0200005F RID: 95
		public enum NoteColorType
		{
			// Token: 0x0400017F RID: 383
			ColorA,
			// Token: 0x04000180 RID: 384
			ColorB
		}

		// Token: 0x02000060 RID: 96
		[Serializable]
		public class ColorNoteData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000081 RID: 129
			// (get) Token: 0x060001A0 RID: 416 RVA: 0x00005BDC File Offset: 0x00003DDC
			public int line
			{
				get
				{
					return this.x;
				}
			}

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x060001A1 RID: 417 RVA: 0x00005BE4 File Offset: 0x00003DE4
			public int layer
			{
				get
				{
					return this.y;
				}
			}

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x00005BEC File Offset: 0x00003DEC
			public int angleOffset
			{
				get
				{
					return this.a;
				}
			}

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x060001A3 RID: 419 RVA: 0x00005BF4 File Offset: 0x00003DF4
			public BeatmapSaveData.NoteColorType color
			{
				get
				{
					return this.c;
				}
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x060001A4 RID: 420 RVA: 0x00005BFC File Offset: 0x00003DFC
			public NoteCutDirection cutDirection
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x00005C04 File Offset: 0x00003E04
			public ColorNoteData(float beat, int line, int layer, BeatmapSaveData.NoteColorType color, NoteCutDirection cutDirection, int angleOffset) : base(beat)
			{
				this.x = line;
				this.y = layer;
				this.c = color;
				this.d = cutDirection;
				this.a = angleOffset;
			}

			// Token: 0x04000181 RID: 385
			[SerializeField]
			protected int x;

			// Token: 0x04000182 RID: 386
			[SerializeField]
			protected int y;

			// Token: 0x04000183 RID: 387
			[SerializeField]
			protected int a;

			// Token: 0x04000184 RID: 388
			[SerializeField]
			protected BeatmapSaveData.NoteColorType c;

			// Token: 0x04000185 RID: 389
			[SerializeField]
			protected NoteCutDirection d;
		}

		// Token: 0x02000061 RID: 97
		[Serializable]
		public class BombNoteData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000086 RID: 134
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x00005C33 File Offset: 0x00003E33
			public int line
			{
				get
				{
					return this.x;
				}
			}

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005C3B File Offset: 0x00003E3B
			public int layer
			{
				get
				{
					return this.y;
				}
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x00005C43 File Offset: 0x00003E43
			public BombNoteData(float beat, int line, int layer) : base(beat)
			{
				this.x = line;
				this.y = layer;
			}

			// Token: 0x04000186 RID: 390
			[SerializeField]
			protected int x;

			// Token: 0x04000187 RID: 391
			[SerializeField]
			protected int y;
		}

		// Token: 0x02000062 RID: 98
		[Serializable]
		public class WaypointData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000088 RID: 136
			// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005C5A File Offset: 0x00003E5A
			public int line
			{
				get
				{
					return this.x;
				}
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x060001AA RID: 426 RVA: 0x00005C62 File Offset: 0x00003E62
			public int layer
			{
				get
				{
					return this.y;
				}
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x060001AB RID: 427 RVA: 0x00005C6A File Offset: 0x00003E6A
			public OffsetDirection offsetDirection
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x060001AC RID: 428 RVA: 0x00005C72 File Offset: 0x00003E72
			public WaypointData(float beat, int line, int layer, OffsetDirection offsetDirection) : base(beat)
			{
				this.x = line;
				this.y = layer;
				this.d = offsetDirection;
			}

			// Token: 0x04000188 RID: 392
			[SerializeField]
			protected int x;

			// Token: 0x04000189 RID: 393
			[SerializeField]
			protected int y;

			// Token: 0x0400018A RID: 394
			[SerializeField]
			protected OffsetDirection d;
		}

		// Token: 0x02000063 RID: 99
		public enum SliderType
		{
			// Token: 0x0400018C RID: 396
			Normal,
			// Token: 0x0400018D RID: 397
			Burst
		}

		// Token: 0x02000064 RID: 100
		[Serializable]
		public abstract class BaseSliderData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x1700008B RID: 139
			// (get) Token: 0x060001AD RID: 429 RVA: 0x00005C91 File Offset: 0x00003E91
			public BeatmapSaveData.NoteColorType colorType
			{
				get
				{
					return this.c;
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x060001AE RID: 430 RVA: 0x00005C99 File Offset: 0x00003E99
			public int headLine
			{
				get
				{
					return this.x;
				}
			}

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x060001AF RID: 431 RVA: 0x00005CA1 File Offset: 0x00003EA1
			public int headLayer
			{
				get
				{
					return this.y;
				}
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005CA9 File Offset: 0x00003EA9
			public NoteCutDirection headCutDirection
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x060001B1 RID: 433 RVA: 0x00005CB1 File Offset: 0x00003EB1
			public float tailBeat
			{
				get
				{
					return this.tb;
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005CB9 File Offset: 0x00003EB9
			public int tailLine
			{
				get
				{
					return this.tx;
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x060001B3 RID: 435 RVA: 0x00005CC1 File Offset: 0x00003EC1
			public int tailLayer
			{
				get
				{
					return this.ty;
				}
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x00005CC9 File Offset: 0x00003EC9
			protected BaseSliderData(BeatmapSaveData.NoteColorType colorType, float headBeat, int headLine, int headLayer, NoteCutDirection headCutDirection, float tailBeat, int tailLine, int tailLayer) : base(headBeat)
			{
				this.c = colorType;
				this.x = headLine;
				this.y = headLayer;
				this.d = headCutDirection;
				this.tb = tailBeat;
				this.tx = tailLine;
				this.ty = tailLayer;
			}

			// Token: 0x0400018E RID: 398
			[SerializeField]
			protected BeatmapSaveData.NoteColorType c;

			// Token: 0x0400018F RID: 399
			[SerializeField]
			protected int x;

			// Token: 0x04000190 RID: 400
			[SerializeField]
			protected int y;

			// Token: 0x04000191 RID: 401
			[SerializeField]
			protected NoteCutDirection d;

			// Token: 0x04000192 RID: 402
			[SerializeField]
			protected float tb;

			// Token: 0x04000193 RID: 403
			[SerializeField]
			protected int tx;

			// Token: 0x04000194 RID: 404
			[SerializeField]
			protected int ty;
		}

		// Token: 0x02000065 RID: 101
		[Serializable]
		public class SliderData : BeatmapSaveData.BaseSliderData
		{
			// Token: 0x17000092 RID: 146
			// (get) Token: 0x060001B5 RID: 437 RVA: 0x00005D08 File Offset: 0x00003F08
			public float headControlPointLengthMultiplier
			{
				get
				{
					return this.mu;
				}
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x060001B6 RID: 438 RVA: 0x00005D10 File Offset: 0x00003F10
			public float tailControlPointLengthMultiplier
			{
				get
				{
					return this.tmu;
				}
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005D18 File Offset: 0x00003F18
			public NoteCutDirection tailCutDirection
			{
				get
				{
					return this.tc;
				}
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x060001B8 RID: 440 RVA: 0x00005D20 File Offset: 0x00003F20
			public SliderMidAnchorMode sliderMidAnchorMode
			{
				get
				{
					return this.m;
				}
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x00005D28 File Offset: 0x00003F28
			public SliderData(BeatmapSaveData.NoteColorType colorType, float headBeat, int headLine, int headLayer, float headControlPointLengthMultiplier, NoteCutDirection headCutDirection, float tailBeat, int tailLine, int tailLayer, float tailControlPointLengthMultiplier, NoteCutDirection tailCutDirection, SliderMidAnchorMode sliderMidAnchorMode) : base(colorType, headBeat, headLine, headLayer, headCutDirection, tailBeat, tailLine, tailLayer)
			{
				this.mu = headControlPointLengthMultiplier;
				this.tmu = tailControlPointLengthMultiplier;
				this.tc = tailCutDirection;
				this.m = sliderMidAnchorMode;
			}

			// Token: 0x04000195 RID: 405
			[SerializeField]
			protected float mu;

			// Token: 0x04000196 RID: 406
			[SerializeField]
			protected float tmu;

			// Token: 0x04000197 RID: 407
			[SerializeField]
			protected NoteCutDirection tc;

			// Token: 0x04000198 RID: 408
			[SerializeField]
			protected SliderMidAnchorMode m;
		}

		// Token: 0x02000066 RID: 102
		[Serializable]
		public class BurstSliderData : BeatmapSaveData.BaseSliderData
		{
			// Token: 0x17000096 RID: 150
			// (get) Token: 0x060001BA RID: 442 RVA: 0x00005D68 File Offset: 0x00003F68
			public int sliceCount
			{
				get
				{
					return this.sc;
				}
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x060001BB RID: 443 RVA: 0x00005D70 File Offset: 0x00003F70
			public float squishAmount
			{
				get
				{
					return this.s;
				}
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00005D78 File Offset: 0x00003F78
			public BurstSliderData(BeatmapSaveData.NoteColorType colorType, float headBeat, int headLine, int headLayer, NoteCutDirection headCutDirection, float tailBeat, int tailLine, int tailLayer, int sliceCount, float squishAmount) : base(colorType, headBeat, headLine, headLayer, headCutDirection, tailBeat, tailLine, tailLayer)
			{
				this.sc = sliceCount;
				this.s = squishAmount;
			}

			// Token: 0x04000199 RID: 409
			[SerializeField]
			protected int sc;

			// Token: 0x0400019A RID: 410
			[SerializeField]
			protected float s;
		}

		// Token: 0x02000067 RID: 103
		[Serializable]
		public class ObstacleData : BeatmapSaveData.BeatmapSaveDataItem
		{
			// Token: 0x17000098 RID: 152
			// (get) Token: 0x060001BD RID: 445 RVA: 0x00005DA8 File Offset: 0x00003FA8
			public int line
			{
				get
				{
					return this.x;
				}
			}

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x060001BE RID: 446 RVA: 0x00005DB0 File Offset: 0x00003FB0
			public int layer
			{
				get
				{
					return this.y;
				}
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x060001BF RID: 447 RVA: 0x00005DB8 File Offset: 0x00003FB8
			public float duration
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005DC0 File Offset: 0x00003FC0
			public int width
			{
				get
				{
					return this.w;
				}
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005DC8 File Offset: 0x00003FC8
			public int height
			{
				get
				{
					return this.h;
				}
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x00005DD0 File Offset: 0x00003FD0
			public ObstacleData(float beat, int line, int layer, float duration, int width, int height) : base(beat)
			{
				this.x = line;
				this.y = layer;
				this.d = duration;
				this.w = width;
				this.h = height;
			}

			// Token: 0x0400019B RID: 411
			[SerializeField]
			protected int x;

			// Token: 0x0400019C RID: 412
			[SerializeField]
			protected int y;

			// Token: 0x0400019D RID: 413
			[SerializeField]
			protected float d;

			// Token: 0x0400019E RID: 414
			[SerializeField]
			protected int w;

			// Token: 0x0400019F RID: 415
			[SerializeField]
			protected int h;
		}
	}
}
