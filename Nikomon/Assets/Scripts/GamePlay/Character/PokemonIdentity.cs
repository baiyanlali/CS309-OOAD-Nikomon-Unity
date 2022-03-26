using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Character;
using GamePlay.Utilities;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonIdentity : MonoBehaviour, IInteractive
{
    enum AnimMode
    {
        Fight,
        Movement,
        Pet
    }

    public Pokemon pokemon;

    public float Time = 3f;
    public float MoveRange = 2;
    private float timer = 0f;
    private Animator anim;
    private Rigidbody rigid;

    public void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        // isBattling = false;
    }


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Player>()!=null)
            OnInteractive(other.gameObject);
    }

    public void OnInteractive()
    {
        GlobalManager.Instance.StartBattle(pokemon);
        Destroy(this.gameObject);
    }

    public void OnInteractive(GameObject obj)
    {
        GameObject.FindGameObjectWithTag("BattleField").transform.position =
            obj.transform.position + obj.transform.forward * 4f;
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

    private Dictionary<string, Action> completeList=new Dictionary<string, Action>();
    public void DoMove(CombatMove move, Action onComplete)
    {
        if (move == null)
        {
            onComplete?.Invoke();
            return;
        }
        if (move.move._baseData.Category == Category.Physical)
        {
            anim.SetTrigger(Attack);
            // if (completeList.ContainsKey("Fight_attack"))
            // {
            //     completeList["Fight_attack"]?.Invoke();
            // }
            // completeList.AddOrReplace("Fight_attack",onComplete);
            completeList.AddAndUseIfHas("Fight_attack",onComplete);
        }
        else
        {
            anim.SetTrigger(NoTouchAttack);
            //TODO: May cause twice
            // if (completeList.ContainsKey("Fight_attack"))
            // {
            //     completeList["Fight_attack"]?.Invoke();
            // }
            // completeList.AddOrReplace("Fight_attack",onComplete);
            completeList.AddAndUseIfHas("Fight_attack",onComplete);
            // completeList.Add("Fight_attack",onComplete);
        }
    }

    public void BeHit(Action onComplete)
    {
        anim.SetTrigger(BeAttacked);
        // if (completeList.ContainsKey("Fight_be_attacked"))
        // {
        //     completeList["Fight_be_attacked"]?.Invoke();
        // }
        // completeList.AddOrReplace("Fight_attack",onComplete);
        completeList.AddAndUseIfHas("Fight_be_attacked",onComplete);
        // completeList.Add("Fight_be_attacked",onComplete);
    }

    public void Faint(Action onComplete)
    {
        anim.SetTrigger(Lost);
        completeList.AddAndUseIfHas("Fight_lost",onComplete);
        // completeList.Add("Fight_lost",onComplete);
        LeanTween.scale(this.gameObject, Vector3.zero, 1.5f).setOnComplete(onComplete);
    }

    private bool isBattling = false;
    public static readonly int BeAttacked = Animator.StringToHash("be_attacked");
    public static readonly int Lost = Animator.StringToHash("lost");
    public static readonly int IsWalking = Animator.StringToHash("isWalking");
    public static readonly int IsAppear = Animator.StringToHash("is_appear");
    public static readonly int StartBattle = Animator.StringToHash("startBattle");
    public static readonly int NoTouchAttack = Animator.StringToHash("no_touch_attack");
    public static readonly int Attack = Animator.StringToHash("attack");

    public static Dictionary<int, string> HashToAnimName = new Dictionary<int, string>()
    {
        {Animator.StringToHash("Fight_appear"),         "Fight_appear"},
        {Animator.StringToHash("Fight_attack"),         "Fight_attack"},
        {Animator.StringToHash("Fight_attack_2"),       "Fight_attack_2"},
        {Animator.StringToHash("Fight_be_attacked"),    "Fight_be_attacked"},
        {Animator.StringToHash("Fight_dropping"),       "Fight_dropping"},
        {Animator.StringToHash("Fight_eye_2_emotion"),  "Fight_eye_2_emotion"},
        {Animator.StringToHash("Fight_eye_emotion"),    "Fight_eye_emotion"},
        {Animator.StringToHash("Fight_idle"),       "Fight_idle"},
        {Animator.StringToHash("Fight_landing"),        "Fight_landing"},
        {Animator.StringToHash("Fight_lost"),       "Fight_lost"},
        {Animator.StringToHash("Fight_mouth_emotion"),      "Fight_mouth_emotion"},
        {Animator.StringToHash("Fight_no_touch_attack"),        "Fight_no_touch_attack"},
        {Animator.StringToHash("Fight_no_touch_attack_2"),      "Fight_no_touch_attack_2"},
        {Animator.StringToHash("Fight_release"),        "Fight_release"},
        {Animator.StringToHash("Fight_release_without_landing"),        "Fight_release_without_landing"},
        {Animator.StringToHash("Movement_eye_2_emotion"),       "Movement_eye_2_emotion"},
        {Animator.StringToHash("Movement_eye_emotion"),         "Movement_eye_emotion"},
        {Animator.StringToHash("Movement_idle"),        "Movement_idle"},
        {Animator.StringToHash("Movement_mouth_emotion"),       "Movement_mouth_emotion"},
        {Animator.StringToHash("Movement_run"),         "Movement_run"},
        {Animator.StringToHash("Movement_walk")     , "Movement_walk"}
    };

    public void InitBattle(bool isAppear)
    {
        // print("Init battle");
        isBattling = true;
        anim = anim ? anim : GetComponentInChildren<Animator>();
        //有些宝可梦只有release状态
        // rigid.freezeRotation = true;
        anim.SetBool(IsAppear, false);
        anim.SetTrigger(StartBattle);
        
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }

    public void EndBattle()
    {
        isBattling = false;
    }

    public void OnAnimStart()
    {
    }

    public void OnAnimEnd(int shortNameHash)
    {
        // print("Anim End!");
        string anim = HashToAnimName[shortNameHash]=="Fight_no_touch_attack"?"Fight_attack":HashToAnimName[shortNameHash];
        if (completeList.TryGetValue(anim, out Action oncomplete))
        {
            print($"Anim:{anim} has completed");
            oncomplete?.Invoke();
            completeList.Remove(anim);
        }
        // FindObjectOfType<BattleFieldHandler>().OnAnimEnd(HashToAnimName[shortNameHash]);
    }

    private void Move()
    {
        Vector3 vec = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));

        // transform.LookAt(transform.position+vec);

        float theta = 0;
        if (vec.z > 0)
            theta = Mathf.Atan(vec.x / vec.z);
        else if (vec.z < 0)
            theta = Mathf.Atan(vec.x / vec.z) + Mathf.PI;
        else
        {
            theta = vec.x > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
        }

        theta = Mathf.Rad2Deg * theta;

        LeanTween.rotateY(this.gameObject, theta, Time / 5f).setOnStart(() => { anim.SetBool(IsWalking, true); });
        var position = transform.position;

        rigid.velocity = vec.normalized * MoveRange;
        anim.SetBool(IsWalking, true);
    }
}