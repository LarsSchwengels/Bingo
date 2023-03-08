using Bingo.Entities;
using System.Text.Json;
using Newtonsoft.Json;

namespace Bingo.Repositories
{
    public class GameRepo : IGameRepo
    {
        public CustomBoard? GetExistingSave()
        {
            if (File.Exists(@"../CurrentSave.json"))
            {
                return System.Text.Json.JsonSerializer.Deserialize<CustomBoard>(File.ReadAllText(@"../CurrentSave.json"));
            }
            else
            {
                throw new Exception("Ich bin mal wieder dumm");
            }
        }

        public void SaveCurrent(CustomBoard? board)
        {
            var sz = JsonConvert.SerializeObject(board);
            File.WriteAllText("../CurrentSave.json", sz);
        }

        public void DeleteExistingSave()
        {
            if (File.Exists("../CurrentSave.json"))
            {
                File.Delete("../CurrentSave.json");
            }
        }
    }
}