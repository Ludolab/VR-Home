using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Start Node")]
public class DialogueStartNode : Node
{
	public const string ID = "dialogueStartNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Start"; } }
	public override Vector2 DefaultSize { get { return new Vector2(150, 50); } }

	[ConnectionKnob("Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();
	}
}
