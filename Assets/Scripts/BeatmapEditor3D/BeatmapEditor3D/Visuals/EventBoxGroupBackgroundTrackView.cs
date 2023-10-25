// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.EventBoxGroupBackgroundTrackView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class EventBoxGroupBackgroundTrackView : MonoBehaviour
  {
    [SerializeField]
    private LaneView _laneView;
    [Space]
    [SerializeField]
    private Transform _laneNameTransform;
    [SerializeField]
    private float _laneNameWidth;
    [SerializeField]
    private TextMeshPro _laneName;

        public void Initialize(string laneName, float laneWidth)
        {
            this._laneView.SetWidth(laneWidth);
            this._laneName.text = laneName;
            Vector3 localPosition = this._laneNameTransform.localPosition;
            localPosition.x = -(laneWidth + this._laneNameWidth) * 0.5f;
            this._laneNameTransform.localPosition = localPosition;
        }

        public class Pool : MonoMemoryPool<EventBoxGroupBackgroundTrackView>
    {
    }
  }
}
