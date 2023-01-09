using ShareInstances.Services.Interfaces;

using System.Linq;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities
{
    public class ViewModelHolder<T> : IService
    {
        public string ServiceName {get; init;} = "HolderService";  
        private IDictionary<string, T> holder = new Dictionary<string, T>();

        public ViewModelHolder(int  i = 0)
        {
        }

        public void Add(string name,T viewModel) =>
            holder[name] = viewModel;
        
        public T Get(string name) =>
            (holder.ContainsKey(name))?holder[name]:default(T);

        public T Peek() =>
            holder.ToList().Last().Value;

        public void Clear() =>
            holder.Clear();
    }
}