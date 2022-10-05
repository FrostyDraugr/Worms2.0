using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject _cube;
    public MeshRenderer _mr;
    private BoxCollider _bc;

    private WorldGen.CellularAutomata _wg;
    private GameObject[,,] _children;

    [SerializeField]
    private float _iteration = 3;

    void Start()
    {
        _mr = gameObject.GetComponent<MeshRenderer>();
        _bc = gameObject.GetComponent<BoxCollider>();
        _wg = WorldGen.CellularAutomata.WorldGenerator;
        _cube.transform.localScale = new Vector3(1 / _iteration, 1 / _iteration, 1 / _iteration);
        int floatInt = Mathf.RoundToInt(_iteration);
        _children = new GameObject[floatInt, floatInt, floatInt];
        float div = 1 / _iteration;
        float offset = 1 / _iteration;

        for (int i = 0; i < _iteration; i++)
        {
            for (int j = 0; j < _iteration; j++)
            {
                for (int l = 0; l < _iteration; l++)
                {
                    float topOff = Random.Range(-div / 8, div / 8);
                    Vector3 pos = new Vector3(
                    gameObject.transform.position.x - (div * i) + offset,
                    gameObject.transform.position.y - (div * j) + offset + topOff,
                    gameObject.transform.position.z - (div * l) + offset);
                    _children[i, j, l] =
                    Instantiate(_cube, pos, gameObject.transform.rotation,
                    gameObject.transform);
                    _children[i, j, l].SetActive(false);
                }
            }
        }
    }


    //Move children into a different main parent then destroy Object to limit how many objects are in the scene
    //ALSO, move parents into a different parent at generation to make the hierachy easier.
    public void Hit()
    {
        for (int i = 0; i < _iteration; i++)
        {
            for (int j = 0; j < _iteration; j++)
            {
                for (int l = 0; l < _iteration; l++)
                {
                    _children[i, j, l].SetActive(true);
                    _children[i, j, l].transform.SetParent(gameObject.transform.parent);
                    _wg.UpdateNeighbors(_children[i, j, l].transform.position.x,
                    _children[i, j, l].transform.position.y,
                    _children[i, j, l].transform.position.z);
                }
            }
        }
        Destroy(gameObject);
    }
}
