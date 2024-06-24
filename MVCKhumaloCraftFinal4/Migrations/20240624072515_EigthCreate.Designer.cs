﻿// <auto-generated />
using System;
using MVCKhumaloCraftFinal4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVCKhumaloCraftFinal4.Migrations
{
    [DbContext(typeof(MVCKhumaloCraftFinal4Context))]
    [Migration("20240624072515_EigthCreate")]
    partial class EigthCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.Admin", b =>
                {
                    b.Property<int>("adminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("adminID"));

                    b.Property<string>("adminEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("adminPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("adminID");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemID"));

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("userID")
                        .HasColumnType("int");

                    b.HasKey("CartItemID");

                    b.HasIndex("ProductID");

                    b.HasIndex("userID");

                    b.ToTable("CartItem");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.Order", b =>
                {
                    b.Property<int>("orderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderID"));

                    b.Property<string>("deliveryCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("orderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("productID")
                        .HasColumnType("int");

                    b.Property<string>("shippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shippingMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userID")
                        .HasColumnType("int");

                    b.HasKey("orderID");

                    b.HasIndex("productID");

                    b.HasIndex("userID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.Product", b =>
                {
                    b.Property<int>("productID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("productID"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("stockAvailable")
                        .HasColumnType("bit");

                    b.HasKey("productID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.User", b =>
                {
                    b.Property<int>("userID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enable2FA")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwoFactorCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TwoFactorCodeExpiration")
                        .HasColumnType("datetime2");

                    b.HasKey("userID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.CartItem", b =>
                {
                    b.HasOne("MVCKhumaloCraftFinal4.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVCKhumaloCraftFinal4.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MVCKhumaloCraftFinal4.Models.Order", b =>
                {
                    b.HasOne("MVCKhumaloCraftFinal4.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("productID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVCKhumaloCraftFinal4.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
