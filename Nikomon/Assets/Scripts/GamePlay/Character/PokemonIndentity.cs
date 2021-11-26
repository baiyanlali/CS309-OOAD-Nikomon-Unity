using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonIndentity : MonoBehaviour,IInteractive
{
    enum  AnimMode
    {
        Fight,
        Movement,
        Pet
    }
    
    public Pokemon pokemon;

    public float Time=3f;
    public float MoveRange = 20;
    private float timer = 0f;
    private Animator anim;
    private Rigidbody rigid;
    public void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        // isBattling = false;
    }
    
    
    public void OnInteractive()
    {
        GlobalManager.Instance.StartBattle(pokemon);
        Destroy(this.gameObject);
    }

    public void OnInteractive(GameObject obj)
    {
        GameObject.FindGameObjectWithTag("BattleField").transform.position = obj.transform.position + obj.transform.forward *40f;
        GameObject.FindGameObjectWithTag("BattleField").transform.rotation = obj.transform.rotation;
        GlobalManager.Instance.StartBattle(pokemon);
        Destroy(this.gameObject);
    }


    private void Update()
    {
        if (isBattling) return;
        timer += UnityEngine.Time.deltaTime;

        var result = Physics.Raycast(new Ray(transform.position, Vector3.down), 1);
        if (result)
        {
            
        }
        else
        {
            if (timer > Time)
            {
                Move();
                timer = 0;
            }
        }
        
    }

    public void DoMove(CombatMove move,Action onComplete)
    {
        if (move.move._baseData.Category == Category.Physical)
        {
            anim.SetTrigger(Attack);
        }
        else
        {
            anim.SetTrigger(NoTouchAttack);
        }
    }

    public void BeHit(Action onComplete)
    {
        anim.SetTrigger(BeAttacked);
    }

    public void Faint()
    {
        anim.SetTrigger(Lost);
    }

    private bool isBattling=false;
    private static readonly int BeAttacked = Animator.StringToHash("be_attacked");
    private static readonly int Lost = Animator.StringToHash("lost");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsAppear = Animator.StringToHash("is_appear");
    private static readonly int StartBattle = Animator.StringToHash("startBattle");
    private static readonly int NoTouchAttack = Animator.StringToHash("no_touch_attack");
    private static readonly int Attack = Animator.StringToHash("attack");

    public void InitBattle(bool isAppear)
    {
        // print("Init battle");
        isBattling = true;
        anim = anim ? anim : GetComponentInChildren<Animator>();
        if(isAppear)anim.SetBool(IsAppear,true);
        anim.SetTrigger(StartBattle);
    }

    public void EndBattle()
    {
        isBattling = false;
    }

    public void OnAnimStart()
    {
        
    }

    public void OnAnimEnd()
    {
        // print("Anim End!");
        FindObjectOfType<BattleFieldHandler>().OnAnimEnd();
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

        LeanTween.rotateY(this.gameObject, theta,Time/5f).setOnStart(() =>
        {
            anim.SetBool(IsWalking,true);
        });
        var position = transform.position;

        rigid.velocity = vec.normalized * MoveRange;
        anim.SetBool(IsWalking,true);

    }


    
    
}
