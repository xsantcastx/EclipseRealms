using System.Collections.Generic;
using EclipseRealms.Core.Combat;
using EclipseRealms.Inventory.Data;
using EclipseRealms.Items;
using EclipseRealms.Items.Charms;
using EclipseRealms.Items.Runes;
using EclipseRealms.Items.Runewords;
using UnityEditor;
using UnityEngine;

namespace EclipseRealms.EditorTools
{
    public static class EclipseRealmsSampleAssetsGenerator
    {
        private const string ItemPath = "Assets/Items/";
        private const string RunePath = "Assets/Items/Runes/";
        private const string RunewordPath = "Assets/Items/Runewords/";
        private const string CharmPath = "Assets/Items/Charms/";
        private const string InventoryPath = "Assets/Inventory/Data/";

        [MenuItem("Tools/Eclipse Realms/Generate Sample Assets")]
        public static void Generate()
        {
            EnsureFolders();
            CreateSampleItem("item_bronze_sabre", "Bronze Sabre", ItemSlot.Weapon, ItemRarity.Common, sockets: 1, str: 5, dex: 2);
            CreateSampleItem("item_moonveil_guard", "Moonveil Guard", ItemSlot.Offhand, ItemRarity.Rare, sockets: 2, vit: 4, wis: 3);

            RuneDefinition lightRune = CreateRune("rune_sol", "Sol", RuneAlignment.Light, tier: 1, "Adds +5% healing.");
            RuneDefinition shadowRune = CreateRune("rune_noct", "Noct", RuneAlignment.Shadow, tier: 2, "Adds +4% lifesteal.");
            CreateRuneword("runeword_eclipse", "Eclipse Sigil", ItemSlot.Weapon, new[] { lightRune, shadowRune }, "Combines Sol + Noct for dual-state damage buff.");

            CreateCharm("charm_twilight_petal", "Twilight Petal", RealmAlignment.Neutral, CharmStability.Permanent, 7f,
                "Light: +10% Aether regen.", "Shadow: +5% damage.");

            CreateInventoryTemplate();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Eclipse Realms", "Sample assets generated.", "OK");
        }

        private static void EnsureFolders()
        {
            foreach (string path in new[] { ItemPath, RunePath, RunewordPath, CharmPath, InventoryPath })
            {
                if (!AssetDatabase.IsValidFolder(path.TrimEnd('/')))
                {
                    string parent = path.TrimEnd('/');
                    string[] parts = parent.Split('/');
                    string current = "";
                    foreach (string part in parts)
                    {
                        string next = string.IsNullOrEmpty(current) ? part : $"{current}/{part}";
                        if (!AssetDatabase.IsValidFolder(next))
                        {
                            string baseFolder = string.IsNullOrEmpty(current) ? "Assets" : current;
                            AssetDatabase.CreateFolder(baseFolder, part);
                        }
                        current = next;
                    }
                }
            }
        }

        private static void CreateSampleItem(string id, string assetName, ItemSlot slot, ItemRarity rarity, int sockets, int str = 0, int dex = 0, int vit = 0, int wis = 0)
        {
            ItemDefinition asset = ScriptableObject.CreateInstance<ItemDefinition>();
            asset.name = assetName;

            typeof(ItemDefinition).GetField("itemId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, id);
            typeof(ItemDefinition).GetField("slot", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, slot);
            typeof(ItemDefinition).GetField("rarity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, rarity);
            typeof(ItemDefinition).GetField("sockets", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, sockets);

            var statField = typeof(ItemDefinition).GetField("statBonuses", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (statField != null)
            {
                var statBlock = new Core.Stats.StatBlock
                {
                    Strength = str,
                    Dexterity = dex,
                    Vitality = vit,
                    Wisdom = wis
                };
                statField.SetValue(asset, statBlock);
            }

            AssetDatabase.CreateAsset(asset, $"{ItemPath}{assetName}.asset");
        }

        private static RuneDefinition CreateRune(string id, string assetName, RuneAlignment alignment, int tier, string description)
        {
            RuneDefinition rune = ScriptableObject.CreateInstance<RuneDefinition>();
            typeof(RuneDefinition).GetField("runeId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(rune, id);
            typeof(RuneDefinition).GetField("alignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(rune, alignment);
            typeof(RuneDefinition).GetField("tier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(rune, tier);
            typeof(RuneDefinition).GetField("effectSummary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(rune, description);

            AssetDatabase.CreateAsset(rune, $"{RunePath}{assetName}.asset");
            return rune;
        }

        private static void CreateRuneword(string id, string assetName, ItemSlot slot, RuneDefinition[] sequence, string effect)
        {
            RunewordDefinition runeword = ScriptableObject.CreateInstance<RunewordDefinition>();
            typeof(RunewordDefinition).GetField("runewordId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(runeword, id);
            typeof(RunewordDefinition).GetField("targetSlot", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(runeword, slot);
            typeof(RunewordDefinition).GetField("sequence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(runeword, new System.Collections.Generic.List<RuneDefinition>(sequence));
            typeof(RunewordDefinition).GetField("effectDescription", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(runeword, effect);

            AssetDatabase.CreateAsset(runeword, $"{RunewordPath}{assetName}.asset");
        }

        private static void CreateCharm(string id, string assetName, RealmAlignment alignment, CharmStability stability, float shiftMod, string lightEffect, string shadowEffect)
        {
            CharmDefinition charm = ScriptableObject.CreateInstance<CharmDefinition>();
            typeof(CharmDefinition).GetField("charmId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, id);
            typeof(CharmDefinition).GetField("alignmentBias", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, alignment);
            typeof(CharmDefinition).GetField("stability", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, stability);
            typeof(CharmDefinition).GetField("realmShiftModifier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, shiftMod);
            typeof(CharmDefinition).GetField("lightEffect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, lightEffect);
            typeof(CharmDefinition).GetField("shadowEffect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(charm, shadowEffect);

            AssetDatabase.CreateAsset(charm, $"{CharmPath}{assetName}.asset");
        }

        private static void CreateInventoryTemplate()
        {
            PlayerInventory inventory = ScriptableObject.CreateInstance<PlayerInventory>();
            typeof(PlayerInventory).GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(inventory, new System.Collections.Generic.List<InventorySlot>());
            AssetDatabase.CreateAsset(inventory, $"{InventoryPath}PrototypeInventory.asset");
        }
    }
}
