using System.Collections;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using ChatSharp.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using ChatSharp.Engine;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChatSharp.Core.Data
{
    public static class DBStoreProcedure
    {
        public static IEnumerable<T> ExecStoreProcedure<T>(this ChatSharpDbContext db, string sql,
            params object[] parameters)
        {
            var testExec = $"EXEC {sql}";
            sql = $"EXEC {sql}";

            if (parameters == null)
                parameters = new object[] { };

            if (parameters.Any())
            {
                //add parameters to sql
                for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
                {
                    if (!(parameters[i] is DbParameter parameter))
                        continue;

                    sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";


                    //whether parameter is output
                    if (parameter.Direction == ParameterDirection.InputOutput ||
                        parameter.Direction == ParameterDirection.Output)
                        sql = $"{sql} output";
                }
            }

            #region Test region

            try
            {
                var parameterList = new List<string>();
                if (parameters.Any())
                {
                    //add parameters to sql
                    for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
                    {
                        if (!(parameters[i] is DbParameter parameter))
                            continue;

                        //whether parameter is output
                        if (parameter.Direction == ParameterDirection.InputOutput ||
                            parameter.Direction == ParameterDirection.Output)
                        {
                            //nothing
                        }
                        else
                        {
                            parameterList.Add(
                                parameter.Value != null && !string.IsNullOrEmpty(parameter.Value.ToString())
                                    ? (parameter.DbType == DbType.String
                                        ? $"'{parameter.Value.ToString()}'"
                                        : parameter.Value.ToString())
                                    : "NULL");
                            testExec = testExec + (i > 0 ? ", " : " ") + $"@{parameter.ParameterName}=" + "{" + i + "}";
                        }
                    }
                }

                testExec = string.Format(testExec, parameterList.ToArray());
            }
            catch (Exception e)
            {
                testExec = e.Message;
            }

            #endregion

            if (ProgramEngineHelper.IsDevEnvironment)
            {
                Debug.WriteLine(testExec);
            }
            return db.Database.ExecuteQueryRaw<T>(sql, parameters);
        }

        #region Execute query

        public static IEnumerable<T> ExecuteQueryRaw<T>(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
            => ExecuteQueryRaw<T>(databaseFacade, sql, parameters.AsEnumerable());

        public static IEnumerable<T> ExecuteQueryRaw<T>(this DatabaseFacade databaseFacade, string sql, IEnumerable<object> parameters)
        {
            var isComplexType = typeof(T).IsPlainObjectType();

            using var reader = ExecuteReaderRaw(databaseFacade, sql, parameters);
            while (reader.Read())
            {
                if (reader.DbDataReader.FieldCount > 0)
                {
                    yield return isComplexType
                        ? MapReaderToObject<T>(reader.DbDataReader)
                        : reader.DbDataReader.GetValue(0).Convert<T>();
                }
            }
        }

        public static RelationalDataReader ExecuteReaderRaw(this DatabaseFacade databaseFacade, string sql, IEnumerable<object> parameters)
        {
            var facadeDependencies = GetFacadeDependencies(databaseFacade);
            var concurrencyDetector = facadeDependencies.ConcurrencyDetector;
            var logger = facadeDependencies.CommandLogger;

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = facadeDependencies.RawSqlCommandBuilder
                    .Build(sql, parameters);

                var raqCommand = rawSqlCommand
                    .RelationalCommand
                    .ExecuteReader(
                        new RelationalCommandParameterObject(
                            facadeDependencies.RelationalConnection,
                            rawSqlCommand.ParameterValues,
                            null,
                            ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context,
                            logger));

                return raqCommand;
            }
        }

        internal static IRelationalDatabaseFacadeDependencies GetFacadeDependencies(this DatabaseFacade databaseFacade)
        {
            var dependencies = ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Dependencies;
            if (dependencies is IRelationalDatabaseFacadeDependencies relationalDependencies)
            {
                return relationalDependencies;
            }

            throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
        }

        #endregion

        #region Extensions

        public static bool IsBasicType(this Type type)
        {
            return
                type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(string) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid) ||
                type == typeof(byte[]);
        }

        public static bool IsBasicOrNullableType(this Type type)
        {
            return
                IsBasicType(type) ||
                Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsSequenceType(this Type type)
        {
            if (type.IsBasicOrNullableType())
            {
                return false;
            }

            return
                type.IsArray ||
                typeof(IEnumerable).IsAssignableFrom(type) ||
                // i.e., a direct ref to System.Array
                type == typeof(Array);
        }

        public static bool IsPlainObjectType(this Type type)
        {
            return type.IsClass && !type.IsSequenceType() && !type.IsBasicOrNullableType();
        }

        private static T MapReaderToObject<T>(DbDataReader reader)
        {
            var fastProps = GetProperties(typeof(T));
            var obj = Activator.CreateInstance<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string columnName = reader.GetName(i);

                    if (fastProps.TryGetValue(columnName, out var prop))
                    {
                        var value = reader.GetValue(i);
                        if (ObjectExtensions.TryConvert(value, prop.PropertyType, culture: null, out var converted))
                        {
                            prop.SetValue(obj, converted);
                        }
                    }
                }
            }

            return obj;
        }

        public static Dictionary<string, FastProperty> GetProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    prop => prop.Name,
                    prop => new FastProperty(prop));
        }

        public class FastProperty
        {
            private readonly PropertyInfo _property;

            public FastProperty(PropertyInfo property)
            {
                _property = property;
            }

            public void SetValue(object obj, object value)
            {
                _property.SetValue(obj, value);
            }

            public object GetValue(object obj)
            {
                return _property.GetValue(obj);
            }

            public Type PropertyType => _property.PropertyType;
        }

        #endregion
    }
}
