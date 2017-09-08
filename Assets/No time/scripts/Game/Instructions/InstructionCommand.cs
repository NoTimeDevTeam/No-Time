using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allowes for more powerfull scripting in the instructionsets
/// by using this you can use features of the "NoSS" triggered by time instead of interaction
/// later implications may even enable change of behaviour in certan curcumstances
/// </summary>
public class InstructionCommand : InstructionData {

	public string command;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionCommand"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime.</param>
	/// <param name="c">Command as a string that can be converted.</param>
	public InstructionCommand (int TId, float t,string c) : base (TId, t)
	{
		command = c;
	}
}
