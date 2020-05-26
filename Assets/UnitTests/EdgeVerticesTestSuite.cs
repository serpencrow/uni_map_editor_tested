using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EdgeVerticesTestSuite
    {
        Vector3 v1, v2, v3, v4, v5, corner1, corner2;
        List<Vector3> expected = new List<Vector3>();
        List<Vector3> actual = new List<Vector3>();
        float outerStep;

        [SetUp]
        public void setup()
        {
            outerStep = 0.25f;
            corner1 = new Vector3(1, 2, 3);
            corner2 = new Vector3(2, 1, 3);
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, outerStep);
            v3 = Vector3.Lerp(corner1, corner2, 0.5f);
            v4 = Vector3.Lerp(corner1, corner2, 1 - outerStep);
            v5 = corner2;
        }

        [Test]
        public void edgeVerticiesConstructorTest()
        {
            outerStep = 0.25f;
            corner1 = new Vector3(1, 2, 3);
            corner2 = new Vector3(2, 1, 3);
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, outerStep);
            v3 = Vector3.Lerp(corner1, corner2, 0.5f);
            v4 = Vector3.Lerp(corner1, corner2, 1 - outerStep);
            v5 = corner2;

            expected.Add(v1);
            expected.Add(v2);
            expected.Add(v3);
            expected.Add(v4);
            expected.Add(v5);

            EdgeVertices vert = new EdgeVertices(corner1, corner2);
            actual.Add(vert.v1);
            actual.Add(vert.v2);
            actual.Add(vert.v3);
            actual.Add(vert.v4);
            actual.Add(vert.v5);

            CollectionAssert.AreEqual(expected, actual);
        }
        
        [Test]
        public void edgeVerticiesConstructorChangedStepTest()
        {
            outerStep = 0.1f;
            corner1 = new Vector3(1, 2, 3);
            corner2 = new Vector3(2, 1, 3);
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, outerStep);
            v3 = Vector3.Lerp(corner1, corner2, 0.5f);
            v4 = Vector3.Lerp(corner1, corner2, 1 - outerStep);
            v5 = corner2;

            expected.Add(v1);
            expected.Add(v2);
            expected.Add(v3);
            expected.Add(v4);
            expected.Add(v5);

            EdgeVertices vert = new EdgeVertices(corner1, corner2, outerStep);
            actual.Add(vert.v1);
            actual.Add(vert.v2);
            actual.Add(vert.v3);
            actual.Add(vert.v4);
            actual.Add(vert.v5);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void edgeVerticiesTerraceLerpTest()
        {
            int step = 1;
            EdgeVertices a = new EdgeVertices(corner1, corner2);
            EdgeVertices b = new EdgeVertices(corner1, corner2);
            expected.Add(HexMetrics.TerraceLerp(a.v1, b.v1, step));
            expected.Add(HexMetrics.TerraceLerp(a.v2, b.v2, step));
            expected.Add(HexMetrics.TerraceLerp(a.v3, b.v3, step));
            expected.Add(HexMetrics.TerraceLerp(a.v4, b.v4, step));
            expected.Add(HexMetrics.TerraceLerp(a.v5, b.v5, step));

            EdgeVertices vert = EdgeVertices.TerraceLerp(a, b, step);
            actual.Add(v1);
            actual.Add(v2);
            actual.Add(v3);
            actual.Add(v4);
            actual.Add(v5);

            CollectionAssert.AreEqual(expected, actual);
        }
        
        [TearDown]
        public void tearDown()
        {
            expected.Clear();
            actual.Clear();
        }
    }
}
