using Godot;
using System;

public partial class BallSpawn : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int numberOfBalls;
	private int ballsToShoot;
	private Timer ballTimer;
	private Label numberLabel;
	private bool grabbed;
	private Vector2 jumpDirection;
	public override void _Ready()
	{
		this.ballsToShoot = numberOfBalls;
		this.ballTimer = GetNode<Timer>("Ball_Timer");
		this.numberLabel = GetNode<Label>("Number_Label");
		numberLabel.Text = ballsToShoot.ToString();
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
			SpawnBall();
		}

	}

	private void SetAngle() {
		Vector2 mousePosition = GetGlobalMousePosition();
		if (grabbed) {
			LookAt(mousePosition);
			jumpDirection = (mousePosition - GlobalPosition).Normalized();
		}
	}

	public void Restore() {
		ballsToShoot = numberOfBalls;
		grabbed = false;
		ballTimer.Stop();
		numberLabel.Text = ballsToShoot.ToString();
	}

	public void SpawnBall() {
		if(ballsToShoot > 0) {
			if(ballsToShoot == 0) {
				this.Visible = false;
			}
			PackedScene newBallScene = GD.Load<PackedScene>("res://scenes/ball.tscn");
			Ball newBall = (Ball)newBallScene.Instantiate();
			newBall.Position = this.Position;
			GetTree().Root.AddChild(newBall);
			newBall.isFlying = true;
			newBall.SetFlying(jumpDirection);

			ballsToShoot--;
			numberLabel.Text = ballsToShoot.ToString();
			ballTimer.Start();
		}
	}
	
	private void _on_ball_timer_timeout()
	{
		SpawnBall();
	}


}


