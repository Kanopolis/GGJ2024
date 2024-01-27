using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;
using UnityEngine;

public class LocaSheetsManager
{
    private const string LOCASEARCHTABLESSTRING = "t:StringTableCollection";
    private const string LOCASEARCHPROVIDERSTRING = "t:SheetsServiceProvider";

    [MenuItem("Helper/Localization/Authorize")]
    static void AuthorizeGoogleSheets()
    {
        string[] guids1 = AssetDatabase.FindAssets(LOCASEARCHPROVIDERSTRING, null);

        foreach (string guid1 in guids1)
        {
            SheetsServiceProvider test = AssetDatabase.LoadAssetAtPath<SheetsServiceProvider>(AssetDatabase.GUIDToAssetPath(guid1));
            test.AuthoizeOAuth();
        }
    }

    [MenuItem("Helper/Localization/Pull Sheets")]
    static void PullFromGoogleSheets()
    {
        string[] guids1 = AssetDatabase.FindAssets(LOCASEARCHTABLESSTRING, null);

        foreach (string guid1 in guids1)
        {
            StringTableCollection test = AssetDatabase.LoadAssetAtPath<StringTableCollection>(AssetDatabase.GUIDToAssetPath(guid1));
            if (test.Extensions[0] is GoogleSheetsExtension)
            {
                GoogleSheetsExtension test2 = test.Extensions[0] as GoogleSheetsExtension;

                // Setup the connection to Google
                var googleSheets = new GoogleSheets(test2.SheetsServiceProvider);
                googleSheets.SpreadSheetId = test2.SpreadsheetId;

                // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
                googleSheets.PullIntoStringTableCollection(test2.SheetId, test2.TargetCollection as StringTableCollection, test2.Columns, reporter: new ProgressBarReporter());
            }
        }
    }

    [MenuItem("Helper/Localization/Push Sheets")]
    static void PushToGoogleSheets()
    {
        // Find all assets labelled with 'architecture' :
        string[] guids1 = AssetDatabase.FindAssets(LOCASEARCHTABLESSTRING, null);

        foreach (string guid1 in guids1)
        {
            StringTableCollection test = AssetDatabase.LoadAssetAtPath<StringTableCollection>(AssetDatabase.GUIDToAssetPath(guid1));
            if (test.Extensions[0] is GoogleSheetsExtension)
            {
                GoogleSheetsExtension test2 = test.Extensions[0] as GoogleSheetsExtension;

                // Setup the connection to Google
                var googleSheets = new GoogleSheets(test2.SheetsServiceProvider);
                googleSheets.SpreadSheetId = test2.SpreadsheetId;

                // Now send the update. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
                googleSheets.PushStringTableCollection(test2.SheetId, test2.TargetCollection as StringTableCollection, test2.Columns, new ProgressBarReporter());

            }
        }
    }
}
