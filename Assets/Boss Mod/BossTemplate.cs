using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class BossTemplate : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   //Boss mod shit
   public static string[] ignoredModules = null;
   int ModCount = 8008135;
   int Stage;
   bool WaitForModCount;
   /*--- MAKE SURE TO ADD KM BOSS MODULE AS A COMPONENT TO YOUR MODULE FOR THIS TO WORK ---*/

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      //button.OnInteract += delegate () { buttonPress(); return false; };

      if (ignoredModules == null) {
         ignoredModules = GetComponent<KMBossModule>().GetIgnoredModules("REPLACE THIS WITH YOUR MOD NAME REPLACE THIS WITH YOUR MOD NAME REPLACE THIS WITH YOUR MOD NAME REPLACE THIS WITH YOUR MOD NAME REPLACE THIS WITH YOUR MOD NAME ", new string[] {
                "14",
                "42",
                "501",
                "A>N<D",
                "Bamboozling Time Keeper",
                "Black Arrows",
                "Brainf---",
                "The Board Walk",
                "Busy Beaver",
                "Don't Touch Anything",
                "Floor Lights",
                "Forget Any Color",
                "Forget Enigma",
                "Forget Ligma",
                "Forget Everything",
                "Forget Infinity",
                "Forget It Not",
                "Forget Maze Not",
                "Forget Me Later",
                "Forget Me Not",
                "Forget Perspective",
                "Forget The Colors",
                "Forget Them All",
                "Forget This",
                "Forget Us Not",
                "Iconic",
                "Keypad Directionality",
                "Kugelblitz",
                "Multitask",
                "OmegaDestroyer",
                "OmegaForest",
                "Organization",
                "Password Destroyer",
                "Purgatory",
                "Reporting Anomalies",
                "RPS Judging",
                "Security Council",
                "Shoddy Chess",
                "Simon Forgets",
                "Simon's Stages",
                "Souvenir",
                "Speech Jammer",
                "Tallordered Keys",
                "The Time Keeper",
                "Timing is Everything",
                "The Troll",
                "Turn The Key",
                "The Twin",
                "Übermodule",
                "Ultimate Custom Night",
                "The Very Annoying Button",
                "Whiteout"
            });
      }
   }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   void Start () { //Shit
      WaitForModCount = true;
   }

   void Update () { //Shit that happens at any point after initialization
      if (ModuleSolved || !WaitForModCount) {
         return;
      }

      ModCount = Bomb.GetSolvableModuleNames().Count(x => !ignoredModules.Contains(x));

      int Solved = Bomb.GetSolvedModuleNames().Count(x => !ignoredModules.Contains(x));



      if (Solved == ModCount) { //Do input phase or something here
         return;
      }
      if (Solved > Stage) { //Put whatever your mod is supposed to do after a solve here. If you want a delay of solves for the purposes of TP, make it a coroutine.

         Debug.Log(Stage); //Stage is 0 indexed, so adjust what you need for your specific circumstances.
         Stage++;
      }
   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
