namespace RecordLion.RecordsManager.Client
{
    public interface ISPConnectorClient
    {
        string GetRecordManagerServerUrl();


        string GetDefaultZoneUri(string path);
    }
}