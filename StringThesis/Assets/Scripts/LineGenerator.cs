using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LineGenerator : MonoBehaviour {
	
	public List<GameObject> nodes = new List<GameObject>();

	public List<Vector3> nodePositions = new List<Vector3>();

	public Vector3[] nodePosVec3;
 
	public LayerMask targetLayer;
 
	public int maxNodes = 10;

	[SerializeField]float lineInterval = 1f;
	[SerializeField]float startingLineInterval = 1f;

	public KeyCode drawKey;

	// Use this for initialization
	void Start () {
 	}
	
	// Update is called once per frame
	void Update () {
		if(nodes.Count <= maxNodes){
			DropNodes (drawKey);
		}
 		DrawLine ();
 //		FindLineNodes ();
	}

	void DropNodes(KeyCode key){

		if (Input.GetKey (key)) {
			lineInterval -= Time.deltaTime;
 		
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit rayHit = new RaycastHit ();
			if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, targetLayer)) {
				if (rayHit.transform != null && rayHit.transform.name != "LineMaker") {
					Debug.Log ("Hit " + transform.name);
					if (lineInterval <= 0) {
						GameObject node_ = Instantiate (Resources.Load ("Node")) as GameObject;
						node_.transform.position = rayHit.point;
						if (nodes.Count > 0) {
							node_.AddComponent<CharacterJoint> ().connectedBody = nodes [nodes.Count-1].GetComponent<Rigidbody>();
						}
 						nodes.Add (node_);
						nodePositions.Add (node_.transform.position);
						lineInterval = startingLineInterval;
 					}
				}
			}
		}	
	}


	void DrawLine(){
		Vector3[] points = new Vector3[nodes.Count];
		Debug.Log ("Nodes.count " + nodes.Count);
		Debug.Log ("points length = " + points.Length);
		for (int i = 0; i < nodes.Count; i++) {
			points [i] = new Vector3 (nodes[i].transform.position.x, nodes[i].transform.position.y, nodes[i].transform.position.z);
		}
		gameObject.GetComponent<LineRenderer> ().positionCount = points.Length;
		gameObject.GetComponent<LineRenderer> ().SetPositions (points);

		gameObject.GetComponent<LineRenderer> ().startWidth = 0.1f;
		gameObject.GetComponent<LineRenderer> ().endWidth = 0.1f;
		gameObject.GetComponent<LineRenderer> ().startColor = Color.cyan;
		gameObject.GetComponent<LineRenderer> ().endColor = Color.cyan;
	}
		
	void FindLineNodes(){
		for (int i = 0; i < nodes.Count; i++) {
			nodePosVec3 [i] = nodes [i].transform.position;
		}
	}
}
