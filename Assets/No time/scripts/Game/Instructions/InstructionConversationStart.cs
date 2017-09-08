using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionConversationStart : InstructionData {

	public Entity partner;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionConversationStart"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">T.</param>
	/// <param name="p">P.</param>
	public InstructionConversationStart (int TId, float t,Entity p) : base (TId, t)
	{
		partner = p;
	}
}
