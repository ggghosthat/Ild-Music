namespace ShareInstances.Statistics;
public struct CounterFrame
{
    public int Artists {get; private set;}
    public int Playlists {get; private set;}
    public int Tracks {get; private set;}
    public int AP {get; private set;}
    public int AT {get; private set;}
    public int PA {get; private set;}
    public int PT {get; private set;}
    public int TA {get; private set;}
    public int TP {get; private set;}

    public CounterFrame(int artists, int playlists, int tracks,
                     int ap, int at, int pa, int pt, int ta, int tp)
    {
        Artists = artists;
        Playlists = playlists;
        Tracks = tracks;
        AP = ap;
        AT = at;
        PA = pa;
        PT = pt;
        TA = ta;
        TP =tp;
    }
}
