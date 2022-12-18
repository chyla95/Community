﻿// <auto-generated />
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Community.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221217185201_UpdateTableNamesToPrular")]
    partial class UpdateTableNamesToPrular
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Community.Domain.Models.Abstract.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Community.Domain.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CanManageCustomers")
                        .HasColumnType("bit");

                    b.Property<bool>("CanManageEmployees")
                        .HasColumnType("bit");

                    b.Property<bool>("CanManageRoles")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdministrator")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("EmployeeRole");
                });

            modelBuilder.Entity("Community.Domain.Models.Customer", b =>
                {
                    b.HasBaseType("Community.Domain.Models.Abstract.User");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Community.Domain.Models.Employee", b =>
                {
                    b.HasBaseType("Community.Domain.Models.Customer");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.HasOne("Community.Domain.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Community.Domain.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Community.Domain.Models.Customer", b =>
                {
                    b.HasOne("Community.Domain.Models.Abstract.User", null)
                        .WithOne()
                        .HasForeignKey("Community.Domain.Models.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Community.Domain.Models.Employee", b =>
                {
                    b.HasOne("Community.Domain.Models.Customer", null)
                        .WithOne()
                        .HasForeignKey("Community.Domain.Models.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
