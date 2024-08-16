using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCompanion.SDK;

namespace TravelCompanion.MAUI
{
    public class ApiKeyProvider : IApiKeyProvider
    {
        public string GetApiKey()
        {
            return MauiProgram.ApiKey;
        }
    }
}
