using Godot;
using System;

public partial class StarterBarrier : Area2D
{
	

	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}
	
	
	private void _on_body_entered(Node2D body)
	{
		if(body is Ball ball) {
			//GD.Print("Collision detected with: ", body.Name);
			ball.restore(ball.Position.X);
		 }
	}
}


