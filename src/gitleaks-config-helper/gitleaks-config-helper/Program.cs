using System;
using System.IO;
using Tommy;

namespace GitleaksConfigHelper
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Parameters not set");
                Console.WriteLine("Usage: gitleaks-config-helper merge base_file merge_file out_file");
                Console.WriteLine("Exiting");
                return -1;
            }
            if (VerifyInput(args[1], args[2], args[3]))
            {
                try
                {
                    switch (args[0])
                    {
                        case "merge":
                            MergeConfigs(args[1], args[2], args[3]);
                            break;
                        default:
                            Console.WriteLine("Usage: gitleaks-config-helper merge base_file merge_file out_file");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while merging: " + e.Message);
                    Console.WriteLine("Exiting");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Exiting");
                return -1;
            }
            return 0;
        }

        private static void MergeConfigs(string baseConfig, string childConfig, string outFile)
        {
            // Parse files into a node
            using (StreamReader readerBase = File.OpenText(baseConfig))
            using (StreamReader readerChild = File.OpenText(childConfig))
            {
                // Parse the base and child table
                TomlTable tblBase = TOML.Parse(readerBase);
                TomlTable tblChild = TOML.Parse(readerChild);

                //Merge tables 
                foreach (TomlNode childRow in tblChild["rules"])
                {
                    for (int i = 0; i < tblBase["rules"].ChildrenCount; i++)
                    {
                        if (childRow["id"].ToString() == tblBase["rules"][i]["id"])
                        {
                            //Key found in base table can be replaced with new one
                            tblBase["rules"].Delete(i);
                            break;
                        }
                    }
                    //Add new key
                    tblBase["rules"].Add(childRow);
                }

                using (StreamWriter writer = File.CreateText(outFile))
                {
                    tblBase.WriteTo(writer);
                    writer.Flush();
                }
            }
        }

        private static bool VerifyInput(string baseConfig, string childConfig, string outFile)
        {
            //Check if base file exists
            if (!File.Exists(baseConfig))
            {
                Console.WriteLine("Base file does not exist: " + baseConfig);
                return false;
            }
            //Check if child file exists
            if (!File.Exists(childConfig))
            {
                Console.WriteLine("Child file does not exist: " + childConfig);
                return false;
            }

            return true;
        }
    }
}
