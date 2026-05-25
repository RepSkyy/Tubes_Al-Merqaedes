using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Leopard2RI : Bot
{
    private int moveDirection = 1;

    // Target terdekat
    private int targetId = -1;
    private double targetDistance = double.MaxValue;

    static void Main(string[] args)
    {
        new Leopard2RI().Start();
    }

    Leopard2RI() : base(BotInfo.FromFile("Leopard2RI.json"))
    {
    }

    public override void Run()
    {
        BodyColor = Color.Black;
        RadarColor = Color.Lime;
        GunColor = Color.Black;
        BulletColor = Color.Orange;

        AdjustGunForBodyTurn = true;
        AdjustRadarForGunTurn = false;
        AdjustRadarForBodyTurn = false;

        // Radar muter terus
        SetTurnGunRight(double.PositiveInfinity);

        while (IsRunning)
        {
            // Hindari tembok
            AvoidWall();

            // Gerak utama
            SetForward(250 * moveDirection);

            Go();
        }
    }

    private void AvoidWall()
    {
        double margin = 120;

        // Dekat pojok
        if ((X < margin && Y < margin) ||
            (X < margin && Y > ArenaHeight - margin) ||
            (X > ArenaWidth - margin && Y < margin) ||
            (X > ArenaWidth - margin && Y > ArenaHeight - margin))
        {
            SetTurnRight(90);
        }

        // Dekat tembok kiri
        else if (X < margin)
        {
            SetTurnRight(90);
        }

        // Dekat tembok kanan
        else if (X > ArenaWidth - margin)
        {
            SetTurnLeft(90);
        }

        // Dekat tembok bawah
        else if (Y < margin)
        {
            SetTurnRight(90);
        }

        // Dekat tembok atas
        else if (Y > ArenaHeight - margin)
        {
            SetTurnLeft(90);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double distance = DistanceTo(e.X, e.Y);

        // Pilih musuh paling dekat
        if (targetId == -1)
        {
            targetId = e.ScannedBotId;
            targetDistance = distance;
        }

        if (distance < targetDistance)
        {
            targetId = e.ScannedBotId;
            targetDistance = distance;
        }

        // Fokus hanya ke target terpilih
        if (e.ScannedBotId == targetId)
        {
            targetDistance = distance;

            if (GunHeat == 0)
            {
                Fire(2);
            }
        }

        Rescan();
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        // Cari target baru jika target lama mati
        if (e.VictimId == targetId)
        {
            targetId = -1;
            targetDistance = double.MaxValue;
        }
    }

    public override void OnHitBot(HitBotEvent e)
    {
        if (GunHeat == 0)
        {
            Fire(3);
        }

        ReverseDirection();
    }

    private void ReverseDirection()
    {
        moveDirection *= -1;

        Back(150);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        moveDirection *= -1;

        SetForward(150 * moveDirection);
    }
}
