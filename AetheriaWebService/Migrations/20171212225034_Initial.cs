using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AetheriaWebService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterStats",
                columns: table => new
                {
                    CharacterStatsId = table.Column<Guid>(nullable: false),
                    BaseMaximumActionPoints = table.Column<double>(nullable: false),
                    BaseMaximumHealthPoints = table.Column<double>(nullable: false),
                    CurrentActionPoints = table.Column<double>(nullable: false),
                    CurrentHealthPoints = table.Column<double>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterStats", x => x.CharacterStatsId);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryId);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    WorldId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.WorldId);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    EquippedWeaponEntityId = table.Column<Guid>(nullable: true),
                    InventoryId1 = table.Column<Guid>(nullable: true),
                    StatsCharacterStatsId = table.Column<Guid>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    InventoryId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CharacterEntityId = table.Column<Guid>(nullable: true),
                    ResistAmount = table.Column<double>(nullable: true),
                    ResistType = table.Column<int>(nullable: true),
                    SlotType = table.Column<int>(nullable: true),
                    BaseAPCost = table.Column<double>(nullable: true),
                    BaseDamageValue = table.Column<double>(nullable: true),
                    DamageType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Entities_Entities_EquippedWeaponEntityId",
                        column: x => x.EquippedWeaponEntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entities_Inventories_InventoryId1",
                        column: x => x.InventoryId1,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entities_CharacterStats_StatsCharacterStatsId",
                        column: x => x.StatsCharacterStatsId,
                        principalTable: "CharacterStats",
                        principalColumn: "CharacterStatsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entities_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entities_Entities_CharacterEntityId",
                        column: x => x.CharacterEntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cells",
                columns: table => new
                {
                    CellId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    InventoryId = table.Column<Guid>(nullable: false),
                    WorldId = table.Column<Guid>(nullable: true),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Z = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cells", x => x.CellId);
                    table.ForeignKey(
                        name: "FK_Cells_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cells_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "WorldId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    ChatUserId = table.Column<Guid>(nullable: false),
                    LastMessageDate = table.Column<DateTime>(nullable: false),
                    Platform = table.Column<string>(nullable: true),
                    PlayerEntityId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.ChatUserId);
                    table.ForeignKey(
                        name: "FK_ChatUsers_Entities_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Effects",
                columns: table => new
                {
                    EffectId = table.Column<Guid>(nullable: false),
                    AffectedStat = table.Column<int>(nullable: false),
                    CharacterStatsId = table.Column<Guid>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    EffectName = table.Column<string>(nullable: true),
                    EffectStarted = table.Column<DateTime>(nullable: false),
                    EffectValue = table.Column<double>(nullable: false),
                    ItemEntityId = table.Column<Guid>(nullable: true),
                    ModifyType = table.Column<int>(nullable: false),
                    TimingType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.EffectId);
                    table.ForeignKey(
                        name: "FK_Effects_CharacterStats_CharacterStatsId",
                        column: x => x.CharacterStatsId,
                        principalTable: "CharacterStats",
                        principalColumn: "CharacterStatsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Effects_Entities_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cells_InventoryId",
                table: "Cells",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cells_WorldId",
                table: "Cells",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_PlayerEntityId",
                table: "ChatUsers",
                column: "PlayerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Effects_CharacterStatsId",
                table: "Effects",
                column: "CharacterStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Effects_ItemEntityId",
                table: "Effects",
                column: "ItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_EquippedWeaponEntityId",
                table: "Entities",
                column: "EquippedWeaponEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_InventoryId1",
                table: "Entities",
                column: "InventoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_StatsCharacterStatsId",
                table: "Entities",
                column: "StatsCharacterStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_InventoryId",
                table: "Entities",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_CharacterEntityId",
                table: "Entities",
                column: "CharacterEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cells");

            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.DropTable(
                name: "Effects");

            migrationBuilder.DropTable(
                name: "Worlds");

            migrationBuilder.DropTable(
                name: "Entities");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "CharacterStats");
        }
    }
}
