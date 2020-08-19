using Discord;

namespace auto_server_side_fuck
{
    public class ErrorEmbed
    {
        public ErrorEmbed(string errorMessage)
        {
            ErrorMessage = errorMessage;
            var embed = new EmbedBuilder();
            embed.Title = "An error has occured using with the bot :(";
            embed.Description = ErrorMessage;
            embed.Footer = new EmbedFooterBuilder();
            embed.Footer.Text = "Constant dumper made by bork and bot made by Vini Dalvino";
            embed.Footer.IconUrl =
                "https://cdn.discordapp.com/attachments/744411529725870126/745474345916432514/a_3d0812ca4b7f2656ef072b829289cdda.gif";
            embed.Color = Color.Red;
            Embed = embed;
        }

        public string ErrorMessage { get; }
        public EmbedBuilder Embed { get; }
    }
}