﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Enums
{
    public enum MqttMessagesType
    {
        ConnectedToServer,
        SwitchLightChange,
        LightPointReset
    }
}
