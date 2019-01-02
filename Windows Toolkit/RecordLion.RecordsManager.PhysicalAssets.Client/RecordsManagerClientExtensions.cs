using Newtonsoft.Json;
using RecordLion.RecordsManager.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public static class RecordsManagerClientExtensions
    {
        #region Containers

        public static string GetAllContainersAsJson(this IRecordsManagerClient rmClient)
        {
            return rmClient.GetAllContainersAsJson(0, 0);
        }


        public static string GetAllContainersAsJson(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get(GET_CONTAINERS_ALL.FormatResourceUrl(page, pageSize));
        }


        public static string GetContainersAsJson(this IRecordsManagerClient rmClient, Guid? parentId = null)
        {
            return rmClient.GetContainersAsJson(0, 0, parentId);
        }


        public static string GetContainersAsJson(this IRecordsManagerClient rmClient, int page, int pageSize, Guid? parentId = null)
        {
            return rmClient.Get(GET_CONTAINERS.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }


        public static IEnumerable<Container> GetContainersFromJson(this IRecordsManagerClient rmClient, string json)
        {
            var page = rmClient.GetContainersWithPageDataFromJson(json);

            return page.Items;
        }


        public static IClientPagedItems<Container> GetContainersWithPageDataFromJson(this IRecordsManagerClient rmClient, string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<Container>>(json);
        }


        public static DateTime GetContainerLastEdit(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<DateTime>(GET_CONTAINERS_LASTEDIT.FormatResourceUrl());
        }


        public static IEnumerable<Container> SearchContainers(this IRecordsManagerClient rmClient, string title)
        {
            var page = rmClient.SearchContainers(title, 0, 0);

            return page.Items;
        }


        public static IClientPagedItems<Container> SearchContainers(this IRecordsManagerClient rmClient, string title, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Container>>(GET_CONTAINERS_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }


        public static IEnumerable<Container> GetAllContainers(this IRecordsManagerClient rmClient)
        {
            var page = rmClient.GetAllContainers(0, 0);

            return page.Items;
        }


        public static IClientPagedItems<Container> GetAllContainers(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Container>>(GET_CONTAINERS_ALL.FormatResourceUrl(page, pageSize));
        }


        public static IEnumerable<Container> GetContainers(this IRecordsManagerClient rmClient, Guid? parentId = null)
        {
            var page = rmClient.GetContainers(0, 0, parentId);

            return page.Items;
        }


        public static IClientPagedItems<Container> GetContainers(this IRecordsManagerClient rmClient, int page, int pageSize, Guid? parentId = null)
        {
            return rmClient.Get<ClientPagedItems<Container>>(GET_CONTAINERS.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }


        public static Container GetContainer(this IRecordsManagerClient rmClient, Guid id)
        {
            return rmClient.Get<Container>(GET_CONTAINER_WITH_ID.FormatResourceUrl(id));
        }


        public static Container GetContainer(this IRecordsManagerClient rmClient, string barcode)
        {
            return rmClient.Get<Container>(GET_CONTAINER_WITH_BARCODE.FormatResourceUrl(barcode));
        }


        public static Container CreateContainer(this IRecordsManagerClient rmClient, Container container)
        {
            return rmClient.Post<Container>(POST_CONTAINER.FormatResourceUrl(), container);
        }


        public static Container UpdateContainer(this IRecordsManagerClient rmClient, Container container)
        {
            return rmClient.Put<Container>(PUT_CONTAINER.FormatResourceUrl(), container);
        }


        public static void DeleteContainer(this IRecordsManagerClient rmClient, Guid id)
        {
            rmClient.Delete(DELETE_CONTAINER_WITH_ID.FormatResourceUrl(id));
        }

        #endregion

        #region Assets

        public static string GetAllAssetsAsJson(this IRecordsManagerClient rmClient)
        {
            return rmClient.GetAllAssetsAsJson(0, 0);
        }

        public static string GetAllAssetsAsJson(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get(GET_ASSETS_ALL.FormatResourceUrl(page, pageSize));
        }

        public static string GetAssetsAsJson(this IRecordsManagerClient rmClient, Guid? parentId = null)
        {
            return rmClient.GetAssetsAsJson(0, 0, parentId);
        }

        public static string GetAssetsAsJson(this IRecordsManagerClient rmClient, int page, int pageSize, Guid? parentId = null)
        {
            return rmClient.Get(GET_ASSETS.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }

        public static IEnumerable<Asset> GetAssetsFromJson(this IRecordsManagerClient rmClient, string json)
        {
            var page = rmClient.GetAssetsWithPageDataFromJson(json);

            return page.Items;
        }

        public static IClientPagedItems<Asset> GetAssetsWithPageDataFromJson(this IRecordsManagerClient rmClient, string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<Asset>>(json);
        }

        public static DateTime GetAssetLastEdit(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<DateTime>(GET_ASSETS_LASTEDIT.FormatResourceUrl());
        }

        public static IEnumerable<Asset> SearchAssets(this IRecordsManagerClient rmClient, string title)
        {
            var page = rmClient.SearchAssets(title, 0, 0);

            return page.Items;
        }

        public static IClientPagedItems<Asset> SearchAssets(this IRecordsManagerClient rmClient, string title, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Asset>>(GET_ASSETS_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }

        public static IEnumerable<Asset> GetAllAssets(this IRecordsManagerClient rmClient)
        {
            var page = rmClient.GetAllAssets(0, 0);

            return page.Items;
        }

        public static IClientPagedItems<Asset> GetAllAssets(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Asset>>(GET_ASSETS_ALL.FormatResourceUrl(page, pageSize));
        }

        public static IEnumerable<Asset> GetAssets(this IRecordsManagerClient rmClient, Guid? parentId = null)
        {
            var page = rmClient.GetAssets(0, 0, parentId);

            return page.Items;
        }

        public static IClientPagedItems<Asset> GetAssets(this IRecordsManagerClient rmClient, int page, int pageSize, Guid? parentId = null)
        {
            return rmClient.Get<ClientPagedItems<Asset>>(GET_ASSETS.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }

        public static Asset GetAsset(this IRecordsManagerClient rmClient, Guid id)
        {
            return rmClient.Get<Asset>(GET_ASSET_WITH_ID.FormatResourceUrl(id));
        }

        public static Asset GetAsset(this IRecordsManagerClient rmClient, string barcode)
        {
            return rmClient.Get<Asset>(GET_ASSET_WITH_BARCODE.FormatResourceUrl(barcode));
        }

        public static Asset CreateAsset(this IRecordsManagerClient rmClient, Asset asset)
        {
            return rmClient.Post<IEnumerable<Asset>>(POST_ASSET.FormatResourceUrl(), new List<Asset>() { asset }).First();
        }

        public static IEnumerable<Asset> CreateAssets(this IRecordsManagerClient rmClient, IEnumerable<Asset> assets)
        {
            return rmClient.Post<IEnumerable<Asset>>(POST_ASSET.FormatResourceUrl(), assets);
        }

        public static Asset UpdateAsset(this IRecordsManagerClient rmClient, Asset asset)
        {
            return rmClient.Put<Asset>(PUT_ASSET.FormatResourceUrl(), asset);
        }

        public static void DeleteAsset(this IRecordsManagerClient rmClient, Guid id)
        {
            rmClient.Delete(DELETE_ASSET_WITH_ID.FormatResourceUrl(id));
        }

        #endregion

        #region Barcodes

        public static string GenerateBarcode(this IRecordsManagerClient rmClient, long barcodeSchemeId)
        {
            return rmClient.Get<string>(GET_BARCODES_GENERATE.FormatResourceUrl(barcodeSchemeId));
        }


        public static DateTime GetBarcodeSchemesLastEdit(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<DateTime>(GET_BARCODES_LASTEDIT.FormatResourceUrl());
        }


        public static IEnumerable<BarcodeScheme> SearchBarcodeSchemes(this IRecordsManagerClient rmClient, string title)
        {
            var page = rmClient.SearchBarcodeSchemes(title, 0, 0);

            return page.Items;
        }


        public static IClientPagedItems<BarcodeScheme> SearchBarcodeSchemes(this IRecordsManagerClient rmClient, string title, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<BarcodeScheme>>(GET_BARCODES_WITH_TITLE.FormatResourceUrl(title, page, pageSize));
        }


        public static IEnumerable<BarcodeScheme> GetBarcodeSchemes(this IRecordsManagerClient rmClient)
        {
            var paged = rmClient.GetBarcodeSchemes(0, 0);

            return paged.Items;
        }


        public static IClientPagedItems<BarcodeScheme> GetBarcodeSchemes(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<BarcodeScheme>>(GET_BARCODES_ALL.FormatResourceUrl(page, pageSize));
        }


        public static BarcodeScheme GetBarcodeScheme(this IRecordsManagerClient rmClient, long id)
        {
            return rmClient.Get<BarcodeScheme>(GET_BARCODES_WITH_ID.FormatResourceUrl(id));
        }


        public static BarcodeScheme CreateBarcodeScheme(this IRecordsManagerClient rmClient, BarcodeScheme scheme)
        {
            return rmClient.Post<BarcodeScheme>(POST_BARCODE.FormatResourceUrl(), scheme);
        }


        public static BarcodeScheme UpdateBarcodeScheme(this IRecordsManagerClient rmClient, BarcodeScheme scheme)
        {
            return rmClient.Put<BarcodeScheme>(PUT_BARCODE.FormatResourceUrl(), scheme);
        }


        public static void DeleteBarcodeScheme(this IRecordsManagerClient rmClient, long id)
        {
            rmClient.Delete(DELETE_BARCODE.FormatResourceUrl(id));
        }

        #endregion

        #region Locations

        public static DateTime GetLocationsLastEdit(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<DateTime>(GET_LOCATIONS_LASTEDIT.FormatResourceUrl());
        }

        public static IEnumerable<Location> GetLocations(this IRecordsManagerClient rmClient)
        {
            var paged = rmClient.GetLocations(0, 0);

            return paged.Items;
        }

        public static IClientPagedItems<Location> GetLocations(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Location>>(GET_LOCATIONS_ALL.FormatResourceUrl(page, pageSize));
        }


        public static IEnumerable<Location> GetLocationsByParentId(this IRecordsManagerClient rmClient, long? parentId = null)
        {
            var paged = rmClient.GetLocationsByParentId(0, 0, parentId);

            return paged.Items;
        }

        public static IClientPagedItems<Location> GetLocationsByParentId(this IRecordsManagerClient rmClient, int page, int pageSize, long? parentId = null)
        {
            return rmClient.Get<ClientPagedItems<Location>>(GET_LOCATIONS_BY_PARENT_ID.FormatResourceUrl(page, pageSize, parentId));
        }

        public static IEnumerable<Location> SearchLocations(this IRecordsManagerClient rmClient, string title)
        {
            var page = rmClient.SearchLocations(title, 0, 0);

            return page.Items;
        }

        public static IClientPagedItems<Location> SearchLocations(this IRecordsManagerClient rmClient, string title, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Location>>(GET_LOCATIONS_WITH_TITLE.FormatResourceUrl(title, page, pageSize));
        }

        public static Location GetLocation(this IRecordsManagerClient rmClient, long id)
        {
            return rmClient.Get<Location>(GET_LOCATIONS_WITH_ID.FormatResourceUrl(id));
        }

        public static Location CreateLocation(this IRecordsManagerClient rmClient, Location location)
        {
            return rmClient.Post<Location>(POST_LOCATION.FormatResourceUrl(), location);
        }

        public static Location UpdateLocation(this IRecordsManagerClient rmClient, Location location)
        {
            return rmClient.Put<Location>(PUT_LOCATION.FormatResourceUrl(), location);
        }

        public static void DeleteLocation(this IRecordsManagerClient rmClient, long id)
        {
            rmClient.Delete(DELETE_LOCATION.FormatResourceUrl(id));
        }

        #endregion

        #region ChargeRequests

        public static IClientPagedItems<ChargeOutRequest> GetChargeOutRequests(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<ChargeOutRequest>>(GET_CHARGEOUTREQUESTS.FormatResourceUrl(page, pageSize));
        }

        public static IClientPagedItems<ChargeInRequest> GetChargeInRequests(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<ChargeInRequest>>(GET_CHARGEINREQUESTS.FormatResourceUrl(page, pageSize));
        }

        public static IClientPagedItems<ChargeOutRequest> GetAllChargeOutRequests(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<ClientPagedItems<ChargeOutRequest>>(GET_CHARGEOUTREQUESTS_ALL);
        }

        public static IClientPagedItems<ChargeInRequest> GetAllChargeInRequests(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<ClientPagedItems<ChargeInRequest>>(GET_CHARGEINREQUESTS_ALL);
        }

        public static ChargeOutRequest GetChargeOutRequest(this IRecordsManagerClient rmClient, long chargeOutRequestId)
        {
            return rmClient.Get<ChargeOutRequest>(GET_CHARGEOUTREQUEST.FormatResourceUrl(chargeOutRequestId));
        }

        public static ChargeInRequest GetChargeInRequest(this IRecordsManagerClient rmClient, long chargeInRequestId)
        {
            return rmClient.Get<ChargeInRequest>(GET_CHARGEINREQUEST.FormatResourceUrl(chargeInRequestId));
        }

        public static IClientPagedItems<Asset> GetChargeRequestAssets(this IRecordsManagerClient rmClient, long parentId, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<Asset>>(GET_CHARGEREQUEST_ASSETS.FormatResourceUrl(parentId, page, pageSize));
        }

        public static IClientPagedItems<Asset> GetAllChargeRequestAssets(this IRecordsManagerClient rmClient, long parentId)
        {
            return rmClient.Get<ClientPagedItems<Asset>>(GET_CHARGEREQUEST_ASSETS_ALL.FormatResourceUrl(parentId));
        }

        public static ChargeOutRequest CreateChargeOutRequest(this IRecordsManagerClient rmClient, ChargeOutRequest chargeOutRequest)
        {
            return rmClient.Post<ChargeOutRequest>(POST_CHARGEOUTREQUEST, chargeOutRequest);
        }

        public static ChargeInRequest CreateChargeInRequest(this IRecordsManagerClient rmClient, ChargeInRequest chargeInRequest)
        {
            return rmClient.Post<ChargeInRequest>(POST_CHARGEINREQUEST, chargeInRequest);
        }

        public static ChargeOutRequest UpdateChargeOutRequest(this IRecordsManagerClient rmClient, ChargeOutRequest chargeOutRequest)
        {
            return rmClient.Put<ChargeOutRequest>(PUT_CHARGEOUTREQUEST, chargeOutRequest);
        }

        public static ChargeInRequest UpdateChargeInRequest(this IRecordsManagerClient rmClient, ChargeInRequest chargeInRequest)
        {
            return rmClient.Put<ChargeInRequest>(PUT_CHARGEINREQUEST, chargeInRequest);
        }

        public static void DeleteChargeRequest(this IRecordsManagerClient rmClient, long chargeRequestId)
        {
            rmClient.Delete(DELETE_CHARGEREQUEST.FormatResourceUrl(chargeRequestId));
        }

        #endregion

        #region Custom Metadata

        public static DateTime GetCustomMetadataLastEdit(this IRecordsManagerClient rmClient)
        {
            return rmClient.Get<DateTime>(GET_CUSTOMMETADATA_LASTEDIT.FormatResourceUrl());
        }

        public static IClientPagedItems<CustomMetadata> GetCustomMetadata(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<CustomMetadata>>(GET_CUSTOMMETADATA.FormatResourceUrl(page, pageSize));
        }

        public static CustomMetadata GetCustomMetadata(this IRecordsManagerClient rmClient, long id)
        {
            return rmClient.Get<CustomMetadata>(GET_CUSTOMMETADATA_WITH_ID.FormatResourceUrl(id));
        }

        public static CustomMetadata CreateCustomMetadata(this IRecordsManagerClient rmClient, CustomMetadata customMetadata)
        {
            return rmClient.Post<CustomMetadata>(POST_CUSTOMMETADATA.FormatResourceUrl(), customMetadata);
        }

        public static CustomMetadata UpdateCustomMetadata(this IRecordsManagerClient rmClient, CustomMetadata customMetadata)
        {
            return rmClient.Put<CustomMetadata>(PUT_CUSTOMMETADATA.FormatResourceUrl(customMetadata.Id), customMetadata);
        }

        public static void DeleteCustomMetadata(this IRecordsManagerClient rmClient, long id)
        {
            rmClient.Delete(DELETE_CUSTOMMETADATA_WITH_ID.FormatResourceUrl(id));
        }

        #endregion

        #region Container Custom Metadata

        public static IEnumerable<ContainerCustomMetadata> GetContainerCustomMetadata(this IRecordsManagerClient rmClient, Guid id)
        {
            return rmClient.Get<IEnumerable<ContainerCustomMetadata>>(GET_CONTAINERCUSTOMMETADATA.FormatResourceUrl(id));
        }

        public static ContainerCustomMetadata CreateContainerCustomMetadata(this IRecordsManagerClient rmClient, ContainerCustomMetadata containerCustomMetadata)
        {
            return rmClient.Post<ContainerCustomMetadata>(POST_CONTAINERCUSTOMMETADATA.FormatResourceUrl(), containerCustomMetadata);
        }

        public static IEnumerable<ContainerCustomMetadata> UpdateContainerCustomMetadata(this IRecordsManagerClient rmClient, Guid containerId, IEnumerable<ContainerCustomMetadata> customMetadata)
        {
            return rmClient.Put<IEnumerable<ContainerCustomMetadata>>(PUT_CONTAINERCUSTOMMETADATA.FormatResourceUrl(containerId), customMetadata);
        }

        public static void DeleteContainerCustomMetadata(this IRecordsManagerClient rmClient, long customMetadataId, Guid containerId)
        {
            rmClient.Delete(DELETE_CONTAINERCUSTOMMETADATA.FormatResourceUrl(customMetadataId, containerId));
        }

        #endregion

        #region Asset Custom Metadata

        public static IEnumerable<AssetCustomMetadata> GetAssetCustomMetadata(this IRecordsManagerClient rmClient, Guid id)
        {
            return rmClient.Get<IEnumerable<AssetCustomMetadata>>(GET_ASSETCUSTOMMETADATA.FormatResourceUrl(id));
        }

        public static AssetCustomMetadata CreateAssetCustomMetadata(this IRecordsManagerClient rmClient, AssetCustomMetadata assetCustomMetadata)
        {
            return rmClient.Post<AssetCustomMetadata>(POST_ASSETCUSTOMMETADATA.FormatResourceUrl(), assetCustomMetadata);
        }

        public static IEnumerable<AssetCustomMetadata> UpdateAssetCustomMetadata(this IRecordsManagerClient rmClient, Guid assetId, IEnumerable<AssetCustomMetadata> customMetadata)
        {
            return rmClient.Put<IEnumerable<AssetCustomMetadata>>(PUT_ASSETCUSTOMMETADATA.FormatResourceUrl(assetId), customMetadata);
        }

        public static void DeleteAssetCustomMetadata(this IRecordsManagerClient rmClient, long customMetadataId, Guid assetId)
        {
            rmClient.Delete(DELETE_ASSETCUSTOMMETADATA.FormatResourceUrl(customMetadataId, assetId));
        }

        #endregion

        #region Processing
        public static ProcessorItem GetProcessingItem(this IRecordsManagerClient rmClient, long id)
        {
            return rmClient.Get<ProcessorItem>(GET_PROCESSINGITEMDATA.FormatResourceUrl(id));
        }

        public static IClientPagedItems<ProcessorItem> GetProcessingItems(this IRecordsManagerClient rmClient, int page, int pageSize)
        {
            return rmClient.Get<ClientPagedItems<ProcessorItem>>(GET_PROCESSINGITEMSDATA.FormatResourceUrl(page, pageSize));
        }

        public static ProcessorItem ApproveProcessingItem(this IRecordsManagerClient rmClient, ProcessorItem processorItem)
        {
            return rmClient.Put<ProcessorItem>(PUT_APPROVEPROCESSINGITEMDATA.FormatResourceUrl(), processorItem);
        }

        #endregion

        #region Urls

        private static string GET_CONTAINERS_LASTEDIT = "/api/v1/pamcontainers?lastedit";
        private static string GET_CONTAINERS = "/api/v1/pamcontainers?page={0}&pageSize={1}&parentId={2}";
        private static string GET_CONTAINERS_ALL = "api/v1/pamcontainers?all=true&page={0}&pageSize={1}";
        private static string GET_CONTAINERS_CONTAINING_TITLE = "/api/v1/pamcontainers?title={0}&page={1}&pageSize={2}";
        private static string GET_CONTAINER_WITH_ID = "/api/v1/pamcontainers/{0}";
        private static string GET_CONTAINER_WITH_BARCODE = "/api/v1/pamcontainers?barcode={0}";
        private static string POST_CONTAINER = "/api/v1/pamcontainers";
        private static string PUT_CONTAINER = "/api/v1/pamcontainers";
        private static string DELETE_CONTAINER_WITH_ID = "/api/v1/pamcontainers/{0}";

        private static string GET_ASSETS_LASTEDIT = "/api/v1/pamassets?lastedit";
        private static string GET_ASSETS = "/api/v1/pamassets?page={0}&pageSize={1}&parentId={2}";
        private static string GET_ASSETS_ALL = "api/v1/pamassets?all=true&page={0}&pageSize={1}";
        private static string GET_ASSETS_CONTAINING_TITLE = "/api/v1/pamassets?title={0}&page={1}&pageSize={2}";
        private static string GET_ASSET_WITH_ID = "/api/v1/pamassets/{0}";
        private static string GET_ASSET_WITH_BARCODE = "/api/v1/pamassets?barcode={0}";
        private static string POST_ASSET = "/api/v1/pamassets";
        private static string PUT_ASSET = "/api/v1/pamassets";
        private static string DELETE_ASSET_WITH_ID = "/api/v1/pamassets/{0}";

        private static string GET_BARCODES_LASTEDIT = "/api/v1/pambarcodes?lastedit";
        private static string GET_BARCODES_GENERATE = "/api/v1/pambarcodes/{0}?generate";
        private static string GET_BARCODES_ALL = "/api/v1/pambarcodes?page={0}&pageSize={1}";
        private static string GET_BARCODES_WITH_ID = "/api/v1/pambarcodes/{0}";
        private static string GET_BARCODES_WITH_TITLE = "/api/v1/pambarcodes?title={0}&page={1}&pageSize={2}";
        private static string POST_BARCODE = "/api/v1/pambarcodes";
        private static string PUT_BARCODE = "/api/v1/pambarcodes";
        private static string DELETE_BARCODE = "/api/v1/pambarcodes/{0}";

        private static string GET_LOCATIONS_LASTEDIT = "/api/v1/pamlocations?lastedit";
        private static string GET_LOCATIONS_ALL = "/api/v1/pamlocations?page={0}&pageSize={1}";
        private static string GET_LOCATIONS_BY_PARENT_ID = "/api/v1/pamlocations?page={0}&pageSize={1}&parentId={2}";
        private static string GET_LOCATIONS_WITH_TITLE = "/api/v1/pamlocations?title={0}&page={1}&pageSize={2}";
        private static string GET_LOCATIONS_WITH_ID = "/api/v1/pamlocations/{0}";
        private static string POST_LOCATION = "/api/v1/pamlocations";
        private static string PUT_LOCATION = "/api/v1/pamlocations";
        private static string DELETE_LOCATION = "/api/v1/pamlocations/{0}";

        private static string GET_CHARGEOUTREQUESTS = "api/v1/pamchargerequests?isOut=true&page={0}&pageSize={1}";
        private static string GET_CHARGEINREQUESTS = "api/v1/pamchargerequests?isIn=true&page={0}&pageSize={1}";
        private static string GET_CHARGEOUTREQUESTS_ALL = "api/v1/pamchargerequests?isOut=true";
        private static string GET_CHARGEINREQUESTS_ALL = "api/v1/pamchargerequests?isIn=true";
        private static string GET_CHARGEOUTREQUEST = "api/v1/pamchargerequests?chargeOutRequestId={0}";
        private static string GET_CHARGEINREQUEST = "api/v1/pamchargerequests?chargeInRequestId={0}";
        private static string GET_CHARGEREQUEST_ASSETS = "api/v1/pamchargerequests?parentId={0}&page={1}&pageSize={2}";
        private static string GET_CHARGEREQUEST_ASSETS_ALL = "api/v1/pamchargerequests?parentId={0}";
        private static string POST_CHARGEOUTREQUEST = "/api/v1/pamchargerequests?isOut=true";
        private static string POST_CHARGEINREQUEST = "/api/v1/pamchargerequests?isIn=true";
        private static string PUT_CHARGEOUTREQUEST = "/api/v1/pamchargerequests?isOut=true";
        private static string PUT_CHARGEINREQUEST = "/api/v1/pamchargerequests?isIn=true";
        private static string DELETE_CHARGEREQUEST = "/api/v1/pamchargerequests?chargeRequestId={0}";

        private static string GET_CUSTOMMETADATA_LASTEDIT = "/api/v1/prm/metadata/lastEdit";
        private static string GET_CUSTOMMETADATA = "/api/v1/prm/metadata/page/{0}/pageSize/{1}";
        private static string GET_CUSTOMMETADATA_WITH_ID = "/api/v1/prm/metadata/{0}";
        private static string POST_CUSTOMMETADATA = "/api/v1/prm/metadata/create";
        private static string PUT_CUSTOMMETADATA = "/api/v1/prm/metadata/{0}";
        private static string DELETE_CUSTOMMETADATA_WITH_ID = "/api/v1/prm/metadata/{0}";

        private static string GET_CONTAINERCUSTOMMETADATA = "/api/v1/prm/metadata/containers/{0}";
        private static string POST_CONTAINERCUSTOMMETADATA = "/api/v1/prm/metadata/containers";
        private static string PUT_CONTAINERCUSTOMMETADATA = "/api/v1/prm/metadata/containers/{0}";
        private static string DELETE_CONTAINERCUSTOMMETADATA = "/api/v1/prm/metadata/containers/customMetadataId/{0}/containerId/{1}";

        private static string GET_ASSETCUSTOMMETADATA = "/api/v1/prm/metadata/assets/{0}";
        private static string POST_ASSETCUSTOMMETADATA = "/api/v1/prm/metadata/assets";
        private static string PUT_ASSETCUSTOMMETADATA = "/api/v1/prm/metadata/assets/{0}";
        private static string DELETE_ASSETCUSTOMMETADATA = "/api/v1/prm/metadata/assets/customMetadataId/{0}/assetId/{1}";

        private static string GET_PROCESSINGITEMDATA = "/api/v1/pamprocessing/{0}";
        private static string GET_PROCESSINGITEMSDATA = "/api/v1/pamprocessing?page={0}&pageSize={1}";
        private static string PUT_APPROVEPROCESSINGITEMDATA = "/api/v1/pamprocessing";

        #endregion
    }
}
