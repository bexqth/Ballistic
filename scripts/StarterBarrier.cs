using Godot;
using System;

public partial class StarterBarrier : Area2D
{
	
	private bool ballLineCollided;
	private int ballsCollided;
	private Vector2 newStartingPosition;
	

	[Export]
	public BallSpawn ballSpawn;
	public override void _Ready()
	{
		ballsCollided = 0;
	}

	
	public override void _Process(double delta)
	{
	}
	
	
	private void _on_body_entered(Node2D body)
	{
		if(body is Ball ball) {
			if(ballsCollided == 0) { // first went thro
				ballLineCollided = true;
				newStartingPosition = new Vector2(ball.Position.X, 755);
				ballSpawn.Position = newStartingPosition;
				ballsCollided++;
			} else {
				ballsCollided++;
			}
			if(ballsCollided == ballSpawn.numberOfBalls) {
				ballsCollided = 0;
				ballSpawn.Restore();
				
			}
			ballSpawn.Visible = true;
			//ball.QueueFree();

			//GD.Print("Collision detected with: ", body.Name);
			//ball.restore(ball.Position.X);
		}
	}
}


