using Newtonsoft.Json.Linq;
using NLog;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WeatherApiCall.Models;

namespace WeatherApiCall.Controllers
{
    public class WeatherDataController : Controller
    {
        Logger Logger = LogManager.GetCurrentClassLogger();

        // GET: WeatherData
        public ActionResult Get_User_Input()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Show_Weather_Data(string location)
        {

            List<WeatherDataModel> modellist = new List<WeatherDataModel>();

            using (var client = new HttpClient())
            {
                try { 
               
               // X - RapidAPI - Key
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "c59e7164e0msh0e5bdc83369f6bdp11b688jsnca97e032cc74");    
                client.BaseAddress = new Uri("https://community-open-weather-map.p.rapidapi.com/");
               // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = client.GetAsync("weather?&q=" + location);
                // var responseTask = client.GetAsync("weather?q="+location);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //try
                    //{

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var weatherdata = readTask.Result.ToString();
                        // var lt = JsonConvert.DeserializeObject<WeatherDataModel>(listDestinations);


                        var datakeyvalue = JObject.Parse(weatherdata);

                        WeatherDataModel objweatherdata = new WeatherDataModel();

                        string timez;
                        double timezone = ((Convert.ToDouble(datakeyvalue["timezone"])) / 60) / 60;
                        objweatherdata.Timezone = timezone.ToString().First() != '-' ? (timez = "GMT" + "+" + timezone) : (timez = "GMT" + " " + timezone);

                        // epoch to date and time

                        double SunriseEpoch = Convert.ToDouble(datakeyvalue["sys"]["sunrise"]);
                        objweatherdata.Sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(SunriseEpoch).AddHours(timezone);
                        // string SunriseDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();

                        double SunsetEpoch = Convert.ToDouble(datakeyvalue["sys"]["sunset"]);
                        objweatherdata.Sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(SunsetEpoch).AddHours(timezone);
                        // string SunsetDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();




                        objweatherdata.Longitude = Convert.ToDouble(datakeyvalue["coord"]["lon"]);
                        objweatherdata.Latitude = Convert.ToDouble(datakeyvalue["coord"]["lat"]);
                        objweatherdata.Temp = (Convert.ToDouble(datakeyvalue["main"]["temp"]) - 273.15);
                        objweatherdata.weathertype = Convert.ToString(datakeyvalue["weather"][0]["main"]);
                        objweatherdata.weatherdescription = Convert.ToString(datakeyvalue["weather"][0]["description"]);
                        objweatherdata.Pressure = Convert.ToDouble(datakeyvalue["main"]["pressure"]);
                        objweatherdata.Humidity = Convert.ToDouble(datakeyvalue["main"]["humidity"]);
                        objweatherdata.Temp_Min = ((Convert.ToDouble(datakeyvalue["main"]["temp_min"])) - 273.15);
                        objweatherdata.Temp_Max = (Convert.ToDouble(datakeyvalue["main"]["temp_max"])) - 273.15;
                        objweatherdata.Windspeed = Convert.ToDouble(datakeyvalue["wind"]["speed"]);
                        objweatherdata.visibility = Convert.ToInt32(datakeyvalue["visibility"])/1000;
                       



                        objweatherdata.CountryName = Convert.ToString(datakeyvalue["name"]);

                        // epoch timezone to normal timezone





                        ViewBag.Location = Convert.ToString(datakeyvalue["name"]);

                        modellist.Add(objweatherdata);


                        return Json(modellist, JsonRequestBehavior.AllowGet);

                      //  return View(modellist);
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error Occured in Show_Weather_Data" + DateTime.Now.ToShortTimeString());
                    List<string> Errorlist = new List<string>();
                    //  Errorlist.Add("Some Error Occured. Please Try Again");
                    Errorlist.Add(Convert.ToString(ex));
                    //return Json(null, JsonRequestBehavior.AllowGet);
                    return Json(Errorlist, JsonRequestBehavior.AllowGet);

                }
            }
            return View(modellist);


        }

     
        public JsonResult Ajax_Show_Weather_Data(string location)
        {
            
            //List<WeatherDataModel> modellist = new List<WeatherDataModel>();

            using (var client = new HttpClient())
            {
                try
                {
                    //X - RapidAPI - Key
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "c59e7164e0msh0e5bdc83369f6bdp11b688jsnca97e032cc74");
                    client.BaseAddress = new Uri("https://community-open-weather-map.p.rapidapi.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync("weather?units=%22metric%22+or+%22imperial%22&mode=xml%2C+html&q=" + location);
                    // var responseTask = client.GetAsync("weather?units="+unit +"&mode=xml%2C+html&q=" + location);
                    // var responseTask = client.GetAsync("q="+location);

                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var weatherdata = readTask.Result.ToString();
                        var datakeyvalue = JObject.Parse(weatherdata);


                        WeatherDataModel objweatherdata = new WeatherDataModel();

                        string timez;
                        double timezone = ((Convert.ToDouble(datakeyvalue["timezone"])) / 60) / 60;
                        objweatherdata.Timezone = timezone.ToString().First() != '-' ? (timez = "GMT" + "+" + timezone) : (timez = "GMT" + " " + timezone);

                        double SunriseEpoch = Convert.ToDouble(datakeyvalue["sys"]["sunrise"]);
                        objweatherdata.Sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(SunriseEpoch).AddHours(timezone);
                        objweatherdata.stringSunrise = objweatherdata.Sunrise.ToString();
                        double SunsetEpoch = Convert.ToDouble(datakeyvalue["sys"]["sunset"]);
                        objweatherdata.Sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(SunsetEpoch).AddHours(timezone);
                        objweatherdata.stringSunset = objweatherdata.Sunset.ToString();
                        objweatherdata.Longitude = Convert.ToDouble(datakeyvalue["coord"]["lon"]);
                        objweatherdata.Latitude = Convert.ToDouble(datakeyvalue["coord"]["lat"]);
                        objweatherdata.Temp = (Convert.ToDouble(datakeyvalue["main"]["temp"]) - 273.15);
                        objweatherdata.Temp = (int)objweatherdata.Temp;
                        objweatherdata.weathertype = Convert.ToString(datakeyvalue["weather"][0]["main"]);
                        objweatherdata.weatherdescription = Convert.ToString(datakeyvalue["weather"][0]["description"]);
                        objweatherdata.Pressure = Convert.ToDouble(datakeyvalue["main"]["pressure"]);
                        objweatherdata.Humidity = Convert.ToDouble(datakeyvalue["main"]["humidity"]);
                        objweatherdata.Temp_Min = ((Convert.ToDouble(datakeyvalue["main"]["temp_min"])) - 273.15);
                        objweatherdata.Temp_Min = (int)objweatherdata.Temp_Min;
                        objweatherdata.Temp_Max = (Convert.ToDouble(datakeyvalue["main"]["temp_max"])) - 273.15;
                        objweatherdata.Temp_Max = (int)objweatherdata.Temp_Max;
                        objweatherdata.Windspeed = Convert.ToDouble(datakeyvalue["wind"]["speed"]);
                        objweatherdata.visibility = Convert.ToInt32(datakeyvalue["visibility"]) / 1000;
                        objweatherdata.CountryName = Convert.ToString(datakeyvalue["name"]);
                        objweatherdata.icon = Convert.ToString(datakeyvalue["weather"][0]["icon"]);

                        ViewBag.Location = Convert.ToString(datakeyvalue["name"]);

                        // modellist.Add(objweatherdata);

                        return Json(objweatherdata, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error Occured in Show_Weather_Data" + DateTime.Now.ToShortTimeString());
                    List<string> Errorlist = new List<string>();
                    //  Errorlist.Add("Some Error Occured. Please Try Again");
                    Errorlist.Add(Convert.ToString(ex));
                    //return Json(null, JsonRequestBehavior.AllowGet);
                    return null;

                }
            }
            return Json(new WeatherDataModel(), JsonRequestBehavior.AllowGet);
        }
    }
}