using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{

	private DialogueCanvasType dialogueCanvas;
	private DialogueNode currentNode;

	public void SetCanvas(DialogueCanvasType canvas)
	{
		dialogueCanvas = canvas;
		if (dialogueCanvas != null)
		{
			currentNode = dialogueCanvas.startNode;
		}
	}

	public void NextDay(Neighbor neighbor)
	{
		ProcessDay(neighbor);
	}

	private void ProcessDay(Neighbor neighbor)
	{
		if (currentNode == null) return;

		while (!(currentNode is DialogueNextDayNode))
		{
			currentNode.Process(neighbor);
			currentNode = currentNode.GetNext();
		}

		//advance past the NextDay node we stopped at
		currentNode = currentNode.GetNext();
	}
}
