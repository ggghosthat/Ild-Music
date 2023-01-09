using ShareInstances.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShareInstances.Services.Interfaces
{
    public interface ICenter
    {
        public bool IsCenterActive {get; set;}


        public void OnCenterRegisterActivate();

        
        public void RegistService(IService service); 

       
        public IService GetService(string name);
    }

}