using MUDService.DataAccess;
using MUDService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static MUDService.Models.Cell;

namespace MUDService.Helpers
{
    public interface IMUDHelper
    {
        string ProcessPlayerInput(string input, string chatUserId, string chatUsername, string platform);
    }
    public class MUDHelper : IMUDHelper
    {
        private readonly IMUDDataAccess mudDataAccess;
        private readonly IReplicationHelper _replicationHelper;
        public MUDHelper(IMUDDataAccess dataAccess, IReplicationHelper replicationHelper)
        {
            mudDataAccess = dataAccess;
            _replicationHelper = replicationHelper;
        }

        public string ProcessPlayerInput(string input, string chatUserId, string chatUsername, string platform)
        {
            var player = mudDataAccess.GetPlayer(chatUserId, platform);
            string response = "";



            var command = CommandHelper.MapCommand(input);

            switch (command)
            {
                case CommandHelper.CommandEnum.Login:
                    response = Login(input, chatUserId, chatUsername, platform, player);
                    break;
                case CommandHelper.CommandEnum.Speak:
                    response = Speak(input, player);
                    break;
                case CommandHelper.CommandEnum.Message:
                    response = Message(input, player);
                    break;
                case CommandHelper.CommandEnum.Take:
                    response = Take(input, player);
                    break;
                case CommandHelper.CommandEnum.Attack:
                    response = Attack(input, player);
                    break;
                case CommandHelper.CommandEnum.Look:
                    response = Look(input, player);
                    break;
                case CommandHelper.CommandEnum.Move:
                    response = Move(input, player);
                    break;
                case CommandHelper.CommandEnum.Lock:
                    response = Lock(input, player);
                    break;
                case CommandHelper.CommandEnum.Unlock:
                    response = Unlock(input, player);
                    break;
                case CommandHelper.CommandEnum.Consume:
                    response = Consume(input, player);
                    break;
                case CommandHelper.CommandEnum.Drop:
                    response = Drop(input, player);
                    break;
                case CommandHelper.CommandEnum.Inventory:
                    response = Inventory(player);
                    break;
                case CommandHelper.CommandEnum.Teleport:
                    response = Teleport(input, player);
                    break;
                default:
                    response = "I don't understand what you want to do.";
                    break;
            }

            return response;
        }


        public string Login(string input, string chatUserId, string chatUsername, string platform, Player player)
        {
            //take the player's MUD login data and add their current username to the whitelist of users for that player

            var response = "";
            if (player == null)
            {
                var words = input.Split(" ").ToList();
                words.RemoveAt(0);
                var characterName = "";
                if (input.Contains("\""))
                {
                    var startIndex = words.IndexOf(words.Find(x => x[0] == '"'));
                    var endIndex = words.IndexOf(words.Find(x => x[x.Length] == '"'));
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        characterName += words[i].Replace("\"", "");
                    }
                }
                else
                {
                    characterName = words[0];
                }

                var newPlayer = mudDataAccess.CreateNewPlayer(characterName, platform, chatUsername, chatUserId);
                response = "Created Player " + newPlayer.Name + " for user " + chatUsername;
            }
            else
            {
                response = "You are already logged in.";
            }

            return response;
        }

        public string Speak(string input, Player player)
        {
            var words = input.Split(" ").ToList();
            words.RemoveAt(0);
            //get all players (and maybe one day npcs) in the current cell and send a message to them telling them that this player spoke and what they said
            var playerPhrase = "You speak";
            var otherStartingPhrase = player.Name + ": ";

            var message = string.Join(" ", words);

            Replicate(otherStartingPhrase + message, player);
            return playerPhrase;//playerStartingPhrase + message;
        }

        public string Message(string input, Player player)
        {
            //send a message from this player to the target player if they exist

            return "not implemented";
        }

        public string Inventory(Player player)
        {
            var response = "";

            var itemNames = new List<string>();
            foreach (var item in player.Inventory.Entities)
            {
                itemNames.Add(item.Name);
            }
            response = string.Join(", ", itemNames);

            return response;
        }

        public string Take(string input, Player player)
        {
            //determine what the player wants to take by comparing the subject of their sentence with items in the current cell, attempt to add that item to their inventory
            var response = "";
            var words = input.Split(" ").ToList();
            words.RemoveAt(0);
            var itemName = string.Join(" ", words);

            var cell = mudDataAccess.GetCell(player);
            var item = cell.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(itemName));

            if (item != null)
            {
                mudDataAccess.UpdateEntityInventory(cell.Inventory, player.Inventory, item);
                var playerStartingPhrase = "You pickup ";
                var otherStartingPhrase = player.Name + " picks up a ";

                response += playerStartingPhrase + item.Name;
                Replicate(otherStartingPhrase + item.Name, player);
            }
            else
            {
                response = "You do not see " + itemName;
            }

