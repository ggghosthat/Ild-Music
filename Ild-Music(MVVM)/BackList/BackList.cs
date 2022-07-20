using Ild_Music_MVVM_.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;

namespace Ild_Music_MVVM_
{
    internal class BackList<T> where T : BaseViewModel
    {
        private static IList<T> collection = new List<T>();


        public void Add(T item) =>
            collection.Add(item);

        public T Peek() 
        {
            var last = collection.Last();
            collection.RemoveAt(collection.Count - 1);
            return last;
        }
    }
}
