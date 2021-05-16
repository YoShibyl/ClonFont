using BepInEx;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClonFont
{
    // I bascially took this code from Unity Forums lol sorry
    public class IniBruv
    {
        private static string path = Path.Combine(Paths.BepInExRootPath, "plugins", "ClonFont", "fonts.ini");
        private static Dictionary<string, Dictionary<string, string>> IniDictionary = new Dictionary<string, Dictionary<string, string>>();
        private static bool Initialized = false;

        public enum Sections
        {
            Section01,
        }

        public enum Keys
        {
            Key01,
            Key02,
            Key03,
        }

        private static bool FirstRead()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    string theSection = "";
                    string theKey = "";
                    string theValue = "";
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        line.Trim();
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            theSection = line.Substring(1, line.Length - 2);
                        }
                        else
                        {
                            string[] ln = line.Split(new char[] { '=' });
                            theKey = ln[0].Trim();
                            theValue = ln[1].Trim();
                        }
                        if (theSection == "" || theKey == "" || theValue == "")
                            continue;
                        PopulateIni(theSection, theKey, theValue);
                    }
                }
            }
            return true;
        }

        private static void PopulateIni(string _Section, string _Key, string _Value)
        {
            if (IniDictionary.ContainsKey(_Section))
            {
                if (IniDictionary[_Section].ContainsKey(_Key))
                    IniDictionary[_Section][_Key] = _Value;
                else
                    IniDictionary[_Section].Add(_Key, _Value);
            }
            else
            {
                Dictionary<string, string> neuVal = new Dictionary<string, string>();
                neuVal.Add(_Key.ToString(), _Value);
                IniDictionary.Add(_Section.ToString(), neuVal);
            }
        }

        public static void IniWriteValue(string _Section, string _Key, string _Value)
        {
            if (!Initialized)
                FirstRead();
            PopulateIni(_Section, _Key, _Value);
            //write ini
            WriteIni();
        }

        public static void IniWriteValue(Sections _Section, Keys _Key, string _Value)
        {
            IniWriteValue(_Section.ToString(), _Key.ToString(), _Value);
        }

        private static void WriteIni()
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> sezioni in IniDictionary)
                {
                    sw.WriteLine("[" + sezioni.Key.ToString() + "]");
                    foreach (KeyValuePair<string, string> chiave in sezioni.Value)
                    {
                        // value must be in one line
                        string vale = chiave.Value.ToString();
                        vale = vale.Replace(Environment.NewLine, " ");
                        vale = vale.Replace("\r\n", " ");
                        sw.WriteLine(chiave.Key.ToString() + " = " + vale);
                    }
                }
            }
        }

        public static string IniReadValue(Sections _Section, Keys _Key)
        {
            if (!Initialized)
                FirstRead();
            return IniReadValue(_Section.ToString(), _Key.ToString());
        }
        public static string IniReadValue(string _Section, string _Key)
        {
            if (!Initialized)
                FirstRead();
            if (IniDictionary.ContainsKey(_Section))
                if (IniDictionary[_Section].ContainsKey(_Key))
                    return IniDictionary[_Section][_Key];
            return null;
        }
    }
}