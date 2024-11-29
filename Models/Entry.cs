using System.Collections.ObjectModel;

namespace lab1_2.Models
{
    public class Entry
    {
        public Collection<string> Content { get; set; } = new Collection<string>(); //колекція значень стовпців
        public string TableName { get; set; }

        public Entry(string[] content, string tableName)
        {
            for (int i = 0; i < content.Length; i++)
            {
                Content.Add(content[i]);
            }
            TableName = tableName;
        }

    }
}
