using System.Reflection;

namespace Ild_Music.Assets;
       
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[System.Diagnostics.DebuggerNonUserCodeAttribute()]
[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
public class Resources 
{    
    private static System.Resources.ResourceManager resourceMan;
    
    private static System.Globalization.CultureInfo resourceCulture;
    
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resources() 
    {}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Resources.ResourceManager ResourceManager 
    {
        get
        {
            if (object.Equals(null, resourceMan)) 
            {
                var temp = new System.Resources.ResourceManager("Ild-Music.Assets.Languages.Resources", Assembly.GetExecutingAssembly());
                resourceMan = temp;
            }
            return resourceMan;
        }
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Globalization.CultureInfo Culture
    {
        get => resourceCulture;
        set => resourceCulture = value;
    }
    
    public static string SearchBarWatermark => ResourceManager.GetString("SearchBarWatermark", resourceCulture);

    public static string HomeNavbarItem => ResourceManager.GetString("HomeNavbarItem", resourceCulture);

    public static string ListNavbarItem => ResourceManager.GetString("ListNavbarItem", resourceCulture);

    public static string BrowseNavbarItem => ResourceManager.GetString("BrowseNavbarItem", resourceCulture);
    
    public static string AboutNavbarItem => ResourceManager.GetString("AboutNavbarItem", resourceCulture);

    public static string ArtistLabelStartView => ResourceManager.GetString("ArtistLabelStartView", resourceCulture);

    public static string PlaylistLabelStartView => ResourceManager.GetString("PlaylistLabelStartView", resourceCulture);

    public static string TrackLabelStartView => ResourceManager.GetString("TrackLabelStartView", resourceCulture);
        
    public static string ArtistHeaderListView => ResourceManager.GetString("ArtistHeaderListView", resourceCulture);
    
    public static string PlaylistHeaderListView => ResourceManager.GetString("PlaylistHeaderListView", resourceCulture);
    
    public static string TrackHeaderListView => ResourceManager.GetString("TrackHeaderListView", resourceCulture);

    public static string BrowserTitle => ResourceManager.GetString("BrowserTitle", resourceCulture);

    public static string BrowserDragDropArea => ResourceManager.GetString("BrowserDragDropArea", resourceCulture);
    
    public static string EmptyBannerFace => ResourceManager.GetString("EmptyBannerFace", resourceCulture);

    public static string EmptyBannerText => ResourceManager.GetString("EmptyBannerText", resourceCulture);

    public static string NameArtistEdtorView => ResourceManager.GetString("NameArtistEdtorView", resourceCulture);

    public static string DescriptionArtistEditorView => ResourceManager.GetString("DescriptionArtistEditorView", resourceCulture);

    public static string YearArtistEditorView => ResourceManager.GetString("YearArtistEditorView", resourceCulture);

    public static string ApplyButtonArtistEditorView => ResourceManager.GetString("ApplyButtonArtistEditorView", resourceCulture);
    
    public static string CancelButtonArtistEditorView => ResourceManager.GetString("CancelButtonArtistEditorView", resourceCulture);

    public static string NamePlaylistEditorView => ResourceManager.GetString("NamePlaylistEditorView", resourceCulture);

    public static string DescriptionPlaylistEditorView => ResourceManager.GetString("DescriptionPlaylistEditorView", resourceCulture);

    public static string YearPlaylistEditorView => ResourceManager.GetString("YearPlaylistEditorView", resourceCulture);

    public static string ApplyButtonPlaylistEditorView => ResourceManager.GetString("ApplyButtonPlaylistEditorView", resourceCulture);
    
    public static string CancelButtonPlaylistEditorView => ResourceManager.GetString("CancelButtonPlaylistEditorView", resourceCulture);
    
    public static string FoldingPathTrackEditorView => ResourceManager.GetString("FoldingPathTrackEditorView", resourceCulture);
    
    public static string NameTrackEditorView => ResourceManager.GetString("NameTrackEditorView", resourceCulture);
    
    public static string DescriptionTrackEditorView => ResourceManager.GetString("DescriptionTrackEditorView", resourceCulture);
    
    public static string YearTrackEditorView => ResourceManager.GetString("YearTrackEditorView", resourceCulture);
    
    public static string ApplyButtonTrackEditorView => ResourceManager.GetString("ApplyButtonTrackEditorView", resourceCulture);
    
    public static string CancelButtonTrackEditorView => ResourceManager.GetString("CancelButtonTrackEditorView", resourceCulture);

    public static string SearBarInstanceExplorer => ResourceManager.GetString("SearBarInstanceExplorer", resourceCulture);

    public static string SearchButtonInstanceExplorer => ResourceManager.GetString("SearchButtonInstanceExplorer", resourceCulture);
    
    public static string TitleFailedBootView => ResourceManager.GetString("TitleFailedBootView", resourceCulture);
    
    public static string PhraseFailedBootView => ResourceManager.GetString("PhraseFailedBootView", resourceCulture);
    
    public static string ExportFailedBootView => ResourceManager.GetString("ExportFailedBootView", resourceCulture);
    
    public static string ExitFailedBootView => ResourceManager.GetString("ExitFailedBootView", resourceCulture);

    public static string ArtistViewNoPlaylists => ResourceManager.GetString("ArtistViewNoPlaylists", resourceCulture);

    public static string ArtistViewNoTracks => ResourceManager.GetString("ArtistViewNoTracks", resourceCulture);

    public static string PlaylistViewNoArtists => ResourceManager.GetString("PlaylistViewNoArtists", resourceCulture);
    
    public static string PlaylistViewNoTracks => ResourceManager.GetString("PlaylistViewNoTracks", resourceCulture);

    public static string TrackViewNoArtists => ResourceManager.GetString("TrackViewNoArtists", resourceCulture);
}