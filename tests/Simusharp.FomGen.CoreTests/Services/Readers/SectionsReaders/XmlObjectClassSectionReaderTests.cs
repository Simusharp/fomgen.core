/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlObjectClassSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlObjectClassSectionReader _reader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _reader = new XmlObjectClassSectionReader();
        }

        [Test]
        public void Read_Normal_Succeed()
        {
            // Arrange
            // Act
            var section = (ObjectClassSection)_reader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(39, section.Root.Count);
                var o1 = section.Root.Find(x => "Dishwasher".Equals(x.Value?.Name));
                Assert.NotNull(o1);
                Assert.AreEqual(0, o1.Value.Attributes.Count);
                var o2 = section.Root.Find(x => "Waiter".Equals(x.Value?.Name));
                Assert.NotNull(o2);
                Assert.AreEqual(3, o2.Value.Attributes.Count);
                var a1 = o2.Value.Attributes.FirstOrDefault(x => "Cheerfulness".Equals(x.Name));
                Assert.AreEqual("WaiterValue", a1?.DataType);
                Assert.AreEqual("DivestAcquire", a1?.Ownership);
                var o3 = section.Root.Find(x => "Waiter1".Equals(x.Value?.Name));
                Assert.Null(o3);
            });
        }
    }
}