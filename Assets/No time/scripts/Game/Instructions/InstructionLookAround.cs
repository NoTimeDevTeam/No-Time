using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLookAround : InstructionData {

	public Vector2 direction;
	public float angle;
	public float speed;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionLookAround"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime</param>
	/// <param name="d">Direction</param>
	/// <param name="a">angle</param>
	/// <param name="s">speed of looking</param>
	public InstructionLookAround (int TId, float t,Vector2 d,float a, float s) : base (TId, t)
	{
		direction = d;
		angle = a;
		speed = s;
	}
}
