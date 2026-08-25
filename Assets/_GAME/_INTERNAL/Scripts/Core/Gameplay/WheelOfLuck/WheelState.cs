using System;

namespace Core.WheelOfLuck
{
    /// <summary>
    /// Runtime state of a wheel instance.
    /// It contains no Unity/UI logic and is not persisted by itself.
    /// </summary>
    public sealed class WheelState
    {
        public int FreeSpins { get; set; }
        public DateTimeOffset NextAvailableUtc { get; set; }
        public bool IsSpinning { get; set; }
        public WheelReward PendingReward { get; set; }

        public bool IsAvailable => DateTimeOffset.UtcNow >= NextAvailableUtc;
    }
}
