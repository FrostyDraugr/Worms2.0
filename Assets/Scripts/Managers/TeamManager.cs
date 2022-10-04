using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class TeamManager{
    
    private List<GameObject> _worms;

    private int _activeWorm;
    

    public void CreateTeam() {
        _activeWorm = 0;
        GameObject team = new GameObject();
        Color tc = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        team.name = "Team " + (GameManager._gameManager.teamId);
        _worms = new List<GameObject>();
            for (int i = 0; i < GameManager._gameManager.NumOfWorms; i++){
               
                GameObject go = GameManager._gameManager.
                GenerateWorm(team.transform);
                
                MeshRenderer _mr = go.GetComponent<MeshRenderer>();
                _mr.material.color = tc;
                    
                go.GetComponent<Controllers.CharacterScript>().id =
                GameManager._gameManager.id;
                GameManager._gameManager.id++;
                    
                go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                _worms.Add(go);
            }
    }

    public GameObject GetActiveWorm(){
        return _worms[_activeWorm];
    }



    public void nextWorm(){
        if(_activeWorm == _worms.Count - 1){
            _activeWorm = 0;
        } else {
            _activeWorm++;
        } 
    }
}
}


