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

    public static SolidColorBrush MainColor => ToSolidColor("Color1");
    
    public static SolidColorBrush GlobalColor1 => ToSolidColor("Color2");
    
    public static SolidColorBrush GlobalColor2 => ToSolidColor("Color3");
    
    public static SolidColorBrush MainWindowButton => ToSolidColor("Color4");
    
    public static SolidColorBrush MainWindowButtonOver => ToSolidColor("Color5");
    
    public static SolidColorBrush MainWindowNavBar => ToSolidColor("Color6");
    
    public static SolidColorBrush MainWindowSearchButton => ToSolidColor("Color7");
    
    public static SolidColorBrush MainWindowSearchButtonOver => ToSolidColor("Color8");
    
    public static SolidColorBrush MainWindowBackground => ToSolidColor("Color9");
    
    public static SolidColorBrush MainWindowVolumeArea => ToSolidColor("Color10");
    
    public static SolidColorBrush MainWindowCurrentInstanceArea => ToSolidColor("Color11");
    
    public static SolidColorBrush ListBoxOver => ToSolidColor("Color12");
    
    public static SolidColorBrush ListBoxSelected => ToSolidColor("Color13");
    
    public static SolidColorBrush Transparent => ToSolidColor("Color14");
    
    public static SolidColorBrush TextColor => ToSolidColor("Color15");

    public static SolidColorBrush TextColor1 => ToSolidColor("Color15");

    public static SolidColorBrush MainWindowNavBarBackground => ToSolidColor("Color16");

    public static SolidColorBrush MainWindowSearchArea => ToSolidColor("Color17");

    public static Color PlayerGradient1 => ToColor("PlayerGradient1");
    
    public static Color PlayerGradient2 => ToColor("PlayerGradient2");

    public static SolidColorBrush BrowseArea => ToSolidColor("Color19");

    public static SolidColorBrush FadeBackground => ToSolidColor("Color20");

    public static SolidColorBrush BrowseAreaBorder => ToSolidColor("Color10");

    public static SolidColorBrush BrowseAreaHover => ToSolidColor("Color21");

    public static SolidColorBrush ListCurrentListBackground => ToSolidColor("ListCurrentListBackground");
}