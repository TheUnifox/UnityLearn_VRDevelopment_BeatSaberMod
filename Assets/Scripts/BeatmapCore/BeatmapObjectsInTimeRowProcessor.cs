using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class BeatmapObjectsInTimeRowProcessor
{
    // Token: 0x06000101 RID: 257 RVA: 0x00003E84 File Offset: 0x00002084
    public BeatmapObjectsInTimeRowProcessor(int numberOfLines)
    {
        this._numberOfLines = numberOfLines;
        this._notesInColumnsReusableProcessingListOfLists = new List<NoteData>[numberOfLines];
        for (int i = 0; i < numberOfLines; i++)
        {
            this._notesInColumnsReusableProcessingListOfLists[i] = new List<NoteData>(3);
        }
        foreach (ColorType key in (ColorType[])Enum.GetValues(typeof(ColorType)))
        {
            BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData> timeSliceContainer = new BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData>(4);
            timeSliceContainer.didFinishTimeSliceEvent += this.HandlePerColorTypeTimeSliceContainerDidFinishTimeSlice;
            this._currentTimeSliceNotesByColorType[key] = timeSliceContainer;
        }
        this._currentTimeSliceAllNotesAndSliders.didFinishTimeSliceEvent += this.HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice;
        this._currentTimeSliceAllNotesAndSliders.didStartNewTimeSliceEvent += this.HandleCurrentNewTimeSliceAllNotesAndSlidersDidStartNewTimeSlice;
        this._currentTimeSliceColorNotes.didFinishTimeSliceEvent += this.HandleCurrentTimeSliceColorNotesDidFinishTimeSlice;
        this._currentTimeSliceColorNotes.didAddItemEvent += this.HandleCurrentTimeSliceColorNotesDidAddItem;
    }

    // Token: 0x06000102 RID: 258 RVA: 0x00003FA0 File Offset: 0x000021A0
    public virtual void ProcessNote(NoteData noteData)
    {
        if (noteData.colorType != ColorType.None && noteData.cutDirection != NoteCutDirection.None)
        {
            this._currentTimeSliceColorNotes.Add(noteData);
        }
        this._currentTimeSliceNotesByColorType[noteData.colorType].Add(noteData);
        this._currentTimeSliceAllNotesAndSliders.Add(noteData);
    }

    // Token: 0x06000103 RID: 259 RVA: 0x00003FF0 File Offset: 0x000021F0
    public virtual void ProcessSlider(SliderData sliderData)
    {
        this._currentTimeSliceAllNotesAndSliders.Add(sliderData);
        bool flag = false;
        for (int i = 0; i < this._unprocessedSliderTails.Count; i++)
        {
            if (this._unprocessedSliderTails[i].tailTime > sliderData.tailTime)
            {
                this._unprocessedSliderTails.Insert(i, sliderData);
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            this._unprocessedSliderTails.Add(sliderData);
        }
    }

    // Token: 0x06000104 RID: 260 RVA: 0x0000405C File Offset: 0x0000225C
    public virtual void ProcessAllRemainingData()
    {
        this._currentTimeSliceColorNotes.FinishTimeSlice(float.MaxValue);
        this._currentTimeSliceAllNotesAndSliders.FinishTimeSlice(float.MaxValue);
        foreach (KeyValuePair<ColorType, BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData>> keyValuePair in this._currentTimeSliceNotesByColorType)
        {
            keyValuePair.Value.FinishTimeSlice(float.MaxValue);
        }
        this._unprocessedSliderTails.Clear();
    }

    // Token: 0x06000105 RID: 261 RVA: 0x000040E4 File Offset: 0x000022E4
    public virtual void HandleCurrentTimeSliceColorNotesDidAddItem(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData> timeSliceContainer, NoteData noteData)
    {
        noteData.timeToNextColorNote = float.MaxValue;
        noteData.timeToPrevColorNote = noteData.time - timeSliceContainer.previousTimeSliceTime;
    }

    // Token: 0x06000106 RID: 262 RVA: 0x00004104 File Offset: 0x00002304
    public virtual void HandleCurrentNewTimeSliceAllNotesAndSlidersDidStartNewTimeSlice(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> allObjectsTimeSlice)
    {
        float time = allObjectsTimeSlice.time;
        while (this._unprocessedSliderTails.Count > 0)
        {
            if (this._unprocessedSliderTails[0].tailTime >= time - 0.001f)
            {
                break;
            }
            this._unprocessedSliderTails.RemoveAt(0);
        }
        while (this._unprocessedSliderTails.Count > 0 && Mathf.Abs(this._unprocessedSliderTails[0].tailTime - time) < 0.001f)
        {
            allObjectsTimeSlice.AddWithoutNotifications(new BeatmapObjectsInTimeRowProcessor.SliderTailData(this._unprocessedSliderTails[0]));
            this._unprocessedSliderTails.RemoveAt(0);
        }
    }

    // Token: 0x06000107 RID: 263 RVA: 0x000041A0 File Offset: 0x000023A0
    public virtual void HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> allObjectsTimeSlice, float nextTimeSliceTime)
    {
        List<NoteData>[] notesInColumnsReusableProcessingListOfLists = this._notesInColumnsReusableProcessingListOfLists;
        for (int i = 0; i < notesInColumnsReusableProcessingListOfLists.Length; i++)
        {
            notesInColumnsReusableProcessingListOfLists[i].Clear();
        }
        IEnumerable<NoteData> enumerable = allObjectsTimeSlice.items.OfType<NoteData>();
        IEnumerable<SliderData> enumerable2 = allObjectsTimeSlice.items.OfType<SliderData>();
        IEnumerable<BeatmapObjectsInTimeRowProcessor.SliderTailData> enumerable3 = allObjectsTimeSlice.items.OfType<BeatmapObjectsInTimeRowProcessor.SliderTailData>();
        foreach (NoteData noteData in enumerable)
        {
            List<NoteData> list = this._notesInColumnsReusableProcessingListOfLists[noteData.lineIndex];
            bool flag = false;
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].noteLineLayer > noteData.noteLineLayer)
                {
                    list.Insert(j, noteData);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                list.Add(noteData);
            }
        }
        for (int k = 0; k < this._notesInColumnsReusableProcessingListOfLists.Length; k++)
        {
            List<NoteData> list2 = this._notesInColumnsReusableProcessingListOfLists[k];
            for (int l = 0; l < list2.Count; l++)
            {
                list2[l].SetBeforeJumpNoteLineLayer((NoteLineLayer)l);
            }
        }
        foreach (SliderData sliderData in enumerable2)
        {
            foreach (NoteData noteData2 in enumerable)
            {
                if (BeatmapObjectsInTimeRowProcessor.SliderHeadPositionOverlapsWithNote(sliderData, noteData2))
                {
                    sliderData.SetHasHeadNote(true);
                    sliderData.SetHeadBeforeJumpLineLayer(noteData2.beforeJumpNoteLineLayer);
                    if (sliderData.sliderType == SliderData.Type.Burst)
                    {
                        noteData2.ChangeToBurstSliderHead();
                        if (noteData2.cutDirection == sliderData.tailCutDirection)
                        {
                            Vector2 line = StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(noteData2.lineIndex, this._numberOfLines, noteData2.noteLineLayer) - StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(sliderData.tailLineIndex, this._numberOfLines, sliderData.tailLineLayer);
                            float num = noteData2.cutDirection.Direction().SignedAngleToLine(line);
                            if (Mathf.Abs(num) <= 40f)
                            {
                                noteData2.SetCutDirectionAngleOffset(num);
                                sliderData.SetCutDirectionAngleOffset(num, num);
                            }
                        }
                    }
                    else
                    {
                        noteData2.ChangeToSliderHead();
                    }
                }
            }
        }
        foreach (SliderData sliderData2 in enumerable2)
        {
            foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData in enumerable3)
            {
                if (BeatmapObjectsInTimeRowProcessor.SliderTailPositionOverlapsWithBurstSliderHead(sliderData2, sliderTailData.slider))
                {
                    sliderData2.SetHasHeadNote(true);
                    sliderData2.SetHeadBeforeJumpLineLayer(sliderTailData.slider.tailBeforeJumpLineLayer);
                }
            }
        }
        foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData2 in enumerable3)
        {
            SliderData slider = sliderTailData2.slider;
            foreach (NoteData noteData3 in enumerable)
            {
                if (BeatmapObjectsInTimeRowProcessor.SliderTailPositionOverlapsWithNote(slider, noteData3))
                {
                    slider.SetHasTailNote(true);
                    slider.SetTailBeforeJumpLineLayer(noteData3.beforeJumpNoteLineLayer);
                    noteData3.ChangeToSliderTail();
                }
            }
        }
    }

    // Token: 0x06000108 RID: 264 RVA: 0x00004590 File Offset: 0x00002790
    public virtual void HandleCurrentTimeSliceColorNotesDidFinishTimeSlice(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData> currentTimeSlice, float nextTimeSliceTime)
    {
        IReadOnlyList<NoteData> items = currentTimeSlice.items;
        foreach (NoteData noteData in items)
        {
            noteData.timeToNextColorNote = nextTimeSliceTime - noteData.time;
        }
        float currentTimeSliceTime = currentTimeSlice.time;
        if (items.Count != 2)
        {
            return;
        }
        bool flag;
        if (Math.Abs(this._currentTimeSliceAllNotesAndSliders.time - currentTimeSliceTime) < 0.001f)
        {
            if (this._currentTimeSliceAllNotesAndSliders.items.Any((BeatmapDataItem item) => item is SliderData || item is BeatmapObjectsInTimeRowProcessor.SliderTailData))
            {
                flag = true;
                goto IL_C6;
            }
        }
        flag = this._unprocessedSliderTails.Any((SliderData tail) => Math.Abs(tail.tailTime - currentTimeSliceTime) < 0.001f);
    IL_C6:
        if (flag)
        {
            return;
        }
        NoteData noteData2 = items[0];
        NoteData noteData3 = items[1];
        if (noteData2.colorType != noteData3.colorType && ((noteData2.colorType == ColorType.ColorA && noteData2.lineIndex > noteData3.lineIndex) || (noteData2.colorType == ColorType.ColorB && noteData2.lineIndex < noteData3.lineIndex)))
        {
            noteData2.SetNoteFlipToNote(noteData3);
            noteData3.SetNoteFlipToNote(noteData2);
        }
    }

    // Token: 0x06000109 RID: 265 RVA: 0x000046D0 File Offset: 0x000028D0
    public virtual void HandlePerColorTypeTimeSliceContainerDidFinishTimeSlice(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData> timeSliceContainer, float nextTimeSliceTime)
    {
        IReadOnlyList<NoteData> items = timeSliceContainer.items;
        if (items.Count != 2)
        {
            return;
        }
        NoteData noteData = items[0];
        NoteData noteData2 = items[1];
        if (noteData.cutDirection != noteData2.cutDirection && noteData.cutDirection != NoteCutDirection.Any && noteData2.cutDirection != NoteCutDirection.Any)
        {
            return;
        }
        NoteData noteData3;
        NoteData noteData4;
        if (noteData.cutDirection != NoteCutDirection.Any)
        {
            noteData3 = noteData;
            noteData4 = noteData2;
        }
        else
        {
            noteData3 = noteData2;
            noteData4 = noteData;
        }
        Vector2 line = StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(noteData4.lineIndex, this._numberOfLines, noteData4.noteLineLayer) - StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(noteData3.lineIndex, this._numberOfLines, noteData3.noteLineLayer);
        float num = ((noteData3.cutDirection == NoteCutDirection.Any) ? new Vector2(0f, 1f) : noteData3.cutDirection.Direction()).SignedAngleToLine(line);
        if (noteData4.cutDirection == NoteCutDirection.Any && noteData3.cutDirection == NoteCutDirection.Any)
        {
            noteData3.SetCutDirectionAngleOffset(num);
            noteData4.SetCutDirectionAngleOffset(num);
            return;
        }
        if (Mathf.Abs(num) > 40f)
        {
            return;
        }
        noteData3.SetCutDirectionAngleOffset(num);
        if (noteData4.cutDirection == NoteCutDirection.Any && !noteData3.cutDirection.IsMainDirection())
        {
            noteData4.SetCutDirectionAngleOffset(num + 45f);
            return;
        }
        noteData4.SetCutDirectionAngleOffset(num);
    }

    // Token: 0x0600010A RID: 266 RVA: 0x00004805 File Offset: 0x00002A05
    private static bool SliderHeadPositionOverlapsWithNote(SliderData slider, NoteData note)
    {
        return slider.headLineIndex == note.lineIndex && slider.headLineLayer == note.noteLineLayer;
    }

    // Token: 0x0600010B RID: 267 RVA: 0x00004825 File Offset: 0x00002A25
    private static bool SliderTailPositionOverlapsWithNote(SliderData slider, NoteData note)
    {
        return slider.tailLineIndex == note.lineIndex && slider.tailLineLayer == note.noteLineLayer;
    }

    // Token: 0x0600010C RID: 268 RVA: 0x00004845 File Offset: 0x00002A45
    private static bool SliderTailPositionOverlapsWithBurstSliderHead(SliderData slider, SliderData sliderTail)
    {
        return slider.sliderType == SliderData.Type.Normal && sliderTail.sliderType == SliderData.Type.Burst && slider.headLineIndex == sliderTail.tailLineIndex && slider.headLineLayer == sliderTail.tailLineLayer;
    }

    // Token: 0x040000EC RID: 236
    protected readonly BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData> _currentTimeSliceColorNotes = new BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData>(8);

    // Token: 0x040000ED RID: 237
    protected readonly BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> _currentTimeSliceAllNotesAndSliders = new BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem>(8);

    // Token: 0x040000EE RID: 238
    protected readonly Dictionary<ColorType, BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData>> _currentTimeSliceNotesByColorType = new Dictionary<ColorType, BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<NoteData>>();

    // Token: 0x040000EF RID: 239
    protected readonly List<SliderData> _unprocessedSliderTails = new List<SliderData>(4);

    // Token: 0x040000F0 RID: 240
    protected readonly List<NoteData>[] _notesInColumnsReusableProcessingListOfLists;

    // Token: 0x040000F1 RID: 241
    protected readonly int _numberOfLines;

    // Token: 0x040000F2 RID: 242
    protected const float kTimeRowEpsilon = 0.001f;

    // Token: 0x040000F3 RID: 243
    protected const float kMaxNotesAlignmentAngle = 40f;

    // Token: 0x02000035 RID: 53
    public class TimeSliceContainer<T> where T : BeatmapDataItem
    {
        // Token: 0x1700003E RID: 62
        // (get) Token: 0x0600010D RID: 269 RVA: 0x00004876 File Offset: 0x00002A76
        // (set) Token: 0x0600010E RID: 270 RVA: 0x0000487E File Offset: 0x00002A7E
        public float time { get; private set; } = -1f;

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x0600010F RID: 271 RVA: 0x00004887 File Offset: 0x00002A87
        // (set) Token: 0x06000110 RID: 272 RVA: 0x0000488F File Offset: 0x00002A8F
        public float previousTimeSliceTime { get; private set; } = -1f;

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x06000111 RID: 273 RVA: 0x00004898 File Offset: 0x00002A98
        public IReadOnlyList<T> items
        {
            get
            {
                return this._items;
            }
        }

        public Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> didFinishTimeSliceEventType;
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000112 RID: 274 RVA: 0x000048A0 File Offset: 0x00002AA0
        // (remove) Token: 0x06000113 RID: 275 RVA: 0x000048D8 File Offset: 0x00002AD8
        public event Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> didFinishTimeSliceEvent
        {
            [CompilerGenerated]
            add
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> action = this.didFinishTimeSliceEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> value2 = (Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float>)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float>>(ref this.didFinishTimeSliceEventType, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> action = this.didFinishTimeSliceEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float> value2 = (Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >, float >)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, float>>(ref this.didFinishTimeSliceEventType, value2, action2);
                }
                while (action != action2);
            }
        }

        public Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> didStartNewTimeSliceEventType;
        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000114 RID: 276 RVA: 0x00004910 File Offset: 0x00002B10
        // (remove) Token: 0x06000115 RID: 277 RVA: 0x00004948 File Offset: 0x00002B48
        public event Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> didStartNewTimeSliceEvent
        {
            [CompilerGenerated]
            add
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> action = this.didStartNewTimeSliceEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> value2 = (Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >>)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>>>(ref this.didStartNewTimeSliceEventType, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> action = this.didStartNewTimeSliceEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>> value2 = (Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >>)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>>>(ref this.didStartNewTimeSliceEventType, value2, action2);
                }
                while (action != action2);
            }
        }

        public Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> didAddItemEventType;
        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000116 RID: 278 RVA: 0x00004980 File Offset: 0x00002B80
        // (remove) Token: 0x06000117 RID: 279 RVA: 0x000049B8 File Offset: 0x00002BB8
        public event Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> didAddItemEvent
        {
            [CompilerGenerated]
            add
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> action = this.didAddItemEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> value2 = (Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >, T >)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T>>(ref this.didAddItemEventType, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> action = this.didAddItemEventType;
                Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> action2;
                do
                {
                    action2 = action;
                    Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T> value2 = (Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >, T >)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange<Action<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<T>, T>>(ref this.didAddItemEventType, value2, action2);
                }
                while (action != action2);
            }
        }

        // Token: 0x06000118 RID: 280 RVA: 0x000049ED File Offset: 0x00002BED
        public TimeSliceContainer(int capacity)
        {
            this._items = new List<T > (capacity);
        }

        // Token: 0x06000119 RID: 281 RVA: 0x00004A18 File Offset: 0x00002C18
        public virtual void Add(T item)
        {
            if (item.time > this.time + 0.001f)
            {
                this.FinishTimeSlice(item.time);
                this.StartNewTimeSlice(item.time);
            }
            this._items.Add(item);
            Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >, T > action = this.didAddItemEventType;
            if (action == null)
            {
                return;
            }
            action(this, item);
        }

        // Token: 0x0600011A RID: 282 RVA: 0x00004A7E File Offset: 0x00002C7E
        public virtual void AddWithoutNotifications(T item)
        {
            this._items.Add(item);
        }

        // Token: 0x0600011B RID: 283 RVA: 0x00004A8C File Offset: 0x00002C8C
        public virtual void FinishTimeSlice(float nextTimeSliceTime)
        {
            if (this._items.Count > 0)
            {
                Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >, float> action = this.didFinishTimeSliceEventType;
                if (action != null)
                {
                    action(this, nextTimeSliceTime);
                }
                this._items.Clear();
            }
        }

        // Token: 0x0600011C RID: 284 RVA: 0x00004ABA File Offset: 0x00002CBA
        public virtual void StartNewTimeSlice(float newSliceTime)
        {
            this.previousTimeSliceTime = this.time;
            this.time = newSliceTime;
            Action < BeatmapObjectsInTimeRowProcessor.TimeSliceContainer < T >> action = this.didStartNewTimeSliceEventType;
            if (action == null)
            {
                return;
            }
            action(this);
        }

        // Token: 0x040000F9 RID: 249
        protected readonly List<T> _items;
    }

    // Token: 0x02000036 RID: 54
    public class SliderTailData : BeatmapDataItem
    {
        // Token: 0x0600011D RID: 285 RVA: 0x00004AE0 File Offset: 0x00002CE0
        public SliderTailData(SliderData slider) : base(slider.tailTime, slider.executionOrder, slider.subtypeIdentifier, slider.type)
        {
            this.slider = slider;
        }

        // Token: 0x0600011E RID: 286 RVA: 0x0000236C File Offset: 0x0000056C
        public override BeatmapDataItem GetCopy()
        {
            return this;
        }

        // Token: 0x040000FA RID: 250
        public readonly SliderData slider;
    }
}
