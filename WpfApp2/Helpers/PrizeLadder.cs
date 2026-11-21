namespace WpfApp2
{
    public static class PrizeLadder
    {
        public static readonly int[] Sums =
        {
            100,
            200,
            300,
            500,
            1_000,
            2_000,
            5_000,
            10_000,
            25_000,
            50_000,
            100_000,
            250_000,
            500_000,
            1_000_000,
            5_000_000
        };

        public static string Format(int value) => value.ToString("N0") + " ₽";
    }
}
