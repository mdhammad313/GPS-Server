using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService
{
    public class RequestModel
    {
        public Coordinates LocationsObject { get; set; }

        public int NoOfParameter { get; set; }
    }
}