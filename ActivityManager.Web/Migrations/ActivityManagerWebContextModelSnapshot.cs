﻿// <auto-generated />
using System;
using ActivityManager.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ActivityManager.Web.Migrations
{
    [DbContext(typeof(ActivityManagerWebContext))]
    partial class ActivityManagerWebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ActivityManager.Web.Models.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActivityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("Duration")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActivityTypeId");

                    b.ToTable("Activity", (string)null);
                });

            modelBuilder.Entity("ActivityManager.Web.Models.ActivityType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ActivityType", (string)null);
                });

            modelBuilder.Entity("ActivityManager.Web.Models.Activity", b =>
                {
                    b.HasOne("ActivityManager.Web.Models.ActivityType", null)
                        .WithMany("Activities")
                        .HasForeignKey("ActivityTypeId");
                });

            modelBuilder.Entity("ActivityManager.Web.Models.ActivityType", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
