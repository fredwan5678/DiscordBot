namespace Lib.DataHandlers
{
    public interface IQuoteHandler
    {
        void AddQuote(string quote, string serverName);

        string[] GetAllQuotes(string serverName);

        string getQuote(int quoteNumber, string serverName);

        int GetQuoteAmt(string serverName);

        string GetRandQuote(string serverName);

        bool RemoveQuote(int quoteNumber, string serverName);

    }
}