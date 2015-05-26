using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;


public class AzureStorage
{
    private string con = "";
    private CloudStorageAccount Account;
    private CloudTableClient tblClient;
    private CloudTable tbl;
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    public AzureStorage()
    {
        try
        {
            string path = System.IO.Directory.GetCurrentDirectory() + @"\StorageKey.txt";
            this.con = System.IO.File.ReadAllText(path);
            this.Account = CloudStorageAccount.Parse(this.con);
            this.tblClient = this.Account.CreateCloudTableClient();
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Exception AzureStorage - " + ex.Message);
        }       
    }

    public void GetOrCreateTable(string tableName)
    {
        this.LogIt("Start GetOrCreateTable");
        if(this.tblClient == null)
        {
            this.LogIt("TableClient is null");
            return;
        }
        this.LogIt("Creating or getting table");
        this.tbl = this.tblClient.GetTableReference(tableName);
        
        this.tbl.CreateIfNotExists();
    }

    public void InsertorReplaceEntity<T>(ITableEntity obj) //, string PartKey, string rowKey) //where T : TableEntity , new()
    {
        //ITableEntity newT = new T();
        //newT.RowKey = rowKey;
        //newT.PartitionKey = PartKey;
        TableOperation insert = TableOperation.InsertOrReplace(obj);
        this.tbl.Execute(insert);
        this.LogIt("Insert done");
    }

    public async Task InsertOrReplaceEntityBatch(IList<ITableEntity> list)
    {
        IList<TableResult> results;
        if(list != null)
        {
            TableBatchOperation batch = new TableBatchOperation();
            foreach (ITableEntity item in list)
            {
                batch.InsertOrReplace(item);
            }
            results = await this.tbl.ExecuteBatchAsync(batch);
        }
    }

    public async Task InsertOrReplaceEntityAsync(ITableEntity entity)
    {
        TableOperation insert = TableOperation.InsertOrReplace(entity);
        await this.tbl.ExecuteAsync(insert);
    }

    public List<T> GetAzureTableAll<T>(string partKey) where T : TableEntity , new()
    {
        List<T> list = new List<T>();
        TableQuery<T> query;
        string filter = string.Empty;
        try
        {
            filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey);
            query = new TableQuery<T>().Where(filter);
            list.AddRange(this.tbl.ExecuteQuery<T>(query));
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetAzureTableAll - " + ex.Message);
        }

        return list;
    }

    public MonitorDevice GetAzureTableDevicee(int DeviceId)
    {
        TableQuery<MonitorDevice> query;
        MonitorDevice dev = null;
        string filter = string.Empty;
        try
        {
            filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, DeviceId.ToString());
            query = new TableQuery<MonitorDevice>().Where(filter);
            //TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            dev = this.tbl.ExecuteQuery<MonitorDevice>(query).FirstOrDefault();                 
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetAzureTableDevice - " + ex.Message);
        }
        
        return dev;
    }

    public List<T> GetEntityByRowKey<T> (string rowKey) where T : ITableEntity, new ()
    {
        TableQuery<T> query;
        string filter = string.Empty;
        List<T> results = null;
        try
        {
            filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            query = new TableQuery<T>().Where(filter);
            results = this.tbl.ExecuteQuery(query).Select(ent => (T)ent).ToList();
            return results;
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetEntityByRowKey - " + ex.Message);
        }
        return results;
    }

    public T GetSingleEntity<T> (string PartKey, string rowKey) where T : ITableEntity
    {
        TableResult result = null;
        try
        {
            TableOperation get = TableOperation.Retrieve<T>(PartKey, rowKey);
            result = this.tbl.Execute(get);
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetSingle - " + ex.Message);
        }    
        return (T)result.Result;
    }

    virtual protected void OnLogEvent(LogDetail it)
    {
        if (LogEvent != null)
        {
            LogEvent(it);
        }
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(2, true).GetMethod().Name;
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;

        OnLogEvent(log);
    }
}