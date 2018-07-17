using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{

	//TODO: dialogueVars

	public DialogueCanvasType dialogueCanvas;

	private DialogueNode currentNode;

	private void Start()
	{
		currentNode = dialogueCanvas.startNode;
	}

	public void NextDay(Neighbor neighbor)
	{
		ProcessDay(neighbor);
	}

	private void ProcessDay(Neighbor neighbor)
	{
		while (!(currentNode is DialogueNextDayNode))
		{
			currentNode.Process(neighbor);
			currentNode = currentNode.GetNext();
		}

		//advance past the NextDay node we stopped at
		currentNode = currentNode.GetNext();
	}
}
