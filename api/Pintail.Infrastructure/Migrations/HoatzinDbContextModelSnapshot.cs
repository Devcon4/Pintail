﻿// <auto-generated />
using System;
using Pintail.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Pintail.Infrastructure.Migrations
{
    [DbContext(typeof(PintailDbContext))]
    partial class PintailDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("Pintail.Domain.Aggregates.CheckAggregate.Check", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateCompleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Checks");
                });

            modelBuilder.Entity("Pintail.Domain.Aggregates.SiteAggregate.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.Property<int>("Ordinal")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Pintail.Domain.Aggregates.CheckAggregate.Check", b =>
                {
                    b.OwnsOne("Pintail.Domain.Aggregates.CheckAggregate.CheckProgress", "Status", b1 =>
                        {
                            b1.Property<Guid>("CheckId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Status");

                            b1.HasKey("CheckId");

                            b1.ToTable("Checks");

                            b1.WithOwner()
                                .HasForeignKey("CheckId");
                        });

                    b.Navigation("Status")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
