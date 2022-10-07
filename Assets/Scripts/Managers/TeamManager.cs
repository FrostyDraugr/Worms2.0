using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class TeamManager
    {

        private List<GameObject> _worms;
        private int _activeWorm;
        private Color _teamColor;
        public bool Alive;


        public void CreateTeam(int numOfWorms, int TeamId)
        {
            _teamColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            Alive = true;
            _activeWorm = 0;
            GameObject team = new GameObject();
            team.name = "Team " + (TeamId);
            _worms = new List<GameObject>();
            for (int i = 0; i < numOfWorms; i++)
            {
                GameObject go = GameManager._gameManager.
                GenerateWorm(team.transform);

                MeshRenderer _mr = go.GetComponent<MeshRenderer>();
                _mr.material.color = _teamColor;
                go.GetComponent<Controllers.CharacterScript>().AssignedTeam = this;

                go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                _worms.Add(go);
            }
        }

        public GameObject GetActiveWorm()
        {
            return _worms[_activeWorm];
        }

        public void TeamWormDeath(GameObject worm)
        {
            for (int i = 0; i < _worms.Count; i++)
            {
                if (_worms[i] = worm)
                {
                    _worms.RemoveAt(i);
                    if (_worms.Count == 0)
                    {
                        this.Alive = false;
                    }
                }
            }
        }

        public void nextWorm()
        {
            if (_activeWorm >= _worms.Count - 1)
            {
                _activeWorm = 0;
            }
            else
            {
                _activeWorm++;
            }
        }
    }
}


