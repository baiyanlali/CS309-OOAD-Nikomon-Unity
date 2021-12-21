using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Used to control battle animation used by timeline.
/// </summary>
public class BattleTimelineHandler : MonoBehaviour
{
    private BattleFieldHandler _fieldHandler;
    public PlayableDirector Director;
    private void Start()
    {
        _fieldHandler = GetComponent<BattleFieldHandler>();
        Director = Director==null?GetComponent<PlayableDirector>():Director;
        _fieldHandler.OnHitAnim = HitAnim;
    }

    private Damage dmg;

    public void HitAnim(Damage dmg)
    {
        this.dmg = dmg;
        Director.Play();
    }

    public void MoveEnd()
    {
        _fieldHandler.MoveAnimEnd();
    }

    public void Sponsor()
    {
        _fieldHandler.FieldPokemonIndentities[dmg.sponsor.CombatID].DoMove(dmg.combatMove,null);
    }

    public void Target()
    {
        _fieldHandler.FieldPokemonIndentities[dmg.target.CombatID].BeHit(null);
    }
    
}
