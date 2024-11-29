using System.Collections.ObjectModel;

namespace lab1_2.Models
{
    public class Database
    {
        public string Name { get; set; }
        private Collection<Table> Tables = new Collection<Table> { };


        //public Database(string name) { Name = name; }

        public void AddTable(Table table)
        {
            Tables.Add(table);
            System.IO.File.Create("TextData\\" + Name + "\\" + table.Name + ".txt").Close();
            string tables = "";
            foreach (Table t in Tables) tables = tables + t.Name + "\n";
            System.IO.File.WriteAllText("TextData\\" + Name + "\\tables.txt", tables);

            string schemes = "";

            foreach (Table t in Tables)
            {
                int j = 0;
                foreach ((string, string) s in t.Scheme)
                {
                    j++;
                    if (j == t.Scheme.Count) schemes = schemes + s.Item1 + "|" + s.Item2;
                    else schemes = schemes + s.Item1 + "|" + s.Item2 + "|";

                }
                schemes.Remove(schemes.Length - 2);
                schemes += "\n";
            }
            System.IO.File.WriteAllText("TextData\\" + Name + "\\schemes.txt", schemes);
        }

        public IEnumerable<IEnumerable<string>> Intersect(Table leftTable, Table rightTable)
        {

            bool flag = false;
            leftTable.LoadEntries();
            rightTable.LoadEntries();
            Collection<Collection<string>> lEntries = new Collection<Collection<string>>();
            Collection<Collection<string>> rEntries = new Collection<Collection<string>>();

            foreach (Entry e in leftTable.GetEntries()) lEntries.Add(e.Content);
            foreach (Entry e in rightTable.GetEntries()) rEntries.Add(e.Content);

            IEnumerable<IEnumerable<string>> entries = lEntries.Intersect(rEntries);

            return entries;
        }

        public bool DropTable(string tableName)
        {
            foreach (Table table in Tables) if (table.Name == tableName) { table.DropTable(); return true; }

            return false;
        }

        public Collection<Table> GetTables() { return Tables; }

        public void LoadTables(string pathTables, string pathSchemes)
        {
            string[] lines = System.IO.File.ReadAllLines(pathTables);
            string[] schemes = System.IO.File.ReadAllLines(pathSchemes);
            Collection<(string, string)> scheme = new Collection<(string, string)> { };
            Collection<Entry> entries = new Collection<Entry>();



            for (int i = 0; i < lines.Length; i++)
            {
                entries.Clear();
                scheme.Clear();

                string[] temp = System.IO.File.ReadAllLines("TextData\\" + Name + "\\" + lines[i] + ".txt");//усі ентрі
                foreach (string t in temp)
                {
                    string[] content = t.Split(' '); //кожне ентрі ділиться на стовпчики
                    Entry e = new Entry(content, lines[i]); //та створюється інстанс
                    entries.Add(e); //додається до ентрі таблиці
                }

                string[] tempScheme = schemes[i].Split('|');
                for (int j = 0; j < tempScheme.Length; j += 2)
                {
                    scheme.Add((tempScheme[j], tempScheme[j + 1]));

                }
                Table newTable = new Table();
                newTable.Name = lines[i];
                newTable.Scheme= scheme;
                newTable.DBName = Name;
                Tables.Add(newTable);

            }



        }


        public void SaveClean()
        {
            foreach (Table table in Tables) if (table.Dropped == true) Tables.Remove(table);
        }

    }
}
