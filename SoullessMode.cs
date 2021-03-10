using Modding;
using ModCommon.Util;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Collections.Generic;

namespace SoullessMode
{
    public class SoullessMode : Mod, ITogglableMod
    {
        public static Dictionary<string, GameObject> preloadedGameObjects = new Dictionary<string, GameObject> { };
        public override string GetVersion() => "0.1a";
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Crossroads_07", "Uninfected Parent/Fly 1")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            preloadedGameObjects.Add("Gruzzer", preloadedObjects["Crossroads_07"]["Uninfected Parent/Fly 1"]);
        }
        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
        {
            foreach(PlayMakerFSM fsm in Object.FindObjectsOfType<PlayMakerFSM>())
            {
                //Crawlid
                if (fsm.FsmName == "Crawler")
                {
                    fsm.gameObject.transform.localScale *= 1.25f;
                    fsm.gameObject.transform.localPosition += new Vector3(0, 1, 0);
                    fsm.GetAction<WalkLeftRight>("Walk", 1).walkSpeed = 15;
                    fsm.GetAction<WalkLeftRight>("Maintain", 0).walkSpeed = 15;
                    fsm.GetAction<Wait>("Start L", 1).time = 0.05f;
                    fsm.GetAction<Wait>("Start R", 1).time = 0.05f;
                }
                //Vengefly
                else if (fsm.FsmName == "chaser")
                {
                    fsm.gameObject.transform.localScale *= 1.25f;
                    fsm.GetAction<ChaseObject>("Chase - In Sight", 1).speedMax = 50f;
                    fsm.GetAction<ChaseObject>("Chase - In Sight", 1).acceleration = 2;
                    fsm.GetAction<Wait>("Stop", 4).time = 0.2f;
                }
                //Gruzzer
                else if (fsm.FsmName == "Bouncer Control")
                {
                    fsm.FsmVariables.FindFsmFloat("Speed").Value = 20f;
                }
                //Aspid Hunter
                else if (fsm.FsmName == "spitter")
                {
                    fsm.GetAction<DistanceFly>("Distance Fly", 2).speedMax = 15f;
                    fsm.GetAction<DistanceFly>("Distance Fly", 2).acceleration = 0.25f;
                    fsm.GetAction<WaitRandom>("Distance Fly", 4).timeMax = 0.5f;
                    fsm.GetAction<WaitRandom>("Distance Fly", 4).timeMin = 0.4f;
                    fsm.GetAction<DistanceFly>("Fly Back", 1).speedMax = 15f;
                    fsm.GetAction<DistanceFly>("Fly Back", 1).acceleration = 0.25f;
                    fsm.GetAction<Wait>("Fly Back", 0).time = 0.05f;
                    fsm.GetAction<DistanceFly>("Fire Anticipate", 0).speedMax = 10f;
                    fsm.GetAction<DistanceFly>("Fire Anticipate", 0).acceleration = 0.25f;
                    fsm.GetAction<FireAtTarget>("Fire", 2).speed = 40f;
                    fsm.GetAction<SpawnObjectFromGlobalPool>("Fire", 1).gameObject.Value.transform.localScale *= 2f;
                }
                //Aspid Mother
                /*else if (fsm.FsmName == "Hatcher")
                {
                    fsm.gameObject.transform.localScale *= 1.25f;
                    fsm.GetAction<DistanceFly>("Distance Fly", 5).speedMax = 10f;
                    fsm.GetAction<DistanceFly>("Distance Fly", 5).acceleration = 0.7f;
                    fsm.GetAction<RandomFloat>("Distance Fly", 3).min = 0.1f;
                    fsm.GetAction<RandomFloat>("Distance Fly", 3).max = 0.2f;
                    fsm.GetAction<Wait>("Distance Fly", 6).time = 0.1f;
                    fsm.GetAction<WaitRandom>("Distance Fly", 7).timeMin = 0.1f;
                    fsm.GetAction<WaitRandom>("Distance Fly", 7).timeMax = 0.2f;
                    fsm.GetAction<Wait>("Fire Anticipate", 2).time = 0.1f;
                    fsm.GetAction<IntAdd>("Fire", 9).add = 0;
                    fsm.ChangeTransition("Distance Fly", "WAIT", "Fire Anticipate");
                    fsm.ChangeTransition("Fire", "WAIT", "Fire Anticipate");
                }*/
                //Wandering Husk + Husk Bully + Husk Hornhead
                else if (fsm.FsmName == "Zombie Swipe")
                {
                    fsm.FsmVariables.FindFsmFloat("Lunge Speed").Value = 20f;
                    fsm.FsmVariables.FindFsmFloat("Idle Time").Value = 0.05f;
                    fsm.ChangeTransition("Lunge", "WAIT", "Reset");
                }
                //Leaping Husk
                else if (fsm.FsmName == "Zombie Leap")
                {
                    fsm.FsmVariables.FindFsmFloat("Jump X Speed").Value = 100f;
                    fsm.FsmVariables.FindFsmFloat("Idle Time").Value = 0.05f;
                    fsm.ChangeTransition("Lunge", "LAND", "Reset");
                }
                //Goam
                else if (fsm.FsmName == "Worm Control")
                {
                    fsm.GetAction<Wait>("Up", 3).time = 0.1f;
                    fsm.GetAction<Wait>("Down", 2).time = 0.1f;
                }
                //Baldur
                else if (fsm.FsmName == "Roller")
                {
                    fsm.FsmVariables.FindFsmFloat("Acceleration").Value = 3f;
                    fsm.FsmVariables.FindFsmFloat("Max Speed").Value = 30f;
                    fsm.FsmVariables.FindFsmFloat("Roll time Max").Value = 10000f;
                    fsm.FsmVariables.FindFsmFloat("Roll time Min").Value = 10f;
                }
                //Elder Baldur
                else if (fsm.FsmName == "Blocker Control")
                {
                    fsm.GetAction<WaitRandom>("Idle", 5).timeMin = 0.05f;
                    fsm.GetAction<WaitRandom>("Idle", 5).timeMax = 0.1f;
                    fsm.GetAction<SendRandomEvent>("Attack Choose", 0).weights = new HutongGames.PlayMaker.FsmFloat[] { 0.75f, 0.25f };
                    fsm.GetAction<SpawnObjectFromGlobalPool>("Fire", 4).gameObject.Value.transform.localScale *= 1.25f;
                    fsm.FsmVariables.FindFsmFloat("X Speed Max").Value = 45f;
                    fsm.FsmVariables.FindFsmFloat("X Speed Min").Value = 30f;
                    fsm.ChangeTransition("Roller Assign", "FINISHED", "Idle");
                }
                //False Knight
                else if (fsm.FsmName == "FalseyControl")
                {
                    fsm.gameObject.GetComponent<HealthManager>().hp = 100;

                    fsm.FsmVariables.FindFsmFloat("Idle Min").Value = 0.03f;
                    fsm.FsmVariables.FindFsmFloat("Idle Max").Value = 0.05f;
                    fsm.FsmVariables.FindFsmFloat("emissionRate").Value = 150;
                    fsm.FsmVariables.FindFsmFloat("emissionSpeed").Value = 15;
                    fsm.FsmVariables.FindFsmInt("Jump Barrel Min").Value = 15;
                    fsm.FsmVariables.FindFsmInt("Jump Barrel Max").Value = 16;
                    fsm.FsmVariables.FindFsmInt("Slam Barrel Min").Value = 15;
                    fsm.FsmVariables.FindFsmInt("Slam Barrel Max").Value = 16;
                    fsm.FsmVariables.FindFsmInt("Stunned Amount").Value = -2;

                    fsm.GetAction<SetFloatValue>("To Phase 2", 0).floatValue = 0.02f;
                    fsm.GetAction<SetFloatValue>("To Phase 2", 1).floatValue = 0.03f;
                    fsm.GetAction<SetIntValue>("To Phase 2", 2).intValue = 20;
                    fsm.GetAction<SetIntValue>("To Phase 2", 3).intValue = 21;
                    fsm.GetAction<SetIntValue>("To Phase 2", 4).intValue = 20;
                    fsm.GetAction<SetIntValue>("To Phase 2", 5).intValue = 21;
                    fsm.GetAction<SetFloatValue>("To Phase 3", 0).floatValue = 0.01f;
                    fsm.GetAction<SetFloatValue>("To Phase 3", 1).floatValue = 0.02f;
                    fsm.GetAction<SetIntValue>("To Phase 3", 2).intValue = 25;
                    fsm.GetAction<SetIntValue>("To Phase 3", 3).intValue = 26;
                    fsm.GetAction<SetIntValue>("To Phase 3", 4).intValue = 25;
                    fsm.GetAction<SetIntValue>("To Phase 3", 5).intValue = 26;
                    fsm.GetAction<Wait>("S Attack Antic", 1).time = 0.1f;
                    fsm.GetAction<Wait>("Rage", 3).time = 0.05f;
                    fsm.GetAction<Wait>("First Idle", 4).time = 0.05f;
                    fsm.GetAction<Wait>("Rage End", 4).time = 0.05f;
                    fsm.GetAction<SetGravity2dScale>("Jump", 3).gravityScale = 0.5f;
                    fsm.GetAction<SetGravity2dScale>("JA Jump", 2).gravityScale = 0.48f;
                    fsm.GetAction<SetGravity2dScale>("Idle", 1).gravityScale = 1.56f;

                }
                //Husk Guard
                else if (fsm.FsmName == "Zombie Guard")
                {
                    fsm.GetAction<Wait>("Idle", 12).time = 0.05f;
                    fsm.GetAction<SetFloatValue>("Check Left", 0).floatValue = -20f;
                    fsm.GetAction<SetFloatValue>("Check Left", 1).floatValue = -30f;
                    fsm.GetAction<SetFloatValue>("Check Right", 0).floatValue = 20f;
                    fsm.GetAction<SetFloatValue>("Check Right", 0).floatValue = 30f;
                    fsm.GetAction<SetFloatValue>("Jump L", 0).floatValue = 25f;
                    fsm.GetAction<SetFloatValue>("Jump R", 0).floatValue = -25f;
                    fsm.GetAction<SetFsmFloat>("Land", 10).setValue = 40f;
                    fsm.GetAction<SetFsmFloat>("Land", 14).setValue = 40f;
                    fsm.GetAction<Wait>("Attack", 2).time = 0.05f;
                    fsm.GetAction<Wait>("Attack Recoil", 4).time = 0.05f;
                    fsm.GetAction<Wait>("Cooldown", 1).time = 0.01f;
                    fsm.GetAction<SetScale>("Land", 8).x = 2f;
                    fsm.GetAction<SetScale>("Land", 12).x = 2f;
                    fsm.GetAction<Wait>("Stomp Antic", 3).time = 0.05f;
                    fsm.GetAction<Wait>("Stomp Cooldown", 1).time = 0.02f;

                    foreach (var i in fsm.gameObject.GetComponent<tk2dSpriteAnimator>().Library.clips)
                    {
                        if (i.name == "Attack2")
                        {
                            i.fps *= 2f;
                        }
                        i.fps *= 1f;
                    }
                }
                //Gruz Mother
                else if (fsm.FsmName == "Big Fly Control")
                {
                    fsm.FsmVariables.FindFsmFloat("Slam Speed").Value = 100f;
                    fsm.GetAction<Wait>("Fly", 3).time = 0.05f;
                    fsm.GetAction<RandomFloat>("Buzz", 3).min = 0.25f;
                    fsm.GetAction<RandomFloat>("Buzz", 3).max = 0.75f;
                    fsm.GetAction<Wait>("Slam Antic", 12).time = 0.05f;
                    fsm.GetAction<RandomFloat>("Slam Antic", 9).min = 4f;
                    fsm.GetAction<RandomFloat>("Slam Antic", 9).max = 5f;
                    fsm.GetAction<Wait>("Charge Antic", 11).time = 0.1f;
                    fsm.gameObject.AddComponent<GruzMotherSpawner>();
                }
                else if(fsm.FsmName == "bouncercontrol")
                {
                    fsm.FsmVariables.FindFsmFloat("Speed").Value = 20f;
                }
                //Husk Warrior
                else if (fsm.FsmName == "ZombieShieldControl")
                {
                    fsm.GetAction<Wait>("Block Low", 1).time = 0.05f;
                    fsm.GetAction<Wait>("Block High", 1).time = 0.05f;
                    fsm.GetAction<SetFloatValue>("Shield Left High", 7).floatValue = -45f;
                    fsm.GetAction<SetFloatValue>("Shield Left High", 8).floatValue = -55f;
                    fsm.GetAction<SetFloatValue>("Shield Left Low", 8).floatValue = -45f;
                    fsm.GetAction<SetFloatValue>("Shield Left Low", 9).floatValue = -55f;
                    fsm.GetAction<SetFloatValue>("Shield Right High", 7).floatValue = 45f;
                    fsm.GetAction<SetFloatValue>("Shield Right High", 8).floatValue = 55f;
                    fsm.GetAction<SetFloatValue>("Shield Right Low", 7).floatValue = 45f;
                    fsm.GetAction<SetFloatValue>("Shield Right Low", 8).floatValue = 55f;
                    foreach (var i in fsm.gameObject.GetComponent<tk2dSpriteAnimator>().Library.clips)
                    {
                        if (i.name == "Attack1 A" || i.name == "Attack1 L" || i.name == "Attack1 S" || i.name == "Attack1 CD" || i.name == "Attack3 A1" || i.name == "Attack3 L1" || i.name == "Attack3 S1" || i.name == "Attack3 CD1" || i.name == "Attack3 L2" || i.name == "Attack3 CD2" || i.name == "Attack3 L3" || i.name == "Attack3 S3" || i.name == "Attack3 CD3")
                        {
                            i.fps *= 1.75f;
                        }
                    }
                }
            }

            //Tiktik
            foreach(Climber climber in GameObject.FindObjectsOfType<Climber>())
            {
                climber.speed *= 2.5f;
            }
        }

        public void Unload()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
    }
}
