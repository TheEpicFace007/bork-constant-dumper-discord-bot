using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using quagmire_bot_9000;

// shut the fuck up rider
#pragma warning disable 4014

namespace auto_server_side_fuck
{
    internal class Program
    {
        private static readonly string BotToken =
            Environment.GetEnvironmentVariable("DISCORD_API_KEY", EnvironmentVariableTarget.User);

        private readonly DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info
        });

        public async Task MainAsync()
        {
            _client.Log += ClientOnLog;
            await _client.LoginAsync(TokenType.Bot, BotToken);
            _client.StartAsync();

            _client.MessageReceived += ClientOnMessageReceived;
            await Task.Delay(-1);
        }

        private int dumpedFile = 0;

        private async Task<Task> ClientOnMessageReceived(SocketMessage message)
        {
            if (message.Author == _client.CurrentUser || message.MentionedUsers == null) return Task.CompletedTask;
            foreach (var mention in message.MentionedUsers)
                if (mention.Username == _client.CurrentUser.Username || mention.Username == "Vini Dalvino#1239")
                {
                    var errorMessage =
                        new ErrorEmbed(
                            "Error with getting constants: You did not provided a file to constant dump.");
                    if (message.Attachments.Count == 0)
                    {
                        var cleverAns = CleverBot.AskClever(message.Content);
                        message.Channel.SendMessageAsync(cleverAns.response);
                    }
                    else
                    {
                        var client = new WebClient();
                        var random = new Random();
                        var fileToDump = $"toDump{random.Next(1000)}.lua";
                        client.DownloadFile(message.Attachments.First().Url, fileToDump);
                        var constantDumper = new ConstantDumper(await File.ReadAllTextAsync(fileToDump));
                        await constantDumper.DumpConstantsAsync();
                        await File.WriteAllTextAsync(fileToDump, constantDumper.Constants);
                        // send the file
                        await message.Channel.SendMessageAsync(null, false, new EmbedBuilder
                        {
                            Description = "Dumping the constants, please wait**...**",
                            Color = Color.Blue,
                            Footer = new EmbedFooterBuilder()
                            {
                                IconUrl =
                                    "https://cdn.discordapp.com/attachments/744411529725870126/745474345916432514/a_3d0812ca4b7f2656ef072b829289cdda.gif",
                                Text = "Constant dumper made by bork and bot made by Vini Dalvino"
                            }
                        }.Build());
                        await message.Channel.SendFileAsync(fileToDump, null, false,
                            new EmbedBuilder
                            {
                                Description = "Finished dumping the constants",
                                Color = Color.Green,
                                Footer = new EmbedFooterBuilder
                                {
                                    IconUrl =
                                        "https://cdn.discordapp.com/attachments/744411529725870126/745474345916432514/a_3d0812ca4b7f2656ef072b829289cdda.gif",
                                    Text = "Constant dumper made by bork and bot made by Vini Dalvino"
                                }
                            }.Build(),
                            new RequestOptions
                            {
                                AuditLogReason = $"{message.Author.Username} constants dumped a file."
                            });
                        File.Delete(fileToDump);
                        // manage the award of dumped files
                        if (dumpedFile % 5 == 0 && dumpedFile != 0)
                        {
                            message.Channel.SendMessageAsync(
                                $"Hurray we dumped {dumpedFile} in this session of use of this program!", true,
                                new EmbedBuilder()
                                {
                                    ImageUrl =
                                        "https://www.vhv.rs/dpng/d/406-4065359_transparent-ps4-trophy-png-playstation-platinum-trophy-png.png"
                                }.Build());
                            dumpedFile++;
                        }
                    }
                }

            return Task.CompletedTask;
        }

        private static Task ClientOnLog(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }

        private static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
    }
}