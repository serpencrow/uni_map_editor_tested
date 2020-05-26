using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    class HexGridChunkTestSuite
    {
        [Test]
        public void updateChunkWithWallsTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            cells[index].Walled = true;
            cells[index].SetOutgoingRiver(direction);

            HexCell cell1 = cells[index].GetNeighbor(direction)
                .GetNeighbor(direction);
            cell1.Walled = true;
            cell1.GetNeighbor(direction.Previous()).Elevation = 1;
            cell1.GetNeighbor(direction.Previous()).Walled = true;


            HexCell cell2 = cells[index].GetNeighbor(direction.Next())
                .GetNeighbor(direction.Next()).GetNeighbor(direction);
            cell2.GetNeighbor(direction.Previous()).Walled = true;
            cell2.GetNeighbor(direction).Walled = true;

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.IsTrue(cells[index].Walled);
            Assert.AreEqual(direction, cells[index].OutgoingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithRiverTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;
            
            cells[index].SetOutgoingRiver(direction);
            cells[index].GetNeighbor(direction.Opposite()).SetOutgoingRiver(direction);

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.AreEqual(direction, cells[index].OutgoingRiver);
            Assert.AreEqual(direction, cells[index].GetNeighbor(direction.Opposite()).OutgoingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithRiversAndRoads1Test()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            // ....................................................
            // SET DIRECT RIVER
            // ....................................................
            // Find cell with next steps: go to NE, go to NE
            HexCell cell = cells[index].GetNeighbor(direction)
                .GetNeighbor(direction);
            // Set river to NW direction
            cell.SetOutgoingRiver(direction.Previous());
            // Get neigbor from SE direction cell
            HexCell neighbor = cell.GetNeighbor(direction.Next2());
            // Set river to NW direction
            neighbor.SetOutgoingRiver(direction.Previous());

            // Set road to previous 2 direction
            cell.AddRoad(direction.Next());

            // Set road to opposite direction
            cell.AddRoad(direction.Next().Opposite());
            // ....................................................

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.IsTrue(cell.HasIncomingRiver);
            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(cell.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithRiversAndRoads2Test()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            // ....................................................
            // SET HOOKED RIVER
            // ....................................................
            // Find cell with next steps: go to NE, go to NE
            HexCell cell = cells[index].GetNeighbor(direction)
                .GetNeighbor(direction);
            // Set river to NW direction
            cell.SetOutgoingRiver(direction.Previous());
            // Get neigbor from W direction cell
            HexCell neighbor = cell.GetNeighbor(direction.Previous2());
            // Set river to W direction
            neighbor.SetOutgoingRiver(direction.Next());

            // Set road to next direction (E)
            cell.AddRoad(direction.Next());
            // ....................................................

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.IsTrue(cell.HasIncomingRiver);
            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(cell.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithRiversAndRoads3Test()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            // ....................................................
            // SET HOOKED RIVER
            // ....................................................
            // Find cell with next steps: go to NE, go to NE
            HexCell cell = cells[index].GetNeighbor(direction)
                .GetNeighbor(direction);
            // Set river to NW direction
            cell.SetOutgoingRiver(direction.Previous());
            // Get neigbor from NE direction cell
            HexCell neighbor = cell.GetNeighbor(direction);
            // Set river to NE direction
            neighbor.SetOutgoingRiver(direction.Opposite());

            // Set road to next2 direction (SE)
            cell.AddRoad(direction.Next2());
            // ....................................................

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.IsTrue(cell.HasIncomingRiver);
            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(cell.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithRiversAndRoads4Test()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            // ....................................................
            // SET HOOKED RIVER
            // ....................................................
            // Find cell with next steps: go to NE, go to NE
            HexCell cell = cells[index].GetNeighbor(direction)
                .GetNeighbor(direction);
            // Set river to NW direction
            cell.SetOutgoingRiver(direction.Previous());
            // Get neigbor from NE direction cell
            HexCell neighbor = cell.GetNeighbor(direction.Next());
            // Set river to W direction
            neighbor.SetOutgoingRiver(direction.Previous2());

            // Set road to NE direction
            cell.AddRoad(direction);
            // ....................................................

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.IsTrue(cell.HasIncomingRiver);
            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(cell.HasRoads);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkWithWaterTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            cells[index].WaterLevel = 1;
            cells[index].SetOutgoingRiver(direction);
            cells[index + 2].SetOutgoingRiver(direction);

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.AreEqual(direction, cells[index].OutgoingRiver);
            Assert.AreEqual(direction, cells[index + 2].OutgoingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkByWaterflowsTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.SE;
            int index = 7;

            // First waterflow
            cells[index].Elevation = 3;
            cells[index].SetOutgoingRiver(direction);
            HexCell neighbor = cells[index].GetNeighbor(direction);
            neighbor.WaterLevel = 1;

            // Second waterflow
            HexCell neighbor2 = cells[index + 2].GetNeighbor(direction.Opposite());
            neighbor2.SetOutgoingRiver(direction);
            cells[index + 2].WaterLevel = 1;
            neighbor2.Elevation = 3;

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            Assert.AreEqual(direction, cells[index].OutgoingRiver);
            Assert.AreEqual(direction, neighbor2.OutgoingRiver);

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkBySlopesTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;
            
            cells[index].Elevation = 1;
            cells[index].GetNeighbor(direction).Elevation = 1;

            HexDirection newDirection = HexDirection.E;
            HexCell cell = cells[index].GetNeighbor(newDirection)
                .GetNeighbor(newDirection).GetNeighbor(direction);

            cell.Elevation = 1;
            cell.GetNeighbor(HexDirection.SE).Elevation = 1;

            Assert.IsNotNull(cell);

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            GameObject.Destroy(obj);
        }

        [Test]
        public void updateChunkByCliffsTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();

            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            int index = 7;

            cells[index].Elevation = 1;
            cells[index].GetNeighbor(direction).Elevation = 2;

            HexDirection newDirection = HexDirection.E;
            HexCell cell = cells[index].GetNeighbor(newDirection)
                .GetNeighbor(newDirection).GetNeighbor(direction);

            cell.Elevation = 2;
            cell.GetNeighbor(HexDirection.SE).Elevation = 1;

            Assert.IsNotNull(cell);

            // Update chunk
            foreach (HexGridChunk chunk in chunks)
                chunk.Triangulate();

            GameObject.Destroy(obj);
        }
    }
}
