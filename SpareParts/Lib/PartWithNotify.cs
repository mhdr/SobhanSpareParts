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
    public class PartWithNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int PartId
        {
            get { return _partId; }
            set
            {
                _partId = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public string TagName
        {
            get { return _tagName; }
            set
            {
                _tagName = value;
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

        public string PartName
        {
            get { return _partName; }
            set
            {
                _partName = value;
                OnPropertyChanged();
            }
        }

        public string PartNoOrignal
        {
            get { return _partNoOrignal; }
            set
            {
                _partNoOrignal = value;
                OnPropertyChanged();
            }
        }

        public int? BrandId
        {
            get { return _brandId; }
            set
            {
                _brandId = value;
                OnPropertyChanged();
            }
        }

        public int? MachineId
        {
            get { return _machineId; }
            set
            {
                _machineId = value;
                OnPropertyChanged();
            }
        }

        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
                OnPropertyChanged();
            }
        }

        public string BrandName
        {
            get { return _brandName; }
            set
            {
                _brandName = value;
                OnPropertyChanged();
            }
        }

        public string MachineName
        {
            get { return _machineName; }
            set
            {
                _machineName = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _partId;
        private string _location;
        private string _tagName;
        private string _resolutionPartNo;
        private string _partNo;
        private string _partName;
        private string _partNoOrignal;
        private Nullable<int> _brandId;
        private Nullable<int> _machineId;
        private byte[] _timeStamp;
        private string _brandName;
        private string _machineName;
    }
}
