using System.Collections.ObjectModel;

namespace lab1_2.Models
{
    public class Table
    {
        public string Name { get; set; }
        public Collection<(string, string)> Scheme { get; set; } = new Collection<(string, string)> ();
        private Collection<Entry> Entries = new Collection<Entry> { };
        public bool Dropped { get; set; } = false;
        public string DBName { get; set; }

        //public Table(string name, Collection<(string, string)> scheme, string dbname)
        //{
        //    Name = name;
        //    //foreach (Entry entry in entries) { Entries.Add(entry); }
        //    foreach ((string, string) s in scheme) { Scheme.Add(s); }
        //    DBName = dbname;
        //}

        public void AddEntry(Entry entry)
        {
            Entries.Add(entry);
        }

        public void EditEntry(int number, Entry edits)
        {
            //copying parameters from edits to entry number n in collection
        }

        public Collection<Entry> GetEntries()
        {
            return Entries;
        }

        public void LoadEntries()
        {
            Entries.Clear();
            string[] entries = File.ReadAllLines("TextData\\" + DBName + "\\" + Name + ".txt");
            foreach (string entry in entries)
            {
                string[] entryContent = entry.Split('|');
                //Collection<string> content = new Collection<string>();
                //foreach (string line in entryContent) { content.Add(line); }
                Entry newEntry = new Entry(entryContent, Name);
                AddEntry(newEntry);
            }
        }

        public void DropTable() { Dropped = true; }
    }
}
