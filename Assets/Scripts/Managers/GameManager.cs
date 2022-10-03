using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Managers{
public class GameManager : MonoBehaviour
{
    public static GameManager _gameManager;

    public List<List<GameObject>> _characters = new List<List<GameObject>>();

    //Keep track of the number of destroyable objects for physics calc
    public int Destroyables;
    public bool DestroyBool;
    public bool inCombat;
    
    [Range(2,6)]
    public int NumOfTeams;

    [Range (1,8)]
    public int NumOfWorms;

    //Control active worm
    private int _team;
    private int _wormTeam;
    public bool GameLive;

    public Controllers.CharacterScript _cs;

    [SerializeField]
    private GameObject _worm;

    private void Awake() {
    if (_gameManager == null){
        DontDestroyOnLoad(gameObject);
        _gameManager = this;
    } else if (_gameManager != this){
        Destroy(gameObject);
    }
    }

    // Start is called before the first frame update
    private void Start()
    {
        inCombat = false;
        DestroyBool = true;
        GameLive = false;
        Destroyables = 0;
        WorldGen.CellularAutomata.WorldGenerator.GenerateWorld();
        Spawn();
        StartCoroutine(IntroWait());
        Managers.EventManager._eventManager.OnDeathTrigger += OnWormDeath;
    }

    public void OnWormDeath(int id, bool activePlayer){
        Debug.Log("Death Event");
        if (activePlayer){
            Debug.Log("True");
            StartCoroutine(EndTurn());
        }
    }

    private void Destroy(){
        Managers.EventManager._eventManager.OnDeathTrigger -= OnWormDeath;
    }

    IEnumerator IntroWait(){
        yield return new WaitForSeconds(5);
        _cs.WormActive();
        GameLive = true;
    }

    private void Spawn(){
        int id = 0;
        for (int i = 0; i < NumOfTeams; i++){
            _characters.Add(new List<GameObject>());
            GameObject team = new GameObject();
            Color tc = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            team.name = "Team" + (i + 1);
                for (int l = 0; l < NumOfWorms; l++){
                GameObject go = Instantiate (_worm, FindSpawnPos(), Quaternion.identity
                , team.transform);
                MeshRenderer _mr = go.GetComponent<MeshRenderer>();
                _mr.material.color = tc;
                go.GetComponent<Controllers.CharacterScript>().id = id;
                id++;
                //go.transform.SetParent(team.transform);
                go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                _characters[i].Insert(l, go);
            }
        }
        _team = 0;
        _wormTeam = 0;
        _cs = _characters[_team][_wormTeam].
        GetComponent<Controllers.CharacterScript>();
    }

    private Vector3 FindSpawnPos(){
        int index = Random.Range(0, MapMetrics.SpawnPosList.Count);
        Vector3 returnThis = MapMetrics.SpawnPosList[index] + new Vector3(0, 1.5f, 0);
        MapMetrics.SpawnPosList.RemoveAt(index);
        return returnThis;
    }

    public IEnumerator EndTurn(){
        GameLive = false;
        yield return new WaitForSeconds(5);
        
        RemoveDead();
        
        if (_team == _characters.Count - 1){
            _team = 0;
                if (_wormTeam == _characters[_team].Count - 1){
                    _wormTeam = 0;
                } else {
                    _wormTeam++;
                }
        }else{
            _team++;
        }
        _cs = _characters[_team][_wormTeam].
        GetComponent<Controllers.CharacterScript>();
        _cs.WormActive();
        GameLive = true;
    }

    private void RemoveDead(){
        for (int i = 0; i < _characters.Count; i++)
        {
            for (int l = 0; l < _characters[i].Count; l++)
            {
                var WormD = _characters[i][l].
                GetComponent<Controllers.CharacterScript>().State;
                if (WormD == Controllers.CharacterScript.Mode.dead){
                    
                }
            }
        }
    }
}
}