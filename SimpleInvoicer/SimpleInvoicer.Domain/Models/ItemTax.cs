using System.ComponentModel;

namespace SimpleInvoicer.Domain.Models
{
    public enum ItemTax
    {
        [Description("5")]
        Five = 5,

        [Description("8")]
        Eight = 8,

        [Description("23")]
        TwentyThree = 23
    }
}
