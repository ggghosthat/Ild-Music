using Ild_Music_MVVM_.ViewModel.Base;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.ExtensionMethods
{
    public static class Extensions
    {
        public static ListViewModel DefineListVM_Type(this BaseViewModel baseVM, List listType)
        {
            if (baseVM is ListViewModel listVM)
            {
                listVM.SetListType(listType);
                return listVM;
            }

            return null;
        }
    }
}
