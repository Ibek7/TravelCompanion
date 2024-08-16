using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCompanion.MAUI
{
    /// <summary>
    /// Config settings. 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// This is the base address where you are hosting the TravelCompanion API. It should include a trailing forward slash.
        /// </summary>
        public static string ApiBaseAddress = ""; // e.g. https://yourapi.azurewebsites.net/

        /// <summary>
        /// This is your OpenAI Api Key and model
        /// </summary>
        public static string OpenAiApiKey = "";
        public static string OpenAiModel = "gpt-4o";

        /// <summary>
        /// This is your Syncfusion License Key
        /// </summary>
        public static string SyncfusionLicenseKey = "";
    }
}