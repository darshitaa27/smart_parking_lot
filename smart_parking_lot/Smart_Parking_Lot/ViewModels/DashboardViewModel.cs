using SmartParkingSystem.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SmartParkingSystem.ViewModels 
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public EntranceSensor EntranceSensor { get; private set; } = new EntranceSensor();
        public GateBarrierActuator Gate { get; private set; } = new GateBarrierActuator();
        public SlotSensor SlotSensor { get; private set; } = new SlotSensor(10);
        public LightingController Lights { get; private set; } = new LightingController();

        private DispatcherTimer refreshTimer;

        private string _gateStatus;
        public string GateStatus
        {
            get => _gateStatus;
            set { _gateStatus = value; OnPropertyChanged(); }
        }

        private string _lightingStatus;
        public string LightingStatus
        {
            get => _lightingStatus;
            set { _lightingStatus = value; OnPropertyChanged(); }
        }

        private string _slotInfo;
        public string SlotInfo
        {
            get => _slotInfo;
            set { _slotInfo = value; OnPropertyChanged(); }
        }

        public DashboardViewModel()
        {
            EntranceSensor.CarDetected += async () => await HandleCarDetected();
            StartTimer();
        }
    }
}
