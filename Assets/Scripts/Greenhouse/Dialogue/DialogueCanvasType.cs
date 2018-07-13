using NodeEditorFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCanvasType("Dialogue Canvas")]
public class DialogueCanvasType : NodeCanvas
{
	public override string canvasName { get { return "Dialogue"; } }

	private string startNodeID { get { return "dialogueStartNode"; } }
	public DialogueStartNode startNode;

	protected override void OnCreate()
	{
		Traversal = new DialogueTraversal(this);
		startNode = Node.Create(startNodeID, Vector2.zero) as DialogueStartNode;
	}

	private void OnEnable()
	{
		if (Traversal == null)
			Traversal = new DialogueTraversal(this);
		// Register to other callbacks
		//NodeEditorCallbacks.OnDeleteNode += CheckDeleteNode;
	}

	protected override void ValidateSelf()
	{
		if (Traversal == null)
			Traversal = new DialogueTraversal(this);
		if (startNode == null && (startNode = nodes.Find((Node n) => n.GetID == startNodeID) as DialogueStartNode) == null)
		{
			startNode = Node.Create(startNodeID, Vector2.zero) as DialogueStartNode;
		}
	}

	public override bool CanAddNode(string nodeID)
	{
		if (nodeID == startNodeID)
		{
			return !nodes.Exists((Node n) => n.GetID == startNodeID);
		}
		return true;
	}
}
