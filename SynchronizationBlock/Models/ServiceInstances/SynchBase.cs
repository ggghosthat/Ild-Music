using System;
using System.Collections.Generic;
using System.Text;

namespace SynchronizationBlock.Models.SynchArea
{
    public abstract class SynchBase<T>
    {
        private string path;

        protected string Path 
        {
            get 
            {
                return path;
            }
            set 
            {
                path = value;
            }
        }

        public abstract IList<T> Instances { get; }



        public abstract void AddInstance(T instance);

        public abstract void EditInstance(T instance);

        public abstract void RemoveInstance(T instance);



        public abstract void Serialize();

        public abstract void Deserialize();
    }
}
