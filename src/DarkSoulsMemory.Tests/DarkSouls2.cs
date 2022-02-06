using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using DarkSoulsMemory.DarkSouls2;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Testing.Tas;

namespace Testing
{
    [TestFixture]
    internal class DarkSouls2
    {
        private static DarkSouls2ToolAssistant _assistant;
        private static DarkSoulsMemory.DarkSouls2.DarkSouls2 _darkSouls2; 
        private static string SaveFileLocation = @"C:\Users\Frank\AppData\Roaming\DarkSoulsII\0110000104593e46";
        private static string SaveFileName = @"DS2SOFS0000.sl2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var ReplacementSave = "Sotfs";

            //Rename current save
            if (File.Exists(SaveFileLocation + "\\" + SaveFileName))
            {
                if (File.Exists(SaveFileLocation + "\\automated_backup"))
                {
                    File.Delete(SaveFileLocation + "\\automated_backup");
                }
                File.Move(SaveFileLocation + "\\" + SaveFileName, SaveFileLocation + "\\automated_backup");
            }
            //Replace with test save
            File.Move(Environment.CurrentDirectory + $@"\saves\DarkSouls2\{ReplacementSave}\ThingsBetwixt", SaveFileLocation + "\\" + SaveFileName);




            _assistant = new DarkSouls2ToolAssistant(GameType.DarkSouls2Sotfs);
            _darkSouls2 = new DarkSoulsMemory.DarkSouls2.DarkSouls2();
            _darkSouls2.Refresh();

            //Cheaty
            _darkSouls2.DisableAllAi = true;
            _darkSouls2.RightWeapon1DamageMultiplier = 9999.0f;
            _assistant.MainMenuContinue();
        }




        public delegate void KillBossMethod();

        public static List<(BossType, KillBossMethod)> BossKills = new List<(BossType, KillBossMethod)>()
        {
            (BossType.TheLastGiant                                             , KillTheLastGiant),
            (BossType.ThePursuer                                               , KillThePursuer                                                 ),
            (BossType.Dragonrider                                              , KillDragonrider                                                ),
            (BossType.OldDragonSlayer                                          , KillOldDragonSlayer                                            ),
            (BossType.FlexileSentry                                            , KillFlexileSentry                                              ),

            (BossType.RuinSentinels                                            , KillRuinSentinels                                              ),
            (BossType.BelfryGargoyles                                          , KillBelfryGargoyles                                            ),
            (BossType.LostSinner                                               , KillLostSinner                                                 ),

            (BossType.ExecutionersChariot                                      , KillExecutionersChariot                                        ),
            (BossType.TheSkeletonLords                                         , KillTheSkeletonLords                                           ),
            
            (BossType.CovetousDemon                                            , KillCovetousDemon                                              ),
            (BossType.MythaTheBanefulQueen                                     , KillMythaTheBanefulQueen                                       ),

            (BossType.SmelterDemon                                             , KillSmelterDemon                                               ),
            (BossType.OldIronKing                                              , KillOldIronKing                                                ),

            (BossType.ScorpionessNajka                                         , KillScorpionessNajka                                           ),
            (BossType.RoyalRatAuthority                                        , KillRoyalRatAuthority                                          ),
            (BossType.ProwlingMagnusAndCongregation                            , KillProwlingMagnusAndCongregation                              ),
            (BossType.TheDukesDearFreja                                        , KillTheDukesDearFreja                                          ),

            (BossType.RoyalRatVanguard                                         , KillRoyalRatVanguard                                           ),
            (BossType.TheRotten                                                , KillTheRotten                                                  ),

            (BossType.TwinDragonriders                                         , KillTwinDragonriders                                           ), 
            (BossType.Darklurker                                               , KillDarklurker                                                 ),
            (BossType.LookingGlassKnight                                       , KillLookingGlassKnight                                         ),

            (BossType.DemonOfSong                                              , KillDemonOfSong                                                ),
            (BossType.VelstadtTheRoyalAegis                                    , KillVelstadtTheRoyalAegis                                      ),
            (BossType.Vendrick                                                 , KillVendrick                                                   ),

            (BossType.GuardianDragon                                           , KillGuardianDragon                                             ),                      
            (BossType.AncientDragon                                            , KillAncientDragon                                              ),
            (BossType.GiantLord                                                , KillGiantLord                                                  ),

            (BossType.ThroneWatcherAndThroneDefender                           , KillThroneWatcherAndThroneDefender                             ),                      
            (BossType.Nashandra                                                , KillNashandra                                                  ),                      
            (BossType.AldiaScholarOfTheFirstSin                                , KillAldiaScholarOfTheFirstSin                                  ),

            //Crown of the Sunken king                  
            (BossType.ElanaSqualidQueen                                        , KillElanaSqualidQueen                                          ),
            (BossType.SinhTheSlumberingDragon                                  , KillSinhTheSlumberingDragon                                    ),     
            (BossType.AfflictedGraverobberAncientSoldierVargCerahTheOldExplorer, KillAfflictedGraverobberAncientSoldierVargCerahTheOldExplorer  ),    
                
            //Crown of the old iron king
            (BossType.BlueSmelterDemon                                         , KillBlueSmelterDemon                                           ),                                             
            (BossType.Fumeknight                                               , KillFumeknight                                                 ),                                             
            (BossType.SirAlonne                                                , KillSirAlonne                                                  ),                                             
        
            //Crown of the Ivory king                                                                               
            (BossType.BurntIvoryKing                                           , KillBurntIvoryKing                                             ),                                   
            (BossType.AavaTheKingsPet                                          , KillAavaTheKingsPet                                            ),    
            (BossType.LudAndZallenTheKingsPets                                 , KillLudAndZallenTheKingsPets                                   ),
        };

