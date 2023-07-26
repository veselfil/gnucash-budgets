﻿// <auto-generated />
using System;
using GnuCashBudget.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GnuCashBudget.Data.EntityFramework.Migrations
{
    [DbContext(typeof(BudgetsContext))]
    partial class BudgetsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("GnuCashBudget.Data.EntityFramework.Entities.BudgetEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int>("BudgetedAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("GnuCashBudget.Data.EntityFramework.Entities.BudgetedAccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountGuid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BudgetedAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}