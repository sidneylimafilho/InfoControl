using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Web.ScheduledTasks
{
    public class SchedulerSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get
            {
                return (bool)base["enabled"];
            }            
        }        
    }

}
