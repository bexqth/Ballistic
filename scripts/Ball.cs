using Godot;
using System;

public partial class Ball : CharacterBody2D
{
    private bool grabbed;
    private bool isFlying;
    private Vector2 jumpDirection;
    private Vector2 velocity = new Vector2();
    private float jumpForce = 1500f;
    private float gravity = 20f;

    public override void _Ready()
    {
        grabbed = false;
        isFlying = false;
        velocity = Vector2.Zero;  // Ensure the ball stays in place initially
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("left_click"))
        {
            grabbed = true;
            isFlying = false;
            velocity = Vector2.Zero;  // Stop any movement while grabbed
        }

        if (grabbed)
        {
            SetAngle();
        }

        if (grabbed && Input.IsActionJustReleased("left_click"))
        {
            SetFlying();
            grabbed = false;
            isFlying = true;
        }

        // Apply gravity only if the ball is flying
        if (isFlying)
        {
            velocity.Y += gravity * (float)delta;
        }

        // Move and collide
        KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
        if (collision != null)
        {
            // Handle collision response
            velocity = velocity.Bounce(collision.GetNormal());
        }
    }

    private void SetAngle()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        if (grabbed)
        {
            LookAt(mousePosition);
            jumpDirection = (mousePosition - GlobalPosition).Normalized();
        }
    }

    private void SetFlying()
    {
        velocity = jumpDirection * jumpForce;
    }
}
