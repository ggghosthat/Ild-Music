using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using ShareInstances.Stage;
using ShareInstances.Filer;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;
public class BrowseViewModel : BaseViewModel
{
	public static readonly string nameVM = "BrowseVM";        
    public override string NameVM => nameVM;

    #region Services
    private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    private Stage Stage => App.Stage;
    #endregion

    #region Properties
    public ObservableCollection<MusicFile> Items {get; private set;} = new();
    public MusicFile SelectedItem {get; set;}
    #endregion

    #region Commands
    public CommandDelegator BrowseFromManagerCommand { get; }
    public CommandDelegator CancelCommand { get; }
    #endregion

    public BrowseViewModel()
    {
        BrowseFromManagerCommand = new(BrowseFromManager, null);
        CancelCommand = new(Cancel, null);
    }

    #region Private methods
    private async Task UpdateItems()
    {
        Items.Clear();
        IList<MusicFile> musicFiles = Stage.Filer.GetMusicFiles();
        musicFiles.ToList()
                  .ForEach(mf => Items.Add(mf));
    }
    #endregion

    #region Public methods
    public async Task Browse(IEnumerable<string> paths)
    {
        await Stage.Filer.BrowseFiles(paths);
        await UpdateItems();
    }
    #endregion

    #region Command Methods
    private void BrowseFromManager(object obj)
    {
        Console.WriteLine("Your want to browse from file system music file.");
    }

    private void Cancel(object obj)
    {
        Console.WriteLine("xxx");
        if(obj is MusicFile mf)
        {
            Console.WriteLine(mf.FileName);
            Items.Remove(mf);
            OnPropertyChanged("Items");
        }
    }
    #endregion
}

