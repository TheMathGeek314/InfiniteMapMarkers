using Modding;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteMapMarkers {
    public class InfiniteMapMarkers: Mod {
        new public string GetName() => "InfiniteMapMarkers";
        public override string GetVersion() => "1.0.0.0";

        static GameObject bluePrefab, redPrefab, yellowPrefab, whitePrefab;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            On.GameMap.Start += gameMapStart;
            On.GameMap.DisableMarkers += disableMarkers;
            IL.MapMarkerMenu.Open += openIL;
            IL.MapMarkerMenu.PlaceMarker += placeIL;
            IL.MapMarkerMenu.RemoveMarker += removeIL;
            IL.MapMarkerMenu.UpdateAmounts += updateAmountsIL;
        }

        private void disableMarkers(On.GameMap.orig_DisableMarkers orig, GameMap self) {
            string[] colors = { "b", "r", "y", "w" };
            foreach(string color in colors) {
                int listCount = PlayerData.instance.GetVariable<List<Vector3>>($"placedMarkers_{color}").Count;
                ref GameObject[] array = ref getArray(self, color);
                if(array.Length < listCount) {
                    GameObject[] tempList = new GameObject[listCount];
                    for(int i = 0; i < listCount; i++) {
                        if(i < array.Length) {
                            tempList[i] = array[i];
                        }
                        else {
                            GameObject prefab = findMarkerPrefab(color);
                            tempList[i] = GameObject.Instantiate(prefab, prefab.transform.parent);
                            tempList[i].name = color.ToUpper()+(i+1);
                            tempList[i].GetComponentInChildren<InvMarker>().id = i;
                        }
                    }
                    array = tempList;
                }
            }
            orig(self);
        }

        private ref GameObject[] getArray(GameMap self, string color) {
            switch(color) {
                case "b":
                    return ref self.mapMarkersBlue;
                case "r":
                    return ref self.mapMarkersRed;
                case "y":
                    return ref self.mapMarkersYellow;
                case "w":
                    return ref self.mapMarkersWhite;
                default:
                    return ref self.mapMarkersBlue;
            }
        }

        private GameObject findMarkerPrefab(string color) {
            switch(color) {
                case "b":
                    return bluePrefab;
                case "r":
                    return redPrefab;
                case "y":
                    return yellowPrefab;
                case "w":
                    return whitePrefab;
                default:
                    return bluePrefab;
            }
        }

        private void gameMapStart(On.GameMap.orig_Start orig, GameMap self) {
            orig(self);
            bluePrefab = self.gameObject.transform.Find("Map Markers/B1").gameObject;
            redPrefab = self.gameObject.transform.Find("Map Markers/R1").gameObject;
            yellowPrefab = self.gameObject.transform.Find("Map Markers/Y1").gameObject;
            whitePrefab = self.gameObject.transform.Find("Map Markers/W1").gameObject;
        }

        private void openIL(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_b"),
                            i => i.MatchCallvirt<PlayerData>("GetInt"),
                            i => i.MatchLdcI4(0));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_r"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_y"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_w"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
        }

        private void placeIL(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            // if(this.pd.GetInt("spareMarkers_x") > 0)
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_b"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_r"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_y"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_w"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });

            // playerData.SetIntSwappedArgs(playerData.GetInt("spareMarkers_x") - 1, "spareMarkers_x");
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_b"));
            cursor.GotoNext(i => i.MatchSub());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_r"));
            cursor.GotoNext(i => i.MatchSub());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_y"));
            cursor.GotoNext(i => i.MatchSub());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_w"));
            cursor.GotoNext(i => i.MatchSub());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
        }

        private void removeIL(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_b"));
            cursor.GotoNext(i => i.MatchAdd());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_r"));
            cursor.GotoNext(i => i.MatchAdd());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_y"));
            cursor.GotoNext(i => i.MatchAdd());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_w"));
            cursor.GotoNext(i => i.MatchAdd());
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return 0; });
        }

        private void updateAmountsIL(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            // this.amount_x.text = this.pd.spareMarkers_x.ToString();
            cursor.GotoNext(i => i.MatchCallvirt<TMPro.TMP_Text>("set_text"));
            cursor.EmitDelegate<Func<string, string>>(j => { return "∞"; });
            cursor.GotoNext(i => i.MatchLdfld<MapMarkerMenu>("amount_r"));
            cursor.GotoNext(i => i.MatchCallvirt<TMPro.TMP_Text>("set_text"));
            cursor.EmitDelegate<Func<string, string>>(j => { return "∞"; });
            cursor.GotoNext(i => i.MatchLdfld<MapMarkerMenu>("amount_y"));
            cursor.GotoNext(i => i.MatchCallvirt<TMPro.TMP_Text>("set_text"));
            cursor.EmitDelegate<Func<string, string>>(j => { return "∞"; });
            cursor.GotoNext(i => i.MatchLdfld<MapMarkerMenu>("amount_w"));
            cursor.GotoNext(i => i.MatchCallvirt<TMPro.TMP_Text>("set_text"));
            cursor.EmitDelegate<Func<string, string>>(j => { return "∞"; });

            // if(this.pd.GetInt("spareMarkers_x") > 0)
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_r"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_y"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
            cursor.GotoNext(i => i.MatchLdstr("spareMarkers_w"));
            cursor.GotoNext(i => i.Match(OpCodes.Ble_S));
            cursor.EmitDelegate<Func<Int32, Int32>>(j => { return -1; });
        }
    }
}