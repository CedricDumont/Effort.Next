using System;
using System.IO;
using Effort.DataLoaders;
using Effort.DataLoaders.Xml;
using Effort.Next.Test.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using System.Linq;
using Shouldly;

namespace Effort.Next.Test
{
    public class XmlDataLoaderTest
    {
        [Fact]
        public void ShouldCreateDatabaseFromXml_Transient()
        {
            // create the test file
            string fileName = this.GetType().AssemblyDirectory() + "\\input\\test1_in.xml";

            IDataLoader loader = new CachingDataLoader(new XmlDataLoader(fileName));

            using (SampleContext ctx = new SampleContext(DbConnectionFactory.CreateTransient(loader)))
            {
                ctx.Authors.ToList().Count.ShouldBe(3);
                ctx.Posts.ToList().Count.ShouldBe(3);
                ctx.Authors.Where(a => a.Experience > 100).Count().ShouldBe(2);
            }
        }

        [Fact]
        public void ShouldCreateDatabaseFromXml_Persistent()
        {
            // create the test file
            string fileName = this.GetType().AssemblyDirectory() + "\\input\\test1_in.xml";

            IDataLoader loader = new CachingDataLoader(new XmlDataLoader(fileName));

            using (SampleContext ctx = new SampleContext(DbConnectionFactory.CreatePersistent("myConn", loader)))
            {
                //ensure the author does not exist
                ctx.Authors.Where(a => a.FirstName == "FromTest").Count().ShouldBe(0);

                //create the new author
                Author author = new Author();
                author.FirstName = "FromTest";
                ctx.Authors.Add(author);
                ctx.SaveChanges();
            }

            //create a new context with same connection id
            using (SampleContext ctx = new SampleContext(DbConnectionFactory.CreatePersistent("myConn")))
            {
                ctx.Authors.Where(a => a.FirstName == "FromTest").Count().ShouldBe(1);
            }
        }
    }

    /// <summary>
    /// Just a helper class
    /// </summary>
    public static class TypeExtensions
    {
        public static string AssemblyDirectory(this Type typeOfTest)
        {
            string codeBase = typeOfTest.Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
