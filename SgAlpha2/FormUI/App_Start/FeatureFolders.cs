using System.Linq;
using System.Web.Mvc;

namespace FormUI.App_Start
{
    public class FeatureFolders
    {
        public static void Register(ViewEngineCollection viewEngines)
        {
            var engine = new MvcFoldersRazorViewEngine("Controllers");
            viewEngines.Clear();
            viewEngines.Add(engine);
        }
    }

    public class MvcFoldersRazorViewEngine : RazorViewEngine
    {
        public MvcFoldersRazorViewEngine(string rootFolder)
        {
            ViewLocationFormats = new string[0];
            MasterLocationFormats = new string[0];
            PartialViewLocationFormats = new string[0];

            var fileExtensions = new string[] { "cshtml" };

            foreach (var fileExtension in fileExtensions)
            {
                var folderViewLocations = new string[]
                {
                    "~/" + rootFolder + "/{1}/{0}." + fileExtension,
                    "~/" + rootFolder + "/Shared/{0}." + fileExtension,
                };

                ViewLocationFormats = folderViewLocations.Union(ViewLocationFormats).ToArray();
                MasterLocationFormats = folderViewLocations.Union(MasterLocationFormats).ToArray();
                PartialViewLocationFormats = folderViewLocations.Union(PartialViewLocationFormats).ToArray();

                var areaFolderViewLocations = new string[]
                {
                    "~/" + rootFolder + "/{2}/{1}/{0}." + fileExtension,
                    "~/" + rootFolder + "/Shared/{0}." + fileExtension,
                };

                AreaViewLocationFormats = areaFolderViewLocations.Union(AreaViewLocationFormats).ToArray();
                AreaMasterLocationFormats = areaFolderViewLocations.Union(AreaMasterLocationFormats).ToArray();
                AreaPartialViewLocationFormats = areaFolderViewLocations.Union(AreaPartialViewLocationFormats).ToArray();
            }
        }
    }
}