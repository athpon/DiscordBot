using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace TestDiscord
{
    public class BotConfig
    {
        public const string token = "Your Bot Token Here";
        public const string prefix = "?";
    }

    public class Bot
    {
        public DiscordClient DiscordClient { get; private set; }
        public CommandsNextExtension CommandsNextExtension { get; private set; }
        public async Task RunAsyn()
        {
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = BotConfig.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                Intents = DiscordIntents.AllUnprivileged
            };
            DiscordClient = new DiscordClient(config);

            CommandsNextConfiguration commandConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[]
                {
                    BotConfig.prefix
                },
                EnableMentionPrefix = true,
                EnableDms = false
            };
            CommandsNextExtension = DiscordClient.UseCommandsNext(commandConfig);
            if (CommandsNextExtension != null)
                CommandsNextExtension.RegisterCommands<Commands>();
            await DiscordClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }

    public class Commands : BaseCommandModule
    {
        [Command("getallguildmemberlist")]
        public async Task GetAllGuildMemberList(CommandContext commandContext)
        {
            System.Collections.Generic.IReadOnlyDictionary<ulong, DiscordGuild> guilds = commandContext.Client.Guilds;
            await commandContext.Channel.SendMessageAsync("Guild count " + guilds.Count.ToString());
            foreach (System.Collections.Generic.KeyValuePair<ulong, DiscordGuild> guild in guilds)
            {
                System.Collections.Generic.IReadOnlyCollection<DiscordMember> readOnlyCollections = await guild.Value.GetAllMembersAsync();
                await commandContext.Channel.SendMessageAsync("Guild " + guild.Value.Name + " members count " + readOnlyCollections.Count.ToString());
                string memberList = string.Empty;
                foreach (DiscordMember member in readOnlyCollections)
                {
                    memberList += member.DisplayName + ",";
                }
                await commandContext.Channel.SendMessageAsync(memberList.TrimEnd(','));
            }
        }
        [Command("getguildmemberlist")]
        public async Task GetGuildMemberList(CommandContext commandContext)
        {
            DiscordGuild guild = commandContext.Member.Guild;
            System.Collections.Generic.IReadOnlyCollection<DiscordMember> readOnlyCollections = await guild.GetAllMembersAsync();
            await commandContext.Channel.SendMessageAsync("Guild " + guild.Name + " members count " + readOnlyCollections.Count.ToString());
            string memberList = string.Empty;
            foreach (DiscordMember member in readOnlyCollections)
            {
                memberList += member.DisplayName + ",";
            }
            await commandContext.Channel.SendMessageAsync(memberList.TrimEnd(','));
        }
    }
}
