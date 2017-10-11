namespace Infra.Wpf.Controls
{
    public enum StringOperator
    {
        [EnumDisplay("برابر")]
        Equals,
        [EnumDisplay("نابرابر")]
        NotEquals,
        [EnumDisplay("شامل")]
        Contains,
        [EnumDisplay("شروع با")]
        StartsWith,
        [EnumDisplay("پایان با")]
        EndsWith
    }
}
