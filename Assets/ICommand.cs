using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public interface ICommand
{
    void Execute();
}

public class MoveCommand : ICommand
{
    playerController player;
    Vector3 direction;

    public MoveCommand(playerController plr, Vector3 dir)
    {
        player = plr;
        direction = dir;
    }
    public void Execute()
    {
        player.PlayerMove(direction);
    }

}
public class JumpCommand : ICommand
{
    playerController player;


    public JumpCommand(playerController plr, Vector3 dir)
    {
        player = plr;

    }
    public void Execute()
    {
        player.PlayerJump();
    }

}
//public class InteractionCommand : ICommand
//{
//    private IInteractable target;
//    playerController player;
//
//    public InteractCommand(IInteractable target)
//    {
//        this.target = target;
//        this.player = target.player;
//    }
//
//
//    public void Execute()
//    {
//
//    }
//}

