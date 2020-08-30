using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Configurations
{
    public interface IConfiguration
    {
        [JsonIgnore]
        string LOCATION { get; }
    } 
}
