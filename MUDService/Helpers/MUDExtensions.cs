using Microsoft.EntityFrameworkCore;
using MUDService.DataAccess;
using MUDService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUDService.Helpers
{
    public static class MUDExtensions
    {
        public static async Task EnsureSeedData(this MUDContext context)
        {
            var mudDataAccess = new MUDDataAccess(context);
            if (!context.Database.GetPendingMigrations().Any())
            {
                if (!context.Cells.Any())
                {
                    var world = await mudDataAccess.CreateNewWorld("Test");

                    //int x = 0;
                    //int y = 0;
                    string description = "Rolling grassy hills stretch out as far as you can see.";
                    var entities = new List<Entity>();
                    int z = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            await mudDataAccess.CreateNewCell(world, i, j, z, description, entities);
                        }
                    }
                }

                //if (!context.Players.Any())
                //{
                //}
            }
        }

        public static string RemoveWordsFromString(this string input, int startIndex, int count)
        {
            var words = input.Split(" ").ToList();
            words.RemoveRange(startIndex, count);
            var result = string.Join(" ", words);
            return result;
        }

        public static string GetNameFromInput(this string input)
        {
            var words = input.Split(" ").ToList();
            var characterName = "";
            if (input.Contains("\""))
            {
                var startIndex = words.IndexOf(words.Find(x => x[0] == '"'));
                var endIndex = words.IndexOf(words.Find(x => x[x.Length - 1] == '"'));
                for (int i = startIndex; i <= endIndex; i++)
                {
                    characterName += words[i].Replace("\"", "") + " ";
                }
                if (characterName[characterName.Length - 1] == ' ')
                {
                    characterName = characterName.TrimEnd(' ');
                }
            }
            else
            {
                if (words.Count >= 1)
                {
                    characterName = input;
                }
            }

            return characterName;
        }
        const string a = "a";
        const string an = "an";
        public static string GetAOrAnFromInput(this string input)
        {
            var firstCharacter = input[0];
            var firstWord = input.Split(" ").First();

            switch (firstWord)
            {
                case "hour":
                    return an;
                case "university":
                    return a;
                default:
                    switch (firstCharacter)
                    {
                        case 'a':
                        case 'e':
                        case 'i':
                        case 'o':
                        case 'u':
                            return an;
                        default:
                            return a;
                    }
            }
        }

        public static void DescriptionList(this StringBuilder stringBuilder, List<string> items)
        {
            if (items.Count > 2)
            {
                foreach (var item in items.Take(items.Count - 1))
                {
                    stringBuilder.Append(item).Append(", ");
                }
                var lastItem = items.Last();
                stringBuilder.Append("and ").Append(lastItem.GetAOrAnFromInput()).Append(" ").Append(lastItem);
            }
            else if (items.Count == 2)
            {
                var first = items[0];
                var second = items[1];
                stringBuilder.Append(first).Append(", and ").Append(second.GetAOrAnFromInput()).Append(" ").Append(second);
            }
            else if (items.Count == 1)
            {
                var first = items.First();
                stringBuilder.Append(first.GetAOrAnFromInput()).Append(" ").Append(first);
            }
        }
    }
}
