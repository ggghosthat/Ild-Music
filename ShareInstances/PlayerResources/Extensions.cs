using ShareInstances.PlayerResources.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareInstances.PlayerResources
{
    public static class Extesions
    {
    	public static List<T> ToEntity<T>(this IList<Guid> guids, IList<T> store, bool full_return = false ) where T : ICoreEntity
    	{
            if (!full_return)
        		return store.ToList()
                            .FindAll(delegate(T item) { return guids.Contains(item.Id); });
            else
            {
                return store.ToList();   
            }
    	}
	}
}