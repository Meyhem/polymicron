using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProjectPlaguemangler.Extensions;
using Serilog;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ProjectPlaguemangler.Controllers
{
    public enum BoxType
    {
        Success,
        Warning,
        Error
    }

    public class MessageBox
    {
        public BoxType Type { get; set; }

        public string Message { get; set; }

        public string Key { get; set; }
    }

    public abstract class BaseController: Controller
    {
        public RedirectResult RedirectBack()
        {
            return Redirect(Request.Headers["Referer"]);
        }

        public void AddBox(string message, BoxType type = BoxType.Success, string key = "Boxes")
        {
            List<MessageBox> list;

            if (TempData.ContainsKey(key))
            {
                list = TempData.Get<List<MessageBox>>(key);
            }
            else
            {
                list = new List<MessageBox>();
            }


            list.Add(new MessageBox
            {
                Type = type,
                Message = message,
                Key = key
            });

            TempData.Put(key, list);
        }

        public bool ReCaptchaPassed(string gRecaptchaResponse, string secret)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                Log.Error("Unable to send recaptcha request to GServices");
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }
    }
}
