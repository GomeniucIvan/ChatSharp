using ChatSharp.Core.Data;
using ChatSharp.Engine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text.Json;
using System.Text.RegularExpressions;
using ChatSharp.Core.Platform.Identity.Domain;
using ChatSharp.Domain;
using ChatSharp.Extensions;
using ChatSharp.Web.Models.Install;
using Microsoft.Extensions.FileProviders;

namespace ChatSharp.Web.Controllers
{
    public class InstallController : BaseController
    {
        #region Fields

        private readonly IApplicationContext _appContext;

        #endregion

        #region Ctor

        public InstallController(IApplicationContext app)
        {
            _appContext = app;
        }

        #endregion

        #region Methods

        [Route("Install")]
        [HttpPost]
        public async Task<IActionResult> Install([FromBody] InstallationModel model, CancellationToken cancelToken = default)
        {
            var genericResponse = new GenericResponse<InstallationResult>();

            if (_appContext.IsDatabaseInstalled)
            {
                return Ok("Already Installed");
            }

            model.DbRawConnectionString = model.DbRawConnectionString?.Trim();

            ChatSharpDbContext db = null;
            DbConnectionStringBuilder conStringBuilder = null;

            try
            {
                // Try to create connection string
                if (model.UseRawConnectionString)
                {
                    conStringBuilder = ChatSharpDbContextExtensions.CreateConnectionStringBuilder(model.DbRawConnectionString);
                }
                else
                {
                    // Structural connection string
                    var userId = model.DbUserId;
                    var password = model.DbPassword;
                    if (model.DataProvider == "sqlserver" && model.DbAuthType == "windows")
                    {
                        userId = null;
                        password = null;
                    }
                    conStringBuilder = ChatSharpDbContextExtensions.CreateConnectionStringBuilder(model.DbServer, model.DbName, userId, password);
                }
            }
            catch (Exception ex)
            {
                return Ok(genericResponse.Error("Wrong connection string format"));
            }

            var shouldDeleteDbOnFailure = false;

            try
            {
                cancelToken.ThrowIfCancellationRequested();

                var conString = conStringBuilder.ConnectionString;

                // Define the object that we'll be serializing
                var connectionData = new { ConnectionString = conString };

                // Serialize the object to a JSON string
                var json = JsonSerializer.Serialize(connectionData);

                // Write the JSON string to a file
                await System.IO.File.WriteAllTextAsync("connection.json", json);

                var optionsBuilder = new DbContextOptionsBuilder<ChatSharpDbContext>();
                optionsBuilder.UseSqlServer(conString);

                using var context = new ChatSharpDbContext(optionsBuilder.Options);

                // Delete only on failure if WE created the database.
                var canConnectDatabase = await context.Database.CanConnectAsync(cancelToken);
                shouldDeleteDbOnFailure = !canConnectDatabase;

                // Create the DbContext using the options
                db = new ChatSharpDbContext(optionsBuilder.Options);

                // Creates database
                await db.Database.EnsureCreatedAsync(cancelToken);
                cancelToken.ThrowIfCancellationRequested();

                // Create customer admin
                var saltKey = PasswordExtensions.CreateSaltKey(5);
                db.Customers.Add(new Customer()
                {
                    Email = model.AdminEmail,
                    Password = model.AdminPassword,
                    IsAdmin = true,
                    PasswordSalt = PasswordExtensions.CreatePasswordHash(model.AdminPassword, saltKey),
                    CustomerGuid = Guid.NewGuid()
                });
                await db.SaveChangesAsync();

                // Import data
                if (_appContext != null)
                {
                    // Create procedures
                    var installationEntry = _appContext.ContentRoot.GetDirectoryContents("/App_Data/Installation/");
                    if (installationEntry.Exists)
                    {
                        var sqlFile = _appContext.ContentRoot.GetFileInfo("/App_Data/Installation/SqlServer.StoredProcedures.sql");
                        if (sqlFile.Exists)
                        {
                            var sql = await sqlFile.ReadAllTextAsync();

                            IEnumerable<string> batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            foreach (string batch in batches)
                            {
                                if (batch.Trim() != "")
                                {
                                    await db.Database.ExecuteSqlRawAsync(batch, cancelToken);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Delete Db if it was auto generated
                if (db != null && shouldDeleteDbOnFailure)
                {
                    try
                    {
                        await db.Database.EnsureDeletedAsync(cancelToken);

                    }
                    catch { }

                    try
                    {
                        var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "connection.json");
                        if (System.IO.File.Exists(pathToFile))
                        {
                            System.IO.File.Delete(pathToFile);
                        }
                    }
                    catch (Exception exception) { }
                }

                return Ok(genericResponse.Error(e.Message));

            }

            return Ok(genericResponse.Success(new InstallationResult()));

        }

        #endregion
    }
}
