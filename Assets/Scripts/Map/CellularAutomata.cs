using UnityEngine;

namespace WorldGen{
public class CellularAutomata : MonoBehaviour
{
    private bool[,] _noiseGrid;
    private bool[,] _tempNoiseGrid;

    public static CellularAutomata WorldGenerator;

    public GameObject[,,] CubeGrid;

    [SerializeField]
    private int _iterations;

    [SerializeField]
    private int _density;

    [SerializeField]
    private int _layers;

    [SerializeField]
    private int _neigbors;

    [SerializeField]
    private GameObject _cube;

    private GameObject _mp;

    private void Awake() {
    if (WorldGenerator == null){
        WorldGenerator = this;
    } else if (WorldGenerator != this){
        Destroy(WorldGenerator);
    }
    }

    public void GenerateWorld()
    {
        _mp = new GameObject("Enviornment");
        _noiseGrid = new bool[MapMetrics.Width, MapMetrics.Height];
        CubeGrid = new GameObject[_layers,MapMetrics.Width, MapMetrics.Height];

        makeNoise();

        for (int l = 0; l < _iterations; l++){
            CellularIteration();
        }
        SpawnMap(0);

        for (int l = 1; l < _layers; l++){
            CellularIteration();
            SpawnMap(l);
        }

        GenerateSpawnPoints();

    }

    private void GenerateSpawnPoints(){
        MapMetrics.SpawnPosList = new System.Collections.Generic.List<Vector3>();
        for (int i = 0; i < MapMetrics.Width; i++){
            for (int j = 0; j < MapMetrics.Height; j++){
                for (int z = _layers - 1 ; z > -1; z--){
                    if (CubeGrid[z, i, j] != null){
                        MapMetrics.SpawnPosList.Add(CubeGrid[z, i, j].transform.position);
                        break;
                    }
                }
            }
        }
    }

    private void SpawnMap(int y){
        for (int i = 0; i < MapMetrics.Width; i++){
            for (int j = 0; j < MapMetrics.Height; j++){
                if (_noiseGrid[i,j] == true){
                    CubeGrid[y,i,j] = Instantiate(_cube, GridToWorldPosition(i, y, j), Quaternion.identity);
                    CubeGrid[y,i,j].transform.SetParent(_mp.transform);
                    if (y == 0) continue;
                    HasNeighbor(y - 1,i,j);
                }
            }
        }
    }

    public void UpdateNeighbors(float x, float y, float z) {
        int xI = Mathf.RoundToInt(x + -0.5f + (MapMetrics.Width * 0.5f));
        int zI = Mathf.RoundToInt(z + -0.5f + (MapMetrics.Height * 0.5f));
        int yI = Mathf.RoundToInt(y);

        //Check Bellow
        if (yI > 0 && CubeGrid[yI - 1, xI , zI] != null){
           MeshRenderer mr = CubeGrid[yI - 1, xI , zI].GetComponent<MeshRenderer>();
           mr.enabled = true;
        }

        //Check Above
        if (yI < _layers - 1 && CubeGrid[yI , xI , zI] != null){
            MeshRenderer mr = CubeGrid[yI , xI , zI].GetComponent<MeshRenderer>();
            mr.enabled = true;
        }

      //Check Right
        if (CubeGrid[yI, xI + 1, zI] != null){
            MeshRenderer mr = CubeGrid[yI, xI + 1, zI].GetComponent<MeshRenderer>();
            mr.enabled = true;
        }

        //Check Left
        if (CubeGrid[yI, xI - 1, zI] != null){
            MeshRenderer mr = CubeGrid[yI, xI - 1, zI].GetComponent<MeshRenderer>();
            mr.enabled = true;
        }

        //Check in Front
        if (CubeGrid[yI, xI , zI + 1] != null){
            MeshRenderer mr = CubeGrid[yI, xI, zI + 1].GetComponent<MeshRenderer>();
            mr.enabled = true;
        }

        //Check in Back
        if (CubeGrid[yI, xI , zI - 1] != null){
            MeshRenderer mr = CubeGrid[yI, xI, zI - 1].GetComponent<MeshRenderer>();
            mr.enabled = true;
        }
        
    }

    private void HasNeighbor(int y, int x, int z){
        if (CubeGrid[y, x, z] != null){
        if (CubeGrid[y,x + 1,z] != null && CubeGrid[y, x - 1, z] != null && CubeGrid [y, x, z + 1] != null && CubeGrid[y, x, z - 1] != null){
            if (y == 0 || CubeGrid[y - 1, x, z] != null){
            MeshRenderer mr = CubeGrid[y,x,z].GetComponent<MeshRenderer>();
            mr.enabled = false;
            }
        }
        }
    }

    private void makeNoise(){
        //Reset Temporary Grid
        _tempNoiseGrid = new bool[MapMetrics.Width, MapMetrics.Height];
        for (int i = 0; i < MapMetrics.Width; i++){
            for (int j = 0; j < MapMetrics.Height; j++){
                int random = Random.Range(1, 101);
                if (random <= _density){
                    //Active
                    _tempNoiseGrid[i,j] = true;
                } else
                {
                    //Inactive
                    _tempNoiseGrid[i,j] = false;
                }
            }
        }
        //Transfer temporary grid to permanent grid
        _noiseGrid = _tempNoiseGrid;
    }

    private void CellularIteration(){
        //Reset TempGrid
        _tempNoiseGrid = new bool[MapMetrics.Width, MapMetrics.Height];
        
        for (int i = 0; i < MapMetrics.Width; i++){
            for (int j = 0; j < MapMetrics.Height; j++){
                checkNeighbors(i, j);
            }
        }

        //Transfer temporary grid to permanent grid
        _noiseGrid = _tempNoiseGrid;
    }

    private void checkNeighbors(int x, int y){
        
        int n = 0;
        
        for (int row = 0; row < 3; row++){
            for (int column = 0; column < 3; column++){
                if (column == 1 && row == 1)continue;        
                if (x + (row-1) < 0 || y + (column-1) < 0){
                    n--;
                } else if ( x + row > MapMetrics.Width || y + column > MapMetrics.Height){
                    n--;
                } else if (_noiseGrid[x + (row-1), y + (column-1)] != false){
                    n++;
                }
            }
        }
        
        if (n >= _neigbors){
            _tempNoiseGrid[x,y] = true;
        } else {
            _tempNoiseGrid[x,y] = false;
        }
    }

        private Vector3 GridToWorldPosition(int x, int y,int z){
        return new Vector3(x - (MapMetrics.Width * 0.5f) + 0.5f, y, z - (MapMetrics.Height * 0.5f) + 0.5f);
    }
}
}