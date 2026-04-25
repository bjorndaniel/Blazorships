namespace Blazorships.Shared.Helpers
{
    public class ShotOutcome
    {
        public ShotResult Result { get; set; }
        public string SunkShipName { get; set; }
        public bool IsShipSunk => !string.IsNullOrWhiteSpace(SunkShipName);
    }
}