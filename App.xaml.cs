﻿using System.Windows;
using Ynost.Services;

namespace Ynost
{
    public partial class App : Application
    {
        public static DatabaseService Db { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            const string conn =
                "Host=91.192.168.52;" +
                "Port=5432;" +
                "Database=ynost_db;" +
                "Username=teacher_app;" +
                "Password=T_pass;" +
                "Timeout=10;Command Timeout=30;" +
                "Ssl Mode=Prefer;";          // Require + Trust... если сервер принуждает SSL

            Db = new DatabaseService(conn);

            base.OnStartup(e);
        }
    }
}
