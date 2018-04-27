using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Helpers
{
    public class CommandHelper
    {
        public enum CommandEnum
        {
            None = 0,
            Login = 10,
            Speak = 20,
            Message = 30,
            Take = 40,
            Attack = 50,
            Open = 60,
            Look = 70,
            Move = 80,
            Lock = 90,
            Unlock = 100,
            Consume = 110,
            Drop = 120,
            Inventory = 130,
            Teleport = 140
        }
        public static CommandEnum MapCommand(string input)
        {
            //maybe this should also return the keyword it found?
            var command = CommandEnum.None;

            string actionWord = input.ToLower().Split(" ")[0];

            switch (actionWord)
            {
                case "login":
                    command = CommandEnum.Login;
                    break;
                case "speak":
                case "say":
                    command = CommandEnum.Speak;
                    break;
                case "message":
                    command = CommandEnum.Message;
                    break;
                case "take":
                case "pickup":
                    command = CommandEnum.Take;
                    break;
                case "attack":
                case "hit":
                    command = CommandEnum.Attack;
                    break;
                case "open":
                    command = CommandEnum.Open;
                    break;
                case "look":
                    command = CommandEnum.Look;
                    break;
                case "north":
                case "south":
                case "west":
                case "east":
                case "up":
                case "down":
                case "climb":
                    command = CommandEnum.Move;
                    break;
                case "lock":
                    command = CommandEnum.Lock;
                    break;
                case "unlock":
                    command = CommandEnum.Unlock;
                    break;
                case "consume":
                case "eat":
                case "drink":
                case "quaff":
                    command = CommandEnum.Consume;
                    break;
                case "drop":
                    command = CommandEnum.Drop;
                    break;
                case "inventory":
                    command = CommandEnum.Inventory;
                    break;
            }

            return command;
        }
    }
}
