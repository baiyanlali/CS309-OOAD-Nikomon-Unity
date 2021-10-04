using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonIndentity : MonoBehaviour,IInteractive
{
    public Pokemon pokemon;

    public float Time=3f;
    private float timer = 0f;
    
    public void OnInteractive()
    {
        GlobalManager.Instance.StartBattle(pokemon);
        Destroy(this.gameObject);
    }


    private void Update()
    {
        timer += UnityEngine.Time.deltaTime;
        if (timer > Time)
        {
            Move();
            timer = 0;
        }
    }

    private void Move()
    {
        Vector3 vec = new Vector3(Random.Range(-10f, 10f),0, Random.Range(-10f, 10f));
        
        // transform.LookAt(transform.position+vec);
        
        float theta = 0;
        if(vec.z>0)
            theta = Mathf.Atan(vec.x / vec.z);
        else if (vec.z < 0)
            theta = Mathf.Atan(vec.x / vec.z) + Mathf.PI;
        else
        {
            theta = vec.x > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
        }
        theta = Mathf.Rad2Deg * theta;

        LeanTween.rotateY(this.gameObject, theta,Time/5f);
        // transform.position += vec;
        LeanTween.move(this.gameObject,transform.position+vec, Time);
    }
    
}
