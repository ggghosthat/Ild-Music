using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.Pagging;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class InstanceExplorerViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    private static SupportGhost supporterService => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public ObservableCollection<int> ActivePages { get; private set; } = new();
    public ObservableCollection<CommonInstanceDTO> Source {get; private set;} = new();
    public IList<CommonInstanceDTO> Output {get; set;} = new List<CommonInstanceDTO>();
    public string SearchQuery { get; set; }
    public int PageNumber { get; set; }
    public MetaData MetaData => supporterService.GetPageMetaData();
    
    public CommandDelegator CloseExplorerCommand {get;}
    public CommandDelegator ExitExplorerCommand {get;}
    public CommandDelegator ForwardCommand { get; }
    public CommandDelegator BackCommand { get; }
    public CommandDelegator IndexCommand { get; }
    public CommandDelegator SearchCommand { get; }
    public CommandDelegator CleanSearchCommand { get; }

    public event Action OnArtistsSelected;
    public event Action OnPlaylistsSelected;
    public event Action OnTracksSelected;
    public event Action OnTagsSelected;

    public InstanceExplorerViewModel()
    {
        CloseExplorerCommand = new (CloseExplorer, null);
        ExitExplorerCommand = new (ExitExplorer, null);
        ForwardCommand = new (Forward, null);
        BackCommand = new (Back, null);
        IndexCommand = new (Index, null);
        SearchCommand = new (Search, null);
        CleanSearchCommand = new (CleanSearch, null);
    }

    public async void Arrange(
        EntityTag entitytag,
        IEnumerable<CommonInstanceDTO> preselected = null)
    {
        Source.Clear();
        Output.Clear();
        ActivePages.Clear();

        supporterService.ResolveMetaData(0, 100, entitytag);

        for (int i = 1; i <= MetaData.TotalPages; i++)
            ActivePages.Add(i);

        supporterService
            .GetCurrentPage()
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));
       
        if (preselected != null)
            Output = (IList<CommonInstanceDTO>)preselected;
    }

    public void Search(object obj)
    {
        Source.Clear();

        supporterService
            .SearchInstance(SearchQuery, MetaData.EntityTag)
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));
    }

    public void CleanSearch(object obj)
    {
        Source.Clear();
        SearchQuery = String.Empty;

        supporterService
            .GetCurrentPage()
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));
    }

    public void Forward(object obj)
    {
        Source.Clear();
        supporterService.PageForward();

        supporterService
            .GetCurrentPage()
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));   
    }

    public void Back(object obj)
    {
        Source.Clear();
        supporterService.PageBack();

        supporterService
            .GetCurrentPage()
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));   
    }

    public void Index(object obj)
    {
        Source.Clear();

        supporterService
            .GetPage(PageNumber)
            .Result
            .ToList()
            .ForEach(item => Source.Add(item));   
    }

    public void CloseExplorer(object obj)
    {
        switch (MetaData.EntityTag)
        {
            case EntityTag.ARTIST:
                OnArtistsSelected?.Invoke();
                break;
            case EntityTag.PLAYLIST:
                OnPlaylistsSelected?.Invoke();
                break;
            case EntityTag.TRACK:
                OnTracksSelected?.Invoke();
                break;
            case EntityTag.TAG:
                OnTagsSelected?.Invoke();
                break;
        }

        MainVM.ResolveWindowStack();
    }

    public void ExitExplorer(object obj)
    {
        Source.Clear();
        Output.Clear();           
        MainVM.ResolveWindowStack();
    }
}
