using Godot;
using System;

public partial class Ball : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	private bool grabbed;
	private bool flying;
	private Vector2 jumpDirection;
    private float jumpForce = 1500f;
	public override void _Ready()
	{
		grabbed = false;
		flying = false;
		this.GravityScale = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if (Input.IsActionJustPressed("left_click")) {
            grabbed = true;
        }

        if (grabbed) {
            SetAngle();
        }

        if (grabbed && Input.IsActionJustReleased("left_click")) {
            GravityScale = 1f;
            SetFlying();
            grabbed = false;
        }
	}

	public void SetAngle() {
		Vector2 mousePosition = GetGlobalMousePosition();
        if (grabbed)
        {
            LookAt(mousePosition);
            jumpDirection = (mousePosition - GlobalPosition).Normalized();
        }

	}

	public void SetFlying() {
		ApplyCentralImpulse(jumpDirection * jumpForce);
	}
}
