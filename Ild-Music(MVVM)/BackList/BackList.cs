using Ild_Music_MVVM_.ViewModel.Base;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Ild_Music_MVVM_
{
    internal class BackList<T>
    {
        private static IList<T> collection = new List<T>();

        public int Count => collection.Count;

        public void Add(T item) =>
            collection.Add(item);

        public T Peek() 
        {
            var last = collection.Last();
            collection.RemoveAt(collection.Count - 1);
            return last;
        }

        public void Clear() =>
            collection.Clear();
    }
}
