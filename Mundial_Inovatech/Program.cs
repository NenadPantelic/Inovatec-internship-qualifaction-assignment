using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Driver klasa koja testira i pokrece App
 * debug linije zakomentarisane
 * 
 */
namespace Mundial_Inovatech
{
    class Program
    {

        static void Main(string[] args)
        {


            FileOperator fileOperator = new FileOperator();
            fileOperator.ReadFile("ulaz.csv");



            Scheduler sch = new Scheduler(fileOperator.RankDictionary, fileOperator.ContinentDictionaryMap);
            sch.MakeSelectionHats();

            sch.AddBestTeams();
            sch.MakeSchedule();
            /*
             * string letters = "ABCDEFGH";
             * foreach(KeyValuePair<string,string> kv in sch.ContinentDictionaryMap)
            {
                Console.WriteLine(kv.Key + " " + kv.Value);
            }
            /*for (int i = 0; i < 8; i++)
            {
                foreach (string s in sch.Groups[letters.Substring(i,1)])
                    Console.WriteLine(s);
            }
            */


            /*
            for (int i = 0; i < 8; i++)
            {
                foreach (string team in sch.Groups[letters.Substring(i, 1)])
                    Console.WriteLine("Group:" + letters.Substring(i, 1) + " team: " + team);

            }
            */
            fileOperator.WriteFile("izlaz.csv", sch.Groups);
            //Console.ReadLine();
        }
    }
}
