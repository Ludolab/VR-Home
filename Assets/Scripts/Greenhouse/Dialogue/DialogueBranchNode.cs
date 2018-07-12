using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Branch Node")]
public class DialogueBranchNode : Node
{
	public const string ID = "dialogueBranchNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Branch"; } }
	public override Vector2 DefaultSize { get { return new Vector2(150, 150); } }

	[ConnectionKnob("In", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn;
	[ConnectionKnob("Out True", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOutTrue;
	[ConnectionKnob("Out False", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOutFalse;

	//TODO: condition

	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowOutTrue.DisplayLayout();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		flowIn.DisplayLayout();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		flowOutFalse.DisplayLayout();
		GUILayout.EndHorizontal();
	}
}
