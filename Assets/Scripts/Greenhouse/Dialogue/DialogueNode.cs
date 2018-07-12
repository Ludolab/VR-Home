using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;
using NodeEditorFramework;
using UnityEditor;

[System.Serializable]
[Node(false, "Dialogue/Dialogue Node")]
public class DialogueNode : Node
{
	public const string ID = "dialogueNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Dialogue Node"; } }
	public override Vector2 DefaultSize { get { return new Vector2(200, 180); } }

	[ConnectionKnob("Flow In", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn;
	[ConnectionKnob("Flow Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	public string text = "";

	private Vector2 scroll;
	
	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowIn.DisplayLayout();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();

		//text = RTEditorGUI.TextField(new GUIContent("Text", "The text of the dialogue"), text);

		GUILayout.Space(5);

		GUILayout.Label("Text");
		GUILayout.BeginHorizontal();
		scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(100));
		EditorStyles.textField.wordWrap = true;
		text = EditorGUILayout.TextArea(text, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		if (GUI.changed)
		{
			NodeEditor.curNodeCanvas.OnNodeChange(this);
		}
	}
}
