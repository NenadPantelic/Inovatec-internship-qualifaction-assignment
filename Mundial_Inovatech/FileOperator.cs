using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundial_Inovatech
{
    class FileOperator
    {
        /*
         * rankDictionary - recnik oblika drzava - FIFA rang
         * continentDictionary - recnik oblika drzava - kontitent
        */
        private Dictionary<string, int> rankDictionary;
        private Dictionary<string, string> continentDictionaryMap = new Dictionary<string, string>();

        /* properites clanovi */
        public Dictionary<string, int> RankDictionary { get => rankDictionary; set => rankDictionary = value; }
        public Dictionary<string, string> ContinentDictionaryMap { get => continentDictionaryMap; set => continentDictionaryMap = value; }

        public FileOperator()
        {

            this.rankDictionary = new Dictionary<string, int>();
            this.ContinentDictionaryMap = new Dictionary<string, string>();

            //string[] continents = { "Afrika", "Azija", "Evropa", "Severna i Centralna Amerika", "Juzna Amerika" };

        }

        /*
         *  usluzna metoda koja dodaje vrednosti u navedene recnike
         *
        */
        public void AddToDicts(string[] values)
        {

            ContinentDictionaryMap[values[0]] = values[1];
            Int32.TryParse(values[2], out int rank);
            rankDictionary.Add(values[0], rank);
        }

        /*
         * otvara fajl uz zastitu pomocu izuzetaka - cita liniju po liniju i pomocu AddToDicts dodaje podatke u recnike
         * finally mislim da nije neophodno, jer koliko se secam using(..) automatski oslobadja resurse - slicno try-with-resources u Javi, ali cisto formalnosti 
         * i sigurnosti stavljam 
        */
        public void ReadFile(String FileName)
        {
            StreamReader sr = null;
            try
            {
                using (sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        this.AddToDicts(values);
                    }
                }
            }

            catch (FileNotFoundException e)
            {
                Console.WriteLine("Nije moguce procitati fajl.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            finally
            {
                sr.Close();
            }

        }

        /*
         *  metoda koja ispisuje sadrzaj u csv fajl
         * finally mislim da nije neophodno, jer koliko se secam using(..) automatski oslobadja resurse - slicno try-with-resources u Javi, ali cisto formalnosti 
         * i sigurnosti stavljam 
         * 
        */
        public void WriteFile(String filename, Dictionary<string, List<string>> groups)
        {
            StreamWriter sw = null;
            try
            {

                StringBuilder sb = new StringBuilder();
                using (sw = new StreamWriter(filename, false))
                {
                    foreach (KeyValuePair<string, List<string>> item in groups)
                    {

                        sb.Append(item.Key + ",");
                        for (int iter = 0; iter < 3; iter++)
                        {
                            sb.Append(iter + 1 + "." + item.Value[iter] + ",");
                        }
                        sb.Append("4." + item.Value[3]);
                        sw.WriteLine(sb);
                        sb.Clear();
                    }

                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Nije moguce procitati fajl.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            finally
            {
                sw.Close();
            }

        }
    }
}