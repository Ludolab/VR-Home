using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Next Day Node")]
public class DialogueNextDayNode : Node
{
	public const string ID = "dialogueNextDayNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Next Day"; } }
	public override Vector2 DefaultSize { get { return new Vector2(150, 50); } }

	[ConnectionKnob("In", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn;
	[ConnectionKnob("Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	//TODO: condition

	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowIn.DisplayLayout();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();
	}
}
