using Discord;

namespace auto_server_side_fuck
{
    public class ErrorEmbed
    {
        public string ErrorMessage { get; }
        public Discord.EmbedBuilder Embed { get; }

        public ErrorEmbed(string errorMessage)
        {
            ErrorMessage = errorMessage;
            var embed = new EmbedBuilder();
            embed.Title = "An error has occured using with the bot :(";
            embed.Description = ErrorMessage;
            embed.Footer = new EmbedFooterBuilder();
            embed.Footer.Text = "Constant dumper made by bork and bot made by Vini Dalvino";
            embed.Footer.IconUrl = "https://v3rmillion.net/uploads/avatars/avatar_492604.gif";
            embed.Color = Color.Red;
            Embed = embed;
        }
    }
}