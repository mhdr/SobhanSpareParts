using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class ObservableBrands : ObservableCollection<Brand>
    {
        private SparePartsEntities _entities;

        public ObservableBrands(IEnumerable<Brand> brands, SparePartsEntities entities)
            : base(brands)
        {
            Entities = entities;
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        protected override void InsertItem(int index, Brand item)
        {
            Entities.Brands.Add(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Entities.Brands.Remove(this[index]);
            base.RemoveItem(index);
        }

        public void Notify()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
