using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MagmaWorksToolbar
{
    public static class CheckDynamo
    {
        public static bool checkDynamoPackages()
        {
            // need to read this from file at some point...
            List<Tuple<string, string>> packages = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Rhynamo", "2018.4.3"),
                new Tuple<string, string>("archi-lab.net","2018.13.13"),
                new Tuple<string, string>("Archi-lab_Mandrill","2018.0.1"),
                new Tuple<string, string>("Bakery","2018.13.13"),
                new Tuple<string, string>("bimorphNodes","2.2.2"),
                new Tuple<string, string>("Clockwork for Dynamo 1.x","1.31.2"),
                new Tuple<string, string>("Data-Shapes","2018.6.13"),
                new Tuple<string, string>("designtech","1.0.6"),
                new Tuple<string, string>("List Contains","0.1.0"),
                new Tuple<string, string>("Lunchbox for Dynamo","2017.10.4"),
                new Tuple<string, string>("Mantis Shrimp","2017.12.2"),
                new Tuple<string, string>("Rhythm","2018.6.7"),
                new Tuple<string, string>("spring nodes","132.2.7"),
                new Tuple<string, string>("SteamNodes","1.2.4"),
            };

            string output = "";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dynamoFolder = appData + @"\Dynamo\Dynamo Revit\1.3";
            if (!Directory.Exists(dynamoFolder))
            {
                output += "Dynamo 1.3 required" + Environment.NewLine;
            }
            else
            {
                output += "Dynamo 1.3 folder found in " + dynamoFolder + Environment.NewLine;
            }

            // check if Rhynamo installed

            foreach (var package in packages)
            {
                if (File.Exists(dynamoFolder + @"\packages\" + package.Item1 + @"\pkg.json"))
                {
                    output += package.Item1 + " folder found";
                    using (StreamReader r = new StreamReader(dynamoFolder + @"\packages\" + package.Item1 + @"\pkg.json"))
                    {
                        string json = r.ReadToEnd();
                        dynamic packageData = JsonConvert.DeserializeObject<dynamic>(json);
                        if (packageData.version == package.Item2)
                        {
                            output += "and correct version installed (" + package.Item2 + ")." + Environment.NewLine;
                        }
                        else
                        {
                            output += "incorrect version installed (" + packageData.version + "). You should have version " + package.Item2 + ". " + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    output += package.Item1 + " version " + package.Item2 + " is required." + Environment.NewLine;
                }
            }


            //if (File.Exists(dynamoFolder + @"\packages\Rhynamo" + @"\pkg.json"))
            //{
            //    using (StreamReader r = new StreamReader(dynamoFolder + @"\packages\Rhynamo" + @"\pkg.json"))
            //    {
            //        string json = r.ReadToEnd();
            //        dynamic packageData = JsonConvert.DeserializeObject<dynamic>(json);
            //        if (packageData.version == "2018.4.3")
            //        {
            //            output += "Correct version of Rhynamo installed" + Environment.NewLine;
            //        }
            //    }
            //}
            //else
            //{
            //    output += "Rhynamo v.2018.4.3 required" + Environment.NewLine;
            //}

            MessageBox.Show(output);

            return true;
        }
    }
}
