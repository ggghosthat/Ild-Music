namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class ArtistFactoryEntity : FactoryEntity
    {
        public override string Name { get ; set ; }
        public override string Description { get; set; }

        public ArtistFactoryEntity(string factoryName) : base(factoryName)
        {

        }
    }
}
