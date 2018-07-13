using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Join Node")]
public class DialogueJoinNode : Node
{
	public const string ID = "dialogueJoinNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Join"; } }
	public override Vector2 DefaultSize { get { return new Vector2(150, 150); } }

	[ConnectionKnob("In 1", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn1;
	[ConnectionKnob("In 2", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn2;
	[ConnectionKnob("Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	//TODO: color (same/similar to branch node)

	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowIn1.DisplayLayout();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		flowIn2.DisplayLayout();
		GUILayout.EndHorizontal();
	}
}
