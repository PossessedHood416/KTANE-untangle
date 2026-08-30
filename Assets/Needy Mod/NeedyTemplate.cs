using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class NeedyTemplate : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMNeedyModule Needy;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      Needy.OnNeedyActivation += OnNeedyActivation;
      Needy.OnNeedyDeactivation += OnNeedyDeactivation;
      Needy.OnTimerExpired += OnTimerExpired;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      //button.OnInteract += delegate () { buttonPress(); return false; };

   }

   void OnDestroy () { //Shit you need to do when the bomb ends
      
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   protected void OnNeedyActivation () { //Shit that happens when a needy turns on.

   }

   protected void OnNeedyDeactivation () { //Shit that happens when a needy turns off.
      Needy.OnPass();
   }

   protected void OnTimerExpired () { //Shit that happens when a needy turns off due to running out of time.

   }

   void Start () { //Shit that you calculate, usually a majority if not all of the module
      Needy.SetResetDelayTime(30f, 50f);
   }

   void Update () { //Shit that happens at any point after initialization

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

   void TwitchHandleForcedSolve () { //Void so that autosolvers go to it first instead of potentially striking due to running out of time.
      StartCoroutine(HandleAutosolver());
   }

   IEnumerator HandleAutosolver () {
      yield return null;
   }
}
