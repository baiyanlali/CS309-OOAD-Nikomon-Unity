using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace GamePlay.Plot
{
    public class YarnControlScript : MonoBehaviour
    {
        private static List<YarnProject> _programs = new List<YarnProject>();

        public YarnProject YarnProject;
        public string StartNode;
        protected virtual void Start()
        {
        
            // var runner = GameManager.Instance.dialogueRunner;
            // if (runner == null)
            // {
            //     Debug.LogError("No dialogue runner found");
            // }
            //
            //
            // if (YarnProject == null) return;
            //
            // if (_programs.Contains(YarnProject))
            // {
            //     return;
            // }
            //
            // //useless code just to init dialogue
            // // var dialogue = runner.Dialogue;
            //
            // _programs.Add(YarnProject);
            //
            // runner.Add(YarnProject);
        

        }


        public void ShowDialogue()
        {
            // if (string.IsNullOrEmpty(StartNode)) return;
            // var runner = GameManager.Instance.dialogueRunner;
            // runner.StartDialogue(StartNode);
        }
    
        public void ShowDialogue(string str)
        {
            // if (string.IsNullOrEmpty(str)) return;
            // var runner = GameManager.Instance.dialogueRunner;
            // runner.StartDialogue(str);
        }
    }
}