using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Gift Node")]
public class DialogueGiftNode : DialogueNode
{
	public const string ID = "dialogueGiftNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Gift"; } }
	public override Vector2 DefaultSize { get { return new Vector2(150, 150); } }

	[ConnectionKnob("In", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn;
	[ConnectionKnob("Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	//TODO: gift

	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowIn.DisplayLayout();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();

		//TODO: Gift
	}

	public override DialogueNode GetNext()
	{
		return GetConnection(flowOut);
	}
}
