using System.Collections.Generic;
using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances.Services.Storage
{
    public struct Storage
    {
        private int e;
        private Stack<IEnumerable<ICoreEntity>> _storage = new();
        public int Count => _storage.Count;
        
        public Storage(int i)
        {
            e = i;
        }

        public void Push(IEnumerable<ICoreEntity> entity) =>
            _storage.Push(entity);

        public void Pop() =>
            _storage.Pop();

        public void Pekk() => 
            _storage.Peek();
    }
}