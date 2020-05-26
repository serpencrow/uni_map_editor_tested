using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using UnityEngine.TestTools;
using UnityEngine;
using System;
using System.Threading;

namespace Tests
{
    class HexCellTestSuite
    {
        // ................. ELEVATION TESTS ...........................
        [Test]
        public void changeElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            int index = 14;

            cells[index].Elevation = 0;
            int old_elevation = cells[index].Elevation;

            cells[index].Elevation = 3;
            int new_elevation = cells[index].Elevation;

            Assert.Greater(new_elevation, old_elevation);

            GameObject.Destroy(obj);
        }

        [Test]
        public void changeElevationWithSameValueTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            int index = 14;

            cells[index].Elevation = 0;
            int old_elevation = cells[index].Elevation;
            cells[index].Elevation = old_elevation;
            int new_elevation = cells[index].Elevation;

            Assert.AreEqual(old_elevation, new_elevation);

            GameObject.Destroy(obj);
        }

        [Test]
        public void changePositionByChangingElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            int index = 14;
            
            int new_elevation = 2;

            Vector3 new_position = cells[index].Position;
            new_position.y  = new_elevation * HexMetrics.elevationStep;
            new_position.y += (HexMetrics.SampleNoise(new_position).y * 2f - 1f)
                                * HexMetrics.elevationPerturbStrength;

            cells[index].Elevation = new_elevation;
            Assert.AreEqual(new_position, cells[index].Position);

            GameObject.Destroy(obj);
        }
        // ..........................................................

        //  ................. RIVERS TESTS ..........................
        // Have special object in cell
        [Test]
        public void triangulationWithWaterTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 14;
            cells[index].SpecialIndex = 1;

            chunks[0].Triangulate();

            Assert.IsTrue(cells[index].IsSpecial);

            GameObject.Destroy(obj);
        }

        [Test]
        public void incomingAndOutgoingRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            Assert.IsTrue(cells[index].HasOutgoingRiver);

            HexCell neighbor = cells[index].GetNeighbor(direction);
            Assert.IsTrue(neighbor.HasIncomingRiver);

            cells[index].RemoveOutgoingRiver();
            chunks[0].Triangulate();
            Assert.IsTrue(!cells[index].HasOutgoingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void setOutgoingAndCheckIncomingRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexDirection currentOutgoingRiverDirection = cells[index].OutgoingRiver;

            HexCell neighbor = cells[index].GetNeighbor(direction);
            HexDirection neighborIncomingRiverDirection = neighbor.IncomingRiver;

            Assert.IsTrue(cells[index].HasOutgoingRiver);
            Assert.AreEqual(direction, currentOutgoingRiverDirection);

            Assert.IsTrue(neighbor.HasIncomingRiver);
            Assert.AreEqual(direction.Opposite(), neighborIncomingRiverDirection);

            GameObject.Destroy(obj);
        }

        [Test]
        public void setOutgingRiverInSameDirectionTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexDirection prevOutgoingRiverDirection = cells[index].OutgoingRiver;
            
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexDirection currOutgoingRiverDirection = cells[index].OutgoingRiver;

            Assert.AreEqual(prevOutgoingRiverDirection, currOutgoingRiverDirection);

            GameObject.Destroy(obj);
        }

        // Check for situation:
        // --> set new outgoing river from current cell's neighbor
        // in which already flows river from current neighbor's cell,
        //
        //    WAS: 
        //         Neighbor            Current cell
        //      Incoming river        Outgoing river
        //
        //    NOW: 
        //         Neighbor            Current cell
        //      Outgoing river        Incoming river
        [Test]
        public void setOutgingRiverInIncomingRiverDirectionTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // Set river
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexCell prevNeighbor = cells[index].GetNeighbor(direction);

            Assert.IsTrue(cells[index].HasOutgoingRiver);
            Assert.IsTrue(prevNeighbor.HasIncomingRiver);

            // Set new river to previous incoming river direction
            prevNeighbor.SetOutgoingRiver(direction.Opposite());
            chunks[0].Triangulate();

            Assert.IsTrue(prevNeighbor.HasOutgoingRiver);
            Assert.IsTrue(cells[index].HasIncomingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void removeOutgoingRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            Assert.IsTrue(cells[index].HasOutgoingRiver);

            cells[index].RemoveOutgoingRiver();
            chunks[0].Triangulate();

            Assert.IsFalse(cells[index].HasOutgoingRiver);

            GameObject.Destroy(obj);
        }

        // Deletion due to change elevation
        // neighbor cell which has incoming river
        [Test]
        public void deleteIncomingRiverByChangingElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // Set river
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();
            HexCell neighbor = cells[index].GetNeighbor(direction);

            // Change elevation from 0 level to 2
            // to make difference between cells
            neighbor.Elevation = 2;

            Assert.IsFalse(cells[index].HasRiverBeginOrEnd);
            Assert.IsFalse(neighbor.HasRiverBeginOrEnd);

            GameObject.Destroy(obj);
        }

        // Deletion river due to changing cell's elevation
        // value which has outgoing river
        [Test]
        public void deleteOutgoingRiverByChangingElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // Set river
            HexCell neighbor = cells[index].GetNeighbor(direction);
            cells[index].Elevation = 2;
            neighbor.Elevation = 2;

            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            // Change elevation from 2 level to 0
            // to make difference between cells
            cells[index].Elevation = 0;

            Assert.IsFalse(cells[index].HasRiverBeginOrEnd);
            Assert.IsFalse(neighbor.HasRiverBeginOrEnd);

            GameObject.Destroy(obj);
        }

        // Deletion river due to changing cell's elevation
        // value which has outgoing river
        [Test]
        public void failedSetOutgoingRiverDueToNeigborElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // Set river
            HexCell neighbor = cells[index].GetNeighbor(direction);
            // current cell elevation = 0
            // neigbor cell elevation will be 2
            // to fail set river
            neighbor.Elevation = 2;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            Assert.IsFalse(cells[index].HasRiverBeginOrEnd);
            Assert.IsFalse(neighbor.HasRiverBeginOrEnd);

            GameObject.Destroy(obj);
        }

        [Test]
        public void neigborCellHasIncomingRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;
            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexCell neighbor = cells[index].GetNeighbor(direction);

            Assert.IsTrue(neighbor.HasRiverBeginOrEnd);
            Assert.IsTrue(neighbor.HasIncomingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void removeIncomingRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexCell neighbor = cells[index].GetNeighbor(direction);
            neighbor.RemoveIncomingRiver();

            Assert.IsFalse(neighbor.HasIncomingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void removeAllRiverFromCellTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].SetOutgoingRiver(direction);
            HexCell neighbor = cells[index].GetNeighbor(direction.Opposite());
            neighbor.SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            cells[index].RemoveRiver();
            chunks[0].Triangulate();

            Assert.IsFalse(neighbor.HasOutgoingRiver);
            Assert.IsFalse(cells[index].HasIncomingRiver);
            Assert.IsFalse(cells[index].HasOutgoingRiver);
            Assert.IsFalse(cells[index].GetNeighbor(direction).HasIncomingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void getRiverDirectionTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].SetOutgoingRiver(direction);
            chunks[0].Triangulate();

            HexDirection rivDirection = cells[index].RiverBeginOrEndDirection;

            Assert.AreEqual(direction, rivDirection);

            GameObject.Destroy(obj);
        }
        // ...........................................................

        //  ................. ROADS TESTS ............................
        [Test]
        public void setRoadTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].AddRoad(direction);
            chunks[0].Triangulate();

            Assert.IsTrue(cells[index].HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void SetRoadWithDirrentCellsElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // Set current cell elevation = 1
            // and road must have dissapeared
            cells[index].Elevation = 1;
            cells[index].AddRoad(direction);
            chunks[0].Triangulate();

            HexCell neighbor = cells[index].GetNeighbor(direction);
            Assert.IsTrue(cells[index].HasRoads);
            Assert.IsTrue(neighbor.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void failedSetRoadDueToCellElevationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            HexCell neighbor = cells[index].GetNeighbor(direction);
            cells[index].AddRoad(direction);
            // Change current cell elevation
            // to unaccessible
            cells[index].Elevation = 2;
            chunks[0].Triangulate();

            Assert.IsFalse(cells[index].HasRoads);
            Assert.IsFalse(neighbor.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void removeRoadTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].AddRoad(direction);
            cells[index].AddRoad(direction + 1);
            cells[index].AddRoad(direction + 2);
            chunks[0].Triangulate();
            Assert.IsTrue(cells[index].HasRoads);

            cells[index].RemoveRoads();
            chunks[0].Triangulate();
            Assert.IsFalse(cells[index].HasRoads);

            GameObject.Destroy(obj);
        }
        // ............................................................

        // ............... WATER TESTS ................................
        [Test]
        public void getWaterLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            HexDirection direction = HexDirection.SW;
            HexCell neighbor = cells[index].GetNeighbor(direction);
            neighbor.WaterLevel = 1;

            chunks[0].Triangulate();

            Assert.AreEqual(0, cells[index].WaterLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void setWaterLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 10;
            int water_level = 1;
            cells[index].WaterLevel = water_level;

            HexDirection direction = HexDirection.E;
            HexCell neighbor = cells[index].GetNeighbor(direction);
            neighbor.WaterLevel = water_level;
            chunks[0].Triangulate();

            Assert.AreEqual(water_level, cells[index].WaterLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void changeWaterLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int new_water_level = 1;
            cells[index].WaterLevel = new_water_level;
            chunks[0].Triangulate();

            Assert.AreEqual(new_water_level, cells[index].WaterLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void setSameWaterLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int water_level = 2;
            cells[index].WaterLevel = water_level;
            cells[index].WaterLevel = water_level;
            chunks[0].Triangulate();

            Assert.AreEqual(water_level, cells[index].WaterLevel);

            GameObject.Destroy(obj);
        }
        // ............................................................

        // ................ SPECIAL OBJECTS TESTS .....................
        [Test]
        public void farmLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int farm_level = 2;

            cells[index].FarmLevel = 2;
            chunks[0].Triangulate();

            Assert.AreEqual(farm_level, cells[index].FarmLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void plantLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int plant_level = 2;

            cells[index].PlantLevel = 2;
            chunks[0].Triangulate();

            Assert.AreEqual(plant_level, cells[index].PlantLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void urbanLevelTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int urban_level = 2;

            cells[index].UrbanLevel = 2;
            chunks[0].Triangulate();

            Assert.AreEqual(urban_level, cells[index].UrbanLevel);

            GameObject.Destroy(obj);
        }

        [Test]
        public void walledTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;

            cells[index].Walled = true;
            chunks[0].Triangulate();

            Assert.IsTrue(cells[index].Walled);

            GameObject.Destroy(obj);
        }
        // ............................................................


        // ........... Terrain Index ..................................
        [Test]
        public void terrainIndexTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            int index = 7;
            int terrain_type_idx = 2;

            cells[index].TerrainTypeIndex = terrain_type_idx;
            chunks[0].Triangulate();

            Assert.AreEqual(terrain_type_idx, cells[index].TerrainTypeIndex);

            GameObject.Destroy(obj);
        }
        // ............................................................
    }
}
