using System.Collections.Generic;

namespace SynchronizationBlock.Models.SynchArea
{
    public abstract class SynchBase<T>
    {
        protected string path;

        protected string Path 
        {
            get => path;            
            set => path = value;
        }

        public abstract IList<T> Instances { get; }


        #region Basic methods of SynchObject
        public abstract void AddInstance(T instance);

        public abstract void EditInstance(T instance);

        public abstract void RemoveInstance(T instance);
        #endregion

        #region Serialize/Deserialize methods
        public abstract void Serialize();

        public abstract void Deserialize();
        #endregion
    }
}
