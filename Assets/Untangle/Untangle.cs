using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using DeafMath = ExMath;

public class Untangle : MonoBehaviour {

	public KMBombInfo Bomb;
	public KMAudio Audio;

	static int ModuleIdCounter = 1;
	int ModuleId;	
	private bool ModuleSolved;

	public LineRenderer ExampleLine;
	public GameObject LineParent;
	public KMSelectable[] PegGrid;

	private List<Line> LineBag = new List<Line>();
	private int LiftedPeg = -1;
	private bool isReady = false;
	private int[] PegPositionLookup = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14, 15,16,17,18,19, 20,21,22,23,24 };

	public class Line {
		public static LineRenderer baseline;
		public static GameObject parent;
		public int xa, ya, xb, yb;
		public KMSelectable parentA = null;
		public KMSelectable parentB = null;
		public LineRenderer linerend;
		public Line(int x1, int y1, int x2, int y2){
			if(x1 == x2 && y1 == y2){
				x2 = x1 + Rnd.Range(1,4);
				y2 = y1 + Rnd.Range(1,5);
				x2 = x2 > 1 ? x2 - 4 : x2;
				y2 = y2 > 2 ? y2 - 5 : y2;
			}
			xa = x1; ya = y1;
			xb = x2; yb = y2;
		}

		public void Activate(bool active = true){
			linerend = Instantiate(baseline, parent.transform);
			linerend.SetPositions(new Vector3[] { new Vector3(xa,0,ya), new Vector3(xb,0,yb)});
			linerend.gameObject.SetActive(true);
		}

		public override string ToString() {
			return String.Format("{0}{1}-{2}{3}", "ABCDE"[xa+2], 3-ya, "ABCDE"[xb+2], 3-yb);
		} 

		public bool onSegment(List<int> p, List<int> q, List<int> r) {
			if(q[0] == p[0] && q[1] == p[1]) return false;
			if(q[0] == r[0] && q[1] == r[1]) return false;
			return (q[0] <= Math.Max(p[0], r[0]) && 
					q[0] >= Math.Min(p[0], r[0]) &&
					q[1] <= Math.Max(p[1], r[1]) && 
					q[1] >= Math.Min(p[1], r[1]));
		}

		public int orientation(List<int> p, List<int> q, List<int> r) {
			// function to find orientation of ordered triplet (p, q, r)
			// 0 --> p, q and r are collinear
			// 1 --> Clockwise
			// 2 --> Counterclockwise
			int val = (q[1] - p[1]) * (r[0] - q[0]) -
					  (q[0] - p[0]) * (r[1] - q[1]);

			// collinear
			if (val == 0) return 0;

			// clock or counterclock wise
			// 1 for clockwise, 2 for counterclockwise
			return (val > 0) ? 1 : 2;
		}

		public bool checkCollision(Line a, Line b) {
			// function to check if two line segments intersect

			List<List<List<int>>> points = new List<List<List<int>>> {
				new List<List<int>> {	new List<int> {a.xa, a.ya}, 
										new List<int> {a.xb, a.yb} },
				new List<List<int>> {	new List<int> {b.xa, b.ya}, 
										new List<int> {b.xb, b.yb} }
			};

			// find the four orientations needed
			// for general and special cases
			int o1 = orientation(points[0][0], points[0][1], points[1][0]);
			int o2 = orientation(points[0][0], points[0][1], points[1][1]);
			int o3 = orientation(points[1][0], points[1][1], points[0][0]);
			int o4 = orientation(points[1][0], points[1][1], points[0][1]);


			//matching point edgecases
			if((points[0][0][0] == points[1][0][0] && points[0][0][1] == points[1][0][1]) &&
				(points[0][1][0] == points[1][1][0] && points[0][1][1] == points[1][1][1])) return true;
			if ((points[0][1][0] == points[1][0][0] && points[0][1][1] == points[1][0][1]) &&
				(points[0][0][0] == points[1][1][0] && points[0][0][1] == points[1][1][1])) return true;

			if (((o1 == 0) ^ (o2 == 0)) && ((o3 == 0) ^ (o4 == 0))) return false;

			// general case
			if (o1 != o2 && o3 != o4) return true;

			// special cases
			// p1, q1 and p2 are collinear and p2 lies on segment p1q1

			if (o1 == 0 &&
			onSegment(points[0][0], points[1][0], points[0][1])) return true;

			// p1, q1 and q2 are collinear and q2 lies on segment p1q1
			if (o2 == 0 &&
			onSegment(points[0][0], points[1][1], points[0][1])) return true;

			// p2, q2 and p1 are collinear and p1 lies on segment p2q2
			if (o3 == 0 &&
			onSegment(points[1][0], points[0][0], points[1][1])) return true;

			// p2, q2 and q1 are collinear and q1 lies on segment p2q2 
			if (o4 == 0 &&
			onSegment(points[1][0], points[0][1], points[1][1])) return true;

			return false;
		}

		public void AttemptHookParent(KMSelectable p, int i){ //just fucking take this out of line class istj
			if(xa == (i%5)-2 && ya == 2-(i/5)) {
				parentA = p;
				linerend.startColor = p.GetComponent<Renderer>().material.color;
			} else
			if(xb == (i%5)-2 && yb == 2-(i/5)) {
				parentB = p;
				linerend.endColor = p.GetComponent<Renderer>().material.color;
			} else return;
		}
	}

	void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
		ModuleId = ModuleIdCounter++;
		GetComponent<KMBombModule>().OnActivate += Activate;
		
		foreach (KMSelectable peg in PegGrid) {
			peg.OnInteract += delegate () { PegPress(peg); return false; };
		}

		//button.OnInteract += delegate () { buttonPress(); return false; };

	}

	void OnDestroy () { //Shit you need to do when the bomb ends
		
	}

	void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

	}

	void Start () { //Shit that you calculate, usually a majority if not all of the module
		Line.baseline = ExampleLine; Line.parent = LineParent;

		for(int i = 0; i < 25; i++){
			ColourizePeg(PegGrid[i]);
		}

		LineBag.Add(new Line(Rnd.Range(-2, 2),Rnd.Range(-2, 3),Rnd.Range(-2, 2),Rnd.Range(-2, 3)));
		int attemptCount = 0;
		
		while(attemptCount < 300){
			Line contender = new Line(Rnd.Range(-2, 2),Rnd.Range(-2, 3),Rnd.Range(-2, 2),Rnd.Range(-2, 3));
			bool valid = true;

			for(int i = 0; i < LineBag.Count; i++){
				if(contender.checkCollision(LineBag[i], contender)) valid = false;
			}

			if(!valid){ 
				attemptCount++;
				continue;
			}

			LineBag.Add(contender);
			attemptCount = 0;
		}


		Debug.LogFormat("[Untangle #{0}] Generating an untangle with {1} lines. Solution is as follows.", ModuleId, LineBag.Count);
		foreach (Line l in LineBag){
			l.Activate();
			Debug.LogFormat("[Untangle #{0}] Line: {1}.", ModuleId, l);
			for (int i = 0; i < 25; i++){
				l.AttemptHookParent(PegGrid[i], i);
			}
			StartCoroutine(WatchParent(l));
		}

		attemptCount = 0;
		while (attemptCount < 300) {
			attemptCount++;
			HandleSwap(Rnd.Range(0, 25), Rnd.Range(0, 25));
		}

		isReady = true;
	}


	void Update () { //Shit that happens at any point after initialization

	}

	void Solve () {
		ModuleSolved = true;
		Audio.PlaySoundAtTransform("Forgetting How To Dream", transform);
		for(int i = 0; i < 25; i++) {
			StartCoroutine(SolvePegAni(i));
		}
	}

	void Strike () {
		GetComponent<KMBombModule>().HandleStrike();
	}

	void PegPress(KMSelectable peg){
		int i = 0;

		for (i = 0; i < 25; i++) if (PegGrid[i] == peg) break;

		if(PegGrid[i].transform.localPosition.y != 0f && PegGrid[i].transform.localPosition.y != 2f ) return;

		if(LiftedPeg == -1){
			StartCoroutine(LiftPeg(PegGrid[i]));
			LiftedPeg = i;
		} else if(LiftedPeg == i){
			StartCoroutine(LiftPeg(PegGrid[i], false));
			LiftedPeg = -1;
		} else {
			HandleSwap(LiftedPeg, i);
		}

	}

	void ColourizePeg(KMSelectable p){
		p.GetComponent<Renderer>().material.color = new Color(Rnd.Range(0,4)/3f, Rnd.Range(0,4)/3f, Rnd.Range(0,4)/3f);
	}

	void HandleSwap(int i1, int i2) {
		int temp = PegPositionLookup[i1];
		PegPositionLookup[i1] = PegPositionLookup[i2];
		PegPositionLookup[i2] = temp;

		int x1 = (PegPositionLookup[i1] % 5)-2;
		int y1 = 2-(PegPositionLookup[i1] / 5);
		int x2 = (PegPositionLookup[i2] % 5)-2;
		int y2 = 2-(PegPositionLookup[i2] / 5);

		foreach (Line l in LineBag) { 
			if (l.xa == x1 && l.ya == y1) { l.xa = x2; l.ya = y2; }
			else if (l.xa == x2 && l.ya == y2) { l.xa = x1; l.ya = y1; }
			if (l.xb == x1 && l.yb == y1) { l.xb = x2; l.yb = y2; }
			else if (l.xb == x2 && l.yb == y2) { l.xb = x1; l.yb = y1; }
		}

		StartCoroutine(SwapPeg(PegGrid[i1], PegGrid[i2]));
		LiftedPeg = -1;
		
		if (!isReady || ModuleSolved) return;
		CheckSolve();
	}

	void CheckSolve(){
		foreach (Line l in LineBag) {
			foreach (Line m in LineBag) {
				if (l != m && l.checkCollision(l, m)) {
					return;
				}
			}
		}

		Solve();
		return;
	}

	IEnumerator LiftPeg(KMSelectable p, bool forwards = true){
		Vector3 vpos = p.transform.localPosition;
		for(int i = 0; i < 10; i++){
			vpos.y = forwards ? sigmoidLerp(i/10f)*2 : 2 - sigmoidLerp(i/10f)*2;
			p.transform.localPosition = vpos;
			yield return null;
		}
		vpos.y = forwards ? 2f : 0f;
		p.transform.localPosition = vpos;
	}

	IEnumerator SwapPeg(KMSelectable p, KMSelectable q) {
		Vector3 ppos = p.transform.localPosition;
		Vector3 qpos = q.transform.localPosition;

		if(!isReady){
			p.transform.localPosition = qpos;
			q.transform.localPosition = ppos;
			yield break;
		}

		Vector3 pposNew = ppos;
		Vector3 qposNew = qpos;
		pposNew.y = 0f; 
		qposNew.y = 0f;
		float slurp;

		for (int i = 0; i < 10; i++) {
			slurp = sigmoidLerp(i/10f);
			p.transform.localPosition = Vector3.Lerp(ppos, qposNew, slurp);
			q.transform.localPosition = Vector3.Lerp(qpos, pposNew, slurp);
			yield return null;
		}
		p.transform.localPosition = qposNew;
		q.transform.localPosition = pposNew;
	}

	IEnumerator WatchParent(Line l){
		int ticker = 0;
		while (ticker <= 800){
			l.linerend.SetPositions(new Vector3[] { l.parentA.transform.localPosition, l.parentB.transform.localPosition });
			if (ModuleSolved) {
				l.linerend.startColor = l.parentA.GetComponent<Renderer>().material.color;
				l.linerend.endColor = l.parentB.GetComponent<Renderer>().material.color;
				ticker++;
			}
			yield return null;
		}
	}

	IEnumerator SolvePegAni(int p) {
		yield return new WaitForSeconds(Rnd.Range(0.5f, 2.5f));
		Color start = PegGrid[p].GetComponent<Renderer>().material.color;
		float slurp;
		Vector3 initpos = PegGrid[p].transform.localPosition;
		Vector3 skew = PegGrid[p].transform.localPosition + new Vector3(Rnd.Range(-0.4f, 0.4f), -1f, Rnd.Range(-0.4f, 0.4f));
		for (int i = 0; i < 600; i++) {
			slurp = sigmoidLerp((i / 120f));
			PegGrid[p].GetComponent<Renderer>().material.color = Color.Lerp(start, new Color(0f,1f,0.5f), slurp);
			PegGrid[p].transform.localPosition = Vector3.Lerp(initpos, skew, (i / 600f)* (i/600f));
			if (i == 300 && p == 0) GetComponent<KMBombModule>().HandlePass();
			yield return null;
		}
	}

	static float sigmoidLerp(float i){
		if(i < 0f) i = 0f;
		if(i > 1f) i = 1f;
		//paste this into desmos
		//	\frac{2.013475894}{1+e^{-7x}}-1
		return 2.013475894f/(1+Mathf.Pow(2.718281828459f, -7*i)) - 1.0f;
	}

	int ReverseLookup(int i) {
		for(int j = 0; j < 25; j++) {
			if (PegPositionLookup[j] == i) return j;
		}
		return -1;
	}


