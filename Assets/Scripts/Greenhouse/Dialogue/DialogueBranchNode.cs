using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

[System.Serializable]
[Node(false, "Dialogue/Branch Node")]
public class DialogueBranchNode : DialogueNode
{
	/*public enum VarType
	{
		Bool,
		Float,
		Int,
		String
	}

	public enum VarComparison
	{
		Equals,
		NotEquals,
		LessThan,
		GreaterThan
	}*/

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
	public string varName;
	//public VarType varType;
	//public VarComparison varComparison;
	//value to check against- how?

	//should be optional to check against a value for a bool
	//just make subclasses?
	//boolcheck
	//boolcomparison
	//intcomparison
	//floatcomparison
	//stringcomparison

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

	public override void Process(Neighbor neighbor)
	{
		//TODO: cache dialogue vars so we can check in CheckCondition
	}

	private bool CheckCondition()
	{
		//TODO
		return true;
	}

	public override DialogueNode GetNext()
	{
		if (CheckCondition())
		{
			return GetConnection(flowOutTrue);
		}
		else
		{
			return GetConnection(flowOutTrue);
		}
	}
}
