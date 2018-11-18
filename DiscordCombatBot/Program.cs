using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordCombatBot
{
    class Program
    {
        private DiscordSocketClient client;
        private static readonly String token = File.ReadAllText(@"../../../data/token.txt");
        private ZeusCombatSystem combat = new ZeusCombatSystem();

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {

            //I make change
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            client.Log += Log;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.MessageReceived += MessageReceived;
            client.ReactionAdded += ReactionAdded;

            client.Ready += () =>
            {
                startCombatSystem();
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (!message.Author.IsBot)
            {
                if (message.Content.Contains("!zeusStatus") == true)
                {
                    await message.Channel.SendMessageAsync("Zeus is ready to manage some Fights!");

                }
                else if (message.Content.Contains("!zeusRegister") == true)
                {
                    await message.Channel.SendMessageAsync(combat.checkUserString(message.Content, message.Author.Id));
                }
                //Hello
                else if (message.Content.Contains("!secretAdminWeapon") == true)
                {
                    //await message.Channel.SendMessageAsync(combat.userBuysItem(message.Author.Id));
                    //await message.DeleteAsync();
                }
                else if (message.Content.Contains("!zeusInventory") == true)
                {
                    await message.Channel.SendMessageAsync(combat.showInventory(message.Author.Id));

                }
                else if (message.Content.Contains("!zeusShowMoney") == true)
                {
                    await message.Channel.SendMessageAsync(combat.showMoney(message.Author.Id));

                }
                else if (message.Content.Contains("!zeusShop") == true)
                {
                    await message.Channel.SendMessageAsync(combat.showShop());

                }
                else if (message.Content.Contains("!zeusBuyItem") == true)
                {
                        string s = message.Content.Split(' ')[1];

                        await message.Channel.SendMessageAsync(combat.userBuysItem(message.Author.Id, s));
                    

                }
                else if (message.Content.Contains("!zeusHero") == true)
                {
                    String[] args = combat.showHero(message.Author.Id);
                    if(args != null)
                    {
                        await message.Channel.SendMessageAsync("Nickname: "+ args[0]);
                        await message.Channel.SendMessageAsync("Profession: " + args[1]);
                        await message.Channel.SendMessageAsync("Gold: " + args[2]);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("You have to be a registered User");
                    }
                }
                else if (message.Content.Contains("!zeusHelp") == true)
                {
                    String help1 = "You can type the following commands:";
                    String help2 = "!zeusHelp - Displays this help dialog :)";
                    String help3 = "!zeusRegister [Nickname] [Profession] - You can choose a Nickname and a Profession (Warrior, Archer, Magician)";
                    String help4 = "!zeusInventory - Displays you current inventory";
                    String help8 = "!zeusShop - Displays the Shop";
                    String help9 = "!zeusBuyItem [item Nr in Shop] - Buys the item with the nr from the shop";
                    String help5 = "!zeusShowMoney - Displays your current Account Balance";
                    String help7 = "!zeusHero - Displays Information about your Character";
                    String help6 = "!zeusStatus - Displays if Zeus is Activated";

                    await message.Channel.SendMessageAsync(help1);
                    await message.Channel.SendMessageAsync(help2);
                    await message.Channel.SendMessageAsync(help3);
                    await message.Channel.SendMessageAsync(help4);
                    await message.Channel.SendMessageAsync(help8);
                    await message.Channel.SendMessageAsync(help9);
                    await message.Channel.SendMessageAsync(help5);
                    await message.Channel.SendMessageAsync(help7);
                    await message.Channel.SendMessageAsync(help6);
                }
            }
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> before, ISocketMessageChannel channel, SocketReaction react)
        {
            if(react.Emote.GetHashCode() == -2086113389)
            {
                await channel.SendMessageAsync("Stop crying you fool! Start protecting yourself");
            }

        }

        private void startCombatSystem()
        {
            
            combat.readUserFromFile();
            combat.saveUserToFile();
        }


    }
}
