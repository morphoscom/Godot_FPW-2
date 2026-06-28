using Godot;
using System;
using static UiManager;

public partial class Camera : Camera3D
{
    private CharacterBody3D _player;
    public UiManager _uiManager;

    [Export]
    public float Sensitivity { get; set; } = 0.15f;
    [Export]
    public Vector3 CameraOffset { get; set; } = new Vector3(0,0,0);

    public enum CameraTypes
    {
        None,
        OnPlayerFP,
        OnPlayerTP,
        Free
    }

    private bool _mouseLock = false;

    public CameraTypes CurrentCameraType { get; private set; } = CameraTypes.None;

    public void ChangeCameraType(CameraTypes newType)
    {
        switch (newType)
        {
            case CameraTypes.None:
                CurrentCameraType = CameraTypes.None;
                break;
            case CameraTypes.OnPlayerFP:
                if (_player != null)
                {
                    CurrentCameraType = CameraTypes.OnPlayerFP;
                }
                else
                {
                    CurrentCameraType = CameraTypes.None;
                }
                break;
            case CameraTypes.OnPlayerTP:
                CurrentCameraType = CameraTypes.OnPlayerTP;
                if (_player != null)
                {
                    CurrentCameraType = CameraTypes.OnPlayerTP;
                }
                else
                {
                    CurrentCameraType = CameraTypes.None;
                }
                break;
            case CameraTypes.Free:
                CurrentCameraType = CameraTypes.Free;
                break;
        }
    }

    private void CameraOnPlayerFP()
    {
        // move to player head
        Position = new Vector3(_player.Position.X, _player.Position.Y + 2, _player.Position.Z);
        // rotate
        Rotation = new Vector3(Rotation.X, _player.Rotation.Y, Rotation.Z);
    }

    private void CameraOnPlayerTP()
    {
        // move to player head
        Position = new Vector3(_player.Position.X, _player.Position.Y + 2, _player.Position.Z);
        // rotate
        Rotation = new Vector3(Rotation.X, _player.Rotation.Y, Rotation.Z);
        // camera offset
        Position = Position + (Basis * CameraOffset);
    }

    private void OnUIWindowChange(UIWindows newUIWindow)
    {
        switch (newUIWindow)
        {
            case UIWindows.None:
                _mouseLock = true;
                break;
            case UIWindows.MainMenu:
                _mouseLock = false;
                break;
            case UIWindows.Pause:
                _mouseLock = false;
                break;
        }
    }

    public override void _Ready()
    {
        //subscribe to UIWindowChange
        _uiManager = GetParent().GetParent().GetNode<UiManager>("UIManager");
        if (_uiManager != null)
        {
            _uiManager.UIWindowChange += OnUIWindowChange;
        }
        //Get player
        _player = GetParent().GetNode<CharacterBody3D>("Entity/Player");
        //
        ChangeCameraType(CameraTypes.OnPlayerFP);
    }

    //Mouse handler
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (CurrentCameraType != CameraTypes.None && _mouseLock)
            {
                _player.RotateY(-mouseMotion.Relative.X * Sensitivity / 100);

                Vector3 rotation = Rotation;

                rotation.X -= mouseMotion.Relative.Y * Sensitivity / 100;
                rotation.X = Math.Clamp(rotation.X, Mathf.DegToRad(-80), Mathf.DegToRad(80));
                rotation.Z = 0;

                Rotation = rotation;
            }
        }
    }

    public override void _Process(double delta)
    {
        switch (CurrentCameraType)
        {
            case CameraTypes.None:

                break;
            case CameraTypes.OnPlayerFP:
                CameraOnPlayerFP();
                break;
            case CameraTypes.OnPlayerTP:
                CameraOnPlayerTP();
                break;
            case CameraTypes.Free:

                break;
        }
    }
}
