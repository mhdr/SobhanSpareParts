using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class RequestsObservableCollection : ObservableCollection<RequestWithNotify>
    {
        private SparePartsEntities _entities;

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public RequestsObservableCollection(IEnumerable<RequestWithNotify> requests, SparePartsEntities entities) : base(requests)
        {
            this.Entities = entities;
        }

        public RequestsObservableCollection(IEnumerable<Request> requests, SparePartsEntities entities)
        {
            foreach (Request request in requests)
            {
                RequestWithNotify requestWithNotify=new RequestWithNotify();
                requestWithNotify.RequestId = request.RequestId;
                requestWithNotify.ResolutionPartNo = request.ResolutionPartNo;
                requestWithNotify.PartNo = request.PartNo;
                requestWithNotify.RequestDate = request.RequestDate;
                requestWithNotify.Qty = request.Qty;
                requestWithNotify.EntranceDate = request.EntranceDate;
                requestWithNotify.Description = request.Description;
                requestWithNotify.TimeStamp = request.TimeStamp;
                this.Add(requestWithNotify);
            }

            this.Entities = entities;
        }

        public bool AddNew(int index,RequestWithNotify item)
        {
            Request request=new Request();
            request.ResolutionPartNo = item.ResolutionPartNo;
            request.PartNo = item.PartNo;
            request.PartNoOriginal = item.PartNoOriginal;
            request.RequestDate = item.RequestDate;
            request.Qty = item.Qty;
            request.EntranceDate = item.EntranceDate;
            request.Description = item.Description;

            Entities.Requests.Add(request);

            if (Entities.SaveChanges() > 0)
            {
                item.RequestId = request.RequestId;
                item.TimeStamp = request.TimeStamp;

                base.InsertItem(index,item);
                return true;
            }

            return false;
        }

        public bool Delete(int index)
        {
            RequestWithNotify requestWithNotify = this[index];
            Request request = Entities.Requests.FirstOrDefault(x=>x.RequestId==requestWithNotify.RequestId);
            Entities.Requests.Remove(request);

            if (Entities.SaveChanges() > 0)
            {
                base.RemoveItem(index);
                return true;
            }

            return false;
        }

        public bool Update(int index,RequestWithNotify item)
        {
            RequestWithNotify requestWithNotify = this[index];
            Request request = Entities.Requests.FirstOrDefault(x => x.RequestId == requestWithNotify.RequestId);

            request.ResolutionPartNo = item.ResolutionPartNo;
            request.PartNo = item.PartNo;
            request.RequestDate = item.RequestDate;
            request.Qty = item.Qty;
            request.EntranceDate = item.EntranceDate;
            request.Description = item.Description;

            if (Entities.SaveChanges() > 0)
            {
                item.TimeStamp = request.TimeStamp;

                base.SetItem(index,item);
                return true;
            }

            return false;
        }
    }
}
