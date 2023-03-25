using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class ArtistFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "ArtistFactoryVM";
        public override string NameVM => nameVM;
        
        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Instance
        public static ICoreEntity Instance { get; private set; }
        #endregion

        #region Avatar Avatar
        public byte[] AvatarSource {get; private set;} = null;
        #endregion

        #region Commands
        public CommandDelegator SelectAvatarCommand { get; }
        public CommandDelegator CreateArtistCommand { get; }
        public CommandDelegator CancelCommand { get; }
        #endregion

        #region Artist Factory Properties
        public string ArtistName {get; set; }
        public string ArtistDescription { get; set; }
        #endregion

        #region Log Reply Properties
        public string ArtistLogLine { get; set; }
        public bool ArtistLogError { get; set; }
        #endregion

        #region Properties
        public bool IsEditMode {get; private set;} = false;
        public string ViewHeader {get; private set;} = "Artist";
        #endregion

        #region Const
        public ArtistFactoryViewModel()
        {
            SelectAvatarCommand = new(SelectAvatar, null);
            CreateArtistCommand = new(CreateArtist, null);
            CancelCommand = new(Cancel, null);

            // AvatarSource = (Instance is not null)?Instance.GetAvatar():null;
        }
        #endregion


        #region Private Methods
        private void ExitFactory()
        {
            FieldsClear();
            MainVM.ResolveWindowStack();
        }

        private async Task FieldsClear()
        {
            ArtistName = default;
            ArtistDescription = default;
            ArtistLogLine = default;
            AvatarSource = default;
            Instance = default;
        }

        private async Task<byte[]> LoadAvatar(string path)
        {
            byte[] result;

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
            }
            return result;
        }
        #endregion

        #region Public Methods
        public void CreateArtistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var avatar = (byte[])values[2];

                if (!string.IsNullOrEmpty(name))
                {
                    var avatarBase64 = (avatar is not null)?Convert.ToBase64String(avatar):null;
                    factoryService.CreateArtist(name, description, avatarBase64);
                    ArtistLogLine = "Successfully created!";
                    ExitFactory();
                }
            }
            catch (InvalidArtistException ex)
            {
                ArtistLogLine = ex.Message;
            }
        }

        public void EditArtistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var avatar = (byte[])values[2];

                if (!string.IsNullOrEmpty(name))
                {
                    var editArtist = (Artist)Instance;
                    editArtist.Name = name;
                    editArtist.Description = description;
                    editArtist.AvatarBase64 = (avatar is not null)?Convert.ToBase64String(avatar):null;
                    supporterService.EditInstance(editArtist); 

                    IsEditMode = false;
                    ExitFactory();                
                }
            }
            catch(Exception exm)
            {
                ArtistLogLine = exm.Message;
            }
        }

        public void DropInstance(ICoreEntity entity) 
        {
            Instance = entity;
            IsEditMode = true;
            if (entity is Artist artist)
            {
                ArtistName = artist.Name;
                ArtistDescription = artist.Description;

                AvatarSource = (Instance is not null)?Instance.GetAvatar():null;
                OnPropertyChanged("AvatarSource");
            }
        }
        #endregion

        #region Command Methods
        private void Cancel(object obj)
        {
            ExitFactory();
        }


        private void CreateArtist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription, AvatarSource };
            if (IsEditMode == false)
            {
                CreateArtistInstance(value);
            }
            else 
            {
                EditArtistInstance(value); 
            }
        }

        private async void SelectAvatar(object obj)
        {
            OpenFileDialog dialog = new();
            string[] result = await dialog.ShowAsync(new Window());
            if(result != null && result.Length > 0)
            {
                var avatarPath = string.Join(" ", result);
                var avatar64 = await LoadAvatar(avatarPath);
                AvatarSource = avatar64;
                OnPropertyChanged("AvatarSource");
            }
        }
        #endregion
    }
}
