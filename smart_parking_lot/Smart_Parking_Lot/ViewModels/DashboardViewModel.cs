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

        private void StartTimer()
        {
            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(2);
            refreshTimer.Tick += (s, e) => RefreshDisplay();
            refreshTimer.Start();
        }

        private async Task HandleCarDetected()
        {
            LogReport.WriteLog("Car detected at entrance.");
            await Gate.OpenGateAsync();
            LogReport.WriteLog("Gate cycle completed.");
            SlotSensor.SimulateCarEntry();
            EntranceSensor.ClearDetection();
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            GateStatus = Gate.Status;
            Lights.AutoControl(DateTime.Now, SlotSensor.OccupiedSlots);
            LightingStatus = Lights.IsLightOn ? "ON" : "OFF";
            SlotInfo = $"Available Slots: {SlotSensor.FreeSlots}/{SlotSensor.TotalSlots}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
