using Fougerite;
using UnityEngine;

namespace WhoIS
{
    public class Loader : Module
    {
        public override void Initialize()
        {
            Hooks.OnCommand += Whois;
        }

        public override string Author => "MeshBenth, DreTaX, Jakkee";

        public override string Name => "WhoIS";

        public override void DeInitialize()
        {
            Hooks.OnCommand -= Whois;
        }

        public void Whois(Fougerite.Player player, string cmd, string[] args)
        {
            switch (cmd)
            {
                case "who":
                    var position = player.PlayerClient.controllable.character.transform.position;
                    var direction = player.PlayerClient.controllable.character.eyesRay.direction;
                    position.y += player.PlayerClient.controllable.character.stateFlags.crouch ? 0.85f : 1.65f;
                    if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(new Ray(position, direction), out var raycastHit, 10, -1, out var flag, out var meshBatchInstance))
                    {
                        var idmain = flag ? meshBatchInstance.idMain : IDBase.GetMain(raycastHit.collider);
                        if (idmain != null)
                        {
                            if (idmain.gameObject.GetComponent<StructureComponent>())
                            {
                                var id = idmain.gameObject.GetComponent<StructureComponent>()._master.ownerID;
                                var user = Fougerite.Player.FindByGameID(id.ToString());
                                player.Message(idmain.gameObject.name + " belongs to " + user.Name);
                                if (player.Admin)
                                {
                                    player.Message("Location: " + idmain.gameObject.transform.position);
                                    player.Message("Health: " + idmain.gameObject.GetComponent<TakeDamage>().health +
                                                   "/" + idmain.gameObject.GetComponent<TakeDamage>().maxHealth);
                                }
                            }
                            if (idmain.gameObject.GetComponent<DeployableObject>())
                            {
                                var id = idmain.gameObject.GetComponent<DeployableObject>().ownerID;
                                var user = Fougerite.Player.FindByGameID(id.ToString());
                                player.Message(idmain.gameObject.name + " belongs to " + user.Name);
                                if (player.Admin)
                                {
                                    player.Message("Location: " + idmain.gameObject.transform.position);
                                    player.Message("Health: " + idmain.gameObject.GetComponent<TakeDamage>().health +
                                                   "/" + idmain.gameObject.GetComponent<TakeDamage>().maxHealth);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