            return response;
        }

        public string Drop(string input, Player player)
        {
            var response = "";
            var words = input.Split(" ").ToList();
            words.RemoveAt(0);
            var itemName = string.Join(" ", words);

            var cell = mudDataAccess.GetCell(player);
            var item = player.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(itemName));

            if (item != null)
            {
                mudDataAccess.UpdateEntityInventory(player.Inventory, cell.Inventory, item);
                var playerStartingPhrase = "You drop ";
                var otherStartingPhrase = player.Name + " drops a ";

                response += playerStartingPhrase + item.Name;
                Replicate(otherStartingPhrase + item.Name, player);
            }
            else
            {
                response = "You do not have a " + itemName + ".";
            }

            return response;
        }

        public string Attack(string input, Player player)
        {
            //determine the object to attack and attempt to deal damage with the player's currently equipped weapon

            return "not implemented";
        }

        public string Open(string input, Player player)
        {
            //determine what the player wants to open by searching their current cell and attempt to open it, check if its locked, unlock it if the player has the key

            return "not implemented";
        }

        public string Look(string input, Player player)
        {
            //describe the player's current cell
            var description = mudDataAccess.CellDescriptionForPlayer(player);

            Replicate(player.Name + " looks around.", player);

            return description;
        }

        public string Move(string input, Player player)
        {
            var response = "";
            //get direction, move player and return the player's new location
            var word = input.ToLower().Split(" ")[0];
            var direction = DirectionEnum.None;
            switch (word)
            {
                case "north":
                    direction = DirectionEnum.North;
                    break;
                case "south":
                    direction = DirectionEnum.South;
                    break;
                case "west":
                    direction = DirectionEnum.West;
                    break;
                case "east":
                    direction = DirectionEnum.East;
                    break;
                case "up":
                    direction = DirectionEnum.Up;
                    break;
                case "down":
                    direction = DirectionEnum.Down;
                    break;
            }
            var currentCell = mudDataAccess.GetCell(player);
            var newCell = mudDataAccess.GetCellRelativeToCell(currentCell, direction);
            if (newCell != null)
            {
                response += $"You move {direction.ToString()}.\n";

                Replicate($"{player.Name} moves {direction}", player);

                mudDataAccess.UpdateEntityCell(player, newCell);

                var oppDirectionString = "";

                switch (direction)
                {
                    case DirectionEnum.North:
                        oppDirectionString = "the South";
                        break;
                    case DirectionEnum.South:
                        oppDirectionString = "the North";
                        break;
                    case DirectionEnum.West:
                        oppDirectionString = "the East";
                        break;
                    case DirectionEnum.East:
                        oppDirectionString = "the West";
                        break;
                    case DirectionEnum.Up:
                        oppDirectionString = "above";
                        break;
                    case DirectionEnum.Down:
                        oppDirectionString = "below";
                        break;
                    default:
                        oppDirectionString = "somewhere";
                        break;
                }

                Replicate($"{player.Name} arrives from {oppDirectionString}", player);

                response += mudDataAccess.CellDescriptionForPlayer(player);
            }
            else
            {
                response += "You cannot go any further in this direction.";
            }
            return response;
        }

        public string Teleport(string input, Player player)
        {
            var response = "";

            try
            {
                var parameters = input.ToLower().Split(" ");

                var worldName = parameters[0];
                int.TryParse(parameters[1], out var x);
                int.TryParse(parameters[2], out var y);
                int.TryParse(parameters[3], out var z);

                var newCell = mudDataAccess.GetCellInWorld(worldName, x, y, z);

                if (newCell != null)
                {
                    response += $"You teleport to {worldName}.\n";

                    Replicate($"{player.Name} teleports out of sight.", player);

                    mudDataAccess.UpdateEntityCell(player, newCell);

                    Replicate($"{player.Name} arrives from a portal", player);

                    response += mudDataAccess.CellDescriptionForPlayer(player);
                }
                else
                {
                    response += "You cannot go any further in this direction.";
                }
            }
            catch (Exception ex)
            {
                response = "The correct format for Teleport is: Teleport world x y z";
            }
            return response;
        }

        public string Lock(string input, Player player)
        {
            //attempt to lock an item or door if the player has the key and the target object exists

            return "not implemented";
        }

        public string Unlock(string input, Player player)
        {
            //attempt to unlock an item or door if the player has the key and the target object exists

            return "not implemented";
        }

        private string Consume(string input, Player player)
        {
            //attempt to consume an item or door if the player has the key and the target object exists

            return "not implemented";
        }

        private string GetSubject(string input, Player player)
        {
            //find the subject of the player's sentence

            return input.Split(" ")[1];
        }
        private void Replicate(string message, Player player)
        {
            var chatUsers = mudDataAccess.GetRelevantChatUsersForPlayerAction(player);
            if (chatUsers.Count > 0)
            {
                _replicationHelper.ReplicateToClients(message, chatUsers);
            }
        }
    }
}
