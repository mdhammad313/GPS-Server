using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;

using WebService.Models;
using System.Collections;

namespace WebService.Controllers
{
    public class LocationController : ApiController
    {
        private string AmazonServer = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
        private const string LocalServerPath = "D:\\Projects\\UserLocation.txt";
        private const string CompanyServerPath = "D:\\Common Share\\PG Gps Tracker\\Log\\UserLocation.txt";
        /// <summary>
        /// Store Current User Location
        /// </summary>
        /// <param name="longlat"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("getlocation")]
        public HttpResponseMessage GetLocation(Coordinates longlat)
        {
            try
            {
                Coordinates latlon = new Coordinates
                {
                    timeStamp = DateTime.Now
                };

                latlon.Latitude = longlat.Latitude;
                latlon.Longitude = longlat.Longitude;
                latlon.DeviceId = longlat.DeviceId;
                latlon.Accuracy = longlat.Accuracy;
                latlon.UniqueId = longlat.UniqueId;
                latlon.speed =    longlat.speed;
                latlon.bearing = longlat.bearing;
                latlon.altitude = longlat.altitude; 
               

                var json = JsonConvert.SerializeObject(latlon);
                using (FileStream fs = new FileStream(AmazonServer + "\\UserLocation.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (var file = new StreamWriter(fs))
                    {
                        file.WriteLine(json);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Current User Last Location
        /// </summary>
        /// <param name="longlat"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("lastlocation")]
        public HttpResponseMessage LastLocation(RequestModel rm)
        {
            try
            {

                int deviceId = rm.LocationsObject.UniqueId;
                string RequiredValue = "";
                List<Coordinates> encryptedData = new List<Coordinates>();
                using (FileStream fs = new FileStream(AmazonServer + "\\UserLocation.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var file = new StreamReader(fs))
                    {
                        string allLocations = file.ReadToEnd();
                        if (allLocations != null)
                        {
                            int uniqueIdIndex;
                            uniqueIdIndex = allLocations.LastIndexOf("\"" + "UniqueId" + "\":" + rm.LocationsObject.UniqueId.ToString() + ",");

                            if (uniqueIdIndex == -1)
                            {
                                RequiredValue = "N\\A";
                            }

                            else
                            {
                                int lastIndex = allLocations.IndexOf("}", uniqueIdIndex);
                                int startingIndex = allLocations.LastIndexOf("{", uniqueIdIndex);
                                //int startIndex = -1;
                                //System.Text.StringBuilder specifiedLine = new System.Text.StringBuilder();
                                //for (int i = uniqueIdIndex; i >= 0; i--)
                                //{
                                //    if (allLocations.ElementAt(i) == '{')
                                //    {
                                //        startIndex = i;
                                //        break;
                                //    }

                                //    else
                                //    {
                                //        specifiedLine.Append(allLocations.ElementAt(i));
                                //    }
                                //}

                                //char[] arr = specifiedLine.ToString().ToCharArray();
                                //Array.Reverse(arr);


                                //If opening curly braces not found
                                //if (startIndex == -1)
                                //{
                                //    RequiredValue = "Starting Index not found";
                                //}

                                //else
                                //{
                                RequiredValue = allLocations.Substring(startingIndex, lastIndex - startingIndex + 1);
                                encryptedData.Add(JsonConvert.DeserializeObject<Coordinates>(RequiredValue));
                                //}
                            }
                        }
                        file.Close();
                    }
                    fs.Close();
                }

                if (RequiredValue == "N\\A")
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, RequiredValue, "application/json");
                }

                object objectData = JsonConvert.SerializeObject(encryptedData);
                return Request.CreateResponse(HttpStatusCode.OK, encryptedData, "application/json");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Other user last location 
        /// </summary>
        /// <param name="longlat"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("tracelastlocation")]
        public HttpResponseMessage TraceLastLocation(RequestModel rm)
        {
            try
            {
                string RequiredValue = "";
                using (FileStream fs = new FileStream(AmazonServer + "\\UserLocation.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var file = new StreamReader(fs))
                    {
                        string allLocations = file.ReadToEnd();
                        if (allLocations != null)
                        {
                            int uniqueIdIndex;
                            uniqueIdIndex = allLocations.LastIndexOf("\"" + "UniqueId" + "\":" + rm.LocationsObject.UniqueId.ToString() + ",");

                            if (uniqueIdIndex == -1)
                            {
                                RequiredValue = "N\\A";
                            }

                            else
                            {
                                int startingIndex = allLocations.LastIndexOf("{", uniqueIdIndex);
                                int lastIndex = allLocations.IndexOf("}", uniqueIdIndex);
                                RequiredValue = allLocations.Substring(startingIndex, lastIndex - startingIndex + 1);
                            }
                        }
                        file.Close();
                    }
                    fs.Close();
                }

                if (RequiredValue == "N\\A")
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, RequiredValue, "application/json");
                }

                object objectData = JsonConvert.DeserializeObject(RequiredValue);
                return Request.CreateResponse(HttpStatusCode.OK, objectData, "application/json");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}



