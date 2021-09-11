/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core;

namespace Simusharp.FomGen.CoreTests
{
    [TestFixture]
    public class FomPackageTests
    {
        private FomPackage _fomPackage;
        private readonly string _fomPath = AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml";

        [SetUp]
        public void Init()
        {
            _fomPackage = new FomPackageBuilder().Create();
        }

        [Test]
        public void AddRemoveFomModuleTest()
        {
            // Arrange
            // Act
            _fomPackage.AddFomModule("module1");
            _fomPackage.AddFomModule();

            Assert.AreEqual(2, _fomPackage.Count());

            _fomPackage.RemoveModule("module1");
            Assert.AreEqual(1, _fomPackage.Count());

            Assert.Catch<ArgumentException>(() => _fomPackage.RemoveModule((string)null));
            Assert.Catch<ArgumentException>(() => _fomPackage.RemoveModule(string.Empty));
        }

        [Test]
        public void ReadFomModuleTest()
        {
            // Arrange
            // Act
            using (var stream = new FileStream(_fomPath, FileMode.Open))
            {
                _fomPackage.ReadFomModule(stream);
            }

            // Assert
            Assert.AreEqual(1, _fomPackage.Count());
        }

        [Test]
        public void MergeTest()
        {
            // Arrange
            using (var stream = new FileStream(_fomPath, FileMode.Open))
            {
                _fomPackage.ReadFomModule(stream);
            }

            // Act
            var merged = _fomPackage.Merge(true);

            // Assert
            Assert.NotNull(merged.ModelIdentificationSection);
            Assert.NotNull(merged.ObjectClassSection);
            Assert.NotNull(merged.InteractionClassSection);
            Assert.NotNull(merged.DimensionSection);
            Assert.NotNull(merged.DataTypeSection);
            Assert.NotNull(merged.TransportationSection);
            Assert.NotNull(merged.SynchronizationSection);
            Assert.NotNull(merged.TagSection);
            Assert.NotNull(merged.TimeSection);
            Assert.NotNull(merged.UpdateRatesSection);
        }

        [Test]
        public void ValidateTest()
        {
            // Arrange
            using (var stream = new FileStream(_fomPath, FileMode.Open))
            {
                _fomPackage.ReadFomModule(stream);
            }

            // Act
            var result = _fomPackage.Validate();

            // Assert
            Assert.True(result.Values.All(x => x.Count == 0));
        }
    }
}