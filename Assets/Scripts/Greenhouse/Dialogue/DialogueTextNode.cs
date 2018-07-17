using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Dialogue Text Node")]
public class DialogueTextNode : DialogueNode
{
	public const string ID = "dialogueNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "Dialogue Text"; } }
	public override Vector2 DefaultSize { get { return new Vector2(200, 180); } }

	[ConnectionKnob("In", Direction.In, "Flow", NodeSide.Left)]
	public ConnectionKnob flowIn;
	[ConnectionKnob("Out", Direction.Out, "Flow", NodeSide.Right)]
	public ConnectionKnob flowOut;

	private const float TEXT_AREA_HEIGHT = 115;

	public string text = "";

	private Vector2 scroll;
	private int lastCursorIndex;
	
	public override void NodeGUI()
	{
		GUILayout.BeginHorizontal();
		flowIn.DisplayLayout();
		flowOut.DisplayLayout();
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.Label("Text");
		GUILayout.BeginHorizontal();
		scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(TEXT_AREA_HEIGHT));
		EditorStyles.textField.wordWrap = true;
		text = EditorGUILayout.TextArea(text, GUILayout.ExpandHeight(true));
		UpdateScroll();
		EditorGUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		/*if (GUI.changed)
		{
			NodeEditor.curNodeCanvas.OnNodeChange(this);
		}*/
	}

	private void UpdateScroll()
	{
		//Scroll so that the cursor is within the visible window

		//Get the editor of the currently active textarea (or null if none selected)
		//from http://answers.unity.com/answers/1358721/view.html
		TextEditor editor = typeof(EditorGUI).GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as TextEditor;
		if (editor != null)
		{
			//the text area is selected
			int cursorIndex = editor.cursorIndex;
			if (cursorIndex != lastCursorIndex)
			{
				//the cursor was moved- scroll so the cursor is on screen
				lastCursorIndex = cursorIndex;

				float scrollY = scroll.y;

				float cursorY = editor.graphicalCursorPos.y;
				float lineHeight = EditorStyles.textField.lineHeight + 5; //add a little padding below the line
				float windowHeight = TEXT_AREA_HEIGHT;
				float minTop = cursorY + lineHeight - windowHeight; //top of the highest window that fits the bottom of the cursor
				float maxTop = cursorY; //top of the lowest window that fits the top of the cursor
				scrollY = Mathf.Clamp(scrollY, minTop, maxTop);

				scroll = new Vector2(0, scrollY);
			}
		}
	}

	public override DialogueNode GetNext()
	{
		return GetConnection(flowOut);
	}

	public override void Process(Neighbor neighbor)
	{
		//TODO spawn letter
		neighbor.GenerateLetter(text, ""); //TODO: split text into text1, text2
	}
}
