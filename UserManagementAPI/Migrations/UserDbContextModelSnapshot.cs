﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagementAPI.Data;

namespace UserManagementAPI.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UserManagementAPI.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.HasKey("ClientId")
                        .HasName("PK_Domain.Clients");

                    b.HasIndex("ManagerId");

                    b.ToTable("Clients","Domain");
                });

            modelBuilder.Entity("UserManagementAPI.Models.Manager", b =>
                {
                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ManagerId")
                        .HasName("PK_Domain.Managers");

                    b.ToTable("Managers","Domain");
                });

            modelBuilder.Entity("UserManagementAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId")
                        .HasName("PK_Domain.Users");

                    b.ToTable("Users","Domain");
                });

            modelBuilder.Entity("UserManagementAPI.Models.Client", b =>
                {
                    b.HasOne("UserManagementAPI.Models.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("UserManagementAPI.Models.Client", "ClientId")
                        .HasConstraintName("FK_Domain.Clients_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserManagementAPI.Models.Manager", "Manager")
                        .WithMany("Clients")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK_Domain.Clients_Managers")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("UserManagementAPI.Models.Manager", b =>
                {
                    b.HasOne("UserManagementAPI.Models.User", "User")
                        .WithOne("Manager")
                        .HasForeignKey("UserManagementAPI.Models.Manager", "ManagerId")
                        .HasConstraintName("FK_Domain.Managers_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
