using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5ESpellbook.Domain.Entities
{
    [Table(Name="Spells")]
    public class Spell
    {
        [Column(AutoSync=AutoSync.OnInsert,CanBeNull=false,IsDbGenerated=true,IsPrimaryKey=true)]
        public int SpellID { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public int Level { get; set; }

        [Column]
        public string School { get; set; }

        [Column]
        [DisplayName("Of Class")]
        public string OfClass { get; set; }

        [Column]
        public string Action { get; set; }

        [Column]
        public string Range { get; set; }

        [Column]
        public string Components { get; set; }

        [Column]
        public string Duration { get; set; }
        
        [Column]
        public string Description { get; set; }

        [Column]
        [DisplayName("Ritual?")]
        public bool IsRitual { get; set; }

        [Column]
        public string Keywords { get; set; }

        public Spell()
        {
            SpellID = 0;
            Level = 0;
            Name = string.Empty; 
            School = string.Empty; 
            Action = string.Empty; 
            Range = string.Empty; 
            Components = string.Empty;
            Duration = string.Empty;
            Description = string.Empty;
            IsRitual = false;
            Keywords = string.Empty;
        }
    }
}
