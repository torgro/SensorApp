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
    public string tableName;
    public bool TableCreated;
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
            this.TableCreated = false;
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Exception AzureStorage - " + ex.Message);
        }       
    }

    public void GetOrCreateTable(string tableName)
    {
        this.LogIt("Start GetOrCreateTable");
        if (this.tblClient == null)
        {
            this.LogIt("TableClient is null");
            return;
        }
        this.LogIt("Creating or getting table");
        try
        {
            this.tbl = this.tblClient.GetTableReference(tableName);
            this.tbl.CreateIfNotExists();
            this.TableCreated = true;
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetOrCreateTable - " + ex.Message);
        }
    }

    public async Task GetOrCreateTableAsync(string tableName)
    {
        this.LogIt("Start GetOrCreateTableAsync");
        if (this.tblClient == null)
        {
            this.LogIt("TableClient is null");
            return;
        }
        try
        {
            this.tbl = this.tblClient.GetTableReference(tableName);
            await this.tbl.CreateIfNotExistsAsync();
            this.TableCreated = true;
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetOrCreateTableAsync - " + ex.Message);
        }        
    }

    public void InsertorReplaceEntity(ITableEntity entity)
    {
        //ITableEntity newT = new T();
        //newT.RowKey = rowKey;
        //newT.PartitionKey = PartKey;
        try
        {
            TableOperation insert = TableOperation.InsertOrReplace(entity);
            this.tbl.Execute(insert);
            this.LogIt("Insert done");
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in InsertorReplaceEntity - " + ex.Message);
        }        
    }

    public async Task InsertOrReplaceEntityAsync(ITableEntity entity)
    {
        try
        {
            TableOperation insert = TableOperation.InsertOrReplace(entity);
            await this.tbl.ExecuteAsync(insert);
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in InsertOrReplaceEntityAsync - " + ex.Message);
        }        
    }

    public IList<TableResult> InsertOrReplaceEntityBatch<T>(List<T> list)
    {
        IList<TableResult> results = null;
        try
        {
            if (list != null)
            {
                TableBatchOperation batch = new TableBatchOperation();
                foreach (ITableEntity item in list)
                {
                    batch.InsertOrReplace(item);
                }
                results = this.tbl.ExecuteBatch(batch);
            }
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in InsertOrReplaceEntityBatch - " + ex.Message);
        }
        return results;
    }

    public async Task InsertOrReplaceEntityBatchAsync(IList<ITableEntity> list)
    {
        IList<TableResult> results;
        try
        {
            if (list != null)
            {
                TableBatchOperation batch = new TableBatchOperation();
                foreach (ITableEntity item in list)
                {
                    batch.InsertOrReplace(item);
                }
                results = await this.tbl.ExecuteBatchAsync(batch);
            }
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in InsertOrReplaceEntityBatchAsync - " + ex.Message);
        }        
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

    public IList<TableResult> DropAllEntities<T>(string partkey) where T : TableEntity, new()
    {
        List<T> list = this.GetAzureTableAll<T>(partkey);
        TableBatchOperation batch = new TableBatchOperation();
        IList<TableResult> results = null;
        foreach (T item in list)
        {
            batch.Delete(item);
        }

        if (list.Count > 0)
        {
            this.LogIt("Deleting all entities");
            results = this.tbl.ExecuteBatch(batch);
        }
        else
        {
            this.LogIt("Nothing to delete!");
        }

        return results;
    }

    public async Task<IList<TableResult>> DropAllEntitiesAsync<T>(string partKey) where T : TableEntity, new()
    {
        List<T> list = this.GetAzureTableAll<T>(partKey);
        TableBatchOperation batch = new TableBatchOperation();
        IList<TableResult> results = null;
        foreach (T item in list)
        {
            batch.Delete(item);
        }

        if (list.Count > 0)
        {
            this.LogIt("Deleting all entities");
            results = await this.tbl.ExecuteBatchAsync(batch);
        }
        else
        {
            this.LogIt("Nothing to delete!");
        }
        return results;
    }

    public async Task<List<T>> GetAzureTableAllAsync<T>(string partKey) where T : TableEntity , new()
    {
        TableQuerySegment<T> segment = null;
        List<T> list = new List<T>();
        TableQuery<T> query = null;
        string filter = string.Empty;
        filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey);
        query = new TableQuery<T>().Where(filter);
        TableContinuationToken token = null;
        try
        {
            do
            {
                segment = await this.tbl.ExecuteQuerySegmentedAsync<T>(query, token);
                token = segment.ContinuationToken;
                list.AddRange(segment);
            } while (token != null);
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetAzureTableAllAsync - " + ex.Message);
        }       
        return list;
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

    public async Task<List<T>> GetEntityByRowKeyAsync<T>(string rowKey) where T : ITableEntity , new()
    {
        TableQuery<T> query;
        string filter = string.Empty;
        TableQuerySegment<T> segment = null;
        List<T> list = new List<T>();
        filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
        query = new TableQuery<T>().Where(filter);
        TableContinuationToken token = null;
        try
        {
            do
            {
                segment = await this.tbl.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                list.AddRange(segment);
            } while (token != null);
        }
        catch (Exception ex)
        {
            this.LogIt("Exception in GetEntityByRowKeyAsync - " + ex.Message);
        }        
        return list;
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
            this.LogIt("Exception in GetSingleEntity - " + ex.Message);
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

    //private bool GetTableReference(string tableName)
    //{
    //    bool ReturnValue = false;
    //    this.tableName = tableName;
    //    this.LogIt("Start GetTableReference");
    //    if (this.tblClient == null)
    //    {
    //        this.LogIt("TableClient is null");
    //        return ReturnValue;
    //    }
    //    try
    //    {
    //        this.tbl = this.tblClient.GetTableReference(tableName);
    //        ReturnValue = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        this.LogIt("Exception in GetTableReference" + ex.Message);
    //    }

    //    return ReturnValue;
    //}
}