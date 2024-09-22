namespace Ild_Music.Core.Statistics;
public struct InspectFrame
{
    public int InspectTag {get; private set;}
    public int FactFirstItem {get; private set;} = 0;
    public int FactSecondItem {get; private set;} = 0;

    public int RepresentFirstItem {get; private set;} = 0;
    public int RepresentSecondItem {get; private set;} = 0;

    public bool FirstDiffExists => (RepresentFirstItem == FactFirstItem);
    public bool SecondDiffExists => (RepresentSecondItem == FactSecondItem);
    
    public InspectFrame(int tag, int factFirst, int factSecond, int representFirst = 0, int representSecond = 0)
    {
        InspectTag = tag;
        FactFirstItem = factFirst;
        FactSecondItem = factSecond;
        RepresentFirstItem = representFirst;
        RepresentSecondItem = representSecond;
    }

}
