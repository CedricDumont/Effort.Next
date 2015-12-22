using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using System.Threading.Tasks;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;
using System.Data.Common;

namespace Effort.Next.Test.Data
{
    public class SampleContext : DbContext
    {
        public IDbSet<Author> Authors { get; set; } // AUTHOR
        public IDbSet<Post> Posts { get; set; } // POST

        static SampleContext()
        {
            Database.SetInitializer<SampleContext>(new CreateDatabaseIfNotExists<SampleContext>());
        }

        public SampleContext()
            //: base("Name=SampleContext")
        {
        }

        public SampleContext(string connectionString)
            : base(connectionString)
        {
        }

        public SampleContext(DbConnection existingConn)
            : base(existingConn, true)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AuthorConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AuthorConfiguration(schema));
            modelBuilder.Configurations.Add(new PostConfiguration(schema));
            return modelBuilder;
        }

    }
   
   
    internal class AuthorConfiguration : EntityTypeConfiguration<Author>
    {
        public AuthorConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AUTHOR");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("AUT_ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FirstName).HasColumnName("AUT_FIRSTNAME").IsOptional().HasMaxLength(50);
            Property(x => x.LastName).HasColumnName("AUT_LASTNAME").IsOptional().HasMaxLength(50);
            Property(x => x.Experience).HasColumnName("AUT_EXPERIENCE").IsOptional();
        }
    }

    internal class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".POST");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Post_Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Subject).HasColumnName("Subject").IsOptional().HasMaxLength(50);
            Property(x => x.Body).HasColumnName("Body").IsOptional().HasMaxLength(50);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsOptional();
            Property(x => x.DateModified).HasColumnName("DateModified").IsOptional();
            Property(x => x.AutId).HasColumnName("AUT_ID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Author).WithMany(b => b.Posts).HasForeignKey(c => c.AutId); // FK_POST_AUTHOR
        }
    }

}

