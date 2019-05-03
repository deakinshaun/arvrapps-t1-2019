using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using AirtableApiClient;

public class AirtableInterface : MonoBehaviour {

    readonly string baseId = "appxWREHbWVRhUrMR";
    readonly string appKey = "key6kaWf02hedLgNZ";
    readonly string tableName = "RemindAR";

    public static AirtableInterface Singleton;

    string filterFormula = "";
    IEnumerable<string> filterFields = new List<string>() {/*"ID",*/ "Title", "Content"};
    private bool f_connectingToAirtable = false;
    public bool f_dataAcquired = false;

    public List<DatabaseEntry> Database;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Singleton = this;
        Database = new List<DatabaseEntry>();
    }

    public void GetData()
    {
        if (!f_connectingToAirtable)
        {
            getData();
        }
    }

    async private void getData()
    {
        f_connectingToAirtable = true;
        string offset = null;
        string errorMessage = null;
        var records = new List<AirtableRecord>();

        using (AirtableBase airtableBase = new AirtableBase(appKey, baseId))
        {
        //
        // Use 'offset' and 'pageSize' to specify the records that you want
        // to retrieve.
        // Only use a 'do while' loop if you want to get multiple pages
        // of records.
        //
            Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                tableName, 
                "", 
                filterFields, 
                filterFormula, 
                10, 
                10);

            AirtableListRecordsResponse response = await task;

            if (response.Success)
            {
                records.AddRange(response.Records.ToList());
                offset = response.Offset;
            }
            else if (response.AirtableApiError is AirtableApiException)
            {
                errorMessage = response.AirtableApiError.ErrorMessage;
            }
            else
            {
                errorMessage = "Unknown error";
            }
        }
        

        if (!string.IsNullOrEmpty(errorMessage))
        {
            // Error reporting
        }
        else
        {
            // Do something with the retrieved 'records' and the 'offset'
            // for the next page of the record list.
            foreach (AirtableRecord _atr in records)
            {
                var _title = (string)_atr.Fields["Title"];
                var _content = (string)_atr.Fields["Content"];

                Debug.Log(_title + " " + _content);

                Database.Add(new DatabaseEntry(_title, _content));
            }
        }

        f_connectingToAirtable = false;
        f_dataAcquired = true;
    }
    
}

// my lazy implementation of a tuple
public class DatabaseEntry
{
    public string Title {get; private set; }
    public string Content {get; private set; }

    public DatabaseEntry(string _t, string _c)
    {
        Title = _t;
        Content = _c;
    }
}
