using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Block firstBlock;
	[Export]
	public Node2D node;
	private Vector2 nodePosition;
	private Vector2 firstBlockPosition;
	private int space = 10;
	private int blockHeight = 70;
	private int step;
	private List<Block> blocks;

	[Export]
	public BallSpawn ballSpawn;
	public override void _Ready()
	{
		this.firstBlockPosition = firstBlock.Position;
		this.nodePosition = node.Position;
		this.blocks = new List<Block>();
		this.blocks.Add(firstBlock);
		firstBlock.blocks = blocks;
		this.step = this.space + this.blockHeight;
		//GD.Print(nodePosition);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print();
		if(ballSpawn.roundDone == true) {
			MoveBlocksDown();
			SpawnBlocks();
			ballSpawn.roundDone = false;
		}
	}


	public void SpawnBlocks() {
		//Random random = new Random();
		//int numberOfNewBlocks = random.Next(1,4);
		int numberOfNewBlocks = 3;
		int[] positions = new int[3] {1,3,4}; //MAX IS 7
		for(int i = 0; i < numberOfNewBlocks; i++) {
			PackedScene newBlockScene = GD.Load<PackedScene>("res://scenes/block.tscn");
			Block newBlock = (Block)newBlockScene.Instantiate();
			Vector2 newBlockPosition = new Vector2(nodePosition.X  + positions[i]*this.step, nodePosition.Y);
			newBlock.Position = newBlockPosition;
			newBlock.blocks = blocks;
			blocks.Add(newBlock);

			GetTree().Root.AddChild(newBlock);
		}
	}

	public void MoveBlocksDown() {
		GD.Print(blocks.Count);
		for(int i = 0; i < blocks.Count; i++) {
			blocks[i].MoveDown();
		}
	}
}
