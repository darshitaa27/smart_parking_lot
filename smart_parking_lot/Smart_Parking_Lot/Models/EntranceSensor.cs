using System;

namespace SmartParkingSystem.Models
{
    public class EntranceSensor
    {
        public event Action CarDetected;
        public bool IsCarDetected { get; private set; }

        public void DetectCar()
        {
            IsCarDetected = true;
            CarDetected?.Invoke();
        }

        public void ClearDetection()
        {
            IsCarDetected = false;
        }
    }
}
