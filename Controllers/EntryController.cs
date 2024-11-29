using lab1_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace lab1_2.Controllers
{
    public class EntryController : Controller
    {
        // GET: EntryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EntryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EntryController/Create
        public ActionResult Create(string dbname,string tablename)
        {
            ViewBag.dbname=dbname;
            ViewBag.tablename=tablename;
            var tables = System.IO.File.ReadAllLines("TextData\\" + dbname + "\\tables.txt");
            var schemes = System.IO.File.ReadAllLines("TextData\\" + dbname + "\\schemes.txt");
            int ind = 0;
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i] == tablename) { 
                    ind = i;
                break;
                }
            }
            string[] schemeArr = schemes[ind].Split('|');
            Collection<(string, string)> scheme = new Collection<(string, string)>{ };
            for (int i = 0; i < schemeArr.Length; i+=2)
            {
                scheme.Add((schemeArr[i], schemeArr[i+1]));
            }
            ViewBag.Columns = scheme;
            return View();
        }

        // POST: EntryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, string dbName, string tableName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(("TextData\\" + dbName + "\\" + tableName + ".txt"));
                Collection<string> entries = new Collection<string>();
                foreach (var line in lines)
                {
                    entries.Add(line);
                }
                string entry = "";
                foreach (var item in collection)
                {
                    if (item.Key!="dbName" && item.Key!="tableName" && item.Key!= "__RequestVerificationToken")
                        entry = entry + item.Value.ToString() + "|";
                }
                entries.Add(entry);
                System.IO.File.WriteAllLines(("TextData\\" + dbName + "\\" + tableName + ".txt"), entries);
                return RedirectToAction(nameof(Index), "Table", new {id=tableName,db=dbName});
            }
            catch
            {
                return View();
            }
        }

        // GET: EntryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EntryController/Edit/5
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

        // GET: EntryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EntryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
