using System.ComponentModel;

namespace SimpleInvoicer.Domain.Models
{
    public enum Units
    {
        [Description("kg")]
        Kilograms,

        [Description("szt.")]
        Piece
    }
}
