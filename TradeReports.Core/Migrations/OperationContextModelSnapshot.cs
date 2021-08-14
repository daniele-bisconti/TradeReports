﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradeReports.Core.Repository;

namespace TradeReports.Core.Migrations
{
    [DbContext(typeof(OperationContext))]
    partial class OperationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("TradeReports.Core.Models.Capital", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.HasKey("Date");

                    b.ToTable("Capital");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Operation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CapitalAT")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CapitalDT")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CloseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Day")
                        .HasColumnType("TEXT");

                    b.Property<double>("GapMinutes")
                        .HasColumnType("REAL");

                    b.Property<int>("MonthTradeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PL")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PosId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Size")
                        .HasColumnType("REAL");

                    b.Property<int?>("ToolId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TradeNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PosId");

                    b.HasIndex("ToolId");

                    b.ToTable("Operation");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Pos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pos");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Tool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Tool");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Operation", b =>
                {
                    b.HasOne("TradeReports.Core.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("TradeReports.Core.Models.Pos", "Pos")
                        .WithMany()
                        .HasForeignKey("PosId");

                    b.HasOne("TradeReports.Core.Models.Tool", "Tool")
                        .WithMany()
                        .HasForeignKey("ToolId");

                    b.Navigation("Category");

                    b.Navigation("Pos");

                    b.Navigation("Tool");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Tool", b =>
                {
                    b.HasOne("TradeReports.Core.Models.Category", null)
                        .WithMany("Tools")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("TradeReports.Core.Models.Category", b =>
                {
                    b.Navigation("Tools");
                });
#pragma warning restore 612, 618
        }
    }
}
