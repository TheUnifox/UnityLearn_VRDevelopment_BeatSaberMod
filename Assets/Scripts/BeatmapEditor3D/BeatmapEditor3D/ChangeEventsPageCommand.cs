// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChangeEventsPageCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChangeEventsPageCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      EnvironmentTracksDefinitionSO.BasicEventTrackPage currentEventsPage = this._basicEventsState.currentEventsPage;
      EnvironmentTracksDefinitionSO.BasicEventTrackPage page = (EnvironmentTracksDefinitionSO.BasicEventTrackPage) ((int) (currentEventsPage + 1) % 2);
      if (this._beatmapDataModel.environmentTrackDefinition[page].Count == 0)
        return;
      this._basicEventsState.currentEventsPage = page;
      this._basicEventsState.currentHoverPageTrackId = 0;
      this._signalBus.Fire<EventsPageChangedSignal>(new EventsPageChangedSignal()
      {
        prevPage = currentEventsPage,
        newPage = page
      });
    }
  }
}
