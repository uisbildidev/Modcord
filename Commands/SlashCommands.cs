using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Modcord.Commands
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommandGroup("mod", "Commands to moderate the server")]
        public class ModerationCommands
        {
            [SlashCommandGroup("user", "Commands to moderate users")]
            public class UserCommands
            {
                [SlashRequireBotPermissions(Permissions.ModerateMembers)]
                [SlashRequireUserPermissions(Permissions.ModerateMembers)]
                [SlashCommand("timeout", "Timeout user")]
                public async Task TimeoutUser(InteractionContext context,
                    [Option("user", "User to timeout")] DiscordUser user,
                    [Choice("1 minute", 60)]
                    [Choice("3 minutes", 180)]
                    [Choice("5 minutes", 300)]
                    [Choice("10 minutes", 600)]
                    [Choice("30 minutes", 1800)]
                    [Choice("1 hour", 3600)]
                    [Choice("3 hours", 10800)]
                    [Choice("5 hours", 18000)]
                    [Choice("8 hours", 28800)]
                    [Choice("12 hours", 43200)]
                    [Choice("16 hours", 57600)]
                    [Choice("20 hours", 72000)]
                    [Choice("1 day", 86400)]
                    [Choice("3 days", 259200)]
                    [Choice("5 days", 432000)]
                    [Choice("1 week", 604800)]
                    [Option("duration", "Duration of timeout")] double duration,
                    [Option("reason", "Reason to timeout user")] string? reason = null)
                {
                    try
                    {
                        var member = (DiscordMember)user;

                        if (member.CommunicationDisabledUntil == null || member.CommunicationDisabledUntil < DateTimeOffset.UtcNow)
                        {
                            if (context.Member.Hierarchy < member.Hierarchy)
                            {
                                throw new Exception("Hierarchically higher members cannot be timed out.");
                            }

                            await member.TimeoutAsync(until: new DateTimeOffset(
                                DateTime.Now.AddSeconds(duration)),
                                reason: reason);

                            var string_Duration = "unspecified duration";

                            switch (duration)
                            {
                                case 60:
                                    string_Duration = "1 minute";
                                    break;
                                case 180:
                                    string_Duration = "3 minutes";
                                    break;
                                case 300:
                                    string_Duration = "5 minutes";
                                    break;
                                case 600:
                                    string_Duration = "10 minutes";
                                    break;
                                case 1800:
                                    string_Duration = "30 minutes";
                                    break;
                                case 3600:
                                    string_Duration = "1 hour";
                                    break;
                                case 10800:
                                    string_Duration = "3 hour";
                                    break;
                                case 18000:
                                    string_Duration = "5 hour";
                                    break;
                                case 28800:
                                    string_Duration = "8 hour";
                                    break;
                                case 43200:
                                    string_Duration = "12 hour";
                                    break;
                                case 57600:
                                    string_Duration = "16 hour";
                                    break;
                                case 7200:
                                    string_Duration = "20 hour";
                                    break;
                                case 86400:
                                    string_Duration = "1 day";
                                    break;
                                case 259200:
                                    string_Duration = "3 days";
                                    break;
                                case 432000:
                                    string_Duration = "5 days";
                                    break;
                                case 604800:
                                    string_Duration = "1 week";
                                    break;
                                default:
                                    throw new Exception("Choose one of the choices");
                            }

                            var string_Reason = reason == null ? "unspecified reason" : reason;

                            await context.CreateResponseAsync(content: $":white_check_mark: {member.Mention} was timed out for {string_Duration} by {context.Member.Mention} because of {string_Reason}");
                            return;
                        }

                        throw new Exception($"The specified user is already timed out.");
                    }
                    catch (Exception exception)
                    {
                        await context.CreateResponseAsync(
                                embed: new DiscordEmbedBuilder()
                                {
                                    Title = exception.Message,
                                    Color = DiscordColor.Red
                                },
                                ephemeral: true
                                );
                    }
                }

                [SlashRequireBotPermissions(Permissions.ModerateMembers)]
                [SlashRequireUserPermissions(Permissions.ModerateMembers)]
                [SlashCommand("remove_timeout", "Remove the timeout of the user")]
                public async Task RemoveTimeout(InteractionContext context,
                    [Option("user", "User to remove timeout")] DiscordUser user,
                    [Option("reason", "Reason to remove the timeout of the user")] string? reason = null)
                {
                    try
                    {
                        var member = (DiscordMember)user;

                        if (member.CommunicationDisabledUntil != null || member.CommunicationDisabledUntil > DateTimeOffset.UtcNow)
                        {
                            await member.TimeoutAsync(until: new DateTimeOffset(DateTime.Now), reason: reason);

                            var string_Reason = reason == null ? "unspecified reason" : reason;
                            await context.CreateResponseAsync(content: $":white_check_mark: Timeout of {user.Mention} was removed by {context.Member.Mention} because of {string_Reason}");

                            return;
                        }

                        throw new Exception("Specified user is already NOT timed out");
                    }
                    catch (Exception exception)
                    {
                        await context.CreateResponseAsync(
                                embed: new DiscordEmbedBuilder()
                                {
                                    Title = exception.Message,
                                    Color = DiscordColor.Red
                                },
                                ephemeral: true
                                );
                    }
                }

                [SlashRequireBotPermissions(Permissions.KickMembers)]
                [SlashRequireUserPermissions(Permissions.KickMembers)]
                [SlashCommand("kick", "Kick user")]
                public async Task KickUser(InteractionContext context,
                    [Option("user", "User to kick")] DiscordUser user,
                    [Option("reason", "Reason to kick user")] string? reason = null)
                {
                    try
                    {
                        var member = (DiscordMember)user;

                        if (context.Member.Hierarchy < member.Hierarchy)
                        {
                            throw new Exception("Hierarchically higher members cannot be kicked.");
                        }

                        await member.RemoveAsync(reason: reason);

                        var string_Reason = reason == null ? "unspecified reason" : reason;
                        await context.CreateResponseAsync(content: $":white_check_mark: {user.Mention} was kicked by {context.Member.Mention} because of {string_Reason}");
                    }
                    catch (Exception exception)
                    {
                        await context.CreateResponseAsync(
                                embed: new DiscordEmbedBuilder()
                                {
                                    Title = exception.Message,
                                    Color = DiscordColor.Red
                                },
                                ephemeral: true
                                );
                    }
                }

                [SlashRequireBotPermissions(Permissions.BanMembers)]
                [SlashRequireUserPermissions(Permissions.BanMembers)]
                [SlashCommand("ban", "Ban user")]
                public async Task BanUser(InteractionContext context,
                    [Option("user", "User to ban")] DiscordUser user,
                    [Option("reason", "Reason to ban user")] string? reason = null)
                {
                    try
                    {
                        var member = (DiscordMember)user;

                        if (context.Member.Hierarchy < member.Hierarchy)
                        {
                            throw new Exception("Hierarchically higher members cannot be banned.");
                        }

                        await member.BanAsync(reason: reason);

                        var string_Reason = reason == null ? "unspecified reason" : reason;
                        await context.CreateResponseAsync(content: $":white_check_mark: {user.Mention} was banned by {context.Member.Mention} because of {string_Reason}");
                    }
                    catch (Exception exception)
                    {
                        await context.CreateResponseAsync(
                                embed: new DiscordEmbedBuilder()
                                {
                                    Title = exception.Message,
                                    Color = DiscordColor.Red
                                },
                                ephemeral: true
                                );
                    }
                }

                [SlashRequireBotPermissions(Permissions.BanMembers)]
                [SlashRequireUserPermissions(Permissions.BanMembers)]
                [SlashCommand("unban", "Unban user")]
                public async Task UnbanUser(InteractionContext context,
                    [Option("user", "User to unban")] DiscordUser user,
                    [Option("reason", "Reason to unban user")] string? reason = null)
                {
                    try
                    {
                        if (context.Guild.GetMemberAsync(user.Id).Result == (DiscordMember)user)
                        {
                            throw new Exception("A user that is already a member of this guild cannot be unbanned");
                        }

                        await user.UnbanAsync(guild: context.Guild, reason: reason);

                        var string_Reason = reason == null ? "unspecified reason" : reason;
                        await context.CreateResponseAsync(content: $":white_check_mark: {user.Mention} was unbanned by {context.Member.Mention} because of {string_Reason}");
                    }
                    catch (Exception exception)
                    {
                        await context.CreateResponseAsync(
                                embed: new DiscordEmbedBuilder()
                                {
                                    Title = exception.Message,
                                    Color = DiscordColor.Red
                                },
                                ephemeral: true
                                );
                    }
                }
            }
        }
    }
}
