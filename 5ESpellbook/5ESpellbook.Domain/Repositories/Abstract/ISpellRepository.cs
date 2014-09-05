using _5ESpellbook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5ESpellbook.Domain.Repositories.Abstract
{
    public interface ISpellRepository
    {
        IQueryable<Spell> Spells { get; }

        void SaveSpell(Spell spell);

        void DeleteSpell(Spell spell);

        void Save();
    }
}
