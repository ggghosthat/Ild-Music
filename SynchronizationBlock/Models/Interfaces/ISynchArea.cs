using System;

namespace SynchronizationBlock.Models.SynchArea
{
    public interface ISynchArea
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }
    }
}