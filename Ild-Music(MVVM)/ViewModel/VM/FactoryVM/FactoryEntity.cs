namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{

    public abstract class FactoryEntity
    {
        public string FactoryName { get; set; }
        public abstract string Name { get; set; }
        public abstract string Description { get; set; }

        protected FactoryEntity(string factoryName) => FactoryName = factoryName;
    }
}
