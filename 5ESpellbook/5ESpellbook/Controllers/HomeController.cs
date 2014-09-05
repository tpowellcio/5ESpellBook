using _5ESpellbook.Builders;
using _5ESpellbook.Domain.Entities;
using _5ESpellbook.Domain.Repositories.Abstract;
using _5ESpellbook.Domain.Repositories.Concrete;
using _5ESpellbook.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5ESpellbook.Controllers
{
    public class HomeController : Controller
    {
        private ISpellRepository _repo;
        private SpellModelBuilder _builder;
        public HomeController()
        {
            _repo = new SQLSpellRepository(ConfigurationManager.ConnectionStrings["Spellbook"].ToString());
            _builder = new SpellModelBuilder(_repo);
        }

        private static string[] mobileDevices = new string[] {"iphone","ppc",
                                                      "windows ce","blackberry",
                                                      "opera mini","mobile","palm",
                                                      "portable","opera mobi" };

        public static bool IsMobileDevice(string userAgent)
        {
            // TODO: null check
            userAgent = userAgent.ToLower();
            return mobileDevices.Any(x => userAgent.Contains(x));
        }

        public ActionResult Index()
        {
            if (IsMobileDevice(Request.UserAgent))
            {
                // Return mobile view
                return View("mIndex", _builder.BuildSpellIndexModel());
            }

            return View(_builder.BuildSpellIndexModel());
        }

        public ActionResult SpellIndexFilter(string[] SelectedClasses, string[] SelectedSchools, string[] SelectedLevels, string[] SelectedActions, string[] SelectedKeywords)
        {
            return View("Index", _builder.BuildSpellIndexModel(SelectedSchools, SelectedLevels, SelectedClasses, SelectedActions, SelectedKeywords));
        }

        public ActionResult Generate(IEnumerable<string> SelectedSpells, string ofType)
        {
            if(SelectedSpells == null)
            {
                TempData["ErrorMessage"] = "You must first select the spells you wish to display. (Check a checkbox)";
                return RedirectToAction("Index");
            }

            List<Spell> spells = new List<Spell>();
            try
            {
                foreach (string spellId in SelectedSpells)
                {
                    Spell spell = _repo.Spells.FirstOrDefault(x => x.SpellID == Convert.ToInt32(spellId));
                    if (spell != null)
                        spells.Add(spell);
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "There was an error generating your book, please try again.";
                return RedirectToAction("Index");
            }

            if (ofType.Trim() == "Generate Spell Cards")
            {
                return View("SpellCards", _builder.BuildSpellListModel(spells));
            }
            else if (ofType.Trim() == "Generate Spellbook")
            {
                return View("Spellbook", _builder.BuildSpellListModel(spells));
            }
            else
            {
                TempData["ErrorMessage"] = "Select how you want your spells displayed.";
                return RedirectToAction("Index");
            }

        }

        public ActionResult AddSpell(int? id)
        {
            if (id.HasValue)
            {
                Spell spell = _repo.Spells.FirstOrDefault(x => x.SpellID == id.Value);

                if (spell == null)
                    spell = new Spell { SpellID = 0 };

                return View(_builder.BuildSpellModel(spell));
            }
            else
            {
                return View(_builder.BuildSpellModel(new Spell { SpellID = 0 }));
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Changelog()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SpellBookSort(List<int> spellID, string sortOrder)
        {
            ModelState.Remove("spellID");
            return View("SpellBook", _builder.BuildSpellListModel(spellID, sortOrder));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveSpell(SpellModel model, string ContCode)
        {
            if (ContCode != ConfigurationManager.AppSettings["ContributorCode"].ToString())
            {
                TempData["ErrorMessage"] = "You have not spoken friend, try again (error in contributor code).";

                return View("AddSpell", model);
            }

            try
            {
                Spell spell = _repo.Spells.FirstOrDefault(x => x.SpellID == model.spell.SpellID);

                if (spell == null)
                    spell = new Spell { SpellID = 0 };

                spell.School = model.spell.School;
                spell.Range = model.spell.Range;
                spell.OfClass = model.spell.OfClass;
                spell.Name = model.spell.Name;
                spell.Level = model.spell.Level;
                spell.Duration = model.spell.Duration;
                spell.Description = model.spell.Description;
                spell.Components = model.spell.Components;
                spell.Action = model.spell.Action;
                spell.IsRitual = model.spell.IsRitual;
                spell.Keywords = string.Empty;

                if(spell.Description.Contains("<p>"))
                {
                    spell.Description = spell.Description.Replace("<p>", string.Empty);
                    spell.Description = spell.Description.Replace("</p>", string.Empty);
                }


                if (!string.IsNullOrEmpty(model.spell.Keywords))
                {
                    foreach (string keyword in model.spell.Keywords.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(spell.Keywords))
                            spell.Keywords += " ";
                        spell.Keywords += keyword.ToLower();
                    }
                }


                _repo.SaveSpell(model.spell);
                _repo.Save();

                TempData["SuccessMessage"] = "Spell added to the master spell list.";

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorMessage"] = "The runic script could not be read, please try again...";

                return RedirectToAction("AddSpell", model);
            }
        }

        //public ActionResult DeleteSpell(int id)
        //{
        //    Spell spell = _repo.Spells.FirstOrDefault(x => x.SpellID == id);

        //    if (spell != null)
        //    {
        //        _repo.DeleteSpell(spell);
        //        _repo.Save();
        //    }

        //    TempData["SuccessMessage"] = "One spell has been banished from the scroll to be forgotten in the nethers...";

        //    return RedirectToAction("Index");
        //}

    }
}