using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    class HexCoordinatesTestSuite
    {
        [Test]
        public void constructorTest()
        {
            int x = 10;
            int z = 12;
            int y = -x - z;
            HexCoordinates coord = new HexCoordinates(x, z);

            Assert.AreEqual(x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(z, coord.Z);
        }

        [Test]
        public void newCoordinatesFormOffsetCoordinatesTest()
        {
            int x = 10;
            int z = 12;

            int new_x = x - z / 2;
            int y = -new_x - z;

            HexCoordinates coord = HexCoordinates.FromOffsetCoordinates(x, z);

            Assert.AreEqual(new_x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(z, coord.Z);
        }

        [Test]
        public void newCoordinatesFromPositionTest()
        {
            Vector3 position = new Vector3();
            position.x = 2;
            position.y = 1;
            position.z = 3;

            float x = position.x / (HexMetrics.innerRadius * 2f);
            float y = -x;

            float offset = position.z / (HexMetrics.outerRadius * 3f);
            x -= offset;
            y -= offset;

            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            HexCoordinates coord = HexCoordinates.FromPosition(position);
            Assert.AreEqual(iX, coord.X);
            Assert.AreEqual(iY, coord.Y);
            Assert.AreEqual(iZ, coord.Z);
        }

        [Test]
        public void newCoordinatesFromPositionWithRoundTest()
        {
            Vector3 position = new Vector3();
            position.x = 10.16119f;
            position.y = 0.187369f;
            position.z = 5.539244f;

            float x = position.x / (HexMetrics.innerRadius * 2f);
            float y = -x;

            float offset = position.z / (HexMetrics.outerRadius * 3f);
            x -= offset;
            y -= offset;

            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
            }

            HexCoordinates coord = HexCoordinates.FromPosition(position);
            Assert.AreEqual(iX, coord.X);
            Assert.AreEqual(iY, coord.Y);
            Assert.AreEqual(iZ, coord.Z);
        }

        [Test]
        public void coordinatesToStringTest()
        {
            int x = 2;
            int z = 2;
            int y = -x - z;

            HexCoordinates coord = new HexCoordinates(x, z);
            string expected = "(" + x + ", " + y + ", " + z + ")";
            string actual   = coord.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void coordinatesToStringOnSeparateLinesTest()
        {
            int x = 2;
            int z = 2;
            int y = -x - z;

            HexCoordinates coord = new HexCoordinates(x, z);
            string expected = x + "\n" + y + "\n" + z;
            string actual = coord.ToStringOnSeparateLines();

            Assert.AreEqual(expected, actual);
        }
    }
}
