using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.Interfaces;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.Services
{
    public class BrowserService : IBrowserService
    {
        public void ClearBrowserCache()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;

            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);

            var websiteDataTypes = new NSSet<NSString>(new[]
            {
                //Choose which ones you want to remove
                WKWebsiteDataType.Cookies,
                WKWebsiteDataType.DiskCache,
                WKWebsiteDataType.IndexedDBDatabases,
                WKWebsiteDataType.LocalStorage,
                WKWebsiteDataType.MemoryCache,
                WKWebsiteDataType.OfflineWebApplicationCache,
                WKWebsiteDataType.SessionStorage,
                WKWebsiteDataType.WebSQLDatabases
            });

            WKWebsiteDataStore.DefaultDataStore.FetchDataRecordsOfTypes(websiteDataTypes, (NSArray records) =>
            {
                for (nuint i = 0; i < records.Count; i++)
                {
                    var record = records.GetItem<WKWebsiteDataRecord>(i);

                    WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypes(record.DataTypes,
                        new[] { record }, () => { Console.Write($"deleted: {record.DisplayName}"); });
                }
            });
        }
    }
}