using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            this.Entities = entities;

            foreach (Request request in requests)
            {
                RequestWithNotify requestWithNotify=new RequestWithNotify();
                requestWithNotify.RequestId = request.RequestId;
                requestWithNotify.ResolutionPartNo = request.ResolutionPartNo;
                requestWithNotify.PartNo = request.PartNo;
                requestWithNotify.PartNoOriginal = request.PartNoOriginal;


                IEnumerable<string> machines = null;

                if (!string.IsNullOrEmpty(request.PartNo))
                {
                    var firstOrDefault = Entities.Parts.FirstOrDefault(x => x.PartNo == request.PartNo);
                    if (firstOrDefault != null)
                        requestWithNotify.PartName = firstOrDefault.PartName;

                    Request request1 = request;
                    machines = from part in Entities.Parts
                               where part.PartNo == request1.PartNo
                                   select part.Machine.MachineName;
                }
                else if (!string.IsNullOrEmpty(request.PartNoOriginal))
                {
                    var firstOrDefault = Entities.Parts.FirstOrDefault(x => x.PartNoOrignal == request.PartNoOriginal);
                    if (firstOrDefault != null)
                        requestWithNotify.PartName = firstOrDefault.PartName;

                    Request request1 = request;
                    machines = from part in Entities.Parts
                               where part.PartNoOrignal==request1.PartNoOriginal
                                   select part.Machine.MachineName;
                }
                else if (!string.IsNullOrEmpty(request.ResolutionPartNo))
                {
                    var firstOrDefault = Entities.Parts.FirstOrDefault(x => x.ResolutionPartNo == request.ResolutionPartNo);
                    if (firstOrDefault != null)
                        requestWithNotify.PartName = firstOrDefault.PartName;

                    Request request1 = request;
                    machines = from part in Entities.Parts
                               where part.ResolutionPartNo == request1.ResolutionPartNo
                                   select part.Machine.MachineName;
                }

                requestWithNotify.RequestDate = request.RequestDate;
                requestWithNotify.Qty = request.Qty;
                requestWithNotify.EntranceDate = request.EntranceDate;
                requestWithNotify.Description = request.Description;
                requestWithNotify.TimeStamp = request.TimeStamp;
                requestWithNotify.RequestStatus = (RequestStatus) request.RequestStatus;

                if (machines != null)
                {
                    var machinesDistinct = machines.Distinct();

                    string machinesComma = string.Join(",", machinesDistinct);
                    requestWithNotify.Machines = machinesComma;
                }

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
            request.RequestStatus = (int) item.RequestStatus;

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
            request.RequestStatus = (int) item.RequestStatus;

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
