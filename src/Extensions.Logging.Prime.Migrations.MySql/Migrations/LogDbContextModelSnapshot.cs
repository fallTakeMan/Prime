﻿// <auto-generated />
using System;
using Extensions.Logging.Prime.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Extensions.Logging.Prime.Migrations.MySql.Migrations
{
    [DbContext(typeof(LogDbContext))]
    partial class LogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.AppLogEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("主键，雪花id");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasComment("Event Id");

                    b.Property<string>("EventName")
                        .HasColumnType("VARCHAR(200)")
                        .HasComment("Event Name");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasComment("日志级别");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("UTC时间");

                    b.Property<string>("Logger")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasComment("Logger");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("日志内容");

                    b.Property<string>("UserId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("系统登录用户id");

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(100)")
                        .HasComment("系统登录用户名");

                    b.HasKey("Id");

                    b.ToTable("AppLogs");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.EntityAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuditMessage")
                        .HasColumnType("TEXT")
                        .HasComment("审核信息");

                    b.Property<int>("SaveChangesAuditId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int")
                        .HasComment("EntityState(2=Delete;3=Update;4=Insert)");

                    b.HasKey("Id");

                    b.HasIndex("SaveChangesAuditId");

                    b.ToTable("EntityAudit");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.ExceptionLogEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("主键，雪花id");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("请求连接id");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasComment("Event Id");

                    b.Property<string>("EventName")
                        .HasColumnType("VARCHAR(200)")
                        .HasComment("Event Name");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("TEXT")
                        .HasComment("异常消息");

                    b.Property<string>("ExceptionSource")
                        .HasColumnType("TEXT")
                        .HasComment("异常源");

                    b.Property<string>("ExceptionStackTrace")
                        .HasColumnType("TEXT")
                        .HasComment("异常堆栈");

                    b.Property<string>("Host")
                        .HasColumnType("VARCHAR(200)")
                        .HasComment("服务主机");

                    b.Property<string>("IpAddress")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("客户端ip");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasComment("日志级别");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("UTC时间");

                    b.Property<string>("Logger")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasComment("Logger");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("日志内容");

                    b.Property<string>("Method")
                        .HasColumnType("VARCHAR(10)")
                        .HasComment("请求方法");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT")
                        .HasComment("请求路径");

                    b.Property<string>("QueryString")
                        .HasColumnType("TEXT")
                        .HasComment("请求url参数");

                    b.Property<string>("RequestBody")
                        .HasColumnType("TEXT")
                        .HasComment("请求消息体");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("TEXT")
                        .HasComment("请求头");

                    b.Property<string>("TraceId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("追踪id");

                    b.Property<string>("UserId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("系统登录用户id");

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(100)")
                        .HasComment("系统登录用户名");

                    b.HasKey("Id");

                    b.ToTable("ExceptionLogs");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.HttpLogEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("主键，雪花id");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("请求连接id");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TIME")
                        .HasComment("请求响应持续时间");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasComment("服务主机");

                    b.Property<string>("IpAddress")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("客户端ip");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("UTC时间");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)")
                        .HasComment("请求方法");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("请求路径");

                    b.Property<string>("QueryString")
                        .HasColumnType("TEXT")
                        .HasComment("请求url参数");

                    b.Property<string>("RequestBody")
                        .HasColumnType("TEXT")
                        .HasComment("请求消息体");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("TEXT")
                        .HasComment("请求头");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("TEXT")
                        .HasComment("响应消息体");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("TEXT")
                        .HasComment("响应头");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int")
                        .HasComment("响应状态码");

                    b.Property<string>("TraceId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("追踪id");

                    b.Property<string>("UserId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("系统登录用户id");

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(100)")
                        .HasComment("系统登录用户名");

                    b.HasKey("Id");

                    b.ToTable("HttpLogs");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.SaveChangesAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("UTC时间");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("TEXT")
                        .HasComment("错误信息");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("UTC时间");

                    b.Property<bool>("Succeeded")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserId")
                        .HasColumnType("VARCHAR(64)")
                        .HasComment("系统登录用户id");

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(100)")
                        .HasComment("系统登录用户名");

                    b.HasKey("Id");

                    b.ToTable("SaveChangesAudits");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.EntityAudit", b =>
                {
                    b.HasOne("Extensions.Logging.Prime.Database.Entity.SaveChangesAudit", "SaveChangesAudit")
                        .WithMany("Entities")
                        .HasForeignKey("SaveChangesAuditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SaveChangesAudit");
                });

            modelBuilder.Entity("Extensions.Logging.Prime.Database.Entity.SaveChangesAudit", b =>
                {
                    b.Navigation("Entities");
                });
#pragma warning restore 612, 618
        }
    }
}
