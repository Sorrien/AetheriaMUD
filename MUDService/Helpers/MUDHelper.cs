using MUDService.DataAccess;
using MUDService.Logic;
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
        private readonly IMUDDataAccess _mudDataAccess;
        private readonly IReplicationLogic _replicationLogic;
        public MUDHelper(IMUDDataAccess dataAccess, IReplicationLogic replicationLogic)
        {
            _mudDataAccess = dataAccess;
            _replicationLogic = replicationLogic;
        }

        public string ProcessPlayerInput(string input, string chatUserId, string chatUsername, string platform)
        {
            var player = _mudDataAccess.GetPlayer(chatUserId, platform);
            string response;

            var command = CommandHelper.MapCommand(input);

            if (player == null && command != CommandHelper.CommandEnum.Login)
            {
                response = "You must first login with your character name. ex: login Bob Ross";
            }
            else
            {
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
                    case CommandHelper.CommandEnum.Help:
                        response = Help(input, player);
                        break;
                    case CommandHelper.CommandEnum.Rename:
                        response = Rename(input, player);
                        break;
                    case CommandHelper.CommandEnum.Mute:
                        response = Mute(input, player);
                        break;
                    case CommandHelper.CommandEnum.Unmute:
                        response = Unmute(input, player);
                        break;
                    case CommandHelper.CommandEnum.Equipment:
                        response = Equipment(input, player);
                        break;
                    case CommandHelper.CommandEnum.LookAt:
                        response = LookAt(input, player);
                        break;
                    default:
                        response = "I don't understand what you want to do.";
                        break;
                }
            }

            return response;
        }

        public string Login(string input, string chatUserId, string chatUsername, string platform, Player player)
        {
            //take the player's MUD login data and add their current username to the whitelist of users for that player

            string response;
            if (player == null)
            {
                var success = true;
                var relevantInput = input.RemoveWordsFromString(0, 1);

                var characterName = relevantInput.GetNameFromInput();
                var loginFailureReason = "";
                if (string.IsNullOrWhiteSpace(characterName))
                {
                    success = false;
                    loginFailureReason = "Please enter a character name.";
                }

                if (success)
                {
                    var newPlayer = _mudDataAccess.CreateNewPlayer(characterName, platform, chatUsername, chatUserId);
                    response = $"Created Player {newPlayer.Name} for user {chatUsername}";
                }
                else
                {
                    response = $"Login Failed: {loginFailureReason}";
                }
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
            var itemNames = new List<string>();
            foreach (var item in player.Inventory.Entities)
            {
                itemNames.Add(item.Name);
            }
            var response = string.Join(", ", itemNames);

            return response;
        }

        public string Take(string input, Player player)
        {
            //determine what the player wants to take by comparing the subject of their sentence with items in the current cell, attempt to add that item to their inventory
            var response = "";
            var itemName = input.RemoveWordsFromString(0, 1);


            var cell = _mudDataAccess.GetCell(player);
            var item = cell.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(itemName));

            if (item != null)
            {
                _mudDataAccess.UpdateEntityInventory(cell.Inventory, player.Inventory, item);
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
            var itemName = input.RemoveWordsFromString(0, 1);

            var cell = _mudDataAccess.GetCell(player);
            var item = player.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(itemName));

            if (item != null)
            {
                _mudDataAccess.UpdateEntityInventory(player.Inventory, cell.Inventory, item);
                var playerStartingPhrase = "You drop ";
                var otherStartingPhrase = $"{player.Name} drops a ";

                response += $"{playerStartingPhrase}{item.Name}";
                Replicate($"{otherStartingPhrase}{item.Name}", player);
            }
            else
            {
                response = $"You do not have a {itemName}.";
            }

            return response;
        }

        public string Attack(string input, Player player)
        {
            //determine the object to attack and attempt to deal damage with the player's currently equipped weapon

            var response = "";
            var entityName = input.RemoveWordsFromString(0, 1).ToLower();

            var cell = _mudDataAccess.GetCell(player);
            var entity = cell.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(entityName));


            if (entity != null)
            {
                if (entity.Type == Entity.EntityType.NPC || entity.Type == Entity.EntityType.Player)
                {
                    //TODO: there should be a way for the person being attacked to see a message specific to them
                    var replicateResponse = "";

                    var random = new Random();
                    var attackRoll = random.Next(1, 20);
                    var defenseRoll = random.Next(1, 20);
                    if (attackRoll > defenseRoll)
                    {
                        //attacker wins
                        response += $"You put the smackdown on {entity.Name}";
                        replicateResponse = $"{player.Name} put the smackdown on {entity.Name}";
                    }
                    else if (defenseRoll > attackRoll)
                    {
                        //defender wins
                        response += $"{entity.Name} blocked your attack, and put the smackdown on you.";
                        replicateResponse = $"{player.Name} tried to attack {entity.Name} but {entity.Name} whooped them instead.";
                    }
                    else
                    {
                        //draw
                        response += $"{entity.Name} blocked your attack.";
                        replicateResponse = $"{player.Name} tried to attack {entity.Name} but was blocked.";
                    }

                    Replicate(replicateResponse, player);
                }
                else
                {
                    response = $"You fruitlessly attack {entityName}, and you look silly doing it.";
                    Replicate($"{player.Name} fruitlessly attacks {entityName}, and they look ridiculous.", player);
                }
            }
            else
            {
                response = "You do not see " + entityName;
            }

            return response;
        }

        public string Open(string input, Player player)
        {
            //determine what the player wants to open by searching their current cell and attempt to open it, check if its locked, unlock it if the player has the key

            return "not implemented";
        }

        public string Look(string input, Player player)
        {
            //describe the player's current cell
            var description = _mudDataAccess.CellDescriptionForPlayer(player);
            Replicate($"{player.Name} looks around.", player);

            return description;
        }

        public bool IsDirection(string input)
        {
            var directions = new List<string> {
                "north",
                "south",
                "west",
                "east",
                "up",
                "down"
            };
            return directions.Contains(input);
        }

        public string Move(string input, Player player)
        {
            var response = "";
            //get direction, move player and return the player's new location
            var words = input.ToLower().Split(" ").ToList();
            string word;
            if (IsDirection(words[0]))
            {
                word = words[0];
            }
            else
            {
                word = input.RemoveWordsFromString(0, 1);
            }
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
            var currentCell = _mudDataAccess.GetCell(player);
            var newCell = _mudDataAccess.GetCellRelativeToCell(currentCell, direction);
            if (newCell != null)
            {
                response += $"You move {direction.ToString()}.\n";

                Replicate($"{player.Name} moves {direction}", player);

                _mudDataAccess.UpdateEntityCell(player, newCell);

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

                response += _mudDataAccess.CellDescriptionForPlayer(player);
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

                var newCell = _mudDataAccess.GetCellInWorld(worldName, x, y, z);

                if (newCell != null)
                {
                    response += $"You teleport to {worldName}.\n";

                    Replicate($"{player.Name} teleports out of sight.", player);

                    _mudDataAccess.UpdateEntityCell(player, newCell);

                    Replicate($"{player.Name} arrives from a portal", player);

                    response += _mudDataAccess.CellDescriptionForPlayer(player);
                }
                else
                {
                    response += "You cannot teleport somewhere that does not exist.";
                }
            }
            catch (Exception)
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

        private void Replicate(string message, Player player)
        {
            _replicationLogic.ReplicatePlayerAction(message, player);
        }

        public string Help(string input, Player player)
        {
            var response = "";

            var commandValues = Enum.GetValues(typeof(CommandHelper.CommandEnum)).Cast<CommandHelper.CommandEnum>().ToList();
            commandValues.Remove(CommandHelper.CommandEnum.Help);
            commandValues.Remove(CommandHelper.CommandEnum.None);

            foreach (var command in commandValues)
            {
                response += $"{command.ToString()}, ";
            }

            return response;
        }

        public string Rename(string input, Player player)
        {
            var relevantInput = input.RemoveWordsFromString(0, 1);
            var newName = relevantInput.GetNameFromInput();
            _mudDataAccess.RenameEntity(newName, player);
            var response = $"Your new name is {newName}";

            return response;
        }

        public string Mute(string input, Player player)
        {
            var response = "You will no longer receive notifcations, please use the unmute command to unmute when you are ready.";

            _mudDataAccess.UpdatePlayerIsMuted(player, true);

            return response;
        }

        public string Unmute(string input, Player player)
        {
            var response = "You will now receive notifications again.";

            _mudDataAccess.UpdatePlayerIsMuted(player, false);

            return response;
        }

        public string Equipment(string input, Player player)
        {
            var response = "";

            if (player.EquippedWeapon == null)
            {
                response += "You are wielding your fists. ";
            }
            else
            {
                response += $"You are wielding a {player.EquippedWeapon.Name}. ";
            }

            if (player.WornEquipment != null && player.WornEquipment.Count > 0)
            {
                response += "You are wearing: ";
                var itemNames = new List<string>();
                foreach (var equipment in player.WornEquipment)
                {
                    itemNames.Add(equipment.Name);
                }
                string.Join(", ", itemNames);
            }
            else
            {
                response += "You definitely feel the breeze.";
            }

            return response;
        }

        public string LookAt(string input, Player player)
        {
            var response = "";
            var relevantInput = input.RemoveWordsFromString(0, 2);
            var name = relevantInput.GetNameFromInput();

            var cell = _mudDataAccess.GetCell(player);
            var item = cell.Inventory.Entities.FirstOrDefault(x => x.Name.ToLower().Contains(name));

            if (item != null)
            {
                var playerPhrase = $"You see a {item.Name}. {item.Description}";
                var otherPhrase = $"{player.Name} looks at {item.Name}";

                response += playerPhrase;
                Replicate(otherPhrase, player);
            }
            else
            {
                response = $"You do not see {name}.";
            }

            return response;
        }

        
    }
}
