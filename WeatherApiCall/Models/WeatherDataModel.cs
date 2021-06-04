using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApiCall.Models
{
    public class WeatherDataModel
    {
        //get weather api data
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
        public double Windspeed  { get; set; }
        public DateTime Sunrise { get; set; }
        public string stringSunrise { get; set; }
        public string stringSunset { get; set; }
        public DateTime Sunset { get; set; }
        public string Timezone { get; set; }
        public string CountryName { get; set; }
        public string weathertype { get; set; }
        public string weatherdescription { get; set; }
        public int visibility { get; set; }
        public string icon { get; set; }

        //User Input
        public string Location { get; set; }

        public string Mode { get; set; }
        public string Unit { get; set; }

    }
}