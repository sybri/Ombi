﻿#region Copyright
// /************************************************************************
//    Copyright (c) 2016 Jamie Rees
//    File: PlexDatabase.cs
//    Created By: Jamie Rees
//   
//    Permission is hereby granted, free of charge, to any person obtaining
//    a copy of this software and associated documentation files (the
//    "Software"), to deal in the Software without restriction, including
//    without limitation the rights to use, copy, modify, merge, publish,
//    distribute, sublicense, and/or sell copies of the Software, and to
//    permit persons to whom the Software is furnished to do so, subject to
//    the following conditions:
//   
//    The above copyright notice and this permission notice shall be
//    included in all copies or substantial portions of the Software.
//   
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  ************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mono.Data.Sqlite;
using PlexRequests.Store.Models.Plex;

namespace PlexRequests.Store
{
    /// <summary>
    /// We should only ever READ, NEVER WRITE!
    /// </summary>
    public class PlexDatabase : IPlexDatabase
    {
        public PlexDatabase(SqliteFactory provider)
        {
            Factory = provider;
        }

        private SqliteFactory Factory { get; }
        /// <summary>
        /// https://support.plex.tv/hc/en-us/articles/202915258-Where-is-the-Plex-Media-Server-data-directory-located-
        /// </summary>
        public string DbLocation { get; set; }

        private IDbConnection DbConnection()
        {
            var fact = Factory.CreateConnection();
            if (fact == null)
            {
                throw new SqliteException("Factory returned null");
            }
            fact.ConnectionString = "Data Source=" + "Plex Path";
            return fact;
        }

        public IEnumerable<MetadataItems> GetMetadata()
        {
            using (var con = DbConnection())
            {
                return con.GetAll<MetadataItems>();
            }
        }

        public async Task<IEnumerable<MetadataItems>> GetMetadataAsync()
        {
            using (var con = DbConnection())
            {
                return await con.GetAllAsync<MetadataItems>();
            }
        }

        public IEnumerable<MetadataItems> QueryMetadataItems(string query, object param)
        {
            using (var con = DbConnection())
            {
                return (IEnumerable<MetadataItems>)con.Query(query, param);
            }
        }
    }
}