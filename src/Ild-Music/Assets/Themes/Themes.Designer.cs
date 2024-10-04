using System.Reflection;
using Avalonia.Media;

namespace Ild_Music.Assets;
       
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[System.Diagnostics.DebuggerNonUserCodeAttribute()]
[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
public class Themes
{ 
    private static System.Resources.ResourceManager resourceMan;
    
    private static string themeName = "default";
    
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Themes() 
    {}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Resources.ResourceManager ResourceManager 
    {
        get
        {
            if (object.Equals(null, resourceMan)) 
            {
                var temp = new System.Resources.ResourceManager($"Ild-Music.Assets.Themes.{themeName}", Assembly.GetExecutingAssembly());
                resourceMan = temp;
            }
            return resourceMan;
        }
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static string ThemeName
    {
        get => themeName;
        set => themeName = value;
    }

    private static SolidColorBrush ToSolidColor(string hexName)
    {
        string hex = ResourceManager.GetString(hexName);
        var color = Color.Parse(hex);
        return new SolidColorBrush(color);
    }

    private static Color ToColor(string hexName)
    {
        string hex = ResourceManager.GetString(hexName);
        return Color.Parse(hex);
    }

    public static SolidColorBrush MainColor => ToSolidColor("MainColor");
    
    public static SolidColorBrush GlobalColor1 => ToSolidColor("GlobalColor1");
    
    public static SolidColorBrush GlobalColor2 => ToSolidColor("GlobalColor2");
    
    public static SolidColorBrush MainWindowButton => ToSolidColor("MainWindowButton");
    
    public static SolidColorBrush MainWindowButtonOver => ToSolidColor("MainWindowButtonOver");
    
    public static SolidColorBrush MainWindowNavBar => ToSolidColor("MainWindowNavBar");
    
    public static SolidColorBrush MainWindowSearchButton => ToSolidColor("MainWindowSearchButton");
    
    public static SolidColorBrush MainWindowSearchButtonOver => ToSolidColor("MainWindowSearchButtonOver");
    
    public static SolidColorBrush MainWindowBackground => ToSolidColor("MainWindowBackground");
    
    public static SolidColorBrush MainWindowVolumeArea => ToSolidColor("MainWindowVolumeArea");
    
    public static SolidColorBrush MainWindowCurrentInstanceArea => ToSolidColor("MainWindowCurrentInstanceArea");
    
    public static SolidColorBrush ListBoxOver => ToSolidColor("ListBoxOver");
    
    public static SolidColorBrush ListBoxSelected => ToSolidColor("ListBoxSelected");
    
    public static SolidColorBrush Transparent => ToSolidColor("Transparent");
    
    public static SolidColorBrush TextColor => ToSolidColor("TextColor");

    public static Color PlayerGradient1 => ToColor("PlayerGradient1");
    
    public static Color PlayerGradient2 => ToColor("PlayerGradient2");

    public static SolidColorBrush MainWindowNavBarBackground => ToSolidColor("MainWindowNavBarBackground");

    public static SolidColorBrush MainWindowSearchArea => ToSolidColor("MainWindowSearchArea");

    public static SolidColorBrush TextColor1 => ToSolidColor("TextColor1");
}