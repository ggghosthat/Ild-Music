

using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Holder
{
    internal class TabHolder
    {
        private SubControlService subControlService;
        public IList<FactorySubControlTab> Tabs { get; private set; } = new List<FactorySubControlTab>();
        public TabHolder(SubControlService subControlService)
        {
            this.subControlService = subControlService;
            InitializeTabs();
        }
        private void InitializeTabs()
        {
            while (subControlService == null) { }
            Tabs.Add(new FactorySubControlTab(subControlService.UserSubControls[0], "Artist"));
            Tabs.Add(new FactorySubControlTab(subControlService.UserSubControls[1], "Playlist"));
            Tabs.Add(new FactorySubControlTab(subControlService.UserSubControls[2], "Track"));
        }
        public FactorySubControlTab DisplayInstance(ICoreEntity instance)
        {
            if (instance is Artist)
                return Tabs[0];
            if (instance is Playlist)
                return Tabs[1];
            if (instance is Track)
                return Tabs[2];

            return null;
        }
        public FactorySubControlTab DisplayInstance(int instance)
        {
            if (instance == 0)
                return Tabs[0];
            if (instance == 1)
                return Tabs[1];
            if (instance == 2)
                return Tabs[2];

            return null;
        }
    }
}
