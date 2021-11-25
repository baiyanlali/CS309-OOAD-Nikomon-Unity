using System.Collections.Generic;
using System.Linq;
using GamePlay.StateMachineBehaviours;
using PokemonCore.Utility;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.AssetImporters;
using UnityEngine;

public class PokemonAnimatorAutoBind
{
    private static string[] anims = new[]
    {
        "Fight_appear", "Fight_attack", "Fight_attack_2", "Fight_be_attacked", "Fight_dropping", "Fight_eye_2_emotion",
        "Fight_eye_emotion", "Fight_idle", "Fight_landing", "Fight_lost", "Fight_mouth_emotion",
        "Fight_no_touch_attack", "Fight_no_touch_attack_2", "Fight_release","Fight_release_without_landing", "Movement_eye_2_emotion",
        "Movement_eye_emotion", "Movement_idle", "Movement_mouth_emotion", "Movement_run", "Movement_walk"
    };


    [MenuItem("PokemonTools/Animator Binder")]
    public static void AnimatorAutoBind()
    {
        var obj = Selection.gameObjects;
        foreach (var o in obj)
        {
            o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // AnimatorController anim = AssetImporterEditor.FindObjectOfType<AnimatorController>();
            // Debug.Log(anim.name);
            Bind(o.GetComponentInChildren<Animator>());
        }
    }
    
    [MenuItem("PokemonTools/Clear Animator")]
    public static void ClearAnimatorAutoBind()
    {
        var obj = Selection.gameObjects;
        foreach (var o in obj)
        {
            // AnimatorController anim = AssetImporterEditor.FindObjectOfType<AnimatorController>();
            // Debug.Log(anim.name);
            ClearBind(o.GetComponentInChildren<Animator>());
        }
    }




    public static void ClearBind(Animator anim)
    {
        var runtimeController = anim.runtimeAnimatorController;
        if (runtimeController == null)
        {
            Debug.LogErrorFormat("RuntimeAnimatorController must not be null.");
            return;
        }

        var controller =
            AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(runtimeController));
        if (controller == null)
        {
            Debug.LogErrorFormat("AnimatorController must not be null.");
            return;
        }


        var stateMachine = controller.layers[0].stateMachine;
        var states = stateMachine.states;
        
        
        for (int i = 0; i < states.Length; i++)
        {
            var lll = states[i].state.transitions;
            foreach (var lTransition in lll)
            {
                states[i].state.RemoveTransition(lTransition);
            }
        }

        foreach (var t in stateMachine.anyStateTransitions)
        {
            stateMachine.RemoveAnyStateTransition(t);
        }
        // Debug.Log(controller.parameters.Length);
        controller.parameters = null;

        foreach (var v in stateMachine.states)
        {
            if (v.state.name == "Fight" || v.state.name == "Movement" || v.state.name == "Pet")
            {
                stateMachine.RemoveState(v.state);
            }
        }
        
