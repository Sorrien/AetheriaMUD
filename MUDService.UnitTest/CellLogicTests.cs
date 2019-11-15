using Moq;
using MUDService.DataAccess;
using MUDService.Logic;
using MUDService.Models;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MUDService.UnitTest
{
    public class CellLogicTests
    {
        protected Mock<IMUDDataAccess> MockMUDDataAccess;
        protected CellLogic cellLogic;

        [SetUp]
        public void Setup()
        {
            MockMUDDataAccess = new Mock<IMUDDataAccess>();
            cellLogic = new CellLogic(MockMUDDataAccess.Object);
        }

        [Test]
        public void GetCellTest()
        {
            var player = new Player()
            {
                Name = "Boberts",
                Description = "Boberts is a well dressed gentlefolk"
            };
            var setupCell = new Cell()
            {
                CellId = Guid.NewGuid(),
                World = new World() { Name = "Phantasia" },
                Description = "What a world"
            };
            MockMUDDataAccess.Setup(x => x.GetCell(It.Is<Player>(y => y == player))).Returns(setupCell);
            var cell = cellLogic.GetPlayerCell(player);
            Assert.AreEqual(setupCell, cell);
        }

        [TestCaseSource(typeof(CellDescriptionDataClass), "TestCases")]
        public void GetCellDescription(Cell cell, Player player, string expected)
        {
            var cellDescription = cellLogic.CellDescription(cell, player);

            Assert.AreEqual(expected, cellDescription);
        }
    }

    public class CellDescriptionDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                new Cell()
                {
                    Description = "Rolling green hills stretch out before you.",
                    Inventory = new Inventory()
                    {
                        Entities = new List<Entity>()
                    {
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "bannana",
                            Type = Entity.EntityType.Consumable
                        },
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "gold bar",
                            Type = Entity.EntityType.Item
                        },
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "silver bar",
                            Type = Entity.EntityType.Item
                        },
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "dagger",
                            Type = Entity.EntityType.Weapon
                        },
                    }
                    }
                },
                new Player()
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Sorrien",
                    Type = Entity.EntityType.Player
                },
                "Rolling green hills stretch out before you. You can also see the following: bannana, gold bar, silver bar, and a dagger."
                );
                yield return new TestCaseData(
                new Cell()
                {
                    Description = "Rolling green hills stretch out before you.",
                    Inventory = new Inventory()
                    {
                        Entities = new List<Entity>()
                    {
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "bannana",
                            Type = Entity.EntityType.Consumable
                        },
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "gold bar",
                            Type = Entity.EntityType.Item
                        }
                    }
                    }
                },
                new Player()
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Sorrien",
                    Type = Entity.EntityType.Player
                },
                "Rolling green hills stretch out before you. You can also see the following: bannana, and a gold bar."
                );
                yield return new TestCaseData(
                new Cell()
                {
                    Description = "Rolling green hills stretch out before you.",
                    Inventory = new Inventory()
                    {
                        Entities = new List<Entity>()
                    {
                        new Entity()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "bannana",
                            Type = Entity.EntityType.Consumable
                        }
                    }
                    }
                },
                new Player()
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Sorrien",
                    Type = Entity.EntityType.Player
                },
                "Rolling green hills stretch out before you. You can also see a bannana."
                );
            }
        }
    }
}