#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"Use !{0} A5 B3 C2 D1 E4 to press those pegs.";
#pragma warning restore 414

	IEnumerator ProcessTwitchCommand (string Command) {
		Command = Command.Trim().ToUpper();
		string[] cmds = Command.Split(' ');
		yield return null;
		foreach (string c in cmds) {
			if (c.Length != 2 || !"ABCDE".Contains(c[0]) || !"12345".Contains(c[1])) {
				yield return "sendtochaterror Invalid peg: \"" + c + "\". Halting commands.";
				yield break;
			} else {
				int i = "ABCDE".IndexOf(c[0]) + (int.Parse(c[1].ToString()) -1) * 5;
				PegGrid[ReverseLookup(i)].OnInteract();
				yield return new WaitForSeconds(0.2f);
				if (ModuleSolved) {
					yield return new WaitForSeconds(5f);
					yield break;
				}
			}
		}
	}

	IEnumerator TwitchHandleForcedSolve () {
		yield return null;

		for (int i = 0; i < 25; i++){
			PegGrid[i].OnInteract();
			yield return new WaitForSeconds(0.2f);
			PegGrid[ReverseLookup(i)].OnInteract();
			yield return new WaitForSeconds(0.2f);
			if (ModuleSolved) {
				yield return new WaitForSeconds(5f);
				yield break;
			}
		}
	}
}