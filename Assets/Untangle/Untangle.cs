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

	public class Line {
		public static LineRenderer baseline;
		public static GameObject parent;
		int xa, ya, xb, yb; 
		public Color startClr;
		public Color endClr;
		public LineRenderer linerend;
		public Line(int x1, int y1, int x2, int y2){
			xa = x1; ya = y1;
			xb = x2; yb = y2;
			linerend = Instantiate(baseline, parent.transform);
			linerend.gameObject.SetActive(true);
			linerend.SetPositions(new Vector3[] { new Vector3(xa,0,ya), new Vector3(xb,0,yb)});
		}


		public static bool onSegment(List<int> p, List<int> q, List<int> r) {
	        return (q[0] < Math.Max(p[0], r[0]) && 
	                q[0] > Math.Min(p[0], r[0]) &&
	                q[1] < Math.Max(p[1], r[1]) && 
	                q[1] > Math.Min(p[1], r[1]));
	    }

	    public static int orientation(List<int> p, List<int> q, List<int> r) {
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


	    public static bool checkCollision(Line a, Line b) {
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

			//matching point case
			if((o1 == 0 ^ o2 == 0) && (o3 == 0 ^ o4 == 0)) return false;

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
	}

	void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
		ModuleId = ModuleIdCounter++;
		GetComponent<KMBombModule>().OnActivate += Activate;
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

	void Start () { //Shit that you calculate, usually a majority if not all of the module
		Line.baseline = ExampleLine; Line.parent = LineParent;

		List<Line> lineland = new List<Line>();
		lineland.Add(new Line(Rnd.Range(-2, 3),Rnd.Range(-2, 3),Rnd.Range(-2, 3),Rnd.Range(-2, 3)));


		while(lineland.Count < 5){
			Line contender = new Line(Rnd.Range(-2, 3),Rnd.Range(-2, 3),Rnd.Range(-2, 3),Rnd.Range(-2, 3));
			foreach(Line l in lineland){
				if(Line.checkCollision(l, contender)) continue;
			}
			lineland.Add(contender);
		}
	}


	void Update () { //Shit that happens at any point after initialization

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
		Command = Command.Trim().ToUpper();
		string[] cmds = Command.Split(' ');

		yield return null;
	}

	IEnumerator TwitchHandleForcedSolve () {
		yield return null;
	}
}
