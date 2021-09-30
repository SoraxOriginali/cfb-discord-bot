using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace CFBDiscordBot;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private DiscordClient _discordClient;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        string discordBotToken = _configuration["DiscordBotToken"];
        _discordClient = new DiscordClient(new DiscordConfiguration()
        {
            Token = discordBotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting discord bot");

        var soraxLtdGuildId = Convert.ToUInt64(_configuration["SoraxLtdGuildId"]);
        _discordClient.UseSlashCommands()
            .RegisterCommands<SlashCommands>(soraxLtdGuildId);

        await _discordClient.ConnectAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discordClient.DisconnectAsync();
        _discordClient.Dispose();
        _logger.LogInformation("Discord bot stopped");
    }
}

public class SlashCommands : ApplicationCommandModule
{
    [SlashCommand("ping", "The default pong response.")]
    public async Task Ping(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            .WithContent("Pong!"));
    }
}