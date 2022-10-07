using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _gameManager;

        public List<TeamManager> _teams;

        //Keep track of the number of destroyable objects for physics calc
        public int Destroyables;
        public bool DestroyBool;

        [Range(2, 6)]
        [SerializeField] private int NumOfTeams;

        [Range(1, 8)]
        [SerializeField] private int NumOfWorms;

        [SerializeField] private int _team;
        public bool GameLive;
        public Controllers.CharacterScript _cs;

        [SerializeField] private GameObject Worm;

        private int _teamId;

        private void Awake()
        {
            if (_gameManager == null)
            {
                DontDestroyOnLoad(gameObject);
                _gameManager = this;
            }
            else if (_gameManager != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DestroyBool = true;
            GameLive = false;
            Destroyables = 0;
            WorldGen.CellularAutomata.WorldGenerator.GenerateWorld();
            Spawn();
            StartCoroutine(IntroWait());
            Managers.EventManager._eventManager.OnDeathTrigger += OnWormDeath;
        }

        public void OnWormDeath(Managers.TeamManager team, GameObject worm
        , bool activePlayer)
        {
            if (activePlayer)
            {
                StartCoroutine(EndTurn());
            }

            team.TeamWormDeath(worm);
        }

        private void Destroy()
        {
            Managers.EventManager._eventManager.OnDeathTrigger -= OnWormDeath;
        }

        IEnumerator IntroWait()
        {
            yield return new WaitForSeconds(5);
            _cs.WormActive();
            GameLive = true;
        }

        private void Spawn()
        {
            _teamId = 1;
            _teams = new List<TeamManager>();
            for (int i = 0; i < NumOfTeams; i++)
            {
                _teams.Add(new TeamManager());
                _teams[i].CreateTeam(NumOfWorms, _teamId);
                _teamId++;
            }
            _team = 0;
            _cs = _teams[_team].GetActiveWorm().
            GetComponent<Controllers.CharacterScript>();
        }

        public GameObject GenerateWorm(Transform transform)
        {
            GameObject go = Instantiate(Worm, FindSpawnPos()
            , Quaternion.identity, transform);

            return go;
        }

        private Vector3 FindSpawnPos()
        {
            int index = Random.Range(0, MapMetrics.SpawnPosList.Count);
            Vector3 returnThis = MapMetrics.SpawnPosList[index] + new Vector3(0, 1.5f, 0);
            MapMetrics.SpawnPosList.RemoveAt(index);
            return returnThis;
        }

        public IEnumerator EndTurn()
        {
            GameLive = false;
            yield return new WaitForSeconds(7);


            if (_team == _teams.Count - 1)
            {
                _team = 0;
            }
            else
            {
                _team++;
            }

            _teams[_team].nextWorm();

            _cs = _teams[_team].GetActiveWorm().
            GetComponent<Controllers.CharacterScript>();
            _cs.WormActive();
            GameLive = true;
        }

        /*     private void RemoveDead(){
                for (int i = 0; i < _characters.Count; i++)
                {
                    for (int l = 0; l < _characters[i].Count; l++)
                    {
                        var WormD = _characters[i][l].
                        GetComponent<Controllers.CharacterScript>();
                        if (WormD.State == Controllers.CharacterScript.Mode.dead){
                            _characters[i].RemoveAt(l);
                                if (_characters[i].Count == 0){
                                    _characters.RemoveAt(i);
                                }

                                if (_characters.Count == 1){
                                    Debug.Log("Victory! " + _characters[0][0].transform.parent
                                    .name);
                                    GameLive = false;
                                    StopAllCoroutines();
                            }
                        }


                    }
                }
            } */

    }


}
