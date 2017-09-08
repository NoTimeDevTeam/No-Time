using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionFindWayTo : InstructionData {

	public Vector2 goal;
	public float speed;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionFindWayTo"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime</param>
	/// <param name="g">target goal to go to</param>
	/// <param name="s">speed</param>
	public InstructionFindWayTo (int TId, float t,Vector2 g,float s) : base (TId, t)
	{
		goal = g;
		speed = s;
	}
}
