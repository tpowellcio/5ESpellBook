using _5ESpellbook.Domain.Entities;
using _5ESpellbook.Domain.Repositories.Abstract;
using _5ESpellbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5ESpellbook.Builders
{
    public class SpellModelBuilder
    {
        ISpellRepository _repo;

        public SpellModelBuilder(ISpellRepository repo)
        {
            _repo = repo;
        }

        public SpellModel BuildSpellModel(Spell spell)
        {
            try
            {
                SpellModel model = new SpellModel();

                model.spell = spell;

                string description = string.Empty;

                if (spell.Level == 0)
                    model.levelSchool = spell.School + " Cantrip";
                else
                    model.levelSchool = spell.Level.ToString() + ordinal(spell.Level) + " level " + spell.School;

                if (spell.Components.Length > 20)
                {
                    model.components = "See description";
                    description = "Components: " + spell.Components + "<br />";
                }
                else
                {
                    model.components = spell.Components;
                }

                if (spell.Duration.Length > 20)
                {
                    model.duration = "See description";
                    description += "Duration: " + spell.Duration + "<br />";
                }
                else
                {
                    model.duration = spell.Duration;
                }

                description += spell.Description;

                int descriptionLength = 600;
                int longDescriptionLength = 875;


                while (!string.IsNullOrEmpty(description))
                {
                    if (model.descriptions.Count == 0)
                    {
                        if (descriptionLength >= description.Length)
                        {
                            model.descriptions.Add(description);
                            description = string.Empty;
                        }
                        else
                        {
                            int modifiedDescLength = modifiedLength(description, descriptionLength);
                            model.descriptions.Add(description.Substring(0, modifiedDescLength) + "...");
                            description = description.Substring(modifiedDescLength);
                        }
                    }
                    else
                    {
                        if (longDescriptionLength >= description.Length && longDescriptionLength >= calculateLengthFromHtml(description))
                        {
                            model.descriptions.Add(description);
                            description = string.Empty;
                        }
                        else
                        {
                            int modifiedDescLength = modifiedLength(description, longDescriptionLength);
                            model.descriptions.Add(description.Substring(0, modifiedDescLength) + "...");
                            description = description.Substring(modifiedDescLength);
                        }
                    }
                }
                return model;
            }
            catch
            {
                throw new Exception("Error generating data for: " + spell.Name);
            }

        }

        public SpellIndexModel BuildSpellIndexModel()
        {
            SpellIndexModel model = new SpellIndexModel();
            foreach (var spell in _repo.Spells.ToList())
            {
                model.Spells.Add(spell);
            }

            GenerateSpellIndexEnumerables(ref model);

            model.Spells = model.Spells.OrderBy(x => x.Name).ToList();

            return model;
        }

        public SpellIndexModel BuildSpellIndexModel(string[] SelectedSchools, string[] SelectedLevels, string[] SelectedClasses, string[] SelectedActions, string[] SelectedKeywords)
        {
            SpellIndexModel model = new SpellIndexModel();

            List<Spell> allSpells = _repo.Spells.ToList();
            List<Spell> spellsFromSchools = new List<Spell>();
            List<Spell> spellsFromLevels = new List<Spell>();
            List<Spell> spellsFromClasses = new List<Spell>();
            List<Spell> spellsFromActions = new List<Spell>();
            List<Spell> spellsFromKeywords = new List<Spell>();

            if (SelectedSchools != null)
            {
                List<Spell> spellsWithSchools = allSpells.Where(x => !string.IsNullOrEmpty(x.School)).ToList();
                foreach (string school in SelectedSchools)
                {
                    spellsFromSchools.AddRange(spellsWithSchools.Where(x => x.School.ToLower() == school.ToLower()));
                }
            }
            else
            {
                spellsFromSchools = allSpells;
            }

            if (SelectedLevels != null)
            {
                foreach (string level in SelectedLevels)
                {
                    spellsFromLevels.AddRange(allSpells.Where(x => x.Level.ToString() == level.ToString()));
                }
            }
            else
            {
                spellsFromLevels = allSpells;
            }

            if (SelectedClasses != null)
            {
                List<Spell> spellsWithClass = allSpells.Where(x => !string.IsNullOrEmpty(x.OfClass)).ToList();
                foreach (string ofClass in SelectedClasses)
                {
                    spellsFromClasses.AddRange(spellsWithClass.Where(x => x.OfClass.ToLower().Contains(ofClass.ToLower())));
                }
            }
            else
            {
                spellsFromClasses = allSpells;
            }

            if (SelectedActions != null)
            {
                List<Spell> spellsWithActions = allSpells.Where(x => !string.IsNullOrEmpty(x.Action)).ToList();
                foreach (string action in SelectedActions)
                {
                    spellsFromActions.AddRange(spellsWithActions.Where(x => x.Action.Trim() == action));
                }
            }
            else
            {
                spellsFromActions = allSpells;
            }

            if (SelectedKeywords != null)
            {
                List<Spell> spellsWithKeyword = allSpells.Where(x => !string.IsNullOrEmpty(x.Keywords)).ToList();
                foreach (string keyword in SelectedKeywords)
                {
                    if (keyword == "NONE")
                    {
                        spellsFromKeywords.AddRange(allSpells.Where(x => string.IsNullOrEmpty(x.Keywords)));
                    }
                    else
                    {
                        spellsFromKeywords.AddRange(spellsWithKeyword.Where(x => x.Keywords.ToLower().Contains(keyword.ToLower())));
                    }
                }
            }
            else
            {
                spellsFromKeywords = allSpells;
            }

            GenerateSpellIndexEnumerables(ref model);

            model.Spells = spellsFromClasses.Intersect(spellsFromLevels).ToList();
            model.Spells = model.Spells.Intersect(spellsFromSchools).ToList();
            model.Spells = model.Spells.Intersect(spellsFromActions).ToList();
            model.Spells = model.Spells.Intersect(spellsFromKeywords).ToList();

            model.Spells = model.Spells.Distinct().ToList();


            model.Spells = model.Spells.OrderBy(x => x.Name).ToList();

            return model;
        }

        public void GenerateSpellIndexEnumerables(ref SpellIndexModel model)
        {
            List<SelectListItem> levelList = new List<SelectListItem>();
            levelList.Add(new SelectListItem { Text = "Cantrip", Value = "0" });
            levelList.Add(new SelectListItem { Text = "1", Value = "1" });
            levelList.Add(new SelectListItem { Text = "2", Value = "2" });
            levelList.Add(new SelectListItem { Text = "3", Value = "3" });
            levelList.Add(new SelectListItem { Text = "4", Value = "4" });
            levelList.Add(new SelectListItem { Text = "5", Value = "5" });
            levelList.Add(new SelectListItem { Text = "6", Value = "6" });
            levelList.Add(new SelectListItem { Text = "7", Value = "7" });
            levelList.Add(new SelectListItem { Text = "8", Value = "8" });
            levelList.Add(new SelectListItem { Text = "9", Value = "9" });

            List<SelectListItem> classList = new List<SelectListItem>();
            classList.Add(new SelectListItem { Text = "Barbarian", Value = "Barbarian" }); 
            classList.Add(new SelectListItem { Text = "Bard", Value = "Bard" });
            classList.Add(new SelectListItem { Text = "Cleric", Value = "Cleric" });
            classList.Add(new SelectListItem { Text = "Druid", Value = "Druid" });
            classList.Add(new SelectListItem { Text = "Monk", Value = "Monk" });
            classList.Add(new SelectListItem { Text = "Paladin", Value = "Paladin" });
            classList.Add(new SelectListItem { Text = "Ranger", Value = "Ranger" });
            classList.Add(new SelectListItem { Text = "Sorcerer", Value = "Sorcerer" });
            classList.Add(new SelectListItem { Text = "Warlock", Value = "Warlock" });
            classList.Add(new SelectListItem { Text = "Wizard", Value = "Wizard" });

            List<SelectListItem> schoolList = new List<SelectListItem>();
            schoolList.Add(new SelectListItem { Text = "Abjuration", Value = "Abjuration" });
            schoolList.Add(new SelectListItem { Text = "Conjuration", Value = "Conjuration" });
            schoolList.Add(new SelectListItem { Text = "Divination", Value = "Divination" });
            schoolList.Add(new SelectListItem { Text = "Enchantment", Value = "Enchantment" });
            schoolList.Add(new SelectListItem { Text = "Evocation", Value = "Evocation" });
            schoolList.Add(new SelectListItem { Text = "Illusion", Value = "Illusion" });
            schoolList.Add(new SelectListItem { Text = "Necromancy", Value = "Necromancy" });
            schoolList.Add(new SelectListItem { Text = "Transmutation", Value = "Transmutation" });

            List<SelectListItem> actionList = new List<SelectListItem>();
            
            List<SelectListItem> keyWordList = new List<SelectListItem>();
            keyWordList.Add(new SelectListItem { Text = "<No Keyword>", Value = "NONE" });
            foreach (Spell spell in _repo.Spells.Where(x => x.Keywords != null))
            {
                foreach (string keyword in spell.Keywords.Split(' '))
                {
                    if (!string.IsNullOrEmpty(keyword))
                        keyWordList.Add(new SelectListItem { Text = keyword.Trim().ToLower(), Value = keyword.Trim().ToLower() });
                }
                actionList.Add(new SelectListItem { Text = spell.Action.Trim(), Value = spell.Action.Trim() });
            }
            keyWordList = keyWordList.GroupBy(x => x.Text).Select(x => x.First()).ToList();
            keyWordList = keyWordList.OrderBy(x => x.Text).ToList();

            actionList = actionList.GroupBy(x => x.Text).Select(x => x.First()).ToList();
            actionList = actionList.OrderBy(x => x.Text).ToList();

            model.ActionList = actionList;
            model.KeywordList = keyWordList;
            model.SchooLlist = schoolList;
            model.LevelList = levelList;
            model.ClassList = classList;
        }

        private string ordinal(int number)
        {
            int ordPos = number % 10;

            if (ordPos == 1)
                return "st";
            if (ordPos == 2)
                return "nd";
            if (ordPos == 3)
                return "rd";
            else
                return "th";
        }

        private int modifiedLength(string toTake, int grabLength)
        {
            if (grabLength > toTake.Length)
                grabLength = toTake.Length - 1;

            string replaceHtml = toTake.Replace("<br />", "aaaaaa");
            replaceHtml = replaceHtml.Replace("<strong>", "aaaaaaaa");
            replaceHtml = replaceHtml.Replace("</strong>", "aaaaaaaaa");
            replaceHtml = replaceHtml.Replace("&nbsp;", "aaaaaa");

            //scope to a blank space
            while (replaceHtml[grabLength] != ' ')
            {
                grabLength--;
            }


            //check html and make adjustments from that
            grabLength = modifyGrabLengthFromHTML(toTake, grabLength);


            //rescope to blank
            while (replaceHtml[grabLength] != ' ')
            {
                grabLength--;
            }


            return grabLength;
        }

        //Reduces the grab length to account for spacing generated by html tags
        private int modifyGrabLengthFromHTML(string toTake, int grabLength)
        {
            if (toTake.Length < grabLength)
                grabLength = toTake.Length;
            string modifiedBRs = toTake.Replace("<br />\r\n<br />", "ABCABCABCABCAB");
            string[] findStrong = toTake.Substring(0, grabLength).Split(new string[] { "<strong>" }, StringSplitOptions.None);
            string[] findStrongClose = toTake.Substring(0, grabLength).Split(new string[] { "</strong>" }, StringSplitOptions.None);
            string[] findBRBR = modifiedBRs.Substring(0, grabLength).Split(new string[] { "ABCABCABCABCAB" }, StringSplitOptions.None);
            string[] findBR = modifiedBRs.Substring(0, grabLength).Split(new string[] { "<br />" }, StringSplitOptions.None);
            string[] findnbsp = toTake.Substring(0, grabLength).Split(new string[] { "&nbsp;" }, StringSplitOptions.None);

            return grabLength + ((findStrong.Length - 1) * 7) + ((findStrongClose.Length - 1) * 8) - ((findBR.Length - 1) * 20) + ((findnbsp.Length - 1) * 5) - ((findBRBR.Length - 1) * 80);

        }

        private int calculateLengthFromHtml(string description)
        {
            string modifiedBRs = description.Replace("<br />\r\n<br />", "ABCABCABCABCAB");
            string[] findStrong = description.Split(new string[] { "<strong>" }, StringSplitOptions.None);
            string[] findStrongClose = description.Split(new string[] { "</strong>" }, StringSplitOptions.None);
            string[] findBRBR = modifiedBRs.Split(new string[] { "ABCABCABCABCAB" }, StringSplitOptions.None);
            string[] findBR = modifiedBRs.Split(new string[] { "<br />" }, StringSplitOptions.None);
            string[] findnbsp = description.Split(new string[] { "&nbsp;" }, StringSplitOptions.None);

            return description.Length - ((findStrong.Length - 1) * 7) - ((findStrongClose.Length - 1) * 8) + ((findBR.Length - 1) * 20) - ((findnbsp.Length - 1) * 5) + ((findBRBR.Length - 1) * 80);

        }

        public List<SpellModel> BuildSpellListModel(List<Spell> spells)
        {
            List<SpellModel> spellList = new List<SpellModel>();
            foreach (var spell in spells)
            {
                spellList.Add(BuildSpellModel(spell));
            }

            return spellList;
        }

        public List<SpellModel> BuildSpellListModel(List<int> spellID, string sortOrder)
        {
            List<SpellModel> spellList = new List<SpellModel>();
            List<Spell> spells = _repo.Spells.ToList();

            foreach(int sid in spellID)
            {
                Spell spell = spells.FirstOrDefault(x => x.SpellID == Convert.ToInt32(sid));
                if (spell != null)
                    spellList.Add(BuildSpellModel(spell));
            }

            if(sortOrder.ToLower() == "alphabetical")
            {
                spellList = spellList.OrderBy(x => x.spell.Name).ToList();
            }
            else if (sortOrder.ToLower() == "school-level")
            {
                spellList = spellList.OrderBy(x => x.spell.School).ThenBy(x => x.spell.Level).ThenBy(x => x.spell.Name).ToList();
            }
            else if (sortOrder.ToLower() == "school-alpha")
            {
                spellList = spellList.OrderBy(x => x.spell.School).ThenBy(x => x.spell.Name).ToList();
            }
            else if (sortOrder.ToLower() == "level")
            {
                spellList = spellList.OrderBy(x => x.spell.Level).ThenBy(x => x.spell.Name).ToList();
            }

            return spellList;
        }
    }
}