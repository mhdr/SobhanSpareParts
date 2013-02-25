using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class ObservableMachines : ObservableCollection<Machine>
    {
        private SparePartsEntities _entities;

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public ObservableMachines(IEnumerable<Machine> machines, SparePartsEntities entities)
            : base(machines)
        {
            Entities = entities;
        }

        protected override void InsertItem(int index, SpareParts.Machine item)
        {
            Entities.Machines.Add(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Entities.Machines.Remove(this[index]);
            base.RemoveItem(index);
        }

        public void Notify()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
