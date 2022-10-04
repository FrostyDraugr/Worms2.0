using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class TeamManager
    {

        private List<GameObject> _worms;
        private int _activeWorm;
        private Color _teamColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);


        public void CreateTeam()
        {
            _activeWorm = 0;
            GameObject team = new GameObject();
            team.name = "Team " + (GameManager._gameManager.TeamId);
            _worms = new List<GameObject>();
            for (int i = 0; i < GameManager._gameManager.NumOfWorms; i++)
            {
                GameObject go = GameManager._gameManager.
                GenerateWorm(team.transform);

                MeshRenderer _mr = go.GetComponent<MeshRenderer>();
                _mr.material.color = _teamColor;
                Controllers.CharacterScript comp = go.GetComponent<Controllers.
                CharacterScript>();

                comp.Id = GameManager._gameManager.Id;
                comp.TeamId = GameManager._gameManager.TeamId;

                go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                _worms.Add(go);
            }
        }

        public GameObject GetActiveWorm()
        {
            return _worms[_activeWorm];
        }



        public void nextWorm()
        {
            if (_activeWorm == _worms.Count - 1)
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


