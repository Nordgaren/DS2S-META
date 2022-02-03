using System.ComponentModel.DataAnnotations;

namespace LiveSplit.DarkSouls2
{
    public enum TimingType
    {
        [Display(Name = "immediate")]
        Immediate,

        [Display(Name = "on warp")]
        OnWarp,

        [Display(Name = "on save/quit")]
        OnSaveQuit,
    }
}
