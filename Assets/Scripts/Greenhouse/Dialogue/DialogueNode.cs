using NodeEditorFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : Node
{

	public abstract DialogueNode GetNext();

	public virtual void Process(Neighbor neighbor)
	{
		//Perform any actions for the current day
	}

	protected DialogueNode GetConnection(ConnectionKnob outKnob)
	{
		if (!outKnob.connected())
		{
			return null;
		}
		return outKnob.connection(0).body as DialogueNode;
	}
	
}
