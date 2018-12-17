using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WhitbyWoodToolbar
{
    public static class CheckDynamo
    {
        public static bool checkDynamoPackages()
        {
            string output = "";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dynamoFolder = appData + @"\Dynamo\Dynamo Core\1.3";
            if (!Directory.Exists(dynamoFolder))
            {
                output += "Dynamo 1.3 required" + Environment.NewLine;
            }
            else
            {
                output += "Dynamo 1.3 folder found in " + dynamoFolder + Environment.NewLine;
            }

            // check if Rhynamo installed

            if (File.Exists(dynamoFolder + @"\packages\Rhynamo" + @"\pkg.json"))
            {
                using (StreamReader r = new StreamReader(dynamoFolder + @"\packages\Rhynamo" + @"\pkg.json"))
                {
                    string json = r.ReadToEnd();
                    dynamic packageData = JsonConvert.DeserializeObject<dynamic>(json);
                    if (packageData.version == "2018.4.3")
                    {
                        output += "Correct version of Rhynamo installed" + Environment.NewLine;
                    }
                }
            }
            else
            {
                output += "Rhynamo v.2018.4.3 required" + Environment.NewLine;
            }

            MessageBox.Show(output);

            return true;
        }
    }
}
