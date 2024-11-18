﻿// <auto-generated />
using System;
using BullkyBook.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoreTest2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221108093619_AddProductToDB")]
    partial class AddProductToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BullkyBook.Model.BullkyBookModel", b =>
                {
                    b.Property<int>("book_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("book_Id"), 1L, 1);

                    b.Property<string>("book_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("display_Order")
                        .HasColumnType("int");

                    b.Property<DateTime>("oder_date")
                        .HasColumnType("datetime2");

                    b.HasKey("book_Id");

                    b.ToTable("bullkyBookModels");
                });

            modelBuilder.Entity("BullkyBook.Model.CoverTypeModel", b =>
                {
                    b.Property<int>("cover_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("cover_Id"), 1L, 1);

                    b.Property<string>("cover_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("cover_Id");

                    b.ToTable("CoverTypeModels");
                });

            modelBuilder.Entity("BullkyBook.Model.ProductModel", b =>
                {
                    b.Property<int>("pro_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("pro_id"), 1L, 1);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<int>("categoryID")
                        .HasColumnType("int");

                    b.Property<int>("coverTypeID")
                        .HasColumnType("int");

                    b.Property<double>("price100")
                        .HasColumnType("float");

                    b.Property<double>("price50")
                        .HasColumnType("float");

                    b.Property<string>("pro_description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pro_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("pro_id");

                    b.HasIndex("categoryID");

                    b.HasIndex("coverTypeID");

                    b.ToTable("productModels");
                });

            modelBuilder.Entity("BullkyBook.Model.ProductModel", b =>
                {
                    b.HasOne("BullkyBook.Model.BullkyBookModel", "category")
                        .WithMany()
                        .HasForeignKey("categoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BullkyBook.Model.CoverTypeModel", "coverType")
                        .WithMany()
                        .HasForeignKey("coverTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("coverType");
                });
#pragma warning restore 612, 618
        }
    }
}