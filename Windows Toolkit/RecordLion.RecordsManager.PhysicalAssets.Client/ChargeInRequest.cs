﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class ChargeInRequest : ChargeRequest
    {
        public DateTime PickupDate { get; set; }
        public string PickupLocation { get; set; }
    }
}