        // for (int i = 0; i < controller.parameters.Length; i++)
        // {
        //     controller.RemoveParameter(i);
        // }
    }

    private static int block_width = 300;
    private static int block_height = 80;

    public static void Bind(Animator anim)
    {
        var runtimeController = anim.runtimeAnimatorController;
        if (runtimeController == null)
        {
            Debug.LogErrorFormat("RuntimeAnimatorController must not be null.");
            return;
        }

        var controller =
            AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(runtimeController));
        if (controller == null)
        {
            Debug.LogErrorFormat("AnimatorController must not be null.");
            return;
        }


        var stateMachine = controller.layers[0].stateMachine;
        var states = stateMachine.states;
        
        // Debug.Log(controller.layers[0].stateMachine.name);
        Dictionary<string, ChildAnimatorState> animatorStates = new Dictionary<string, ChildAnimatorState>();
        List<string> strs = new List<string>();

        List<ChildAnimatorState> childAnimatorStates = new List<ChildAnimatorState>();

        for (int i = 0; i < states.Length; i++)
        {
            animatorStates.Add(states[i].state.name, states[i]);
            states[i].position = new Vector3(-400, 40*(i+1));
            // Debug.Log(state.state.name);
            strs.Add(states[i].state.name);
        }
        
        
        // Debug.Log(stateMachine.entryPosition);
        // Debug.Log(stateMachine.anyStatePosition);
        // Debug.Log(stateMachine.exitPosition);

        #region auto-bind
        
        
        
        stateMachine.entryPosition=Vector3.zero;
        stateMachine.anyStatePosition=Vector3.down*block_height;
        stateMachine.exitPosition=Vector3.down*block_height*2;
        
        controller.AddParameter("startBattle", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("startMovement", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("startPetting", AnimatorControllerParameterType.Trigger);

        
        
        
        var fightState = stateMachine.AddState("Fight", new Vector3(block_width, 0, 0));
        var movementState =stateMachine.AddState("Movement",new Vector3(block_width,-block_height*4,0));
        var petState =stateMachine.AddState("Pet",new Vector3(block_width,-block_height*8,0));
        
        stateMachine.defaultState=movementState;
        
        
        
        #region build fight state
        
        
        controller.AddParameter("attack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("no_touch_attack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("be_attacked", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("lost", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("is_appear", AnimatorControllerParameterType.Bool);
        
        var trans = stateMachine.AddAnyStateTransition(fightState);
        trans.AddCondition(AnimatorConditionMode.If,0,"startBattle");
        trans.duration = 0;
        trans.hasExitTime = false;

        ChildAnimatorState idle;
        
        if (animatorStates.ContainsKey("Fight_idle"))
        {
            idle  = animatorStates["Fight_idle"];


        }
        else
        {
            idle = new ChildAnimatorState()
            {
                state = new AnimatorState()
                {
                    name = "Fight_idle"
                }
            };
        }
        idle.position = new Vector3(block_width*4, 0);
        childAnimatorStates.Add(idle);
        
        if (strs.Contains("Fight_release_without_landing"))
        {
            var into = animatorStates["Fight_release_without_landing"];

            into.state.AddStateMachineBehaviour<PokemonBattleSMB>();
            
            into.position = new Vector3(block_width*2,0);
            childAnimatorStates.Add(into);

            fightState.speed = 100;
            var tmp = fightState.AddTransition(into.state);
            tmp.hasExitTime =true;
            tmp.exitTime = 0;
            tmp.duration = 0;
            
            tmp.hasExitTime =false;
            tmp.exitTime = 0;
            tmp.duration = 0;
            
            
            
        
            tmp= into.state.AddTransition(idle.state);
            tmp.hasExitTime = true;
        }
        else
        {
            ChildAnimatorState appear;
            ChildAnimatorState release;
            if (animatorStates.ContainsKey("Fight_appear"))
            {
                appear = animatorStates["Fight_appear"];

            }
            else
            {
                appear = new ChildAnimatorState()
                {
                    state = new AnimatorState()
                    {
                        name = "Fight_appear"
                    }
                };
            }
            
            if (animatorStates.ContainsKey("Fight_release"))
            {
                release = animatorStates["Fight_release"];

            }
            else
            {
                release = new ChildAnimatorState()
                {
                    state = new AnimatorState()
                    {
                        name = "Fight_release"
                    }
                };
            }
            


            appear.state.AddStateMachineBehaviour<PokemonBattleSMB>();
            release.state.AddStateMachineBehaviour<PokemonBattleSMB>();

            appear.position = new Vector3(block_width*2, 0);
            release.position = new Vector3(block_width*2, -block_height);
            
            

            childAnimatorStates.Add(appear);
            childAnimatorStates.Add(release);
            
            trans = fightState.AddTransition(appear.state);
            trans.AddCondition(AnimatorConditionMode.If,0,"is_appear");
            trans = fightState.AddTransition(release.state);
            trans.AddCondition(AnimatorConditionMode.IfNot,0,"is_appear");
            
            var drop = animatorStates["Fight_dropping"];
            
        
            drop.position = new Vector3(block_width*3, -block_height);
            
            childAnimatorStates.Add(drop);

            
            trans = release.state.AddTransition(idle.state);
            trans.hasExitTime = true;
            trans = appear.state.AddTransition(idle.state);
            trans.hasExitTime = true;
            // drop.state.AddTransition(idle.state).hasExitTime = true;
        }

        ChildAnimatorState attack;
        ChildAnimatorState no_touch_attack;
        ChildAnimatorState be_attacked;
        ChildAnimatorState lost;
        if (animatorStates.ContainsKey("Fight_attack"))
        {
            attack = animatorStates["Fight_attack"];
        }
        else
        {
            attack = new ChildAnimatorState()
            {
                state = new AnimatorState()
                {
                    name = "Fight_attack"
                }
            };
        }
        
        if (animatorStates.ContainsKey("Fight_no_touch_attack"))
        {
            no_touch_attack = animatorStates["Fight_no_touch_attack"];

        }
        else
        {
            no_touch_attack = new ChildAnimatorState()
            {
                state = new AnimatorState()
                {
                    name = "Fight_no_touch_attack"
                }
            };
        }
        
        
        if (animatorStates.ContainsKey("Fight_be_attacked"))
        {
            be_attacked = animatorStates["Fight_be_attacked"];


        }
        else
        {
            be_attacked = new ChildAnimatorState()
            {
                state = new AnimatorState()
                {
                    name = "Fight_be_attacked"
                }
            };
        }
        
        if (animatorStates.ContainsKey("Fight_lost"))
        {
            lost = animatorStates["Fight_lost"];


        }
        else
        {
            lost = new ChildAnimatorState()
            {
                state = new AnimatorState()
                {
                    name = "Fight_lost"
                }
            };
        }

        

        attack.state.AddStateMachineBehaviour<PokemonBattleSMB>();
        no_touch_attack.state.AddStateMachineBehaviour<PokemonBattleSMB>();
        be_attacked.state.AddStateMachineBehaviour<PokemonBattleSMB>();
        lost.state.AddStateMachineBehaviour<PokemonBattleSMB>();
        
        
        attack.position = new Vector3(block_width*5, 0);
        no_touch_attack.position = new Vector3(block_width*5, -block_height);
        be_attacked.position = new Vector3(block_width*5, -block_height*2);
        lost.position = new Vector3(block_width*5, -block_height*3);
        
        childAnimatorStates.Add(attack);
        childAnimatorStates.Add(no_touch_attack);
        childAnimatorStates.Add(be_attacked);
        childAnimatorStates.Add(lost);
        
        idle.state.AddTransition(attack.state).AddCondition(AnimatorConditionMode.If,0,"attack");
        attack.state.AddTransition(idle.state).hasExitTime = true;
        idle.state.AddTransition(no_touch_attack.state).AddCondition(AnimatorConditionMode.If,0,"no_touch_attack");
        no_touch_attack.state.AddTransition(idle.state).hasExitTime = true;
        idle.state.AddTransition(be_attacked.state).AddCondition(AnimatorConditionMode.If,0,"be_attacked");
        be_attacked.state.AddTransition(idle.state).hasExitTime = true;
        idle.state.AddTransition(lost.state).AddCondition(AnimatorConditionMode.If,0,"lost");

        lost.state.AddExitTransition().hasExitTime=true;
        
        #endregion

        if (animatorStates.ContainsKey("Movement_idle"))
        {
            
        idle = animatorStates["Movement_idle"];
        idle.position = new Vector3(block_width*2,-block_height*4);
        
        childAnimatorStates.Add(idle);
        
        #region build movement state
        
        // "Movement_idle", "Movement_run", "Movement_walk";
        trans = stateMachine.AddAnyStateTransition(movementState);
        trans.AddCondition(AnimatorConditionMode.If,1,"startMovement");
        trans.duration = 0;
        trans.hasExitTime = false;
        
        controller.AddParameter("isWalking", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isRunning", AnimatorControllerParameterType.Bool);
        
        trans = movementState.AddTransition(idle.state);
        trans.exitTime = 0;
        trans.hasExitTime = true;
        
        var walk = animatorStates["Movement_walk"];
        var run = animatorStates["Movement_run"];
        
        walk.position = new Vector3(block_width*3, -block_height*4);
        run.position = new Vector3(block_width*3, -block_height*4);
        
        childAnimatorStates.Add(walk);
        childAnimatorStates.Add(run);
        
        idle.state.AddTransition(walk.state).AddCondition(AnimatorConditionMode.If,1,"isWalking");
        idle.state.AddTransition(run.state).AddCondition(AnimatorConditionMode.If,1,"isRunning");
        
        walk.state.AddTransition(idle.state).AddCondition(AnimatorConditionMode.IfNot,1,"isWalking");
        run.state.AddTransition(idle.state).AddCondition(AnimatorConditionMode.IfNot,1,"isRunning");
        
        #endregion
        
        }

        
        #endregion

        int idx = 0;
        foreach (var ssss in stateMachine.states)
        {
            if (ssss.state.name == "Fight" || ssss.state.name == "Movement" || ssss.state.name == "Pet")
            {
                childAnimatorStates.Add(ssss);
                animatorStates[ssss.state.name] = ssss;
                continue;
            }

            var lis = (from ccc in childAnimatorStates select ccc.state.name).ToList();
            if (!lis.Contains(ssss.state.name))
            {
                ChildAnimatorState ca = ssss;
                ca.position = new Vector3(-block_width, -block_height * idx *0.5f);
                childAnimatorStates.Add(ca);
                idx++;
            }
        }

        stateMachine.states = childAnimatorStates.ToArray();
        
        
        EditorUtility.SetDirty(anim);
        EditorUtility.SetDirty(controller);
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        // stateMachine.AddAnyStateTransition()
        Debug.Log("Bind successfully!");

        // controller.layers[0].stateMachine.AddAnyStateTransition()

        // if (anim == null) return;
        // RuntimeAnimatorController ra = anim.runtimeAnimatorController;
        // if (ra == null) return;
    }
}