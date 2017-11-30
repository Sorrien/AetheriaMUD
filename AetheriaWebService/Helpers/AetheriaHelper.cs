using AetheriaWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Helpers
{
    public class AetheriaHelper
    {
        enum DirectionEnum
        {
            North = 10,
            South = 20,
            West = 30,
            East = 40,
            Up = 50,
            Down = 60
        }

        public Player GetPlayer(string chatUsername)
        {

            return new Player();
        }

        public string ProcessPlayerInput(string input, Player player)
        {
            string response = "";

            var command = CommandHelper.MapCommand(input);

            switch(command)
            {
                case CommandHelper.CommandEnum.Login:
                    response = Login(input);
                    break;
                case CommandHelper.CommandEnum.Speak:
                    response = Speak(input);
                    break;
                case CommandHelper.CommandEnum.Message:
                    response = Message(input);
                    break;
                case CommandHelper.CommandEnum.Take:
                    response = Take(input);
                    break;
                case CommandHelper.CommandEnum.Attack:
                    response = Attack(input);
                    break;
                case CommandHelper.CommandEnum.Look:
                    response = Look(input);
                    break;
                case CommandHelper.CommandEnum.Move:
                    response = Move(input);
                    break;
                case CommandHelper.CommandEnum.Lock:
                    response = Lock(input);
                    break;
                case CommandHelper.CommandEnum.Unlock:
                    response = Unlock(input);
                    break;
                case CommandHelper.CommandEnum.Consume:
                    response = Consume(input);
                    break;
            }

            return response;
        }


        public string Login(string input)
        {
            //take the player's aetheria login data and add their current username to the whitelist of users for that player

            return "";
        }

        public string Speak(string input)
        {
            //get all players (and maybe one day npcs) in the current cell and send a message to them telling them that this player spoke and what they said

            return "";
        }

        public string Message(string input)
        {
            //send a message from this player to the target player if they exist

            return "";
        }

        public string Take(string input)
        {
            //determine what the player wants to take by comparing the subject of their sentence with items in the current cell, attempt to add that item to their inventory

            return "";
        }

        public string Attack(string input)
        {
            //determine the object to attack and attempt to deal damage with the player's currently equipped weapon

            return "";
        }

        public string Open(string input)
        {
            //determine what the player wants to open by searching their current cell and attempt to open it, check if its locked, unlock it if the player has the key

            return "";
        }

        public string Look(string input)
        {
            //describe the player's current cell

            return "";
        }

        public string Move(string input)
        {
            //get direction, move player and return the player's new location
            return "";
        }

        public string Lock(string input)
        {
            //attempt to lock an item or door if the player has the key and the target object exists

            return "";
        }

        public string Unlock(string input)
        {
            //attempt to unlock an item or door if the player has the key and the target object exists

            return "";
        }

        private string Consume(string input)
        {
            //attempt to consume an item or door if the player has the key and the target object exists

            return "";
        }

        private string GetSubject(string input)
        {
            //find the subject of the player's sentence

            return input.Split(" ")[1];
        }
    }
}
