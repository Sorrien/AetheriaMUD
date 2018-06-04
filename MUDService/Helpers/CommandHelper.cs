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
            Teleport = 140,
            Help = 150,
            Rename = 160,
            Mute = 170,
            Unmute = 180,
            Equipment = 190,
            LookAt = 200
        }
        public static CommandEnum MapCommand(string input)
        {
            //maybe this should also return the keyword it found?
            var command = CommandEnum.None;


            var words = input.ToLower().Split(" ");
            var actionWord = words[0];

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
                    if (words.Length > 1 && words[1] == "at")
                    {
                        command = CommandEnum.LookAt;
                    }
                    else
                    {
                        command = CommandEnum.Look;
                    }
                    break;
                case "north":
                case "south":
                case "west":
                case "east":
                case "up":
                case "down":
                case "climb":
                case "move":
                case "go":
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
                case "help":
                    command = CommandEnum.Help;
                    break;
                case "rename":
                    command = CommandEnum.Rename;
                    break;
                case "mute":
                    command = CommandEnum.Mute;
                    break;
                case "unmute":
                    command = CommandEnum.Unmute;
                    break;
                case "equipment":
                    command = CommandEnum.Equipment;
                    break;
            }

            return command;
        }
    }
}
