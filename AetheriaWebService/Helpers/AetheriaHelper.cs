using AetheriaWebService.Controllers;
using AetheriaWebService.DataAccess;
using AetheriaWebService.Hubs;
using AetheriaWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AetheriaWebService.Models.Cell;

namespace AetheriaWebService.Helpers
{
    public class AetheriaHelper
    {
        private AetheriaDataAccess aetheriaDataAccess;
        private ReplicationHelper _replicationHelper;
        public AetheriaHelper(AetheriaDataAccess dataAccess, ReplicationHelper replicationHelper) {
            aetheriaDataAccess = dataAccess;
            _replicationHelper = replicationHelper;
        }

        public string ProcessPlayerInput(string input, Player player)
        {
            string response = "";

            var command = CommandHelper.MapCommand(input);

            switch(command)
            {
                case CommandHelper.CommandEnum.Login:
                    response = Login(input, player);
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
            }

            return response;
        }


        public string Login(string input, Player player)
        {
            //take the player's aetheria login data and add their current username to the whitelist of users for that player

            return "";
        }

        public string Speak(string input, Player player)
        {
            var words = input.Split(" ").ToList();
            words.Remove("say");
            words.Remove("speak");
            //get all players (and maybe one day npcs) in the current cell and send a message to them telling them that this player spoke and what they said
            var playerStartingPhrase = "You say ";
            var otherStartingPhrase = player.Name + " says ";

            var message = string.Join(" ", words);

            var chatUsers = aetheriaDataAccess.GetRelevantChatUsersForPlayerAction(player);
            if(chatUsers.Count > 0)
            {
                _replicationHelper.ReplicateToClients(otherStartingPhrase + message, chatUsers);
            }
            return playerStartingPhrase + message;
        }

        public string Message(string input, Player player)
        {
            //send a message from this player to the target player if they exist

            return "";
        }

        public string Take(string input, Player player)
        {
            //determine what the player wants to take by comparing the subject of their sentence with items in the current cell, attempt to add that item to their inventory

            return "";
        }

        public string Attack(string input, Player player)
        {
            //determine the object to attack and attempt to deal damage with the player's currently equipped weapon

            return "";
        }

        public string Open(string input, Player player)
        {
            //determine what the player wants to open by searching their current cell and attempt to open it, check if its locked, unlock it if the player has the key

            return "";
        }

        public string Look(string input, Player player)
        {
            //describe the player's current cell
            var cell = aetheriaDataAccess.GetCell(player);

            var chatUsers = aetheriaDataAccess.GetRelevantChatUsersForPlayerAction(player);
            if (chatUsers.Count > 0)
            {
                _replicationHelper.ReplicateToClients(player.Name + " looks around.", chatUsers);
            }

            return cell.Description + " " + cell.EntitiesDescription;
        }

        public string Move(string input, Player player)
        {
            var response = "";
            //get direction, move player and return the player's new location
            var word = input.ToLower().Split(" ")[0];
            var direction = DirectionEnum.None;
            switch(word)
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
            var currentCell = aetheriaDataAccess.GetCell(player);
            var newCell = aetheriaDataAccess.GetCellRelativeToCell(currentCell, direction);
            if(newCell != null)
            {
                response += "You move " + direction.ToString() + ".\n";
                response += newCell.Description + " " + newCell.EntitiesDescription;

                var chatUsers = aetheriaDataAccess.GetRelevantChatUsersForPlayerAction(player);
                if (chatUsers.Count > 0)
                {
                    _replicationHelper.ReplicateToClients(player.Name + " moves " + direction.ToString(), chatUsers);
                }

                aetheriaDataAccess.UpdateEntityCell(player, newCell);             
            }
            else
            {
                response += "You cannot go any further in this direction.";
            }
            return response;
        }

        public string Lock(string input, Player player)
        {
            //attempt to lock an item or door if the player has the key and the target object exists

            return "";
        }

        public string Unlock(string input, Player player)
        {
            //attempt to unlock an item or door if the player has the key and the target object exists

            return "";
        }

        private string Consume(string input, Player player)
        {
            //attempt to consume an item or door if the player has the key and the target object exists

            return "";
        }

        private string GetSubject(string input, Player player)
        {
            //find the subject of the player's sentence

            return input.Split(" ")[1];
        }
    }
}
