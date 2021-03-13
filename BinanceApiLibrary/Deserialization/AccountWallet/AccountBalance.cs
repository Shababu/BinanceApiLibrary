namespace BinanceApiLibrary.Deserialization.AccountWallet
{
    public class AccountBalance
    {
        public string Asset { get; set; }

        public string Free { get; set; }

        public override string ToString()
        {
            return Asset + ": " + Free;
        }
    }
}
