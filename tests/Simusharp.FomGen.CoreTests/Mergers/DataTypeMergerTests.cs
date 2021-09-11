/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using AutoFixture;
using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class DataTypeMergerTests
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var fixture = new Fixture();

            var b1 = fixture.Create<BasicData>();
            var b2 = fixture.Create<BasicData>();
            var b3 = fixture.Create<BasicData>();

            var sim1 = fixture.Create<SimpleData>();
            var sim2 = fixture.Create<SimpleData>();
            var sim3 = fixture.Create<SimpleData>();

            var ar1 = fixture.Create<ArrayData>();
            var ar2 = fixture.Create<ArrayData>();
            var ar3 = fixture.Create<ArrayData>();

            var e1 = fixture.Create<EnumeratedData>();
            var e2 = fixture.Create<EnumeratedData>();
            var e3 = fixture.Create<EnumeratedData>();

            var f1 = fixture.Create<FixedRecordData>();
            var f2 = fixture.Create<FixedRecordData>();
            var f3 = fixture.Create<FixedRecordData>();

            var v1 = fixture.Create<VariantRecordData>();
            var v2 = fixture.Create<VariantRecordData>();
            var v3 = fixture.Create<VariantRecordData>();

            for (var i = 0; i < 3; i++)
            {
                e1.AddEnumerator(fixture.Create<EnumeratedItem>());
                e2.AddEnumerator(fixture.Create<EnumeratedItem>());
                e3.AddEnumerator(fixture.Create<EnumeratedItem>());

                f1.AddField(fixture.Create<FixedRecordField>());
                f2.AddField(fixture.Create<FixedRecordField>());
                f3.AddField(fixture.Create<FixedRecordField>());

                v1.AddAlternative(fixture.Create<AlternativeItem>());
                v2.AddAlternative(fixture.Create<AlternativeItem>());
                v3.AddAlternative(fixture.Create<AlternativeItem>());
            }

            var s1 = new DataTypeSection();
            s1.AddBasicData(b1);
            s1.AddBasicData(b2);
            s1.AddSimpleData(sim1);
            s1.AddSimpleData(sim2);
            s1.AddArrayData(ar1);
            s1.AddArrayData(ar2);
            s1.AddEnumeratedData(e1);
            s1.AddEnumeratedData(e2);
            s1.AddFixedRecordData(f1);
            s1.AddFixedRecordData(f2);
            s1.AddVariantRecordData(v1);
            s1.AddVariantRecordData(v3);

            var s2 = new DataTypeSection();
            s2.AddBasicData(b1);
            s2.AddBasicData(b3);
            s2.AddSimpleData(sim2);
            s2.AddSimpleData(sim3);
            s2.AddArrayData(ar1);
            s2.AddArrayData(ar3);
            s2.AddEnumeratedData(e1);
            s2.AddEnumeratedData(e3);
            s2.AddFixedRecordData(f2);
            s2.AddFixedRecordData(f3);
            s2.AddVariantRecordData(v1);
            s2.AddVariantRecordData(v2);

            var merger = new DataTypeMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, result.BasicData.Count);
                Assert.AreEqual(3, result.SimpleData.Count);
                Assert.AreEqual(3, result.ArrayData.Count);
                Assert.AreEqual(3, result.EnumeratedData.Count);
                Assert.AreEqual(3, result.FixedRecordData.Count);
                Assert.AreEqual(3, result.VariantRecordData.Count);
            });
        }
    }
}