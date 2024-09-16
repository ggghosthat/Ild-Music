using System;
using System.Net;
using Ild_Music.ViewModels.Base;

namespace Ild_Music.ViewModels;

public class AboutViewModel : BaseViewModel
{   
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    private const string ABOUT_PAGE_LINK = "https://raw.githubusercontent.com/ggghosthat/Projects/main/Ild-Music.about.md";


    public AboutViewModel()
    {
        DownloadAboutContent();
    }
    
    public string Content { get; set; }

    private void DownloadAboutContent()
    {
        while(!TryDownloadContent());
    }

    private bool TryDownloadContent()
    {
        int result = 0;

        using (var wc = new WebClient())
        {
            try
            {
                Content = wc.DownloadString(ABOUT_PAGE_LINK);
            }
            catch(Exception ex)
            {
                result = -1;
            }
        }

        return result == 0;
    }
}