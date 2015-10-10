using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class Coordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public float Accuracy { get; set; }

        public DateTime timeStamp { get; set; }

        public string DeviceId { get; set; }

        public int UniqueId { get; set; }

        public float speed { get; set; }

        public float bearing { get; set; }

        public double altitude { get; set; }
    }

}
