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
    public class MachineWithNotify : INotifyPropertyChanged
    {
        private int _machineId;
        private string _machineName;
        private byte[] _timeStamp;

        public int MachineId
        {
            get { return _machineId; }
            set
            {
                _machineId = value;
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

        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
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
