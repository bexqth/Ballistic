using Godot;
using System;
using System.Collections.Generic;

public partial class StarterBarrier : Area2D
{
	
	private bool ballLineCollided;
	private int ballsCollided;
	private Vector2 newStartingPosition;
	private int minX = 20;
	private int maxX = 535;
	private int minY = 400;
	private int maxY = 500;
	[Export]
	public Timer gameOverTimer;
	[Export]
	public BallSpawn ballSpawn;
	public List<BallSpawn> ballspawns{get;set;}
	public bool endGame{get;set;}
	public bool roundDone{get;set;}
	public override void _Ready()
	{
		ballsCollided = 0;
	}

	
	public override void _Process(double delta)
	{

	}
	
	
	private void _on_body_entered(Node2D body)
	{		
		/*if(body is Ball ball) {
			//GD.Print("ball colided");
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
		}*/
		
		if(body is Ball ball) {
			for(int i = 0; i < ballspawns.Count; i++) {
				if(ballspawns[i].spawnIndex == ball.spawnIndex) {
					newStartingPosition = new Vector2(ball.Position.X, 755);
					ballspawns[i].Position = newStartingPosition;
					//ballspawns[i].Restore();
					ballspawns[i].Visible = true;
					ballsCollided++;
				}
			} 

			if(ballsCollided == ballspawns.Count) {
				this.roundDone = true;
				ballsCollided = 0;
				GD.Print("ROUND DONE");
				for(int i = 0; i < ballspawns.Count; i++) {
					ballspawns[i].Restore();
				}
			}
		}

	}

	private void _on_area_entered(Area2D area)
	{
		if (area.GetParent() is Block block) {
			//GD.Print("Block colided");
			endGame = true;

			for(int i = 0; i < this.ballspawns.Count; i++) {
				ballspawns[i].endGame = this.endGame;
			}
			
			gameOverTimer.Start();
			//ballSpawn.timer.Start();
			for(int i = 0; i < this.ballspawns.Count; i++) {
				ballspawns[i].timer.Start();
			}

		}
	}

}


