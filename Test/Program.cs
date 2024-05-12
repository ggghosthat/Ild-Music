using Cube;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts;

ICube guido = new GuidoForklift();
guido.Init(".", true);

var guidA1 = Guid.NewGuid();
var guidA2 = Guid.NewGuid();
var guidA3 = Guid.NewGuid();
var guidA4 = Guid.NewGuid();
var guidA5 = Guid.NewGuid();
var guidA6 = Guid.NewGuid();

var a1 = new Artist(guidA1, "Artist 1".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a1);
var a2 = new Artist(guidA2, "Artist 2".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a2);
var a3 = new Artist(guidA3, "Artist 3".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a3);
var a4 = new Artist(guidA4, "Artist 4".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a4);
var a5 = new Artist(guidA5, "Artist 5".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a5);
var a6 = new Artist(guidA6, "Artist 6".AsMemory(), String.Empty.AsMemory(), new byte[0], 0);
guido.AddArtistObj(a6);

Guid[] guidArr = new Guid[] {guidA1, guidA3, guidA5};

var i = guido.QueryInstanceDtosFromIds(guidArr, EntityTag.ARTIST).Result;
Console.WriteLine(i.Count());
foreach (var j in i)
    Console.WriteLine(j.Name);
