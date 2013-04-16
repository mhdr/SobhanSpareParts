using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SpareParts.Annotations;

namespace SpareParts.Lib
{
    public class RequestWithNotify : INotifyPropertyChanged
    {
        private int _requestId;
        private string _resolutionPartNo;
        private string _partNo;
        private string _partNoOriginal;
        private System.DateTime _requestDate;
        private int _qty;
        private Nullable<System.DateTime> _entranceDate;
        private string _description;
        private RequestStatus _requestStatus;
        private byte[] _timeStamp;

        public int RequestId
        {
            get { return _requestId; }
            set
            {
                _requestId = value;
                OnPropertyChanged();
            }
        }

        public string ResolutionPartNo
        {
            get { return _resolutionPartNo; }
            set
            {
                _resolutionPartNo = value;
                OnPropertyChanged();
            }
        }

        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                OnPropertyChanged();
            }
        }

        public string PartNoOriginal
        {
            get { return _partNoOriginal; }
            set
            {
                _partNoOriginal = value;
                OnPropertyChanged();
            }
        }

        public DateTime RequestDate
        {
            get { return _requestDate; }
            set
            {
                _requestDate = value;
                OnPropertyChanged();
            }
        }

        public int Qty
        {
            get { return _qty; }
            set
            {
                _qty = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EntranceDate
        {
            get { return _entranceDate; }
            set
            {
                _entranceDate = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public RequestStatus RequestStatus
        {
            get { return _requestStatus; }
            set
            {
                _requestStatus = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
