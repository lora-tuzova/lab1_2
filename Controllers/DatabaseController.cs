using lab1_2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IO;
using lab1_2.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace lab1_2.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly CustomValidator validator;
        private readonly CollectionComparer collectionComparer;

        public DatabaseController(CustomValidator validator, CollectionComparer comparer) {
            this.validator = validator;
            this.collectionComparer = comparer;
        }

        // GET: DatabaseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DatabaseController/Details/5
        public ActionResult Details(string id) //shows a list of database tables
        {

            Database ThisDB = new Database();
            ThisDB.Name = id;
            string pathTables = "TextData\\" + id + "\\tables.txt";
            string pathSchemes = "TextData\\" + id + "\\schemes.txt";
            ThisDB.LoadTables(pathTables, pathSchemes);
            ViewBag.DB = ThisDB;
            ViewBag.Tables = new Collection<string>();
            foreach (Table t in ThisDB.GetTables())
                ViewBag.Tables.Add(t.Name); //passing a list of active tables
            return View();
        }

        // GET: DatabaseController/Create
        public ActionResult Create()
        {
            ViewBag.message = "";
            return View();
        }

        // POST: DatabaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name)
        {
            Validator validator = new Validator();
            string messag = validator.ValidateNameForPath(name);
            try
            {
                if (messag ==  "" )
                {
                    Directory.CreateDirectory("TextData\\" + name);
                    System.IO.File.Create("TextData\\" + name + "\\tables.txt");
                    var dbs = System.IO.File.ReadAllLines("TextData\\databases.txt");
                    Collection <string> bases = new Collection<string>();
                    foreach (string s in dbs)
                    {
                        bases.Add(s);
                    }
                    bases.Add(name);
                    System.IO.File.WriteAllLines("TextData\\databases.txt", bases );
                    return RedirectToAction(nameof(Index), "Home");
                }
                ViewBag.message = messag;
                return View();
            }
            catch
            {
                ViewBag.message = messag;
                return View();
            }
        }

        // GET: DatabaseController/Edit/5
        public ActionResult Edit(string id)
        {
            Database DB = new Database();
            DB.Name = id;
            return View(DB);
        }

        // POST: DatabaseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            //try
            //{
            //    if (validator.ValidateNameForPath(newNameDB.Name))
            //    {
            //        return RedirectToAction(nameof(Index),newNameDB.Name);
            //    }
            //    return View();
            //}
            //catch
            //{
                return View();
            //}
        }

        // GET: DatabaseController/Delete/5
        public ActionResult Delete(string table, string db) //deletes a table from the view
        {
            ViewBag.table = table;
            ViewBag.db = db;
            return View();
        }

        // POST: DatabaseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string table, string db, IFormCollection collection)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(("TextData\\" + db + "\\" + "tables.txt"));
                string[] schemes = System.IO.File.ReadAllLines(("TextData\\" + db + "\\" + "schemes.txt"));
                Collection<string> tables = new Collection<string>();
                Collection<string> scheme = new Collection<string>();
                int i=-1;
                for(int j=0;j<lines.Length;j++) { tables.Add(lines[j]); scheme.Add(schemes[j]); if (lines[j] == table) i = j; }
                if (i >= 0)
                {
                    tables.Remove(tables[i]);
                    scheme.Remove(scheme[i]);
                }
                //edits the table's file
                System.IO.File.WriteAllLines(("TextData\\" + db + "\\" + "tables.txt"), tables);
                System.IO.File.WriteAllLines(("TextData\\" + db + "\\" + "schemes.txt"), scheme);
                return RedirectToAction(nameof(Details), new {id=db}); 
            }
            catch
            {
                return View();
            }
        }

        // Метод для знаходження перетину двох таблиць
        public IActionResult FindIntersection(IFormCollection collection)
        {
            
            // Отримуємо таблиці за іменами
            var table1 = System.IO.File.ReadAllLines(("TextData\\" + collection["dbName"] + "\\" + collection["tableName1"] +".txt")).ToList();
            var table2 = System.IO.File.ReadAllLines(("TextData\\" + collection["dbName"] + "\\" + collection["tableName2"] + ".txt")).ToList();

            // Якщо таблиці знайдені
            if (table1 != null && table2 != null)
            {
                // Пошук перетину між двома таблицями (порівнюємо їхні стовпці)
                var intersection = table1.Intersect(table2).ToList();
                Collection<string[]> rows = new Collection<string[]>();
                foreach (var row in intersection) {
                    rows.Add(row.Split('|'));
                }
                ViewBag.Rows= rows;

                var tables = System.IO.File.ReadAllLines("TextData\\" + collection["dbName"] + "\\tables.txt");
                var schemes = System.IO.File.ReadAllLines("TextData\\" + collection["dbName"] + "\\schemes.txt");
                int ind = 0;
                for (int i = 0; i < tables.Length; i++)
                {
                    if (tables[i] == collection["tableName1"])
                    {
                        ind = i;
                        break;
                    }
                }
                string[] schemeArr = schemes[ind].Split('|');
                Collection<(string, string)> scheme = new Collection<(string, string)> { };
                for (int i = 0; i < schemeArr.Length; i += 2)
                {
                    scheme.Add((schemeArr[i], schemeArr[i + 1]));
                }
                ViewBag.Columns = scheme;
                ViewBag.Message = "Intersection between " + collection["tableName1"] + " and " + collection["tableName2"];
                    // Передаємо результат в View
                return View();
            }

            // Якщо таблиці не знайдені
            ViewBag.Message = "Таблиці не знайдено.";
            return View();
        }
    }

}

class Validator
{
    public string ValidateNameForPath(string name)
    {
        Collection<char> invalidCharacters = new Collection<char>{ '[','<','>',':','"','/','\\','|','?','*',']'};
            if (name.Length<0)
        {
            return "Name cannot be empty.";
        }
        foreach (char c in invalidCharacters) {
            if (name.Contains(c))
            {
                return "Name contains invalid characters.";
            } 
        }
        if (name.Length > 255)
        {
            return "Name is too long. Maximum length is 255 characters.";
        }
        return "";
    }
}
