using UnityEngine;
using NodeEditorFramework;

//Is this actually useful?

public class DialogueTraversal : NodeCanvasTraversal
{
	DialogueCanvasType Canvas;

	public DialogueTraversal(DialogueCanvasType canvas) : base(canvas)
	{
		Canvas = canvas;
	}

	public override void TraverseAll()
	{
		DialogueStartNode startNode = Canvas.startNode;
		startNode.Calculate();
	}
}
