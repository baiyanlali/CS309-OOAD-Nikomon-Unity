using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using GamePlay;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = System.Object;


public class BattleFieldHandler : MonoBehaviour
{
    [SerializeField] private PlayableDirector Director;

    public struct TimeSequence
    {
        public enum SequenceTag
        {
            OnMove,
            BeHit,
            Capture,
            BeCaptured,
            Fainted,
            UseItem,
            UpdateState
        }

        public CombatPokemon poke;
        public int hp;
        public SequenceTag tag;
        public Object[] param;

        public TimeSequence(CombatPokemon poke, SequenceTag tag, params Object[] objs)
        {
            this.poke = poke;
            this.hp = poke.HP;
            this.tag = tag;
            param = objs;
        }
    }

    public CinemachineTargetGroup TargetGroup;
    public CinemachineVirtualCamera Camera;

    public float padding = 15;
    public Transform allyPosition;
    public Transform oppoPosition;


    public static BattleFieldHandler Instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = FindObjectOfType<BattleFieldHandler>();
            }

            if (sInstance == null)
            {
                CreateBattleFieldHandler();
            }

            return sInstance;
        }
    }

    private static BattleFieldHandler sInstance;

    private static void CreateBattleFieldHandler()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/BattleField");
        var battle = Instantiate(obj);
        sInstance = battle.GetComponent<BattleFieldHandler>();
    }

    private Dictionary<int, PokemonIndentity> dics;

    public void Init(List<CombatPokemon> allies, List<CombatPokemon> oppos)
    {
        //先暂时不用这种方法
        Director = GetComponent<PlayableDirector>();
        TimeSequences = new Queue<TimeSequence>();

        dics = new Dictionary<int, PokemonIndentity>();
        Camera.Priority = 12;
        for (int i = 0; i < allies.Count; i++)
        {
            var allyID = allies[i].pokemon.ID;

            float offset = CalculatePointPosition(allies.Count, i + 1, padding);
            GameObject obj = null;
            if (GameResources.Pokemons[allyID].Length == 1)
            {
                obj = Instantiate(GameResources.Pokemons[allyID][0], allyPosition);
            }
            else if (GameResources.Pokemons[allyID].Length == 2)
            {
                if (allies[i].pokemon.isMale)
                {
                    obj = Instantiate(GameResources.Pokemons[allyID][0], allyPosition);
                }
                else
                {
                    obj = Instantiate(GameResources.Pokemons[allyID][1], allyPosition);
                }
            }

            PokemonIndentity indentity = obj.AddComponent<PokemonIndentity>();
            indentity.InitBattle(false);
            dics.Add(allies[i].CombatID, indentity);
            obj.transform.localPosition = Vector3.right * offset;
            TargetGroup.AddMember(obj.transform, 1, 5);
        }

        for (int i = 0; i < oppos.Count; i++)
        {
            var oppoID = oppos[i].pokemon.ID;

            float offset = CalculatePointPosition(oppos.Count, oppos.Count - i, padding);
            GameObject obj = null;
            if (GameResources.Pokemons[oppoID].Length == 1)
            {
                obj = Instantiate(GameResources.Pokemons[oppoID][0], oppoPosition);
            }
            else if (GameResources.Pokemons[oppoID].Length == 2)
            {
                if (oppos[i].pokemon.isMale)
                {
                    obj = Instantiate(GameResources.Pokemons[oppoID][0], oppoPosition);
                }
                else
                {
                    obj = Instantiate(GameResources.Pokemons[oppoID][1], oppoPosition);
                }
            }

            var identity = obj.AddComponent<PokemonIndentity>();
            identity.InitBattle(true);
            dics.Add(oppos[i].CombatID, identity);
            obj.transform.localPosition = Vector3.right * offset;
            TargetGroup.AddMember(obj.transform, 1, 5);
        }
    }

    private Queue<TimeSequence> TimeSequences;

    public void OnMove(CombatMove move)
    {
        // dics[move.Sponsor.CombatID].DoMove(move,null);
        // foreach (var pokes in move.Targets)
        // {
        //     dics[pokes].BeHit(null);
        // }
    }

    public void OnHit(Damage dmg)
    {
        TimeSequences.Enqueue(new TimeSequence(dmg.sponsor, TimeSequence.SequenceTag.OnMove, dmg.combatMove));

        TimeSequences.Enqueue(new TimeSequence(dmg.target, TimeSequence.SequenceTag.BeHit));
    }

    public void OnHitted(CombatPokemon poke)
    {
        TimeSequences.Enqueue(new TimeSequence(poke, TimeSequence.SequenceTag.UpdateState, poke.HP));
    }

    public void OnTurnEnd()
    {
        DoNextSequence();
    }

    public void DoNextSequence()
    {
        if (TimeSequences.Count == 0) return;
        print(TimeSequences.Peek().tag);
        // if (TimeSequences.Peek().tag != TimeSequence.SequenceTag.BeHit)
        // {
            var sequence = TimeSequences.Dequeue();
            if (sequence.tag == TimeSequence.SequenceTag.OnMove)
            {
                dics[sequence.poke.CombatID].DoMove(sequence.param[0] as CombatMove, null);
            }
            else if (sequence.tag == TimeSequence.SequenceTag.UpdateState)
            {
                BattleUIHandler.Instance.UpdateStatus(sequence.poke, (int) sequence.param[0],DoNextSequence);
            }else if (sequence.tag == TimeSequence.SequenceTag.BeHit)
            {
                dics[sequence.poke.CombatID].BeHit(null);
            }
        // }
        // else
        // {
        //     do
        //     {
        //         var sequence = TimeSequences.Dequeue();
        //         dics[sequence.poke.CombatID].BeHit(null);
        //     } while (TimeSequences.Count != 0 && TimeSequences.Peek().tag == TimeSequence.SequenceTag.BeHit);
        // }
    }

    public void OnAnimEnd()
    {
        DoNextSequence();
    }

    public void OnPokemonFainting(CombatPokemon poke)
    {
        dics[poke.CombatID].Faint();
    }

    public void OnReplacePokemon(CombatPokemon p1, CombatPokemon p2)
    {
        Transform trans = dics[p1.CombatID].transform;
        int id = p2.pokemon.ID;
        GameObject obj = null;

        if (GameResources.Pokemons[id].Length == 1)
        {
            obj = Instantiate(GameResources.Pokemons[id][0], trans.parent);
        }
        else if (GameResources.Pokemons[id].Length == 2)
        {
            if (p2.pokemon.isMale)
            {
                obj = Instantiate(GameResources.Pokemons[id][0], trans.parent);
            }
            else
            {
                obj = Instantiate(GameResources.Pokemons[id][1], trans.parent);
            }
        }

        obj.transform.position = trans.position;
        obj.transform.rotation = trans.rotation;
        TargetGroup.RemoveMember(trans);
        TargetGroup.AddMember(obj.transform, 1, 5);

        dics.Remove(p1.CombatID);
        PokemonIndentity indentity = obj.AddComponent<PokemonIndentity>();
        indentity.InitBattle(false);
        dics.Add(p2.CombatID, indentity);
        Destroy(trans.gameObject);
    }

    public void EndBattle()
    {
        StartCoroutine(EndBattling());
    }

    private IEnumerator EndBattling()
    {
        Camera.Priority = 9;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < allyPosition.childCount; i++)
        {
            var a = allyPosition.GetChild(i);
            TargetGroup.RemoveMember(a);
            Destroy(a.gameObject);
        }

        for (int i = 0; i < oppoPosition.childCount; i++)
        {
            var a = oppoPosition.GetChild(i);
            TargetGroup.RemoveMember(a);
            Destroy(a.gameObject);
        }

        dics.Clear();
    }

    /// <summary>
    /// 用来计算n个点时，第k个点距离中心点的距离，k从范围[1,n]
    /// </summary>
    /// <param name="n">一共n个点</param>
    /// <param name="k">计算第k个点</param>
    /// <param name="padding">点之间的间距</param>
    /// <returns></returns>
    private float CalculatePointPosition(int n, int k, float padding)
    {
        if (k > n) throw new Exception("Cannot find a point k more than n");
        return padding * (k - 1) - padding * (n - 1) / 2.0f;
        // return 0;
    }
}