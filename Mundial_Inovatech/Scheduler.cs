using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundial_Inovatech
{

    class Scheduler
    {


        /*
         * rand - PRNG instanca koja generise pseudorandom sekvence potrebne za smestanje timova u grupe
         * selectionHats - sesiri (4 sesira po 8 timova) prema FIFA rangu
         * groups - recnik koji predstavlja raspored grupa
         * 
         * 
         */
        private Random rand;
        private Dictionary<int, List<string>> selectionHats = new Dictionary<int, List<string>>();
        const string letters = "ABCDEFGH";
        private Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
        private Dictionary<string, int> rankDictionary;
        private Dictionary<string, string> continentDictionaryMap;


        public Dictionary<string, List<string>> Groups { get => groups; set => groups = value; }
        public Dictionary<int, List<string>> SelectionHats { get => selectionHats; set => selectionHats = value; }
        public Dictionary<string, string> ContinentDictionaryMap { get => continentDictionaryMap; set => continentDictionaryMap = value; }
        public Dictionary<string, int> RankDictionary { get => rankDictionary; set => rankDictionary = value; }

        /*
         * postavljanje random seeda i inicijalizacija vrednosti koje daje FileOperator objekat
         */
        public Scheduler(Dictionary<string, int> rankDictionary, Dictionary<string, string> continentDictionaryMap)
        {
            this.rand = new Random((int)DateTime.Now.Ticks + 100);
            this.RankDictionary = rankDictionary;
            this.ContinentDictionaryMap = continentDictionaryMap;
            //fileOperator.ReadFile(@fileName);
            this.InitGroups();
        }

        /*
         * kreiranje selekcionih sesira prema FIFA rangu, preuzeto iz sortiranog recnika
         */
        public void MakeSelectionHats()
        {
            for (int i = 0; i < 4; i++)
            {
                SelectionHats[i] = new List<string>();
            }

            int count = 0;
            foreach (KeyValuePair<string, int> kv in RankDictionary.OrderBy(i => i.Value))
            {
                SelectionHats[count / 8].Add(kv.Key);
                ++count;
            }

        }

        /*
         * Inicijalizacija praznih grupa
         */
        public void InitGroups()
        {
            for (int i = 0; i < letters.Length; i++)
            {
                groups.Add(letters.Substring(i, 1), new List<string>()
                {
                    "",
                    "",
                    "",
                    ""
                });

            }
        }

        /*
         *  dodavanje timova iz prvog sesira na prvo mesto svake grupe
         * 
         */ 
        public void AddBestTeams()
        {

            int position;
            string team;
            for (int i = 0; i < 8; i++)
            {
                position = rand.Next(0, 8 - i);
                team = SelectionHats[0][position];
                SelectionHats[0].RemoveAt(position);
                Groups[letters.Substring(i, 1)][0] = team;


            }

        }

        /*
         * pomocna metoda koja vraca koliko zemalja sa odredjenog kontinenta (samo za nonempty stringove)postoji u nekoj grupi
         * 
         */
        public int GetNumOfCountries(string continent, List<string> group)
        {
            int count = 0;
            foreach (string team in group)
            {
                if (team != "" && ContinentDictionaryMap[team] == continent)
                    count++;
            }

            return count;
        }

        /*
         * proverava da li je moguce da se neka ekipa smesti u neku grupu, tj da li je ispunjen uslov nepoklapanja iz istih zona, sem Evropa za koju vazi najvise
         * 2 drzave
         * 
         */ 
        public bool CheckContinentMatching(string country, List<string> group)
        {
            bool res = true;
            int numOfCountries = this.GetNumOfCountries(ContinentDictionaryMap[country], group);
            if (ContinentDictionaryMap[country] == "Evropa" && numOfCountries < 2)
            {
                res = true;
            }

            else if (numOfCountries >= 1)
            {
                res = false;
            }
            return res;
        }

        /*
         * usluzna metoda za prebrojavanje dodeljenih timova u nekoj grupi
         * 
         */ 
        public int CountNonEmptyStrings(List<string> group)
        {
            int count = 0;
            foreach (string s in group)
            {
                if (s != "") count++;
            }
            return count;
        }

        /*
         * 
         * metoda koja izvrsava rasporedjivanje timova u grupe:
         * 
         * logika je sledeca:
         * -inicijalizuj pozicije za svaku grupu koji predstavljaju pozicije gde je mopguce smestiti tim unutar grupe(2.,3.,4. mesto)
         * - iteriraj kroz sesire
         * -dokle god nije ispunjen uslov da su svi timovi iz tog sesira dodeljeni,  vrti petlju
         * -preuzmi ekipu
         * -kreni kroz grupe - proveri kojoj grupi nije dodeljena ekipa u tekucoj iteraciji - odnosno iz tekuceg sesira (konkretno linija 189)
         * -ukoliko je moguce smestiti tim u neku grupu, odredi mu poziciju za tu grupu, smesti tim i izbrisi tim iz selekcionog sesira
         * 
         * 
         * 
         */
        public void MakeSchedule()
        {
            string letter;
            int position, num;
            string choice;
            Dictionary<string, List<int>> availablePositions = new Dictionary<string, List<int>>();
            for (int i = 0; i < letters.Length; i++)
            {
                availablePositions.Add(letters.Substring(i, 1), new List<int>(){1, 2, 3});
            }
            for (int i = 1; i < 4; i++)
            {
                while (SelectionHats[i].Count > 0)
                {
                    position = rand.Next(0, SelectionHats[i].Count);
                    choice = SelectionHats[i][position];

                    for (int k = 0; k < letters.Length; k++)
                    {

                        letter = letters.Substring(k, 1);
                        if (this.CountNonEmptyStrings(Groups[letter]) >= i + 1)
                            continue;
                        if (CheckContinentMatching(choice, Groups[letter]))
                        {
                            num = availablePositions[letter][rand.Next(0, availablePositions[letter].Count)];
                            Groups[letter][num] = choice;
                            SelectionHats[i].RemoveAt(position);
                            availablePositions[letter].Remove(num);
                            break;
                        }

                    }
                }

            }

        }

    }

}