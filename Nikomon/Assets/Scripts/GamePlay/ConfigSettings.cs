using Newtonsoft.Json;
using UnityEngine;

namespace GamePlay
{
    public class ConfigSettings
    {
        public bool UseVirtualControl;

        [JsonConstructor]
        public ConfigSettings(bool useVirtualControl)
        {
            UseVirtualControl = useVirtualControl;
        }

        public ConfigSettings()
        {
            if (Application.isMobilePlatform)
            {
                UseVirtualControl = true;
            }
            else
            {
                UseVirtualControl = false;
            }
        }
    }
}