﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Extensions.Logging.Prime.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaveChangesAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "系统登录用户id"),
                    UserName = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "系统登录用户名"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC时间"),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC时间"),
                    Succeeded = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true, comment: "错误信息")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveChangesAudits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false, comment: "EntityState(2=Delete;3=Update;4=Insert)"),
                    AuditMessage = table.Column<string>(type: "TEXT", nullable: true, comment: "审核信息"),
                    SaveChangesAuditId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityAudit_SaveChangesAudits_SaveChangesAuditId",
                        column: x => x.SaveChangesAuditId,
                        principalTable: "SaveChangesAudits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityAudit_SaveChangesAuditId",
                table: "EntityAudit",
                column: "SaveChangesAuditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityAudit");

            migrationBuilder.DropTable(
                name: "SaveChangesAudits");
        }
    }
}
