using BepInEx;
using RoR2;
using RoR2.ContentManagement;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VanillaItemsRevamped.Items;

namespace VanillaItemsRevamped
{
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class Main : BaseUnityPlugin
  {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Nuxlar";
    public const string PluginName = "VanillaItemsRevamped";
    public const string PluginVersion = "1.0.0";

    internal static Main Instance { get; private set; }
    public static string PluginDirectory { get; private set; }

    public static ItemDef emptyItemDef = null;
    public static GameObject warbannerWard;
    public static Sprite infusionIcon;

    public void Awake()
    {
      Instance = this;

      Log.Init(Logger);
      LoadAssets();

      PluginDirectory = System.IO.Path.GetDirectoryName(Info.Location);
      LanguageFolderHandler.Register(PluginDirectory);

      new ItemsCore();
    }

    private static void LoadAssets()
    {
      AssetReferenceT<GameObject> warbannerRef = new AssetReferenceT<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_WardOnLevel.WarbannerWard_prefab);
      AssetAsyncReferenceManager<GameObject>.LoadAsset(warbannerRef).Completed += x => warbannerWard = x.Result;

      AssetReferenceT<Sprite> infusionIconRef = new AssetReferenceT<Sprite>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Infusion.texInfusionIcon_png);
      AssetAsyncReferenceManager<Sprite>.LoadAsset(infusionIconRef).Completed += x => infusionIcon = x.Result;
    }

    /*
        private static void TweakAssets()
        {
          Example for how to edit an asset once it finishes loading
          AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<Material>(RoR2BepInExPack.GameAssetPaths.RoR2_DLC2_Items_LowerPricedChests.PickupSaleStar_prefab)).Completed += delegate (AsyncOperationHandle<GameObject> obj)
          {
            MeshCollider collider = obj.Result.transform.find("SaleStar")?.GetComponent<MeshCollider>();
            if (collider)
            {
              collider.convex = true;
            }
          };
        }
    */
  }
}