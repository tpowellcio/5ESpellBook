using _5ESpellbook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5ESpellbook.Models
{
    public class SpellModel
    {
        public Spell spell { get; set; }

        public string levelSchool { get; set; }

        public string components { get; set; }

        public string duration { get; set; }



        public List<string> descriptions { get; set; }

        public SpellModel()
        {
            spell = new Spell();
            levelSchool = string.Empty;
            components = string.Empty;
            duration = string.Empty;

            descriptions = new List<string>();
            
        }
    }
}