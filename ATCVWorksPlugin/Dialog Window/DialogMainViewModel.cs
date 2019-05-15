using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VworksAtcPlugin
{
    class DialogMainViewModel 
    {
        public DialogMainViewModel()
        {

            DeviceTab = new DeviceControlViewModel();
            ProtocolTab = new ProtocolViewModel(new ProtocolModel());
        }

        public DeviceControlViewModel DeviceTab { get; set; }
        public ProtocolViewModel ProtocolTab { get; set; }
    }
}
