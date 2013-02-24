using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class ObservableParts : ObservableCollection<Part>
    {
        private SparePartsEntities _entities;

        public ObservableParts(IEnumerable<Part> parts,SparePartsEntities entities):base(parts)
        {
            Entities = entities;
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        protected override void InsertItem(int index, Part item)
        {
            Entities.Parts.Add(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Entities.Parts.Remove(this[index]);
            base.RemoveItem(index);
        }
    }
}
