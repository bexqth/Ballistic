using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	public bool isFlying{get;set;}
	//private Vector2 jumpDirection;
	private Vector2 velocity = new Vector2();
	private float jumpForce = 1000f;
	private float gravity = 30f;
	private Timer ballTimer;
	public int number{get;set;}
	
	[Signal]
	public delegate void BallCollidedEventHandler(Ball ball);

	public override void _Ready() {
		isFlying = false;
		ballTimer = GetNode<Timer>("Ball_Timer");
	}


	public override void _PhysicsProcess(double delta) {

		if (isFlying) {
			velocity.Y += gravity * (float)delta;
		}

		// Move and collide
		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null) {
			velocity = velocity.Bounce(collision.GetNormal());
			//EmitSignal(nameof(Ball.BallCollided), this);
		}
	}
	public void restore(float positionX) {
		isFlying = false;
		velocity = Vector2.Zero;
		Position = new Vector2(positionX, 755);
	}


	public void SetFlying(Vector2 jumpDirection) {
		velocity = jumpDirection * jumpForce;
	}
	
	
	private void _on_ball_timer_timeout()
	{
		// Replace with function body.
	}
}


