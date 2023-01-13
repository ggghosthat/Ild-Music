using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities
{
    public class PlayerService : IService
    {
    	public string ServiceName {get; init;} = "PlayerService"; 

    	public IPlayer PlayerInstance {get; private set;}

    	public void EnablePlayer(IPlayer _player) =>    	
    		PlayerInstance = _player;    	
    }
}