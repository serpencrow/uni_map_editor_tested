using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    class HexMetricsTestSuite
    {
        [Test]
        public void featureTresholdsTest()
        {
            float[] expected1 = new float[] { 0.0f, 0.0f, 0.4f };
            float[] expected2 = new float[] { 0.0f, 0.4f, 0.6f };
            float[] expected3 = new float[] { 0.4f, 0.6f, 0.8f };
            int level = 0;
            float[] actual1 = HexMetrics.GetFeatureThresholds(level);
            level = 1;
            float[] actual2 = HexMetrics.GetFeatureThresholds(level);
            level = 2;
            float[] actual3 = HexMetrics.GetFeatureThresholds(level);
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
        }

        [Test]
        public void firstCornerTest()
        {
            float outRad = HexMetrics.outerRadius;
            
            Vector3 expected = new Vector3(0f, 0f, outRad);

            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetFirstCorner(direction);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void secondCornerTest()
        {
            float innRad = HexMetrics.innerRadius;
            float outRad = HexMetrics.outerRadius;

            Vector3 expected = new Vector3(innRad, 0f, 0.5f * outRad);

            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetSecondCorner(direction);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void solidEdgeMiddleTest()
        {
            Vector3 corner1 = new Vector3(0f, 0f, HexMetrics.outerRadius);
            Vector3 corner2 = new Vector3(HexMetrics.innerRadius, 0f, 0.5f * HexMetrics.outerRadius);
            Vector3 expected = (corner1 + corner2) * (0.5f * HexMetrics.solidFactor);
            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetSolidEdgeMiddle(direction);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void firstWaterCornerTest()
        {
            Vector3 corner = new Vector3(0f, 0f, HexMetrics.outerRadius);
            Vector3 expected = corner * HexMetrics.waterFactor;
            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetFirstWaterCorner(direction);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void secondWaterCornerTest()
        {
            Vector3 corner = new Vector3(HexMetrics.innerRadius, 0f, 0.5f * HexMetrics.outerRadius);
            Vector3 expected = corner * HexMetrics.waterFactor;
            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetSecondWaterCorner(direction);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void getWaterBridgeTest()
        {
            Vector3 corner1 = new Vector3(0f, 0f, HexMetrics.outerRadius);
            Vector3 corner2 = new Vector3(HexMetrics.innerRadius, 0f, 0.5f * HexMetrics.outerRadius);
            Vector3 expected = (corner1 + corner2) * HexMetrics.waterBlendFactor;
            HexDirection direction = HexDirection.NE;
            Vector3 actual = HexMetrics.GetWaterBridge(direction);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEdgeTypeFlatTest()
        {
            Assert.AreEqual(HexEdgeType.Flat, HexMetrics.GetEdgeType(3, 3));
        }

        [Test]
        public void GetEdgeTypeSlopeTest()
        {
            Assert.AreEqual(HexEdgeType.Slope, HexMetrics.GetEdgeType(3, 4));
        }

        [Test]
        public void GetEdgeTypeSlopeTest2()
        {
            Assert.AreEqual(HexEdgeType.Slope, HexMetrics.GetEdgeType(5, 4));
        }

        [Test]
        public void GetEdgeTypeCliffTest()
        {
            Assert.AreEqual(HexEdgeType.Cliff, HexMetrics.GetEdgeType(2, 5));
        }

        [Test]
        public void GetEdgeTypeCliffTest2()
        {
            Assert.AreEqual(HexEdgeType.Cliff, HexMetrics.GetEdgeType(5, 2));
        }

        [Test]
        public void WallThicknessOffsetTest()
        {
            Vector3 testNear = new Vector3(4, 5, 6);
            Vector3 testFar = new Vector3(5, 6, 7);
            Vector3 test = HexMetrics.WallThicknessOffset(testNear, testFar);
            Assert.AreEqual(0.265165031f, test.x);
            Assert.AreEqual(0, test.y);
            Assert.AreEqual(0.265165031f, test.z);
        }

        [Test]
        public void WallLerpTest1()
        {
            Vector3 testNear = new Vector3(4, 5, 6);
            Vector3 testFar = new Vector3(5, 6, 7);
            Vector3 test = HexMetrics.WallLerp(testNear, testFar);
            Assert.AreEqual(4.5f, test.x);
            Assert.AreEqual(4.33333349f, test.y);
            Assert.AreEqual(6.5f, test.z);
        }

        [Test]
        public void WallLerpTest2()
        {
            Vector3 testNear = new Vector3(4, 4, 6);
            Vector3 testFar = new Vector3(5, 3, 7);
            Vector3 test = HexMetrics.WallLerp(testNear, testFar);
            Assert.AreEqual(4.5f, test.x);
            Assert.AreEqual(2.33333349f, test.y);
            Assert.AreEqual(6.5f, test.z);
        }

        [Test]
        public void TerraceLerpColorTest()
        {
            Color a = new Color(1f, 1f, 1f);
            Color b = new Color(2f, 2f, 2f);
            Color test = new Color(1.4f, 1.4f, 1.4f);
            Assert.AreEqual(b, HexMetrics.TerraceLerp(a, b, 5));
            Assert.AreEqual(a, HexMetrics.TerraceLerp(a, b, 0));
            Assert.AreEqual(test, HexMetrics.TerraceLerp(a, b, 2));
        }

        [Test]
        public void TerraceLerpVectorTest()
        {
            Vector3 a = new Vector3(4, 5, 6);
            Vector3 b = new Vector3(5, 6, 7);
            Vector3 test1 = HexMetrics.TerraceLerp(a, b, 1);
            Vector3 test2 = HexMetrics.TerraceLerp(a, b, 3);
            Assert.AreEqual(4.2f, test1.x);
            Assert.AreEqual(5.33333349f, test1.y);
            Assert.AreEqual(6.2f, test1.z);
            Assert.AreEqual(4.6f, test2.x);
            Assert.AreEqual(5.66666651f, test2.y);
            Assert.AreEqual(6.6f, test2.z);
        }
    }
}
