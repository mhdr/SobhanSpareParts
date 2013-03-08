using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class BrandsObservableCollection : ObservableCollection<BrandWithINotify>
    {
        private SparePartsEntities _entities;

        public BrandsObservableCollection(IEnumerable<BrandWithINotify> brands, SparePartsEntities entities)
            : base(brands)
        {
            this.Entities = entities;
        }

        public BrandsObservableCollection(IEnumerable<Brand> brands, SparePartsEntities entities)
        {
            this.SuppressEntity = true;

            foreach (var brand in brands)
            {
                this.Add(new BrandWithINotify() { BrandId = brand.BrandId, BrandName = brand.BrandName, TimeStamp = brand.TimeStamp });
            }

            this.Entities = entities;

            this.SuppressEntity = false;
        }

        private SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public bool SuppressEntity { get; set; }

        public bool AddNew(int index,BrandWithINotify item)
        {
            if (!this.SuppressEntity)
            {
                Brand newBrand = new Brand();
                newBrand.BrandName = item.BrandName;
                Entities.Brands.Add(newBrand);
                
                if (Entities.SaveChanges() > 0)
                {
                    item.BrandId = newBrand.BrandId;
                    item.TimeStamp = newBrand.TimeStamp;

                    base.InsertItem(index,item);
                    return true;
                }

                return false;
            }

            base.InsertItem(index, item);
            return true;
        }

        public bool Delete(int index)
        {
            BrandWithINotify brandWithINotify = this[index];
            Brand brand = Entities.Brands.FirstOrDefault(x => x.BrandId == brandWithINotify.BrandId);
            Entities.Brands.Remove(brand);

            if (Entities.SaveChanges() > 0)
            {
                base.RemoveItem(index);
                return true;
            }

            return false;
        }

        public bool Update(int index,BrandWithINotify item)
        {
            Brand brand = Entities.Brands.FirstOrDefault(x => x.BrandId == item.BrandId);
            brand.BrandName = item.BrandName;

            if (Entities.SaveChanges() > 0)
            {
                item.TimeStamp = brand.TimeStamp;

                base.SetItem(index, item);
                return true;
            }

            return false;
        }
    }
}
