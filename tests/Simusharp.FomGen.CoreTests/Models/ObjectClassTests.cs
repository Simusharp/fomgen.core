/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using AutoFixture;
using NUnit.Framework;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Models
{
    [TestFixture]
    public class ObjectClassTests
    {
        [Test]
        public void IsScaffold_Normal()
        {
            var fixture = new Fixture();
            var o1 = fixture.Create<ObjectClass>();
            var o2 = fixture.Build<ObjectClass>()
                .Without(x => x.Semantics)
                .Without(x => x.Sharing)
                .Do(x => x.AddAttribute(fixture.Create<AttributeItem>()))
                .Create();
            var o3 = fixture.Build<ObjectClass>()
                .Without(x => x.Semantics)
                .Without(x => x.Sharing)
                .Create();

            Assert.False(o1.IsScaffold);
            Assert.False(o2.IsScaffold);
            Assert.True(o3.IsScaffold);
        }

        [Test]
        public void IsMatch_Normal_Succeed()
        {
            var fixture = new Fixture();
            var o1 = fixture.Create<ObjectClass>();
            var o2 = fixture.Build<ObjectClass>()
                .Do(x => x.AddAttribute(fixture.Create<AttributeItem>()))
                .Create();

            var o3 = new ObjectClass
            {
                Name = o2.Name,
                Semantics = o2.Semantics,
                Sharing = o2.Sharing
            };

            var o4 = new ObjectClass
            {
                Name = o2.Name,
                Semantics = o2.Semantics,
                Sharing = o2.Sharing
            };

            foreach (var attribute in o2.Attributes)
            {
                o4.AddAttribute(new AttributeItem
                {
                    Name = attribute.Name,
                    Semantics = attribute.Semantics,
                    Sharing = attribute.Sharing,
                    DataType = attribute.DataType,
                    Order = attribute.Order,
                    Ownership = attribute.Ownership,
                    Transportation = attribute.Transportation,
                    UpdateCondition = attribute.UpdateCondition,
                    UpdateType = attribute.UpdateType
                });
            }

            Assert.False(o1.IsMatch(o2));
            Assert.False(o2.IsMatch(o1));
            Assert.False(o2.IsMatch(o3));
            Assert.False(o3.IsMatch(o2));
            Assert.True(o2.IsMatch(o4));
        }
    }
}