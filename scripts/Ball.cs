using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	private bool grabbed;
	private bool isFlying;
	private Vector2 jumpDirection;
	private Vector2 velocity = new Vector2();
	private float jumpForce = 1000f;
	private float gravity = 30f;
	public int numberOfBalls;
	private int ballsToShoot;
	private Timer ballTimer;

	[Signal]
	public delegate void BallCollidedEventHandler(Ball ball);

	public override void _Ready() {
		grabbed = false;
		isFlying = false;
		numberOfBalls = 1;
		ballTimer = GetNode<Timer>("Ball_Timer");
		this.ballsToShoot = numberOfBalls;
	}


	public override void _PhysicsProcess(double delta) {
		if (Input.IsActionJustPressed("left_click")) {
			grabbed = true;
			isFlying = false;
		}

		if (grabbed) {
			SetAngle();
		}

		if (grabbed && Input.IsActionJustReleased("left_click")) {
			SetFlying();
			grabbed = false;
			isFlying = true;
		}

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
		grabbed = false;
		isFlying = false;
		velocity = Vector2.Zero;
		Position = new Vector2(positionX, 755);
	}

	private void SetAngle() {
		Vector2 mousePosition = GetGlobalMousePosition();
		if (grabbed) {
			LookAt(mousePosition);
			jumpDirection = (mousePosition - GlobalPosition).Normalized();
		}
	}

	private void SetFlying() {
		velocity = jumpDirection * jumpForce;
	}
	
	
	private void _on_ball_timer_timeout()
	{
		// Replace with function body.
	}
}


