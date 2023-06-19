using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

internal class Program
{
    public class Settings
    {
        [JsonProperty(Required = Required.Always)]
        public int IntegerParam { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool BooleanParam { get; set; }

        public string? StringParam { get; set; }

        public string? Description { get; set; }
    }

    public static bool LoadSettings(string filePath, out Settings? settings)
    {
        settings = null;

        // JSONスキーマの作成
        var schemaJson = @"
    {
        'type': 'object',
        'properties': {
            'IntegerParam': {'type': 'integer'},
            'BooleanParam': {'type': 'boolean'},
            'StringParam': {'type': 'string'},
            'Description': {'type': 'string'}
        },
        'required': ['IntegerParam', 'BooleanParam']
    }";
        var schema = JSchema.Parse(schemaJson);

        // 設定ファイルの読み込み
        var jsonText = File.ReadAllText(filePath);
        var jObj = JObject.Parse(jsonText);

        // JSONスキーマでの妥当性チェック
        if (jObj.IsValid(schema, out IList<string> errorMessages))
        {
            settings = jObj.ToObject<Settings>();
            return true;
        }
        else
        {
            foreach (var error in errorMessages)
            {
                Console.WriteLine(error);
            }
            return false;
        }
    }


    private static void Main()
    {
        string filePath = "input.json";

        if (LoadSettings(filePath, out Settings? settings))  // Null許容型に変更
        {
            if (settings != null)  // settingsがnullでないことを確認
            {
                Console.WriteLine("設定を読み込みました。");
                Console.WriteLine($"IntegerParam: {settings.IntegerParam}");
                Console.WriteLine($"BooleanParam: {settings.BooleanParam}");
                Console.WriteLine($"StringParam: {settings.StringParam}");
                Console.WriteLine($"Description: {settings.Description}");
            }
            else
            {
                Console.WriteLine("設定がnullです。");
            }
        }
        else
        {
            Console.WriteLine("設定の読み込みに失敗しました。");
        }
    }
}