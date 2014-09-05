using _5ESpellbook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5ESpellbook.Models
{
    public class SpellIndexModel
    {
        public string[] SelectedSchools { get; set; }

        public string[] SelectedClasses { get; set; }

        public string[] SelectedLevels { get; set; }

        public string[] SelectedActions { get; set; }

        public string[] SelectedKeywords { get; set; }

        public IEnumerable<SelectListItem> SchooLlist { get; set; }

        public IEnumerable<SelectListItem> ClassList { get; set; }

        public IEnumerable<SelectListItem> LevelList { get; set; }

        public IEnumerable<SelectListItem> ActionList { get; set; }

        public IEnumerable<SelectListItem> KeywordList { get; set; }

        public List<Spell> Spells { get; set; }

        public IEnumerable<string> SelectedSpells { get; set; }

        public SpellIndexModel()
        {
            Spells = new List<Spell>();
        }
    }
}