using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    class HexGridTestSuite
    {
        [Test]
        public void creationTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            Assert.AreEqual(20, grid.cellCountX);
            Assert.AreEqual(15, grid.cellCountZ);
        }

        [Test]
        public void creationWithInvalidParametersTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            bool isCreate = grid.CreateMap(-1, -2);
            Assert.IsFalse(isCreate);
        }

        [Test]
        public void destroyChunksTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();
            bool isCreate = grid.CreateMap(10, 10);
            Assert.IsTrue(isCreate);
        }

        [Test]
        public void getCellTestByPositionTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();

            Vector3 position = new Vector3();
            position.x = 12;
            position.y = 1;
            position.z = 20;
            
            position = grid.transform.InverseTransformPoint(position);
            HexCoordinates expected_coord = HexCoordinates.FromPosition(position);

            HexCell cell = grid.GetCell(position);
            HexCoordinates actual_coord = cell.coordinates;

            Assert.AreEqual(expected_coord, actual_coord);
        }

        [Test]
        public void getCellTestByHexCoordinatesTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();

            // Normal cell
            int x = 5;
            int z = 6;
            HexCoordinates coord = new HexCoordinates(x, z);
            HexCell cell = grid.GetCell(coord);
            Assert.AreEqual(coord, cell.coordinates);
        }

        [Test]
        public void getCellByHexCoordinatesInvalidZTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();

            // Normal cell
            int x = 5;
            int z = -12;
            HexCoordinates coord = new HexCoordinates(x, z);
            HexCell cell = grid.GetCell(coord);
            Assert.IsNull(cell);
        }

        [Test]
        public void getCellByHexCoordinatesInvalidXTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();

            // Normal cell
            int x = -5;
            int z = 6;
            HexCoordinates coord = new HexCoordinates(x, z);
            HexCell cell = grid.GetCell(coord);
            Assert.IsNull(cell);
        }

        [Test]
        public void showChunksTest()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
            HexGrid grid = obj.GetComponent<HexGrid>();

            grid.ShowUI(true);
            HexGridChunk[] chunks = grid.getHexGridChunks();

            foreach(HexGridChunk chunk in chunks)
            {
                Assert.IsTrue(chunk.isActiveAndEnabled);
            }
        }

        public void saveMapOnPath(string fpath)
        {
            // Check file if exists
            if (File.Exists(fpath))
            {
                File.Delete(fpath);
            }
            using (BinaryWriter bw = new BinaryWriter(File.Open(fpath, FileMode.Create)))
            {
                GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
                HexGrid grid = obj.GetComponent<HexGrid>();

                HexGridChunk[] chunks = grid.getHexGridChunks();
                HexCell[] cells = chunks[0].getCells();
                int index = 7;
                HexDirection direction = HexDirection.NE;

                cells[index].SetOutgoingRiver(direction);

                index = 2;
                cells[index].AddRoad(direction);

                bw.Write(1);
                grid.Save(bw);
            }
        }

        public void saveMapWithErrorsOnPath(string fpath)
        {
            if (File.Exists(fpath))
            {
                File.Delete(fpath);
            }
            using (BinaryWriter bw = new BinaryWriter(File.Open(fpath, FileMode.Create)))
            {
                GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
                HexGrid grid = obj.GetComponent<HexGrid>();
                bw.Write(1);

                // Add problems
                grid.cellCountX = -10;

                grid.Save(bw);
            }
        }

        [Test]
        public void loadGridTest()
        {
            string fpath = Path.Combine(Application.persistentDataPath, "test.map");

            if (!File.Exists(fpath))
            {
                saveMapOnPath(fpath);
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(fpath)))
            {
                GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
                HexGrid grid = obj.GetComponent<HexGrid>();
                int header = reader.ReadInt32();
                grid.Load(reader, header);
                Assert.IsNotNull(grid.getHexGridChunks());
            }
        }

        [Test]
        public void failedloadGridTest()
        {
            string fpath = Path.Combine(Application.persistentDataPath, "failed_test.map");

            if (!File.Exists(fpath))
            {
                saveMapWithErrorsOnPath(fpath);
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(fpath)))
            {
                GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Hex Grid"));
                HexGrid grid = obj.GetComponent<HexGrid>();

                int old_x = grid.cellCountX;
                int old_z = grid.cellCountZ;

                int header = reader.ReadInt32();
                grid.Load(reader, header);

                Assert.AreEqual(old_x, grid.cellCountX);
                Assert.AreEqual(old_z, grid.cellCountZ);
            }
        }

        [Test]
        public void saveGoodGridTest()
        {
            string fpath = Path.Combine(Application.persistentDataPath, "test.map");
            saveMapOnPath(fpath);
            Assert.IsTrue(File.Exists(fpath));
        }

        [Test]
        public void saveFailedGridTest()
        {
            string fpath = Path.Combine(Application.persistentDataPath, "failed_test.map");
            saveMapWithErrorsOnPath(fpath);
            Assert.IsTrue(File.Exists(fpath));
        }
    }
}
