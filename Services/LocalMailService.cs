namespace CityInfo.API.Services
{

    // TODO: Set up an actual mail server as an experiment. For now, just logging to the console.
    public class LocalMailService
    {
        private string _mailTo = "williamlocke.cello@gmail.com";
        private string _mailFrom = "noreply@sanamarostrings.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $"with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
