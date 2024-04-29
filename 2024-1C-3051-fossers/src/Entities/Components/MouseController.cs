using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;

namespace WarSteel.Entities;

class MouseController : Component
{

    private float sensitivity;

    private Vector2 prevMousePosition;

    public MouseController(float sensitivity)
    {
        this.sensitivity = sensitivity;
        prevMousePosition = MousePosition();
    }


    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        MouseState currentMouseState = Mouse.GetState();
        
        // Calculate mouse delta
        Vector2 mouseDelta = new Vector2(currentMouseState.X - prevMousePosition.X, currentMouseState.Y - prevMousePosition.Y);
        
        // Sensitivity factor to scale the mouse movement

        // Convert mouse delta into rotation angles
        float yaw = mouseDelta.X * sensitivity;
        float pitch = -mouseDelta.Y * sensitivity;

        // Update the entity's rotation matrix by appending new rotations
         self.Transform.Pos =Vector3.Transform(self.Transform.Pos,Matrix.CreateFromYawPitchRoll(yaw,pitch,0));

        // Save the current mouse state for the next frame
        prevMousePosition = currentMouseState.Position.ToVector2();
    }

    private Vector2 MousePosition()
    {
        MouseState mouseState = Mouse.GetState();


       
        return mouseState.Position.ToVector2();
    }

    public string id()
    {
        return "camera";
    }


}
