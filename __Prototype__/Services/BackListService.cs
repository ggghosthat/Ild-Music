
using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.ViewModel.Base;

namespace Ild_Music_MVVM_.Services
{
    internal class BackListService : Service
    {
        public override string ServiceType { get; init; } = "BackListService";

        private static BackList<BaseViewModel> backList = new();


        public void Add(BaseViewModel viewModel) =>
            backList.Add(viewModel);

        public BaseViewModel Peek() =>
            backList.Peek();
    }
}
