﻿// <auto-generated />
using System;
using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankTransactionWeb.DAL.Migrations
{
    [DbContext(typeof(BankTransactionContext))]
    [Migration("20200330201721_SeedDB")]
    partial class SeedDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)")
                        .HasMaxLength(16);

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 23m,
                            Number = "9235286739025463",
                            PersonId = 1
                        },
                        new
                        {
                            Id = 2,
                            Balance = 2000m,
                            Number = "3289456729015682",
                            PersonId = 2
                        },
                        new
                        {
                            Id = 3,
                            Balance = 300m,
                            Number = "3782569106739028",
                            PersonId = 2
                        },
                        new
                        {
                            Id = 4,
                            Balance = 0m,
                            Number = "2345678567891253",
                            PersonId = 3
                        });
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfCreation = new DateTime(2001, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "AbstractBank"
                        },
                        new
                        {
                            Id = 2,
                            DateOfCreation = new DateTime(1997, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Aval"
                        });
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(35)")
                        .HasMaxLength(35);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DataOfBirth = new DateTime(1990, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Володимирович",
                            Name = "Андрій",
                            Surname = "Коваленко"
                        },
                        new
                        {
                            Id = 2,
                            DataOfBirth = new DateTime(1930, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Volodymirovich",
                            Name = "Vasil",
                            Surname = "Kondratyuk"
                        },
                        new
                        {
                            Id = 3,
                            DataOfBirth = new DateTime(1987, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Olegivna",
                            Name = "Masha",
                            Surname = "Koshova"
                        });
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Shareholder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PersonId");

                    b.ToTable("Shareholders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CompanyId = 1,
                            PersonId = 1
                        },
                        new
                        {
                            Id = 2,
                            CompanyId = 1,
                            PersonId = 2
                        },
                        new
                        {
                            Id = 3,
                            CompanyId = 2,
                            PersonId = 3
                        });
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountDestinationId")
                        .HasColumnType("int");

                    b.Property<int?>("AccountSourceId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateOftransfering")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountDestinationId");

                    b.HasIndex("AccountSourceId");

                    b.ToTable("Transactions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 300m,
                            DateOftransfering = new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000000"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 70m,
                            DateOftransfering = new DateTime(2006, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000001"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 70m,
                            DateOftransfering = new DateTime(2009, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000002"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 34m,
                            DateOftransfering = new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000003"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 900m,
                            DateOftransfering = new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000004"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 800m,
                            DateOftransfering = new DateTime(2004, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Number = "00000005"
                        });
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Account", b =>
                {
                    b.HasOne("BankTransactionWeb.DAL.Entities.Person", "Person")
                        .WithMany("Accounts")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Shareholder", b =>
                {
                    b.HasOne("BankTransactionWeb.DAL.Entities.Company", "Company")
                        .WithMany("Shareholders")
                        .HasForeignKey("CompanyId");

                    b.HasOne("BankTransactionWeb.DAL.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("BankTransactionWeb.DAL.Entities.Transaction", b =>
                {
                    b.HasOne("BankTransactionWeb.DAL.Entities.Account", "AccountDestination")
                        .WithMany("TransactionsForDestination")
                        .HasForeignKey("AccountDestinationId");

                    b.HasOne("BankTransactionWeb.DAL.Entities.Account", "AccountSource")
                        .WithMany("TransactionsForSource")
                        .HasForeignKey("AccountSourceId");
                });
#pragma warning restore 612, 618
        }
    }
}
