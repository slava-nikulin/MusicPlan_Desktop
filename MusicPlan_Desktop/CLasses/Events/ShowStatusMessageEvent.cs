using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;

namespace MusicPlan_Desktop.CLasses.Events
{
    public class ShowStatusMessageEvent : PubSubEvent<string>
    {
    }
}
