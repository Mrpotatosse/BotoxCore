using BotoxCore.Configurations.Customs;
using BotoxCore.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Configurations
{
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public StartupConfiguration Startup
        {
            get
            {
                return GetConfig<StartupConfiguration>();
            }
        }

        public T GetConfig<T>() where T : class, IConfiguration
        {
            T obj = Activator.CreateInstance<T>();
            string location = typeof(T).GetProperty("LOCATION").GetValue(obj).ToString();

            try
            {
                string content = File.ReadAllText(location);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
            }
            catch (FileNotFoundException)
            {
                string content = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(location, content);

                logger.Info($"config : {location} was not found and created");

                return GetConfig<T>();
            }
            catch(Exception e)
            {
                logger.Error(e);

                return null;
            }
        }

        public void SaveConfig<T>(T obj) where T : class, IConfiguration
        {
            try
            {
                string content = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(obj.LOCATION, content);
            }
            catch(Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
