using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Lib
{
    public class MachinesObservableCollection : ObservableCollection<MachineWithNotify>
    {
        private SparePartsEntities _entities;

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public bool SuppressEntity { get; set; }

        public MachinesObservableCollection(IEnumerable<MachineWithNotify> machines, SparePartsEntities entities)
            : base(machines)
        {
            this.Entities = entities;
        }

        public MachinesObservableCollection(IEnumerable<Machine> machines, SparePartsEntities entities)
        {
            this.SuppressEntity = true;

            foreach (var machine in machines)
            {
                MachineWithNotify newMachine = new MachineWithNotify();
                newMachine.MachineId = machine.MachineId;
                newMachine.MachineName = machine.MachineName;
                newMachine.TimeStamp = machine.TimeStamp;

                this.Add(newMachine);
            }

            this.Entities = entities;
            this.SuppressEntity = false;
        }

        public bool AddNew(int index, MachineWithNotify item)
        {
            if (!this.SuppressEntity)
            {
                Machine newMachine = new Machine();
                newMachine.MachineName = item.MachineName;
                Entities.Machines.Add(newMachine);

                if (Entities.SaveChanges() > 0)
                {
                    item.MachineId = newMachine.MachineId;
                    item.TimeStamp = newMachine.TimeStamp;

                    base.InsertItem(index, item);
                    return true;
                }

                return false;
            }

            base.InsertItem(index, item);
            return true;
        }

        public bool Delete(int index)
        {
            MachineWithNotify machineWithINotify = this[index];
            Machine machine = Entities.Machines.FirstOrDefault(x => x.MachineId == machineWithINotify.MachineId);
            Entities.Machines.Remove(machine);

            if (Entities.SaveChanges() > 0)
            {
                base.RemoveItem(index);
                return true;
            }

            return false;
        }

        public bool Update(int index, MachineWithNotify item)
        {
            MachineWithNotify machineWithNotify = this[index];
            Machine machine = Entities.Machines.FirstOrDefault(x => x.MachineId == machineWithNotify.MachineId);
            machine.MachineName = item.MachineName;

            if (Entities.SaveChanges() > 0)
            {
                item.TimeStamp = machine.TimeStamp;

                base.SetItem(index, item);
                return true;
            }

            return false;
        }
    }
}
