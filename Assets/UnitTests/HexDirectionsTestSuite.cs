using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class HexDirectionTestSuite
    {
        [Test]
        public void HexDirectionOppositeTest()
        {
            Assert.AreEqual(HexDirection.NE, HexDirectionExtensions.Opposite(HexDirection.SW));
            Assert.AreEqual(HexDirection.W, HexDirectionExtensions.Opposite(HexDirection.E));
        }

        [Test]
        public void HexDirectionPreviousTest()
        {
            Assert.AreEqual(HexDirection.NW, HexDirectionExtensions.Previous(HexDirection.NE));
            Assert.AreEqual(HexDirection.SE, HexDirectionExtensions.Previous(HexDirection.SW));
        }

        [Test]
        public void HexDirectionNextTest()
        {
            Assert.AreEqual(HexDirection.NE, HexDirectionExtensions.Next(HexDirection.NW));
            Assert.AreEqual(HexDirection.SW, HexDirectionExtensions.Next(HexDirection.SE));
        }

        [Test]
        public void HexDirectionPrevious2Test()
        {
            Assert.AreEqual(HexDirection.NW, HexDirectionExtensions.Previous2(HexDirection.E));
            Assert.AreEqual(HexDirection.SE, HexDirectionExtensions.Previous2(HexDirection.W));
        }

        [Test]
        public void HexDirectionNext2Test()
        {
            Assert.AreEqual(HexDirection.NE, HexDirectionExtensions.Next2(HexDirection.W));
            Assert.AreEqual(HexDirection.SW, HexDirectionExtensions.Next2(HexDirection.E));
        }
    }
}
