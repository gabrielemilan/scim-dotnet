﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleIdServer.Scim.Persistence.EF;

namespace SimpleIdServer.Scim.SqlServer.Startup.Migrations
{
    [DbContext(typeof(SCIMDbContext))]
    [Migration("20200410110144_AddAttributeMapping")]
    partial class AddAttributeMapping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMAttributeMappingModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SourceAttributeSelector");

                    b.Property<string>("SourceResourceType");

                    b.Property<string>("TargetAttributeId");

                    b.Property<string>("TargetResourceType");

                    b.HasKey("Id");

                    b.ToTable("SCIMAttributeMappingLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ParentId");

                    b.Property<string>("RepresentationId");

                    b.Property<string>("SchemaAttributeId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("RepresentationId");

                    b.HasIndex("SchemaAttributeId");

                    b.ToTable("SCIMRepresentationAttributeLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeValueModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SCIMRepresentationAttributeId");

                    b.Property<bool?>("ValueBoolean");

                    b.Property<DateTime?>("ValueDateTime");

                    b.Property<int?>("ValueInteger");

                    b.Property<string>("ValueReference");

                    b.Property<string>("ValueString");

                    b.HasKey("Id");

                    b.HasIndex("SCIMRepresentationAttributeId");

                    b.ToTable("SCIMRepresentationAttributeValueLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("ExternalId");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("ResourceType");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("SCIMRepresentationLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationSchemaModel", b =>
                {
                    b.Property<string>("SCIMSchemaId");

                    b.Property<string>("SCIMRepresentationId");

                    b.HasKey("SCIMSchemaId", "SCIMRepresentationId");

                    b.HasIndex("SCIMRepresentationId");

                    b.ToTable("SCIMRepresentationSchemaLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaAttributeModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CanonicalValues");

                    b.Property<bool>("CaseExact");

                    b.Property<string>("DefaultValueInt");

                    b.Property<string>("DefaultValueString");

                    b.Property<string>("Description");

                    b.Property<bool>("MultiValued");

                    b.Property<int>("Mutability");

                    b.Property<string>("Name");

                    b.Property<string>("ParentId");

                    b.Property<string>("ReferenceTypes");

                    b.Property<bool>("Required");

                    b.Property<int>("Returned");

                    b.Property<string>("SchemaId");

                    b.Property<int>("Type");

                    b.Property<int>("Uniqueness");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("SchemaId");

                    b.ToTable("SCIMSchemaAttributeModel");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaExtensionModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Required");

                    b.Property<string>("SCIMSchemaModelId");

                    b.Property<string>("Schema");

                    b.HasKey("Id");

                    b.HasIndex("SCIMSchemaModelId");

                    b.ToTable("SCIMSchemaExtensionModel");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsRootSchema");

                    b.Property<string>("Name");

                    b.Property<string>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("SCIMSchemaLst");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeModel", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeModel", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationModel", "Representation")
                        .WithMany("Attributes")
                        .HasForeignKey("RepresentationId");

                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaAttributeModel", "SchemaAttribute")
                        .WithMany("RepresentationAttributes")
                        .HasForeignKey("SchemaAttributeId");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeValueModel", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationAttributeModel", "RepresentationAttribute")
                        .WithMany("Values")
                        .HasForeignKey("SCIMRepresentationAttributeId");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationSchemaModel", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMRepresentationModel", "Representation")
                        .WithMany("Schemas")
                        .HasForeignKey("SCIMRepresentationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaModel", "Schema")
                        .WithMany("Representations")
                        .HasForeignKey("SCIMSchemaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaAttributeModel", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaAttributeModel", "ParentAttribute")
                        .WithMany("SubAttributes")
                        .HasForeignKey("ParentId");

                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaModel", "Schema")
                        .WithMany("Attributes")
                        .HasForeignKey("SchemaId");
                });

            modelBuilder.Entity("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaExtensionModel", b =>
                {
                    b.HasOne("SimpleIdServer.Scim.Persistence.EF.Models.SCIMSchemaModel")
                        .WithMany("SchemaExtensions")
                        .HasForeignKey("SCIMSchemaModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
