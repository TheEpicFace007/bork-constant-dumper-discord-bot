using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

// shut the fuck up rider
#pragma warning disable 4014

namespace auto_server_side_fuck
{
    class Program
    {
        private static readonly string BotToken =
            System.Environment.GetEnvironmentVariable("DISCORD_API_KEY", EnvironmentVariableTarget.User);

        DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig()
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

        async private Task<Task> ClientOnMessageReceived(SocketMessage message)
        {
            if (message.Author != _client.CurrentUser && message.MentionedUsers != null)
            {
                foreach (var mention in message.MentionedUsers)
                {
                    if (mention.Username == _client.CurrentUser.Username)
                    {
                        var errorMessage =
                            new ErrorEmbed(
                                "Error with getting constants: You did not provided a file to constant dump.");
                        if (message.Attachments.Count == 0)
                            await message.Channel.SendMessageAsync(null, false, errorMessage.Embed.Build());
                        else
                        {
                            var client = new WebClient();
                            Random random = new Random();
                            string fileToDump = $"toDump{random.Next(1000)}.lua";
                            client.DownloadFile(message.Attachments.First().Url, fileToDump);
                            var constantDumper = new ConstantDumper(await File.ReadAllTextAsync(fileToDump));
                            await constantDumper.DumpConstantsAsync();
                            await File.WriteAllTextAsync(fileToDump, constantDumper.Constants);
                            // send the file
                            await message.Channel.SendFileAsync(fileToDump, null, false,
                                new EmbedBuilder()
                                {
                                    Description = "Finished dumping the constants",
                                    Color = Color.Green,
                                    Footer = new EmbedFooterBuilder()
                                    {
                                        IconUrl = "https://v3rmillion.net/uploads/avatars/avatar_492604.gif",
                                        Text = "Constant dumper made by bork and bot made by Vini Dalvino"
                                    }
                                }.Build(),
                                new RequestOptions()
                                {
                                    AuditLogReason = $"{message.Author.Username} constants dumped a file."
                                });
                            File.Delete(fileToDump);
                        }
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

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
    }
}