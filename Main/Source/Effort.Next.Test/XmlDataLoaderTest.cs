using System;
using System.IO;
using Effort.DataLoaders;
using Effort.DataLoaders.Xml;
using Effort.Next.Test.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using System.Linq;
using Shouldly;
using System.Data.Common;
using Effort.Provider;
using System.Xml.Linq;

namespace Effort.Next.Test
{
    public class XmlDataLoaderTest
    {
        [Fact]
        public void ShouldCreateDatabaseFromXml_Transient()
        {
            // create the test file
            string fileName = this.GetType().AssemblyDirectory() + "\\input\\test_1.xml";

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
            string fileName = this.GetType().AssemblyDirectory() + "\\input\\test_1.xml";

            //XElement xml = XElement.Load(fileName);

            //Stream s = fileName.AsStream();

            //string s2 = s.ReadFromStart();

            //xml = XElement.Load(fileName.AsStream());

            IDataLoader loader = new CachingDataLoader(new XmlDataLoader(fileName));

            using (SampleContext ctx = new SampleContext(DbConnectionFactory.CreatePersistent("ShouldCreateDatabaseFromXml_Persistent", loader)))
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
            using (SampleContext ctx = new SampleContext(DbConnectionFactory.CreatePersistent("ShouldCreateDatabaseFromXml_Persistent")))
            {
                ctx.Authors.Where(a => a.FirstName == "FromTest").Count().ShouldBe(1);
            }
        }

        [Fact]
        public void ShouldLoadMultipleData()
        {
            // create the test file
            string fileName0 = this.GetType().AssemblyDirectory() + "\\input\\test_0.xml";
            string fileName1 = this.GetType().AssemblyDirectory() + "\\input\\test_1.xml";
            string fileName2 = this.GetType().AssemblyDirectory() + "\\input\\test_2.xml";

            IDataLoader loader0 = new XmlDataLoader(fileName0);
            IDataLoader loader1 = new XmlDataLoader(fileName1);
            IDataLoader loader2 = new XmlDataLoader(fileName2);

            using (EffortConnection conn = (EffortConnection)DbConnectionFactory.CreatePersistent("ShouldLoadMultipleData", loader1))
            {
                //loader 1
                using (SampleContext ctx = new SampleContext(conn))
                {
                    ctx.Authors.ToList().Count.ShouldBe(3);
                    Author albert = (from a in ctx.Authors where a.FirstName == "Albert" select a).FirstOrDefault();
                    albert.Posts.Count.ShouldBe(2);

                }
                //loader 2
                conn.LoadData(loader2);
                using (SampleContext ctx = new SampleContext(conn))
                {
                    ctx.Authors.Local.Count.ShouldBe(0);
                    ctx.Authors.ToList().Count.ShouldBe(1);
                    ctx.Posts.ToList().Count.ShouldBe(1);
                }

                //loader 1 again
                conn.LoadData(loader1);
                using (SampleContext ctx = new SampleContext(conn))
                {
                    ctx.Authors.ToList().Count.ShouldBe(3);
                    ctx.Posts.ToList().Count.ShouldBe(3);
                    var albert = (from a in ctx.Authors where a.FirstName == "Albert" select a).FirstOrDefault();
                    albert.Posts.Count.ShouldBe(2);
                }

                //loader 0 again
                conn.LoadData(loader0);
                using (SampleContext ctx = new SampleContext(conn))
                {
                    ctx.Authors.ToList().Count.ShouldBe(0);
                    ctx.Posts.ToList().Count.ShouldBe(0);
                }
            }

          

        }


        [Fact]
        public void ShouldLoadMultipleDataAndClearLocalContext()
        {
            // create the test file
            string fileName1 = this.GetType().AssemblyDirectory() + "\\input\\test_1.xml";
            string fileName2 = this.GetType().AssemblyDirectory() + "\\input\\test_2.xml";

            IDataLoader loader2 = new XmlDataLoader(fileName2);

            //loader 1
            using (SampleContext ctx = DbContextFactory.CreateFromPersistent<SampleContext>("ShouldLoadMultipleDataAndClearLocalContext", fileName1))
            {
                ctx.Authors.ToList().Count.ShouldBe(3);
                ctx.RefreshContext(loader2);
                ctx.Authors.Local.Count.ShouldBe(0);
                ctx.Authors.ToList().Count.ShouldBe(1);
            }
        }

        [Theory(DisplayName = "ShouldLoadMultipleDataWithTheory")]
        [InlineData("test_1.xml", 3)]
        [InlineData("test_2.xml", 1)]
        [InlineData("test_0.xml", 0)]
        public void ShouldLoadMultipleDataWithTheory(string fileName, int authorCount)
        {
            // create the test file
            string filePath = this.GetType().AssemblyDirectory() + "\\input\\"+ fileName;

           // IDataLoader loader = new XmlDataLoader(filePath);

            //loader 1
            using (SampleContext ctx = DbContextFactory.CreateFromPersistent<SampleContext>("ShouldLoadMultipleDataAndClearLocalContext", filePath))
            {
                //ctx.RefreshContext(loader);
                ctx.Authors.ToList().Count.ShouldBe(authorCount);
            }
        }

        [Fact]
        public void ShouldGetSameDataUsingPersistentFactoryExtension()
        {
            // create the test file
            string fileName1 = this.GetType().AssemblyDirectory() + "\\input\\test_1.xml";

            using (SampleContext ctx = DbContextFactory.CreateFromPersistent<SampleContext>("ShouldGetSameDataUsingPersistentFactoryExtension", fileName1))
            {
                ctx.Authors.ToList().Count.ShouldBe(3);
            }

            using (SampleContext ctx = DbContextFactory.CreateFromPersistent<SampleContext>("ShouldGetSameDataUsingPersistentFactoryExtension"))
            {
                ctx.Authors.ToList().Count.ShouldBe(3);
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
