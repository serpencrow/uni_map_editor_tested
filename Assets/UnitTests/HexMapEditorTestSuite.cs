using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    class HexMapEditorTestSuite
    {
        [UnityTest]
        public IEnumerator chooseAndSetTerrainTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int terrainTypeIndex = 1;
            editor.SetTerrainTypeIndex(terrainTypeIndex);
            editor.HandleTestInput(cell);

            Assert.AreEqual(terrainTypeIndex, editor.activeTerrainTypeIndex);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetElevationTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int elevationLevel = 2;
            int brushSize = 2;
            editor.SetApplyElevation(true);
            editor.SetElevation(elevationLevel);
            editor.SetBrushSize(brushSize);
            editor.HandleTestInput(cell);

            Assert.AreEqual(elevationLevel, editor.activeElevation);
            Assert.AreEqual(brushSize, editor.brushSize);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetWaterTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int waterLevel = 1;
            editor.SetApplyWaterLevel(true);
            editor.SetWaterLevel(waterLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(waterLevel, editor.activeWaterLevel);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetUrbanLevelTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int urbanLevel = 1;
            editor.SetApplyUrbanLevel(true);
            editor.SetUrbanLevel(urbanLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(urbanLevel, editor.activeUrbanLevel);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetFarmLevelTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int farmLevel = 1;
            editor.SetApplyFarmLevel(true);
            editor.SetFarmLevel(farmLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(farmLevel, editor.activeFarmLevel);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetPlantLevelTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int plantLevel = 1;
            editor.SetApplyPlantLevel(true);
            editor.SetPlantLevel(plantLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(plantLevel, editor.activePlantLevel);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetSpecialObjectTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            int specialIndex = 1;
            editor.SetApplySpecialIndex(true);
            editor.SetSpecialIndex(specialIndex);
            editor.HandleTestInput(cell);

            Assert.AreEqual(specialIndex, editor.activeSpecialIndex);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseAndSetWallsTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor.OptionalToggle walled = HexMapEditor.OptionalToggle.Yes;
            editor.SetWalledMode((int)walled);
            editor.HandleTestInput(cell);

            Assert.AreEqual(walled, editor.walledMode);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseRiverTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);
            HexCell neighbor = cell.GetNeighbor(HexDirection.E);

            int elevationLevel = 1;
            editor.SetApplyElevation(true);
            editor.SetElevation(elevationLevel);
            editor.HandleTestInput(cell);

            editor.SetApplyElevation(false);
            editor.SetRiverMode((int)HexMapEditor.OptionalToggle.Yes);
            editor.HandleTestInput(neighbor);

            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(neighbor.HasIncomingRiver);

            editor.SetRiverMode((int)HexMapEditor.OptionalToggle.No);
            editor.HandleTestInput(neighbor);

            Assert.IsFalse(cell.HasOutgoingRiver);
            Assert.IsFalse(neighbor.HasIncomingRiver);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator chooseRoadTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexDirection direction = HexDirection.NE;
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);
            HexCell neighbor = cell.GetNeighbor(HexDirection.E);

            int elevationLevel = 1;
            editor.SetApplyElevation(true);
            editor.SetElevation(elevationLevel);
            editor.HandleTestInput(cell);

            editor.SetApplyElevation(false);
            editor.SetRoadMode((int)HexMapEditor.OptionalToggle.Yes);
            editor.HandleTestInput(neighbor);

            Assert.IsTrue(cell.HasRoadThroughEdge(HexDirection.E));
            Assert.IsTrue(neighbor.HasRoadThroughEdge(HexDirection.W));

            editor.SetRoadMode((int)HexMapEditor.OptionalToggle.No);
            editor.HandleTestInput(neighbor);

            Assert.IsFalse(cell.HasRoadThroughEdge(HexDirection.E));
            Assert.IsFalse(neighbor.HasRoadThroughEdge(HexDirection.W));

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator changeGridViewTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();

            editor.ShowGrid(true);
            CollectionAssert.Contains(editor.terrainMaterial.shaderKeywords, "GRID_ON");

            editor.ShowGrid(false);
            CollectionAssert.DoesNotContain(editor.terrainMaterial.shaderKeywords, "GRID_ON");

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator showGridUITest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponent<HexMapEditor>();

            editor.ShowUI(true);
            CollectionAssert.DoesNotContain(editor.terrainMaterial.shaderKeywords, "GRID_ON");

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }
    }
}
