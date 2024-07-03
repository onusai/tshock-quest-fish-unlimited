using Newtonsoft.Json;
using TShockAPI;

namespace QuestFishUnlimited
{
    internal class Configuration
    {
        [JsonProperty("AnglerQuestSwap (turning it off will allow one type of fish to be used for one day)", Order = 0)]
        public bool SwitchTasks { get; set; } = true;


        #region Write and Read
        public static readonly string FilePath = Path.Combine(TShock.SavePath, "QuestFishUnlimited.json");

        public void Write()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Configuration Read()
        {
            if (!File.Exists(FilePath))
            {
                var NewConfig = new Configuration();
                new Configuration().Write();
                return NewConfig;
            }
            else
            {
                string jsonContent = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Configuration>(jsonContent)!;
            }
        }
        #endregion

    }
}