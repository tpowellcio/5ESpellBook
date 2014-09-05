using _5ESpellbook.Domain.Entities;
using _5ESpellbook.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5ESpellbook.Domain.Repositories.Concrete
{
    public class SQLSpellRepository : ISpellRepository
    {
        private DataContext context;

        private Table<Spell> _spellsTable;

        public SQLSpellRepository(string cs)
        {
            context = new DataContext(cs);

            _spellsTable = context.GetTable<Spell>();
        }

        public IQueryable<Spell> Spells { get { return _spellsTable; } }

        public void SaveSpell(Spell spell)
        {
            if (spell.SpellID == 0)
                _spellsTable.InsertOnSubmit(spell);
        }

        public void DeleteSpell(Spell spell)
        {
            _spellsTable.DeleteOnSubmit(spell);
        }

        public void Save()
        {
            context.SubmitChanges();
        }
    }
}
