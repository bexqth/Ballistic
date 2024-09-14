using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Global : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Block firstBlock;
	[Export]
	public Node2D node;

	[Export]
	public Node2D spawnNode;
	private Vector2 spawnPosition;
	private Vector2 nodePosition;
	private Vector2 firstBlockPosition;
	private int space = 10;
	private int blockHeight = 70;
	private int step;
	private List<Block> blocks;
	private Label scoreLabel;
	private Label scoreLabel2;
	private Label scoreLabel3;
	private ColorRect scoreColorRect;
	public int totalScore{get;set;}
	[Export]
	public BallSpawn ballSpawn;
	private int level;
	private Timer moveBlockTimer;
	private bool blockStop;
	private bool shouldMoveBlocks = true;
	private Timer waitBlockTimer;
	private Button restartGameButton;
	private Label gameOverLabel;
	private Timer gameOverTimer;
	private bool gameOverTimerOut;
	public int scoreOne;
	public int scoreTwo;
	public int scoreThree;
	public int numberOfPlayers{get;set;}
	private TextureRect fadeTextureRect;
	private TextEdit numberOfPlayersTextEdit;
	private Label numberOfPlayersLabel;
	private Button numberOfPlayersButton;
	private bool gameCanStart;
	private List<BallSpawn> ballSpawns;

	[Export]
	public StarterBarrier starterBarrier;	
	public bool endGame{get;set;}
	public bool roundDone{get;set;}
	//public static Global Instance { get; private set; }
	public override void _Ready()
	{
		this.gameCanStart = false;
		this.ballSpawns = new List<BallSpawn>();
		roundDone = false;
		endGame = false;

		this.scoreColorRect = GetNode<ColorRect>("ColorRect");
		this.scoreLabel = this.scoreColorRect.GetNode<Label>("Label");
		this.scoreLabel2 = this.scoreColorRect.GetNode<Label>("Label2");
		this.scoreLabel3 = this.scoreColorRect.GetNode<Label>("Label3");
		this.firstBlockPosition = firstBlock.Position;
		this.nodePosition = node.Position;
		this.spawnPosition = spawnNode.Position;

		this.blocks = new List<Block>();
		this.blocks.Add(firstBlock);
		firstBlock.blocks = blocks;
		firstBlock.scoreLabel = this.scoreLabel;
		firstBlock.scoreLabel2 = this.scoreLabel2;
		firstBlock.scoreLabel3 = this.scoreLabel3;

		firstBlock.scoreOne = this.scoreOne;
		firstBlock.scoreTwo = this.scoreTwo;
		firstBlock.scoreThree = this.scoreThree;

		//firstBlock.scoreAdd = totalScore;
		this.step = this.space + this.blockHeight;
		this.totalScore = 0;
		this.level = 1;
		this.scoreLabel.Text = this.scoreOne.ToString();
		this.scoreLabel2.Text = this.scoreTwo.ToString();
		this.scoreLabel3.Text = this.scoreThree.ToString();
		//Instance = this;
		//GD.Print(nodePosition);
		this.moveBlockTimer = GetNode<Timer>("Timer");
		this.waitBlockTimer = GetNode<Timer>("Timer_Wait");
		this.blockStop = false;
		restartGameButton = GetNode<Button>("Button");
		this.restartGameButton.Visible = false;
		gameOverLabel = GetNode<Label>("Label_Game");
		gameOverLabel.Text = "";
		this.gameOverTimer = GetNode<Timer>("Timer_GameOver");

		this.fadeTextureRect = GetNode<TextureRect>("TextureRect_Fade");
		this.numberOfPlayersTextEdit = GetNode<TextEdit>("TextEdit_NumberOfPlayers");
		this.numberOfPlayersLabel = GetNode<Label>("Label");
		this.numberOfPlayersButton = GetNode<Button>("Button_Set");

		//this.starterBarrier.endGame = this.endGame;
		//this.starterBarrier.roundDone = this.roundDone;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print();
		if(gameCanStart) {
			if(!starterBarrier.endGame) {
				if(starterBarrier.roundDone == true) {		
					GD.Print("SPAWN BLOCKS");
					SpawnBlocks();
					waitBlockTimer.Start();
					starterBarrier.roundDone = false;
					//ballSpawn.StartShooting();
					ballSpawsStartShooting();
				}
			} else {
				for(int i = 0; i < this.blocks.Count; i++) {
					blocks[i].colorRect.Color = new Color("#474743");
					blocks[i].line.DefaultColor = new Color("#474743");
				}
				this.restartGameButton.ZIndex = 5;
				gameOverLabel.Text = "Game Over";
			}
		}
	}

	public void ballSpawsStartShooting() {
		for(int i = 0; i < this.ballSpawns.Count; i++) {
			ballSpawns[i].StartShooting();
		}
	}

	public void SpawnBlocks() {

		this.level++;
		GD.Print(level);
		var rnd = new Random();
		int numberOfNewBlocks = rnd.Next(1,5);
		var positions = Enumerable.Range(1,7).OrderBy(x => rnd.Next()).Take(numberOfNewBlocks).ToList();
		for(int i = 0; i < numberOfNewBlocks; i++) {
			PackedScene newBlockScene = GD.Load<PackedScene>("res://scenes/block.tscn");
			Block newBlock = (Block)newBlockScene.Instantiate();
			Vector2 newBlockPosition = new Vector2(spawnPosition.X  + positions[i]*this.step - this.step, spawnPosition.Y);

			newBlock.Position = newBlockPosition;
			newBlock.blocks = blocks;
			newBlock.scoreLabel = this.scoreLabel;
			newBlock.scoreLabel2 = this.scoreLabel2;
			newBlock.scoreLabel3 = this.scoreLabel3;
			newBlock.scoreOne = this.scoreOne;
			newBlock.scoreTwo = this.scoreTwo;
			newBlock.scoreThree = this.scoreThree;

			blocks.Add(newBlock);
			GetTree().Root.AddChild(newBlock);

			newBlock.setNumber(level);
		}
	}

	public void MoveBlocksDown() {
		if (shouldMoveBlocks) {
		for (int i = 0; i < blocks.Count; i++) {
			blocks[i].MoveDownABit();
		}
		MoveSpawnNodeDownABit();
		moveBlockTimer.Start();
		} else {
			moveBlockTimer.Stop();
			spawnNode.Position = spawnPosition;
		}
	}

	public void MoveSpawnNodeDownABit() {
		var bit = 5;
		var pos = spawnNode.Position;
		pos.Y += bit;
		spawnNode.Position = pos;

		if (spawnNode.Position.Y >= 185) {
			spawnNode.Position = spawnPosition;
			shouldMoveBlocks = false;
		}

	}
	
	public void RestartGame() {
		
		ballSpawn.shootTimer.Stop();
		ballSpawn.ballTimer.Stop();
		waitBlockTimer.Stop();
		moveBlockTimer.Stop();

		//GD.Print("Restarting game...");

		for (int i = 0; i < this.blocks.Count; i++) {
			if (IsInstanceValid(blocks[i])) {
				blocks[i].QueueFree();
			} else {

			}
		}

		this.blocks.Clear();

		PackedScene newBlockScene = GD.Load<PackedScene>("res://scenes/block.tscn");
		firstBlock = (Block)newBlockScene.Instantiate();
		GetTree().Root.AddChild(firstBlock);

		firstBlock.Position = this.firstBlockPosition;
		firstBlock.blocks = blocks;
		firstBlock.scoreLabel = this.scoreLabel;
		firstBlock.scoreLabel2 = this.scoreLabel2;
		firstBlock.scoreLabel3 = this.scoreLabel3;

		firstBlock.scoreOne = this.scoreOne;
		firstBlock.scoreTwo = this.scoreTwo;
		firstBlock.scoreThree = this.scoreThree;

		this.blocks.Add(firstBlock);

		this.scoreLabel.Text = "0";
		this.scoreLabel2.Text = "0";
		this.scoreLabel3.Text = "0";
		this.restartGameButton.Visible = false;
		this.restartGameButton.ZIndex = 1;
		gameOverLabel.Text = "";
		level = 1;
		//GD.Print("Game restarted successfully.");
		ballSpawn.endGame = false;
	}

	public void setPlayers() {
		int playerNumber = 1;
		int xStep = (570) / (this.numberOfPlayers + 1);
		for(int i = 0; i < this.numberOfPlayers; i++) {
			PackedScene newBallSpawnScene = GD.Load<PackedScene>("res://scenes/ball_spawn.tscn");
			BallSpawn newBallSpawn = (BallSpawn)newBallSpawnScene.Instantiate();
			this.ballSpawns.Add(newBallSpawn);
			GetTree().Root.AddChild(newBallSpawn);
			newBallSpawn.numberOfBalls = 1;
			newBallSpawn.spawnIndex = playerNumber;
			newBallSpawn.Position = new Vector2(xStep*playerNumber,755);

			Label playerLabel = new Label();
			playerLabel.Text = "0";
			playerLabel.Position = new Vector2((xStep - 20) * playerNumber, 5);
			playerLabel.AddThemeFontSizeOverride("font_size", 64);

			// Add the label to the scene
			GetTree().Root.AddChild(playerLabel);


			playerNumber++;
		}
		StartBallSpawns();
	}

	public void StartBallSpawns() {
		for(int i = 0; i < this.ballSpawns.Count; i++) {
			ballSpawns[i].timer.Start();
		}
	}

	private void _on_timer_timeout()
	{
		MoveBlocksDown();
	}
	
	private void _on_timer_wait_timeout()
	{
		shouldMoveBlocks = true;
		MoveBlocksDown();
		waitBlockTimer.Stop();
	}
	
	private void _on_button_pressed()
	{
		RestartGame();
	}
	
	private void _on_timer_game_over_timeout()
	{
		//GD.Print("TIMEOUT");
		RestartGame();
		gameOverTimer.Stop();
	}
	
	private void _on_button_set_pressed()
	{
		this.numberOfPlayers = numberOfPlayersTextEdit.Text.ToInt();
		this.setPlayers();
		starterBarrier.ballspawns = this.ballSpawns;
		fadeTextureRect.Visible = false;
		numberOfPlayersButton.Visible = false;
		numberOfPlayersLabel.Visible = false;
		numberOfPlayersTextEdit.Visible = false;
		this.gameCanStart = true;
		
	}
}












