using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class PartsObservableCollection : ObservableCollection<PartWithNotify>
    {
        private SparePartsEntities _entities;

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public PartsObservableCollection(IEnumerable<PartWithNotify> parts, SparePartsEntities entities)
            : base(parts)
        {
            this.Entities = entities;
        }

        public PartsObservableCollection(IEnumerable<Part> parts, SparePartsEntities entities)
        {
            foreach (var part in parts)
            {
                PartWithNotify partWithNotify = new PartWithNotify();
                partWithNotify.PartId = part.PartId;
                partWithNotify.Location = part.Location;
                partWithNotify.TagName = part.TagName;
                partWithNotify.ResolutionPartNo = part.ResolutionPartNo;
                partWithNotify.PartNo = part.PartNo;
                partWithNotify.PartName = part.PartName;
                partWithNotify.PartNoOrignal = part.PartNoOrignal;
                partWithNotify.BrandId = part.BrandId;
                partWithNotify.MachineId = part.MachineId;
                partWithNotify.BrandName = part.Brand.BrandName;
                partWithNotify.MachineName = part.Machine.MachineName;

                this.Add(partWithNotify);
            }

            this.Entities = entities;
        }

        public bool AddNew(int index, PartWithNotify item)
        {
            Part part = new Part();
            part.PartId = item.PartId;
            part.Location = item.Location;
            part.TagName = item.TagName;
            part.ResolutionPartNo = item.ResolutionPartNo;
            part.PartNo = item.PartNo;
            part.PartName = item.PartName;
            part.PartNoOrignal = item.PartNoOrignal;
            part.BrandId = item.BrandId;
            part.MachineId = item.MachineId;

            if (Entities.SaveChanges() > 0)
            {
                item.PartId = part.PartId;
                item.TimeStamp = part.TimeStamp;


                base.InsertItem(index, item);
                return true;
            }

            return false;
        }

        public bool Delete(int index)
        {
            PartWithNotify partWithNotify = this[index];
            Part part = Entities.Parts.FirstOrDefault(x => x.PartId == partWithNotify.PartId);
            Entities.Parts.Remove(part);

            if (Entities.SaveChanges() > 0)
            {
                base.RemoveItem(index);
                return true;
            }

            return false;
        }

        public bool Update(int index, PartWithNotify item)
        {
            PartWithNotify partWithNotify = this[index];
            Part part = Entities.Parts.FirstOrDefault(x => x.PartId == partWithNotify.PartId);

            part.Location = item.Location;
            part.TagName = item.TagName;
            part.ResolutionPartNo = item.ResolutionPartNo;
            part.PartNo = item.PartNo;
            part.PartName = item.PartName;
            part.PartNoOrignal = item.PartNoOrignal;
            part.BrandId = item.BrandId;
            part.MachineId = item.MachineId;

            if (Entities.SaveChanges() > 0)
            {
                item.TimeStamp = part.TimeStamp;

                base.SetItem(index, item);
                return true;
            }

            return false;
        }
    }
}
