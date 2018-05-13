﻿using System;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Refactoring.Helper
{
    public static class StatisticsLogger
    {
        private static SQLiteConnection database;
        private static SQLiteConnection Database
        {
            get
            {
                if (database != null && database.State == System.Data.ConnectionState.Open)
                    return database;
                var databaseFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Statistics.db";
                database = new SQLiteConnection($"Data Source={databaseFilePath}");
                database.Open();
                return database;
            }
        }

        public static void Log(string projectname, string className, [CallerFilePath] string callerClassPath = null, [CallerMemberName] string refactoringMethod = null)
        {
            if (callerClassPath == null || refactoringMethod == null)
                return;

            var refactoringClass = Path.GetFileName(callerClassPath);
            var dateString = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            using (var saveToDatabase = new SQLiteCommand(Database))
            {
                saveToDatabase.CommandText = $"insert into entries (project, date, refactoring_class, refactoring_method) values ('{projectname}', '{dateString}', '{refactoringClass}', '{refactoringMethod}')";
                saveToDatabase.ExecuteNonQuery();
            }
        }
    }
}