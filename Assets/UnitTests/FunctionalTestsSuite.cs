using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace Tests
{
    public class FunctionalTestsSuite
    {
        // FR 1
        [UnityTest]
        public IEnumerator t_1_mainMenuEntriesTest() //Показать экран с двумя кнопками "Map Editor" и "Exit"
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject go = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = go.transform.Find("Map Editor").gameObject.GetComponent<Button>();
            Button BExit = go.transform.Find("Exit").gameObject.GetComponent<Button>();

            Assert.IsNotNull(BMapEditor);
            Assert.IsNotNull(BExit);

            GameObject.Destroy(go);
            GameObject.Destroy(BMapEditor);
            GameObject.Destroy(BExit);
            SceneManager.UnloadScene("MainMenu");
        }

        // FR 2
        [UnityTest]
        public IEnumerator t_2_applicationExitSimulationTest() //Имитация нажатия на кнопку "Exit" и выход из приложения
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject go = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BExit = go.transform.Find("Exit").gameObject.GetComponent<Button>();

            BExit.GetComponent<Button>().onClick.Invoke();

            Assert.IsTrue(Application.isEditor);

            GameObject.Destroy(go);
            GameObject.Destroy(BExit);
            SceneManager.UnloadScene("MainMenu");
        }

        // FR 3
        [UnityTest]
        public IEnumerator t_3_switchToEditorSceneTest() //Имитация нажатия на кнопку "Map Editor" и переход на сцену с редактором карт
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject go = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = go.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject.Destroy(go);
            GameObject.Destroy(BMapEditor);
            SceneManager.UnloadScene("Scene");
        }

        // FR 4
        [UnityTest]
        public IEnumerator t_4_editorSceneEntriesTest() //Показать экран редактирования с панелью редактирования и кнопками сохранения, загрузки и создания новой карты
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Assert.IsNotNull(go[1]);
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Back to menu"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Save Button"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button"));
            Assert.IsNotNull(go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("New Map Button"));

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 5
        [UnityTest]
        public IEnumerator t_5_backToMenuButtonTest() //Имитация нажатия на кнопку "Back to menu" и переход обратно в главное меню
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject go = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BBackToMenu = go.transform.Find("Hex Map Editor").transform.Find("Back to menu").gameObject.GetComponent<Button>();

            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");
            BBackToMenu.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "MainMenu");

            GameObject.Destroy(go);
            GameObject.Destroy(BBackToMenu);
            SceneManager.UnloadScene("MainMenu");
        }

        // FR 6
        [UnityTest]
        public IEnumerator t_6_mapLoaderWindowTest() //Имитация нажатия на кнопку "Load" и открытие панели загрузки c кнопками загрузки/удаления карты и перехода обратно в окно редактирования
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BLoadButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button").gameObject.GetComponent<Button>();

            BLoadButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);
            GameObject slm = go[3].transform.Find("Save Load Menu").gameObject;
            Assert.IsTrue(slm.activeSelf);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Action Button").gameObject);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Delete Button").gameObject);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Cancel Button").gameObject);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BLoadButton);
            SceneManager.UnloadScene("Scene");
        }

        // FR 6.1
        [UnityTest]
        public IEnumerator t_7_deleteMapButtonTest() //Имитация нажатия кнопки "Delete" и удаление выбранного файла
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BLoadButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button").gameObject.GetComponent<Button>();

            BLoadButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            SaveLoadMenu slm = go[3].transform.Find("Save Load Menu").gameObject.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.SelectItem("test3");
            slm.hexGrid = go[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();
            int MapCount = slm.listContent.childCount;

            Button BDelete = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Delete Button").gameObject.GetComponent<Button>();
            BDelete.GetComponent<Button>().onClick.Invoke();

            for (int i = 0; i < slm.listContent.childCount; i++)
            {
                for (int j = 0; j < paths.Length; j++)
                {
                    if (slm.listContent.GetChild(i).gameObject.GetComponent<SaveLoadItem>().MapName != "test3")
                    {
                        Assert.IsTrue(true);
                        yield return null;
                    }
                }
            }
            Assert.IsFalse(false);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BLoadButton);
            GameObject.Destroy(BDelete);
            SceneManager.UnloadScene("Scene");
        }

        // FR 6.2
        [UnityTest]
        public IEnumerator t_8_loadMapButtonTest() //Имитация нажатия кнопки "Load", загрузка карты и возвращение в окно редактирования
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BLoadButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button").gameObject.GetComponent<Button>();

            BLoadButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            SaveLoadMenu slm = go[3].transform.Find("Save Load Menu").gameObject.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.SelectItem("test3");
            slm.hexGrid = go[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();

            Button BLoad = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Action Button").gameObject.GetComponent<Button>();
            BLoad.GetComponent<Button>().onClick.Invoke();

            Assert.IsTrue(File.Exists(Path.Combine(Application.persistentDataPath, "test3.map")));
            Assert.IsFalse(slm.gameObject.activeSelf);
            Assert.IsFalse(HexMapCamera.Locked);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BLoadButton);
            GameObject.Destroy(BLoad);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        // FR 6.3
        [UnityTest]
        public IEnumerator t_9_cancelButtonTest() //Имитация нажатия кнопки "Calcel" и возвращение в окно редактирования
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BLoadButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button").gameObject.GetComponent<Button>();

            BLoadButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            Button BCancel = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Cancel Button").gameObject.GetComponent<Button>();
            BCancel.GetComponent<Button>().onClick.Invoke();

            Assert.IsFalse(go[3].transform.Find("Save Load Menu").gameObject.activeSelf);
            Assert.IsFalse(HexMapCamera.Locked);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BLoadButton);
            GameObject.Destroy(BCancel);
            SceneManager.UnloadScene("Scene");
        }

        // FR 7
        [UnityTest]
        public IEnumerator t10_mapCreationMenuTest() //Имитация нажатия на кнопку "New Map" и открытие окна выбора размера создаваемой карты
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BNewMap = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("New Map Button").gameObject.GetComponent<Button>();

            BNewMap.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);
            GameObject nmm = go[3].transform.Find("New Map Menu").gameObject;

            Assert.IsTrue(nmm.activeSelf);
            Assert.IsNotNull(nmm.transform.Find("Menu").transform.Find("Small Button").gameObject);
            Assert.IsNotNull(nmm.transform.Find("Menu").transform.Find("Medium Button").gameObject);
            Assert.IsNotNull(nmm.transform.Find("Menu").transform.Find("Large Button").gameObject);
            Assert.IsNotNull(nmm.transform.Find("Menu").transform.Find("Cancel Button").gameObject);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BNewMap);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }

        // FR 8
        [UnityTest]
        public IEnumerator t11_saveMapMenuTest() //Имитация нажатия на кнопку "Save" и открытие панели сохранения с кнопками сохранения/удаления и выхода обратно в окно редактора
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BSave = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Save Button").gameObject.GetComponent<Button>();

            BSave.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);
            GameObject slm = go[3].transform.Find("Save Load Menu").gameObject;

            Assert.IsTrue(slm.activeSelf);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Action Button").gameObject);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Delete Button").gameObject);
            Assert.IsNotNull(slm.transform.Find("Menu").transform.Find("Cancel Button").gameObject);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BSave);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        // FR 8.1
        [UnityTest]
        public IEnumerator t12_deleteMapInSaveMenuTest() //Имитация нажатия кнопки "Delete" и удаление выбранного файла
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BSave = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Save Button").gameObject.GetComponent<Button>();

            BSave.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            SaveLoadMenu slm = go[3].transform.Find("Save Load Menu").gameObject.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.SelectItem("test3");
            slm.hexGrid = go[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();
            int MapCount = slm.listContent.childCount;

            Button BDelete = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Delete Button").gameObject.GetComponent<Button>();
            BDelete.GetComponent<Button>().onClick.Invoke();

            for (int i = 0; i < slm.listContent.childCount; i++)
            {
                for (int j = 0; j < paths.Length; j++)
                {
                    if (slm.listContent.GetChild(i).gameObject.GetComponent<SaveLoadItem>().MapName != "test3")
                    {
                        Assert.IsTrue(true);
                        yield return null;
                    }
                }
            }
            Assert.IsFalse(false);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BSave);
            GameObject.Destroy(BDelete);
            SceneManager.UnloadScene("Scene");
        }

        // FR 8.2
        [UnityTest]
        public IEnumerator t13_saveMapInSaveMenuTest() //Имитация нажатия кнопки "Save", сохранение карты и возвращение в окно редактирования
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BSaveButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Save Button").gameObject.GetComponent<Button>();

            BSaveButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            SaveLoadMenu slm = go[3].transform.Find("Save Load Menu").gameObject.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.SelectItem("test3");
            slm.hexGrid = go[1].GetComponent<HexGrid>();

            Button BSave = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Action Button").gameObject.GetComponent<Button>();
            BSave.GetComponent<Button>().onClick.Invoke();

            Assert.IsTrue(File.Exists(Path.Combine(Application.persistentDataPath, "test3.map")));
            Assert.IsFalse(slm.gameObject.activeSelf);
            Assert.IsFalse(HexMapCamera.Locked);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BSaveButton);
            GameObject.Destroy(BSave);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        // FR 8.3
        [UnityTest]
        public IEnumerator t14_cancelInSaveMenuTest() //Имитация нажатия кнопки "Cancel" и возвращение в окно редактирования
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            Button BLoadButton = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Load Button").gameObject.GetComponent<Button>();

            BLoadButton.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(1.0f);

            Button BCancel = go[3].transform.Find("Save Load Menu").transform.Find("Menu").transform.Find("Cancel Button").gameObject.GetComponent<Button>();
            BCancel.GetComponent<Button>().onClick.Invoke();

            Assert.IsFalse(go[3].transform.Find("Save Load Menu").gameObject.activeSelf);
            Assert.IsFalse(HexMapCamera.Locked);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(BLoadButton);
            GameObject.Destroy(BCancel);
            SceneManager.UnloadScene("Scene");
        }

        // FR 9
        [UnityTest]
        public IEnumerator t15_gridGenerationTest()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();
            
            HexGrid grid = go[1].GetComponent<HexGrid>();
            Assert.AreEqual(20, grid.cellCountX);
            Assert.AreEqual(15, grid.cellCountZ);

            HexGridChunk[] chunks = grid.getHexGridChunks();

            for (int i = 0; i < chunks.Length; ++i)
            {
                HexCell[] cells = chunks[i].getCells();
                
                for (int k = 0; k < cells.Length; ++k)
                {
                    // Assert that current elevation is 0
                    Assert.AreEqual(0, cells[k].Elevation);
                    // Asset that current terrain is Sand (index = 0, for None = -1)
                    Assert.AreEqual(0, cells[k].TerrainTypeIndex);
                    // Assert that cell has no water
                    Assert.IsFalse(cells[k].IsUnderwater);
                    // Assert that cells has no rivers/roads
                    Assert.IsFalse(cells[k].HasRiver);
                    Assert.IsFalse(cells[k].HasRoads);
                    // Assert that no specail objects
                    Assert.IsFalse(cells[k].IsSpecial);
                    Assert.AreEqual(0, cells[k].PlantLevel);
                    Assert.AreEqual(0, cells[k].UrbanLevel);
                    Assert.AreEqual(0, cells[k].FarmLevel);
                    Assert.IsFalse(cells[k].Walled);
                }
            }

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }
 
        // Cover FR 10, 10.1, 11
        [UnityTest]
        public IEnumerator t16_changeGridCellsElevationTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // Change Elevation Slider value
            int elevationValue = 1;
            Slider elevationSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Elevation Slider").gameObject.GetComponent<Slider>();
            elevationSlider.onValueChanged.Invoke(elevationValue);

            // Change Brush Size value
            int brushSize = 2;
            Slider brushSizeSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Brush Size Slider").gameObject.GetComponent<Slider>();
            Assert.AreEqual(0, brushSizeSlider.minValue);
            Assert.AreEqual(4, brushSizeSlider.maxValue);
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();
            editor.HandleTestInput(cell);

            Assert.AreEqual(elevationValue, cell.Elevation);
            for (HexDirection dir1 = HexDirection.NE; dir1 <= HexDirection.NW; ++dir1)
            {
                HexCell neighbor = cell.GetNeighbor(dir1);
                if (neighbor)
                {
                    Assert.AreEqual(elevationValue, neighbor.Elevation);

                    if (dir1 < HexDirection.NW)
                    {
                        for (HexDirection dir2 = dir1; dir2 <= dir1 + 1; ++dir2)
                        {
                            HexCell neighbor2 = neighbor.GetNeighbor(dir2);

                            if (neighbor2)
                                Assert.AreEqual(elevationValue, neighbor2.Elevation);
                        }
                    }
                    else
                    {
                        HexCell neighbor2 = neighbor.GetNeighbor(HexDirection.NW);
                        HexCell neighbor3 = neighbor.GetNeighbor(HexDirection.NE);

                        if (neighbor2)
                            Assert.AreEqual(elevationValue, neighbor2.Elevation);
                        if (neighbor3)
                            Assert.AreEqual(elevationValue, neighbor3.Elevation);
                    }
                }
            }

            // Set elevation toggle to OFF state and try
            // to change cells current elevation
            brushSize = 0; // brush size = 0 <---> 1 cell
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            elevationValue = 3;
            Toggle elevationToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Elevation Toggle").gameObject.GetComponent<Toggle>();
            elevationToggle.isOn = false;

            editor.HandleTestInput(cell);
            Assert.AreNotEqual(elevationValue, cell.Elevation);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 10, 10.1, 12
        [UnityTest]
        public IEnumerator t17_changeCellsBiomeTypeTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // Change Biome type to Snow
            Toggle sandToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Terrain Panel")
                                        .transform.Find("Terrain Sand").gameObject.GetComponent<Toggle>();
            Toggle snowToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Terrain Panel")
                                        .transform.Find("Terrain Snow").gameObject.GetComponent<Toggle>();
            Assert.IsFalse(snowToggle.isOn);
            Assert.IsTrue(sandToggle.isOn);
            snowToggle.isOn = true;
            Assert.IsTrue(snowToggle.isOn);
            Assert.IsFalse(sandToggle.isOn);

            // Change Brush Size value
            int brushSize = 0;
            Slider brushSizeSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Brush Size Slider").gameObject.GetComponent<Slider>();
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();
            // drag cursor simulation
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));

            // Check that biome type index contains in cells
            int biomeTypeIndex = 4;
            Assert.AreEqual(biomeTypeIndex, cell.TerrainTypeIndex);
            Assert.AreEqual(biomeTypeIndex, cell.GetNeighbor(direction).TerrainTypeIndex);

            // Select None terrain and try to change new cell
            // and see that its biome type index doesn't change
            Toggle noneToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Terrain Panel")
                                        .transform.Find("Terrain None").gameObject.GetComponent<Toggle>();
            noneToggle.isOn = true;

            HexCell newCell = cell.GetNeighbor(direction.Opposite());
            editor.HandleTestInput(newCell);
            Assert.AreNotEqual(biomeTypeIndex, newCell.TerrainTypeIndex);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 10, 10.1, 13
        [UnityTest]
        public IEnumerator t18_changeCellsWaterLevelTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // Get water slider
            Slider waterSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Water Slider").gameObject.GetComponent<Slider>();
            Assert.AreEqual(0, waterSlider.minValue);
            Assert.AreEqual(6, waterSlider.maxValue);

            // Get water toggle
            Toggle waterToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Water Toggle").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(waterToggle.isOn);
            
            // Change Brush Size value
            int brushSize = 1;
            Slider brushSizeSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Brush Size Slider").gameObject.GetComponent<Slider>();
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // Change water level and see if it changes
            int waterLevel = 1;
            waterSlider.onValueChanged.Invoke(waterLevel);
            Assert.AreEqual(0, cell.WaterLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(waterLevel, cell.WaterLevel);
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; ++dir)
            {
                HexCell neighbor = cell.GetNeighbor(dir);
                if (neighbor)
                    Assert.AreEqual(waterLevel, neighbor.WaterLevel);
            }

            // Delete water
            waterLevel = 0;
            waterSlider.onValueChanged.Invoke(waterLevel);
            editor.HandleTestInput(cell);

            Assert.AreEqual(waterLevel, cell.WaterLevel);
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; ++dir)
            {
                HexCell neighbor = cell.GetNeighbor(dir);
                if (neighbor)
                    Assert.AreEqual(waterLevel, neighbor.WaterLevel);
            }

            // Turn off water toggle
            // and try to change water level
            waterLevel = 2;
            waterToggle.isOn = false;
            waterSlider.onValueChanged.Invoke(waterLevel);
            editor.HandleTestInput(cell);

            Assert.AreNotEqual(waterLevel, cell.WaterLevel);
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; ++dir)
            {
                HexCell neighbor = cell.GetNeighbor(dir);
                if (neighbor)
                    Assert.AreNotEqual(waterLevel, neighbor.WaterLevel);
            }

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 10, 10.1, 14, 14.4
        [UnityTest]
        public IEnumerator t19_roadsAndObjectsInteractionsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(riverNone.isOn);
            Assert.IsFalse(riverYes.isOn);
            Assert.IsFalse(riverNo.isOn);

            // Road toggles
            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                 .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                 .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                 .transform.Find("Road No").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(roadNone.isOn);
            Assert.IsFalse(roadYes.isOn);
            Assert.IsFalse(roadNo.isOn);

            // Change Brush Size value
            int brushSize = 0;
            Slider brushSizeSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Brush Size Slider").gameObject.GetComponent<Slider>();
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // STRAIGHT RIVER
            // Set first cell to future drag
            editor.HandleTestInput(cell);
            // Turn on River and set river
            riverYes.isOn = true;
            HexDirection dir = HexDirection.NE;
            // Simulate drag mode
            editor.HandleTestInput(cell.GetNeighbor(dir));
            editor.HandleTestInput(cell.GetNeighbor(dir).GetNeighbor(dir));
            Assert.IsTrue(cell.HasRiver);
            Assert.IsTrue(cell.GetNeighbor(dir).HasRiver);
            Assert.IsTrue(cell.GetNeighbor(dir).GetNeighbor(dir).HasRiver);

            // Turn off river
            riverNone.isOn = true;
            // Set first cell to future drag
            HexCell roadNeighbor1 = cell.GetNeighbor(dir).GetNeighbor(dir.Next2());
            editor.HandleTestInput(roadNeighbor1);
            // Turn on roads
            roadYes.isOn = true;
            // Simulate drag mode
            HexCell roadNeighbor2 = cell.GetNeighbor(dir).GetNeighbor(dir.Previous());
            editor.HandleTestInput(cell.GetNeighbor(dir));
            editor.HandleTestInput(roadNeighbor2);
            Assert.IsTrue(roadNeighbor1);
            Assert.IsTrue(cell.GetNeighbor(dir).HasRoads);
            Assert.IsTrue(roadNeighbor2);

            // Set river through road (14.4 - 5)
            roadNone.isOn = true;
            // Set first cell to future drag
            editor.HandleTestInput(roadNeighbor1.GetNeighbor(HexDirection.NE));
            riverYes.isOn = true;
            editor.HandleTestInput(roadNeighbor1);
            editor.HandleTestInput(roadNeighbor1.GetNeighbor(HexDirection.SW));
            Assert.IsTrue(roadNeighbor1.HasRoads);
            Assert.IsTrue(roadNeighbor1.HasRiver);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 1, 2
        [UnityTest]
        public IEnumerator t20_riverInterationWithLandspaceObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(riverNone.isOn);
            Assert.IsFalse(riverYes.isOn);
            Assert.IsFalse(riverNo.isOn);

            // Change Brush Size value
            int brushSize = 0;
            Slider brushSizeSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Brush Size Slider").gameObject.GetComponent<Slider>();
            brushSizeSlider.onValueChanged.Invoke(brushSize);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[4].GetNeighbor(direction).GetNeighbor(direction)
                                        .GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 1. Cells
            // Set first cell from which to drag
            editor.HandleTestInput(cell);
            riverYes.isOn = true;
            HexDirection dir = HexDirection.E;
            HexCell neighbor = cell.GetNeighbor(dir);
            editor.HandleTestInput(neighbor);
            chunks[0].Triangulate();
            Assert.IsTrue(cell.HasRiver);
            Assert.IsTrue(neighbor.HasRiver);

            int elevationValue = 2;
            Toggle elevationToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Elevation Toggle").gameObject.GetComponent<Toggle>();
            Slider elevationSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Elevation Slider").gameObject.GetComponent<Slider>();

            elevationSlider.onValueChanged.Invoke(elevationValue);
            editor.HandleTestInput(neighbor);
            chunks[0].Triangulate();
            elevationToggle.isOn = false;
            Assert.IsFalse(cell.HasRiver);
            Assert.IsFalse(neighbor.HasRiver);

            // 2. Water
            int waterLevel = 1;
            Toggle waterToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                    .transform.Find("Water Toggle").gameObject.GetComponent<Toggle>();
            Slider waterSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Water Slider")
                                    .gameObject.GetComponent<Slider>();
            waterSlider.onValueChanged.Invoke(waterLevel);
            editor.HandleTestInput(cell);
            chunks[0].Triangulate();

            waterToggle.isOn = false;
            riverYes.isOn = true;
            dir = HexDirection.SW;
            editor.HandleTestInput(cell.GetNeighbor(dir));
            chunks[0].Triangulate();
            Assert.IsTrue(cell.HasOutgoingRiver);
            Assert.IsTrue(cell.GetNeighbor(dir).HasIncomingRiver);
            Assert.IsTrue(cell.IsUnderwater);
            riverNone.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t21_riverInterationWithUrbanObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider urbanSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Urban Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle urbanToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Urban Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Urban object, check and remoe
            // Add urban objects
            urbanToggle.isOn = true;
            int urbanLevel = 1;
            urbanSlider.onValueChanged.Invoke(urbanLevel);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            int expUrbanCount = chunk.features.container.childCount;

            // Add rivers
            urbanToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            riverYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            yield return new WaitForSeconds(0.1f);
            int actualUrbanCount = chunk.features.container.childCount;
            Assert.Less(actualUrbanCount, expUrbanCount);

            // Remove rivers
            riverNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            yield return new WaitForSeconds(0.1f);
            actualUrbanCount = chunk.features.container.childCount;
            Assert.AreEqual(expUrbanCount, actualUrbanCount);
            riverNone.isOn = true;

            // Remove urban objects
            urbanToggle.isOn = true;
            urbanSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            urbanToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t22_riverInterationWithFarmObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);
            Debug.Log(cell.Position.ToString());

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider farmSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Farm Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle farmToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Farm Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Farm object, check and remove
            // Add urban objects
            farmToggle.isOn = true;
            int farmLevel = 1;
            farmSlider.onValueChanged.Invoke(farmLevel);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            int expFarmCount = chunk.features.container.childCount;

            // Add rivers
            farmToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            riverYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            chunk.Triangulate();
            int actualFarmCount = chunk.features.container.childCount;
            Assert.Less(actualFarmCount, expFarmCount);

            // Remove rivers
            riverNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            chunk.Triangulate();
            actualFarmCount = chunk.features.container.childCount;
            Assert.AreEqual(expFarmCount, actualFarmCount);
            riverNone.isOn = true;

            // Remove farm objects
            farmToggle.isOn = true;
            farmSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            farmToggle.isOn = false;
            
            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t23_riverInterationWithPlantObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);
            
            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider plantSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Plant Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle plantToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Plant Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Plant object, check and remoe
            // Add urban objects
            plantToggle.isOn = true;
            int plantLevel = 1;
            plantSlider.onValueChanged.Invoke(plantLevel);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            int expPlantCount = chunk.features.container.childCount;

            // Add rivers
            plantToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            riverYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            //chunk.Triangulate();
            yield return new WaitForSeconds(0.1f);
            int actualPlantCount = chunk.features.container.childCount;
            Assert.Less(actualPlantCount, expPlantCount);

            // Remove rivers
            riverNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            chunk.Triangulate();
            actualPlantCount = chunk.features.container.childCount;
            Assert.AreEqual(expPlantCount, actualPlantCount);
            riverNone.isOn = true;

            // Remove Plant objects
            plantToggle.isOn = true;
            plantSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            plantToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 3, 15.3
        [UnityTest]
        public IEnumerator t24_riverInterationWithSpecialObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell ncell = chunk.getCells()[0].GetNeighbor(direction).GetNeighbor(direction).GetNeighbor(direction);
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider specialSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Special Slider")
                                      .gameObject.GetComponent<Slider>();
            Toggle specialToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Special Toggle")
                                      .gameObject.GetComponent<Toggle>();

            // 3.1. Set Special object, check and remove
            // Add special object
            specialToggle.isOn = true;
            int specialIndex = 1;
            specialSlider.onValueChanged.Invoke(specialIndex);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(specialIndex, cell.SpecialIndex);
            int expSpecialIndex = chunk.features.container.childCount;

            // Try to add rivers
            specialToggle.isOn = false;
            editor.HandleTestInput(ncell.GetNeighbor(direction).GetNeighbor(direction));
            riverYes.isOn = true;
            editor.HandleTestInput(ncell.GetNeighbor(direction));
            editor.HandleTestInput(ncell.GetNeighbor(direction).GetNeighbor(direction.Opposite()));

            Assert.IsFalse(cell.GetNeighbor(direction).HasRiver);
            Assert.IsFalse(cell.HasRiver);
            Assert.IsFalse(cell.GetNeighbor(direction.Opposite()).HasRiver);
            int actualSpecialIndex = chunk.features.container.childCount;
            Assert.AreEqual(actualSpecialIndex, expSpecialIndex);
           
            // Remove special object
            specialToggle.isOn = true;
            specialSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            specialToggle.isOn = false;
            
            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.2 - 1.1, 2.1
        [UnityTest]
        public IEnumerator t25_riverInteractionWithOtherRivers()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle riverNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River None").gameObject.GetComponent<Toggle>();
            Toggle riverYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River Yes").gameObject.GetComponent<Toggle>();
            Toggle riverNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("River Panel")
                                  .transform.Find("River No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[1].GetNeighbor(direction).GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // Test parts 1.1, 2.1
            // Add first river
            HexCell river1Start = cell.GetNeighbor(direction.Opposite());
            HexCell river1End = cell.GetNeighbor(direction);
            editor.HandleTestInput(river1Start);
            riverYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(river1End);
            // Add second river near to start of first river
            riverNone.isOn = true;
            HexCell river2Start = chunk.getCells()[3];
            editor.HandleTestInput(river2Start);
            riverYes.isOn = true;
            HexCell river2End = river2Start.GetNeighbor(direction.Previous2());
            editor.HandleTestInput(river2End);
            riverNone.isOn = true;
            // Check requirement 1.1
            editor.HandleTestInput(river2End);
            riverYes.isOn = true;
            editor.HandleTestInput(river2End.GetNeighbor(direction.Previous()));
            riverNone.isOn = true;
            Assert.IsTrue(river1Start.HasRiverThroughEdge(direction.Next2()));
            Assert.IsTrue(river1Start.HasRiverThroughEdge(direction));
            // Check requirement 2.1
            editor.HandleTestInput(river1Start);
            riverYes.isOn = true;
            editor.HandleTestInput(river1Start.GetNeighbor(direction.Previous()));
            riverNone.isOn = true;
            Assert.IsTrue(river1Start.HasRiverThroughEdge(direction.Previous()));
            Assert.IsTrue(river1Start.HasRiverThroughEdge(direction.Next2()));
            Assert.IsFalse(river1Start.HasRiverThroughEdge(direction));

            // Test parts 1.2, 2.2
            // Add parts to first river
            editor.HandleTestInput(river1End);
            riverYes.isOn = true;
            river1End = river1End.GetNeighbor(direction);
            editor.HandleTestInput(river1End);
            river1End = river1End.GetNeighbor(direction);
            editor.HandleTestInput(river1End);
            riverNone.isOn = true;
            // Check requirement 1.2
            editor.HandleTestInput(river1End.GetNeighbor(direction.Next2()));
            riverYes.isOn = true;
            editor.HandleTestInput(river1End);
            riverNone.isOn = true;
            Assert.IsTrue(river1End.HasRiverThroughEdge(direction.Next2()));
            Assert.IsFalse(river1End.HasRiverThroughEdge(direction.Opposite()));
            // Check requirement 2.2
            editor.HandleTestInput(river1End);
            riverYes.isOn = true;
            river2End = river1End.GetNeighbor(direction.Next2().Opposite());
            editor.HandleTestInput(river2End);
            riverNone.isOn = true;
            Assert.IsTrue(river2End.HasRiverThroughEdge(river2End.IncomingRiver));

            // Test parts 1.3, 2.3
            // Add parts to first river
            // Get cell after last cell of first river
            river1End = cell.GetNeighbor(direction);
            editor.HandleTestInput(river1End);
            riverYes.isOn = true;
            river1End = river1End.GetNeighbor(direction);
            editor.HandleTestInput(river1End);
            riverNone.isOn = true;
            // Requirement 1.3 check
            // Get cell which divide river on two parts:
            // upper part = 1 cell
            // lower part > 1 cell
            river2Start = river1End.GetNeighbor(direction.Opposite()).GetNeighbor(direction.Next2());
            river2End = river2Start.GetNeighbor(direction.Previous());
            editor.HandleTestInput(river2Start);
            riverYes.isOn = true;
            editor.HandleTestInput(river2End);
            riverNone.isOn = true;
            Assert.IsTrue(river2Start.HasRiverThroughEdge(direction.Previous()));
            Assert.IsTrue(river2End.HasRiverThroughEdge(direction.Next2()));
            Assert.IsTrue(river2End.HasRiverThroughEdge(direction));
            Assert.IsFalse(river2End.HasRiverThroughEdge(direction.Opposite()));
            // Requirement 2.3 check
            editor.HandleTestInput(river2End);
            riverYes.isOn = true;
            river2End = river2End.GetNeighbor(direction.Previous());
            editor.HandleTestInput(river2End);
            riverNone.isOn = true;
            Assert.IsTrue(river2End.HasIncomingRiver);
            Assert.IsTrue(river2End.GetNeighbor(direction.Next2()).HasOutgoingRiver);
            Assert.IsFalse(river2End.GetNeighbor(direction.Next2()).GetNeighbor(direction).HasRiver);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.3 - 1, 2
        [UnityTest]
        public IEnumerator t26_roadInterationWithLandspaceObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(roadNone.isOn);
            Assert.IsFalse(roadYes.isOn);
            Assert.IsFalse(roadNo.isOn);
            
            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[0].GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 1. Cells
            // Set first cell from which to drag
            editor.HandleTestInput(cell);
            roadYes.isOn = true;
            HexDirection dir = HexDirection.E;
            HexCell neighbor = cell.GetNeighbor(dir);
            editor.HandleTestInput(neighbor);
            chunks[0].Triangulate();
            Assert.IsTrue(cell.HasRoads);
            Assert.IsTrue(neighbor.HasRoads);

            int elevationValue = 2;
            Toggle elevationToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Elevation Toggle").gameObject.GetComponent<Toggle>();
            Slider elevationSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                        .transform.Find("Elevation Slider").gameObject.GetComponent<Slider>();

            elevationSlider.onValueChanged.Invoke(elevationValue);
            editor.HandleTestInput(neighbor);
            chunks[0].Triangulate();
            elevationToggle.isOn = false;
            Assert.IsFalse(cell.HasRoads);
            Assert.IsFalse(neighbor.HasRoads);

            // 2. Water
            int waterLevel = 1;
            Toggle waterToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel")
                                    .transform.Find("Water Toggle").gameObject.GetComponent<Toggle>();
            Slider waterSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Water Slider")
                                    .gameObject.GetComponent<Slider>();
            waterSlider.onValueChanged.Invoke(waterLevel);
            editor.HandleTestInput(cell);
            chunks[0].Triangulate();

            waterToggle.isOn = false;
            roadYes.isOn = true;
            dir = HexDirection.SW;
            editor.HandleTestInput(cell.GetNeighbor(dir));
            chunks[0].Triangulate();
            Assert.IsTrue(cell.HasRoads);
            roadNone.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.3 - 1, 2
        [UnityTest]
        public IEnumerator t27_roadInterationWithOtherRoadsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();
            Assert.IsTrue(roadNone.isOn);
            Assert.IsFalse(roadYes.isOn);
            Assert.IsFalse(roadNo.isOn);

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk[] chunks = grid.getHexGridChunks();
            HexCell[] cells = chunks[0].getCells();
            HexCell cell = cells[2].GetNeighbor(direction).GetNeighbor(direction);

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();
            
            editor.HandleTestInput(cell);
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; ++dir)
            {
                editor.HandleTestInput(cell.GetNeighbor(dir));
                roadYes.isOn = true;
                editor.HandleTestInput(cell);
                roadNone.isOn = true;
            }

            int roadCount = 0;
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; ++dir)
            {
                if (cell.HasRoadThroughEdge(dir))
                    ++roadCount;
                Assert.IsTrue(cell.HasRoadThroughEdge(dir));
            }

            Assert.AreEqual(6, roadCount);

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.3 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t28_roadInterationWithUrbanObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[1].GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider urbanSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Urban Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle urbanToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Urban Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Urban object, check and remoe
            // Add urban objects
            urbanToggle.isOn = true;
            int urbanLevel = 1;
            urbanSlider.onValueChanged.Invoke(urbanLevel);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            int expUrbanCount = chunk.features.container.childCount;

            // Add roads
            urbanToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            roadYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            yield return new WaitForSeconds(0.1f);
            int actualUrbanCount = chunk.features.container.childCount;
            Assert.Less(actualUrbanCount, expUrbanCount);

            // Remove roads
            roadNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            yield return new WaitForSeconds(0.1f);
            actualUrbanCount = chunk.features.container.childCount;
            Assert.AreEqual(expUrbanCount, actualUrbanCount);
            roadNone.isOn = true;

            // Remove urban objects
            urbanToggle.isOn = true;
            urbanSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            urbanToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.3 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t29_roadInterationWithFarmObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            // River toggles
            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);
            Debug.Log(cell.Position.ToString());

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider farmSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Farm Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle farmToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Farm Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Farm object, check and remove
            // Add urban objects
            farmToggle.isOn = true;
            int farmLevel = 1;
            farmSlider.onValueChanged.Invoke(farmLevel);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            int expFarmCount = chunk.features.container.childCount;

            // Add roads
            farmToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            roadYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            chunk.Triangulate();
            int actualFarmCount = chunk.features.container.childCount;
            Assert.Less(actualFarmCount, expFarmCount);

            // Remove roads
            roadNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            chunk.Triangulate();
            actualFarmCount = chunk.features.container.childCount;
            Assert.AreEqual(expFarmCount, actualFarmCount);
            roadNone.isOn = true;

            // Remove farm objects
            farmToggle.isOn = true;
            farmSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            farmToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.1 - 3, 15, 15.1
        [UnityTest]
        public IEnumerator t30_roadInterationWithPlantObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider plantSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Plant Slider")
                                    .gameObject.GetComponent<Slider>();
            Toggle plantToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel")
                                    .transform.Find("Plant Toggle").gameObject.GetComponent<Toggle>();

            // 3.1. Set Plant object, check and remoe
            // Add urban objects
            plantToggle.isOn = true;
            int plantLevel = 1;
            plantSlider.onValueChanged.Invoke(plantLevel);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            int expPlantCount = chunk.features.container.childCount;

            // Add roads
            plantToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            roadYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));
            yield return new WaitForSeconds(0.1f);
            int actualPlantCount = chunk.features.container.childCount;
            Assert.Less(actualPlantCount, expPlantCount);

            // Remove roads
            roadNo.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction));
            chunk.Triangulate();
            actualPlantCount = chunk.features.container.childCount;
            Assert.AreEqual(expPlantCount, actualPlantCount);
            roadNone.isOn = true;

            // Remove Plant objects
            plantToggle.isOn = true;
            plantSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            plantToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        // FR 14.3 - 3, 15.3
        [UnityTest]
        public IEnumerator t31_roadInterationWithSpecialObjectsTest()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

            GameObject[] go = SceneManager.GetActiveScene().GetRootGameObjects();

            Toggle roadNone = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road None").gameObject.GetComponent<Toggle>();
            Toggle roadYes = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road Yes").gameObject.GetComponent<Toggle>();
            Toggle roadNo = go[3].transform.Find("Hex Map Editor").transform.Find("Left Panel").transform.Find("Road Panel")
                                  .transform.Find("Road No").gameObject.GetComponent<Toggle>();

            // Get cell from grid (simulate user interaction with grid)
            HexDirection direction = HexDirection.NE;
            HexGrid grid = go[1].gameObject.GetComponent<HexGrid>();
            HexGridChunk chunk = grid.getHexGridChunks()[0];
            HexCell cell = chunk.getCells()[0].GetNeighbor(direction);

            Assert.IsNotNull(cell.GetNeighbor(direction));
            Assert.IsNotNull(cell.GetNeighbor(direction.Opposite()));

            HexMapEditor editor = go[3].transform.Find("Hex Map Editor").GetComponentInChildren<HexMapEditor>();

            // 3. Relief objects
            // Get all toggles and sliders for work
            Slider specialSlider = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Special Slider")
                                      .gameObject.GetComponent<Slider>();
            Toggle specialToggle = go[3].transform.Find("Hex Map Editor").transform.Find("Right Panel").transform.Find("Special Toggle")
                                      .gameObject.GetComponent<Toggle>();

            // 3.1. Set Special object, check and remove
            // Add special object
            specialToggle.isOn = true;
            int specialIndex = 1;
            specialSlider.onValueChanged.Invoke(specialIndex);
            editor.HandleTestInput(cell);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(specialIndex, cell.SpecialIndex);
            int expSpecialIndex = chunk.features.container.childCount;

            // Try to add roads
            specialToggle.isOn = false;
            editor.HandleTestInput(cell.GetNeighbor(direction));
            roadYes.isOn = true;
            editor.HandleTestInput(cell);
            editor.HandleTestInput(cell.GetNeighbor(direction.Opposite()));

            Assert.IsFalse(cell.GetNeighbor(direction).HasRiver);
            Assert.IsFalse(cell.HasRiver);
            Assert.IsFalse(cell.GetNeighbor(direction.Opposite()).HasRiver);
            int actualSpecialIndex = chunk.features.container.childCount;
            Assert.AreEqual(actualSpecialIndex, expSpecialIndex);

            // Remove special object
            specialToggle.isOn = true;
            specialSlider.onValueChanged.Invoke(0);
            editor.HandleTestInput(cell);
            chunk.Triangulate();
            specialToggle.isOn = false;

            foreach (GameObject g in go)
            {
                GameObject.Destroy(g);
            }
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator t32_showGrid()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);
            GameObject button = SceneManager.GetActiveScene().GetRootGameObjects()[3];
            Button BMapEditor = button.transform.Find("Map Editor").gameObject.GetComponent<Button>();

            BMapEditor.GetComponent<Button>().onClick.Invoke();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");

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
    }
}