        [TestCaseSource(nameof(BossKills))]
        public void BossKill((BossType, KillBossMethod) param)
        {
            var bossType = param.Item1;
            var func = param.Item2;

            var stateBefore = _darkSouls2.GetBossKillCount(bossType);
            func();
            var stateAfter = _darkSouls2.GetBossKillCount(bossType);

            Assert.AreEqual(0, stateBefore, "Initial boss kill count not 0");
            Assert.AreEqual(1, stateAfter, "Boss kill count not 1 after killing");
        }

        #region Boss kills

        private static void KillTheLastGiant()
        {
            _darkSouls2.Refresh();
            Warp(WarpType.CardinalTower);

            Teleport(-144.2992f, 93.41512f, -40.57851f, 0.99f, 0.17f, 0.0f);
            EnterFogGate();
            _assistant.SkipCutscene();

            Teleport(-127.565f, 98.29201f, -40.52358f, 0.55f, -0.83f, 0.0f);
            _assistant.Punch();
            Thread.Sleep(7000);
        }

        private static void KillThePursuer(){}
        private static void KillDragonrider() { }
        private static void KillOldDragonSlayer() { }
        private static void KillFlexileSentry() { }
        private static void KillRuinSentinels() { }
        private static void KillBelfryGargoyles() { }
        private static void KillLostSinner() { }
        private static void KillExecutionersChariot() { }
        private static void KillTheSkeletonLords() { }
        private static void KillCovetousDemon() { }
        private static void KillMythaTheBanefulQueen() { }
        private static void KillSmelterDemon() { }
        private static void KillOldIronKing() { }
        private static void KillScorpionessNajka() { }
        private static void KillRoyalRatAuthority() { }
        private static void KillProwlingMagnusAndCongregation() { }
        private static void KillTheDukesDearFreja() { }
        private static void KillRoyalRatVanguard() { }
        private static void KillTheRotten() { }
        private static void KillTwinDragonriders() { }
        private static void KillDarklurker() { }
        private static void KillLookingGlassKnight() { }
        private static void KillDemonOfSong() { }
        private static void KillVelstadtTheRoyalAegis() { }
        private static void KillVendrick() { }
        private static void KillGuardianDragon() { }
        private static void KillAncientDragon() { }
        private static void KillGiantLord() { }
        private static void KillThroneWatcherAndThroneDefender() { }
        private static void KillNashandra() { }
        private static void KillAldiaScholarOfTheFirstSin() { }
        private static void KillElanaSqualidQueen() { }
        private static void KillSinhTheSlumberingDragon() { }
        private static void KillAfflictedGraverobberAncientSoldierVargCerahTheOldExplorer() { }
        private static void KillBlueSmelterDemon() { }
        private static void KillFumeknight() { }
        private static void KillSirAlonne() { }
        private static void KillBurntIvoryKing() { }
        private static void KillAavaTheKingsPet() { }
        private static void KillLudAndZallenTheKingsPets() { }



        #endregion



        #region Helpers

        private static void EnterFogGate()
        {
            _assistant.Interact();
            Thread.Sleep(5000);
        }

        private static void Teleport(float x, float y, float z, float angX = 0.0f, float angY = 0.0f, float angZ = 0.0f)
        {
            _darkSouls2.StableX = x;
            _darkSouls2.StableY = y;
            _darkSouls2.StableZ = z;
            _darkSouls2.AngX = angX;
            _darkSouls2.AngY = angY;
            _darkSouls2.AngZ = angZ;
            Thread.Sleep(500);
        }

        private static void Warp(WarpType warpType)
        {
            _darkSouls2.Warp(warpType);
            Thread.Sleep(7000);
        }

        #endregion

    }
}
