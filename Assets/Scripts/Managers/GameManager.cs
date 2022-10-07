using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GameMang;

        public List<TeamManager> Teams;

        //Keep track of the number of destroyable objects for physics calc
        public int Destroyables;
        public bool DestroyBool;

        [Range(2, 6)]
        [SerializeField] private int _numOfTeams;

        [Range(1, 8)]
        [SerializeField] private int _numOfWorms;

        [SerializeField] private int _team;

        private int _aliveTeams;
        public bool GameLive;
        public Controllers.CharacterScript CharacterStaticScript;

        [SerializeField] private GameObject Worm;

        private int _teamId;

        private void Awake()
        {
            if (GameMang == null)
            {
                DontDestroyOnLoad(gameObject);
                GameMang = this;
            }
            else if (GameMang != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DestroyBool = true;
            GameLive = false;
            Destroyables = 0;
            _aliveTeams = _numOfTeams;
            WorldGen.CellularAutomata.WorldGenerator.GenerateWorld();
            Spawn();
            StartCoroutine(IntroWait());
            Managers.EventManager._eventManager.OnDeathTrigger += OnWormDeath;
        }

        public void ReportTeamDeath()
        {
            _aliveTeams--;
            if (_aliveTeams == 1)
            {
                for (int i = 0; i < _numOfTeams; i++)
                {
                    if (Teams[i].Alive == true)
                    {
                        Debug.Log(Teams[i].TeamName + " is Victorious");
                    }
                }
                Time.timeScale = 0.25f;
                GameLive = false;
            }
        }

        //Start Coroutine from non-monobehaviour script...
        public void RemoveWorm(GameObject worm)
        {
            StartCoroutine(RemoveDeadWorm(worm));
        }

        //Why do I do this to myself
        private IEnumerator RemoveDeadWorm(GameObject worm)
        {
            yield return new WaitForSeconds(5);
            //Insert any death animation triggers here?
            worm.SetActive(false);
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
            CharacterStaticScript.WormActive();
            GameLive = true;
        }

        private void Spawn()
        {
            _teamId = 1;
            Teams = new List<TeamManager>();
            for (int i = 0; i < _numOfTeams; i++)
            {
                Teams.Add(new TeamManager());
                Teams[i].CreateTeam(_numOfWorms, _teamId);
                _teamId++;
            }
            _team = 0;
            CharacterStaticScript = Teams[_team].GetActiveWorm().
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

            bool teamSelected = false;

            while (!teamSelected)
            {
                _team++;

                if (_team >= Teams.Count)
                {
                    _team = 0;
                }

                //Should make a get alive status func instead
                if (Teams[_team].Alive == true)
                {
                    teamSelected = true;
                }
            }


            Teams[_team].nextWorm();
            CharacterStaticScript = Teams[_team].GetActiveWorm().
            GetComponent<Controllers.CharacterScript>();
            CharacterStaticScript.WormActive();
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
