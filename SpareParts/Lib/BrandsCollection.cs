using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class BrandsCollection : ObservableCollection<Brand>
    {
        private SparePartsEntities _entities;

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public BrandsCollection(IEnumerable<Brand> brands, SparePartsEntities entities)
            : base(brands)
        {
            Entities = entities;
        }

        protected override void InsertItem(int index, SpareParts.Brand item)
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
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
