using System;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using WarSteel.Screens;
using WarSteel.UIKit;
using WarSteel.Utils;

public class PlayerScreen : UIScreen
{
    public PlayerScreen() : base("player-screen") { }
    private UI _healthBar;
    private int _healthBarWidth = 400;

    private UI _reloadingTimeUI;
    private TextUI _reloadingTimeUIText;
    private float _reloadingTime;
    private float _currentReloadTime;
    Vector2 screenCenter;

    public override void Initialize(Scene scene)
    {
        GraphicsDeviceManager GraphicsDeviceManager = scene.GraphicsDeviceManager;
        screenCenter = Screen.GetScreenCenter(GraphicsDeviceManager);
        int screenWidth = Screen.GetScreenWidth(GraphicsDeviceManager);
        int screenHeight = Screen.GetScreenHeight(GraphicsDeviceManager);

        Vector3 healthBarPos = new(screenCenter.X, screenHeight - 60, 0);
        UI healthBarBG = new UI(healthBarPos, _healthBarWidth, 50, new Image("UI/health-bar-bg"));
        _healthBar = new UI(healthBarPos, _healthBarWidth, 50, new Image("UI/health-bar-fill"));

        // add ui elements
        AddUIElem(healthBarBG);
        AddUIElem(_healthBar);

        // subscribe to player events and update the ui accordingly
        PlayerEvents.SubscribeToReload(OnPlayerStartedReloading);
        PlayerEvents.SubscribeToHealthChanged(OnPlayerHealthUpdated);
    }

    private string GetReloadingText()
    {
        return $"{_currentReloadTime:F2} s";
    }

    private void UpdateReloadingTimeText()
    {
        _currentReloadTime -= 10 / 1000f;
        if (_currentReloadTime <= 0)
        {
            RemoveUIElem(_reloadingTimeUI);
            return;
        }
        _reloadingTimeUIText.Text = GetReloadingText();
        Timer.Timeout(10, UpdateReloadingTimeText);
    }

    private void OnPlayerStartedReloading(int reloadingTimeInMs)
    {
        RemoveUIElem(_reloadingTimeUI);
        _reloadingTime = reloadingTimeInMs / 1000;
        _currentReloadTime = _reloadingTime;
        _reloadingTimeUIText = new Paragraph(GetReloadingText());
        _reloadingTimeUI = new UI(new(screenCenter.X, screenCenter.Y, 0), _reloadingTimeUIText);
        AddUIElem(_reloadingTimeUI);

        Timer.Timeout(10, UpdateReloadingTimeText);
    }

    private void OnPlayerHealthUpdated(int health)
    {
        // Calculate the width of the health bar based on the player's health percentage
        float healthPercentage = health / 100f; // Calculate health percentage
        int newHealthBarWidth = (int)(_healthBarWidth * healthPercentage); // Calculate new width

        // Ensure the width does not go below 0
        newHealthBarWidth = Math.Max(0, newHealthBarWidth);

        // Update the health bar width
        _healthBar.Width = newHealthBarWidth;
    }
}