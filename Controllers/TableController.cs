using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lab1_2.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace lab1_2.Controllers
{
    public class TableController : Controller
    {
        // GET: TableController
        public ActionResult Index(string id, string db) //shows table's entries
        {
            Database DB = new Database();
            DB.Name = db;
            DB.LoadTables("TextData\\" + db + "\\tables.txt","TextData\\" + db + "\\schemes.txt");
            Collection<Table> tables = DB.GetTables();

            Table thisTable = new Table();

            foreach (Table t in tables)
                if (t.Name == id) { 
                    thisTable.Scheme = t.Scheme; 
                    thisTable.Name = t.Name;
                    thisTable.DBName = t.DBName;
                }

            thisTable.LoadEntries();
            Collection < Entry > entries = thisTable.GetEntries();
            ViewBag.ThisScheme=thisTable.Scheme;
            ViewBag.ThisEntries=entries;
            ViewBag.table=thisTable.Name;
            ViewBag.db=db;
            return View();
        }

        // GET: TableController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TableController/Create
        public ActionResult Create(string dbname) //creates a new table
        {
            ViewBag.dbname = dbname;
            return View();
        }

        // POST: TableController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string dbname,CreateTableRequest request)
        {
            try
            {
                Database thisDB = new Database();
                thisDB.Name = dbname;
                Table thisTable= new Table();
                thisTable.Name = request.TableName;
                foreach (var c in request.Columns) { 
                    thisTable.Scheme.Add((c.Name,c.Type));
                }

                thisDB.LoadTables("TextData//"+thisDB.Name+"//tables.txt", "TextData//" + thisDB.Name + "//schemes.txt");
                thisDB.AddTable(thisTable);

                return RedirectToAction(nameof(Details), "Database", new {id=thisDB.Name});
            }
            catch
            {
                return View();
            }
        }

        // GET: TableController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TableController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TableController/Delete/5
        public ActionResult Delete(int id, string table, string db) //deletes an entry
        {
            ViewBag.id = id;
            ViewBag.table = table;
            ViewBag.db = db;
            return View();
        }

        // POST: TableController/Delete/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string table, string db, int id, IFormCollection collection)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(("TextData\\" + db + "\\" + table + ".txt"));
                Collection<string> entries = new Collection<string>();
                foreach (string line in lines) { entries.Add(line); }
                entries.Remove(entries[id]);
                //edits the table's file
                System.IO.File.WriteAllLines(("TextData\\" + db + "\\" + table + ".txt"), entries);
                
                return RedirectToAction(nameof(Index), new{ id=table, db=db});
            }
            catch
            {
                return View();
            }
        }
    }
}
public class CreateTableRequest
{
    public string TableName { get; set; }
    public List<ColumnRequest> Columns { get; set; }
}

public class ColumnRequest
{
    public string Name { get; set; }
    public string Type { get; set; }
}
