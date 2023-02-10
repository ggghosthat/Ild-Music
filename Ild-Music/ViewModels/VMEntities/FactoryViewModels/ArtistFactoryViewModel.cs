using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
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
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Commands
        public CommandDelegator CreateArtistCommand { get; }
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
        public bool IsEditMode = false;
        #endregion

        #region Const
        public ArtistFactoryViewModel()
        {
            CreateArtistCommand = new(CreateArtist, null);
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
        }
        #endregion

        #region Public Methods
        public void CreateArtistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
             
                if (!string.IsNullOrEmpty(name))
                {
                    factoryService.CreateArtist(name, description);
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

                if (!string.IsNullOrEmpty(name))
                {
                    var editArtist = (Artist)Instance;
                    editArtist.Name = name;
                    editArtist.Description = description;
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
            }
        }
        #endregion

        #region Command Methods
        private void CreateArtist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription };

            if (IsEditMode == false)
                CreateArtistInstance(value);
            else 
                EditArtistInstance(value); 
        }
        #endregion
    }
}
